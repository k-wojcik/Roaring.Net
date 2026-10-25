using System;
using System.Collections;
using System.Collections.Generic;

namespace Roaring.Net.CRoaring;

internal sealed unsafe class Roaring32Enumerator : IEnumerator<uint>, IEnumerable<uint>
{
    private readonly SafeRoaring32IteratorHandle _iteratorHandle;
    private readonly NativeMethods.Iterator* _iterator;
    private bool _isFirst;
    private bool _isDisposed;

    public uint Current
    {
        get
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(ExceptionMessages.DisposedEnumerator);
            }

            return _iterator->current_value;
        }
    }

    object IEnumerator.Current => Current;

    internal Roaring32Enumerator(SafeRoaring32BitmapHandle bitmap, object? owner = null, Roaring32BitmapMemory? memory = null)
    {
        SafeRoaring32MemoryLease? memoryLease = memory?.AcquireLease(null);

        IntPtr ptr;
        try
        {
            ptr = NativeMethods.roaring_iterator_create(bitmap);
        }
        catch
        {
            memoryLease?.Dispose();
            throw;
        }

        if (ptr == IntPtr.Zero)
        {
            memoryLease?.Dispose();
            throw new InvalidOperationException(ExceptionMessages.UnableToAllocateBitmapIterator);
        }

        _iteratorHandle = new SafeRoaring32IteratorHandle(ptr, bitmap, memoryLease, owner);
        _iterator = (NativeMethods.Iterator*)ptr;
        _isFirst = true;
    }

    public bool MoveNext()
    {
        if (_isDisposed)
        {
            throw new ObjectDisposedException(ExceptionMessages.DisposedEnumerator);
        }

        if (_isFirst)
        {
            _isFirst = false;
            return _iterator->has_value;
        }

        return NativeMethods.roaring_uint32_iterator_advance(new IntPtr(_iterator));
    }

    public void Reset() => throw new NotSupportedException(ExceptionMessages.OperationNotSupported);

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;
        _iteratorHandle.Dispose();
    }

    public IEnumerator<uint> GetEnumerator() => this;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
