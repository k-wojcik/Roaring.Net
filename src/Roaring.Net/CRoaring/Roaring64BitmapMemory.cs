using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Roaring.Net.CRoaring;

/// <summary>
/// Represents the memory region used by CRoaring 64-bit bitmaps. <br/>
/// </summary>
public sealed unsafe class Roaring64BitmapMemory : IDisposable
{
    /// <summary>
    /// The size of memory region in bytes allocated for bitmap storage.
    /// </summary>
    public nuint Size { get; }

    internal SafeRoaring64MemoryHandle Handle { get; }

    private byte* MemoryPtr => (byte*)Handle.DangerousGetHandle();

#if NET9_0_OR_GREATER
    private readonly Lock _syncRoot = new();
#else
    private readonly object _syncRoot = new();
#endif

    private bool _isDisposable;
    private int _leaseCount;

    /// <summary>
    /// Initializes a new instance of the <see cref="Roaring64BitmapMemory"/> class.
    /// </summary>
    /// <param name="size">The size of memory region in bytes that will be allocated for bitmap storage.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="size"/> is out of the allowed range.</exception>
    public Roaring64BitmapMemory(nuint size)
    {
        if (size <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(size), ExceptionMessages.BitmapMemorySizeEqualToZero);
        }

        Size = size;
        Handle = AllocateMemory(size);
    }

    internal Roaring64BitmapMemory(nuint size, bool shared)
        : this(size)
    {
        _isDisposable = !shared;
    }

    private static SafeRoaring64MemoryHandle AllocateMemory(nuint size)
    {
        var pressure = checked((long)size);
        GC.AddMemoryPressure(pressure);

        byte* pointer;
        try
        {
            pointer = (byte*)NativeMemory.AlignedAlloc(size, 64);
        }
        catch
        {
            GC.RemoveMemoryPressure(pressure);
            throw;
        }

        try
        {
            return new SafeRoaring64MemoryHandle((IntPtr)pointer, size);
        }
        catch
        {
            NativeMemory.AlignedFree(pointer);
            GC.RemoveMemoryPressure(pressure);
            throw;
        }
    }

    /// <summary>
    /// Creates a new span over the memory region.
    /// </summary>
    /// <returns>The span representation of the memory region.</returns>
    public Span<byte> AsSpan()
    {
        CheckDisposed();
        return new(MemoryPtr, (int)Size);
    }

    /// <summary>
    /// Writes a byte span to the memory region.
    /// </summary>
    /// <param name="buffer">The byte span to write.</param>
    public void Write(ReadOnlySpan<byte> buffer)
    {
        CheckDisposed();

        var span = new Span<byte>(MemoryPtr, (int)Size);
        buffer.CopyTo(span);
    }

    /// <summary>
    /// Writes a subarray of bytes to the memory region.
    /// </summary>
    /// <param name="buffer">A byte array containing the data to write.</param>
    /// <param name="offset">The position in the buffer from which to start reading data.</param>
    /// <param name="count">The number of bytes to write from the offset position.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="buffer"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when arguments have invalid values.</exception>
    public void Write(byte[] buffer, int offset, int count)
    {
        CheckDisposed();
        ArgumentNullException.ThrowIfNull(buffer);
        if ((nuint)count > Size)
        {
            throw new ArgumentOutOfRangeException(nameof(count), count, ExceptionMessages.BufferSizeIsTooSmall);
        }

        buffer.AsSpan(offset, count).CopyTo(new Span<byte>(MemoryPtr, count));
    }

    /// <summary>
    /// Asynchronously writes a byte span to the memory region.
    /// </summary>
    /// <param name="buffer">The byte span to write.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is  The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A value task that represents the asynchronous write operation.</returns>
    public ValueTask WriteAsync(ReadOnlySpan<byte> buffer, CancellationToken cancellationToken = default)
    {
        CheckDisposed();

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled(cancellationToken);
        }

        Write(buffer);

        return default;
    }

    /// <summary>
    /// Asynchronously writes a subarray of bytes to the memory region.
    /// </summary>
    /// <param name="buffer">A byte array containing the data to write.</param>
    /// <param name="offset">The position in the buffer from which to start reading data.</param>
    /// <param name="count">The number of bytes to write from the offset position.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is  The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A value task that represents the asynchronous write operation.</returns>
    public ValueTask WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default)
    {
        CheckDisposed();

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled(cancellationToken);
        }

        Write(buffer, offset, count);

        return default;
    }

    /// <summary>
    /// Converts a memory area containing serialized data of the specified type to a frozen bitmap. <br/>
    /// </summary>
    /// <param name="format">Serialization format used in the filled memory region.</param>
    /// <returns>Managed CRoaring frozen bitmap.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the serialization <paramref name="format"/> is not supported.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the memory region contains invalid data and the bitmap cannot be deserialized.</exception>
    public FrozenRoaring64Bitmap ToFrozen(SerializationFormat format = SerializationFormat.Frozen)
    {
        CheckDisposed();

        var ptr = format switch
        {
            SerializationFormat.Frozen => NativeMethods.roaring64_bitmap_frozen_view_from_memory(Handle, Size),
            SerializationFormat.Portable => NativeMethods.roaring64_bitmap_portable_deserialize_frozen_from_memory(Handle, Size),
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, ExceptionMessages.UnsupportedSerializationFormat)
        };

        if (ptr == IntPtr.Zero)
        {
            throw new InvalidOperationException(ExceptionMessages.DeserializationFailedUnknownReason);
        }

        return new FrozenRoaring64Bitmap(ptr, this);
    }

    private void CheckDisposed()
    {
        if (!Handle.IsClosed)
        {
            return;
        }

        throw new ObjectDisposedException(ExceptionMessages.BitmapMemoryDisposed);
    }

    internal SafeRoaring64MemoryLease AcquireLease(SafeRoaring64BitmapHandle? viewHandle)
    {
        lock (_syncRoot)
        {
            CheckDisposed();

            var lease = new SafeRoaring64MemoryLease(this, viewHandle);
            _leaseCount++;
            return lease;
        }
    }

    internal void ReleaseLease()
    {
        lock (_syncRoot)
        {
            if (_leaseCount > 0)
            {
                _leaseCount--;
            }

            if (_leaseCount != 0 || !_isDisposable)
            {
                return;
            }

            Handle.Dispose();
        }
    }

    internal void DisposeOwnedIfUnleased()
    {
        lock (_syncRoot)
        {
            if (!_isDisposable || _leaseCount != 0)
            {
                return;
            }

            Handle.Dispose();
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        lock (_syncRoot)
        {
            if (_isDisposable)
            {
                return;
            }

            _isDisposable = true;

            if (_leaseCount == 0)
            {
                Handle.Dispose();
            }
        }
    }
}