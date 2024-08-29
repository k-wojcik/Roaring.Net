using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Roaring.Net.CRoaring;

public sealed unsafe class Roaring32BitmapMemory : IDisposable
{
    public nuint Size { get; }

    internal readonly byte* MemoryPtr;

    private bool _isDisposed;
    private bool _isDisposable;

    private readonly HashSet<Roaring32BitmapBase> _bitmapReferences = new();

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

    public Span<byte> AsSpan()
    {
        CheckDisposed();
        return new(MemoryPtr, (int)Size);
    }

    public void Write(ReadOnlySpan<byte> buffer)
    {
        CheckDisposed();

        var span = new Span<byte>(MemoryPtr, (int)Size);
        buffer.CopyTo(span);
    }

    public void Write(byte[] buffer, int offset, int count)
    {
        CheckDisposed();

        var span = new Span<byte>(MemoryPtr, (int)Size);
        buffer.AsSpan()[offset..count].CopyTo(span);
    }

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