using System;
using System.Collections;
using System.Collections.Generic;

namespace Roaring.Net.CRoaring;

internal sealed class Roaring64Enumerator : IEnumerator<ulong>, IEnumerable<ulong>
{
    private readonly SafeRoaring64IteratorHandle _iteratorHandle;
    private readonly IntPtr _iterator;
    private bool _isFirst;
    private bool _isDisposed;

    public ulong Current
    {
        get
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(ExceptionMessages.DisposedEnumerator);
            }

            if (_isFirst)
            {
                return 0;
            }

            return NativeMethods.roaring64_iterator_value(_iterator);
        }
    }

    object IEnumerator.Current => Current;

    internal Roaring64Enumerator(SafeRoaring64BitmapHandle bitmap, object? owner = null, Roaring64BitmapMemory? memory = null)
    {
        SafeRoaring64MemoryLease? memoryLease = memory?.AcquireLease(null);

        IntPtr ptr;
        try
        {
            ptr = NativeMethods.roaring64_iterator_create(bitmap);
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

        _iteratorHandle = new SafeRoaring64IteratorHandle(ptr, bitmap, memoryLease, owner);
        _iterator = ptr;
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
            return NativeMethods.roaring64_iterator_has_value(_iterator);
        }

        return NativeMethods.roaring64_iterator_advance(_iterator);
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

    public IEnumerator<ulong> GetEnumerator() => this;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
