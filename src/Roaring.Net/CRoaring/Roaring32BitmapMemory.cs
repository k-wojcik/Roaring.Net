using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Roaring.Net.CRoaring;

/// <summary>
/// Represents the memory region used by CRoaring bitmaps. <br/>
/// </summary>
public sealed unsafe class Roaring32BitmapMemory : IDisposable
{
    /// <summary>
    /// The size of memory region in bytes allocated for bitmap storage.
    /// </summary>
    public nuint Size { get; }

    internal readonly byte* MemoryPtr;

    private bool _isDisposed;
    private bool _isDisposable;

    private readonly HashSet<Roaring32BitmapBase> _bitmapReferences = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Roaring32BitmapMemory"/> class.
    /// </summary>
    /// <param name="size">The size of memory region in bytes that will be allocated for bitmap storage.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="size"/> is out of the allowed range.</exception>
    public Roaring32BitmapMemory(nuint size)
    {
        if (size <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(size), ExceptionMessages.BitmapMemorySizeEqualToZero);
        }

        Size = size;
        MemoryPtr = AllocateMemory(size);
    }

    private static byte* AllocateMemory(nuint size)
    {
        GC.AddMemoryPressure((long)size);
        return (byte*)NativeMemory.AlignedAlloc(size, 32);
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
    public void Write(byte[] buffer, int offset, int count)
    {
        CheckDisposed();

        var span = new Span<byte>(MemoryPtr, (int)Size);
        buffer.AsSpan()[offset..count].CopyTo(span);
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
    /// <a href="https://github.com/RoaringBitmap/CRoaring/blob/60d0e97fa021b04f8a6ad50e3877ca16d988c80e/include/roaring/roaring.h#L694-L711"> "Frozen" serialization format.</a>
    /// </summary>
    /// <param name="format">Serialization format used in the filled memory region.</param>
    /// <returns>Managed CRoaring frozen bitmap.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the serialization <paramref name="format"/> is not supported.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the memory region contains invalid data and the bitmap cannot be deserialized.</exception>
    public FrozenRoaring32Bitmap ToFrozen(SerializationFormat format = SerializationFormat.Frozen)
    {
        CheckDisposed();

        IntPtr ptr = format switch
        {
            SerializationFormat.Frozen => NativeMethods.roaring_bitmap_frozen_view(MemoryPtr, Size),
            SerializationFormat.Portable => NativeMethods.roaring_bitmap_portable_deserialize_frozen(MemoryPtr),
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, ExceptionMessages.UnsupportedSerializationFormat)
        };

        if (ptr == IntPtr.Zero)
        {
            throw new InvalidOperationException(ExceptionMessages.DeserializationFailedUnknownReason);
        }

        var bitmap = new FrozenRoaring32Bitmap(ptr, this);
        _bitmapReferences.Add(bitmap);
        return bitmap;
    }

    private void CheckDisposed()
    {
        if (!_isDisposed)
        {
            return;
        }

        throw new ObjectDisposedException(ExceptionMessages.BitmapMemoryDisposed);
    }

    internal void Release(Roaring32BitmapBase bitmap)
    {
        _bitmapReferences.Remove(bitmap);

        if (_bitmapReferences.Count > 0 || !_isDisposable)
        {
            return;
        }

        Dispose();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_bitmapReferences.Count > 0)
        {
            _isDisposable = true;
            return;
        }

        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (MemoryPtr != null)
        {
            NativeMemory.AlignedFree(MemoryPtr);
        }

        if (Size > 0)
        {
            GC.RemoveMemoryPressure((long)Size);
        }

        _isDisposed = true;
    }

    ~Roaring32BitmapMemory() => Dispose(false);
}