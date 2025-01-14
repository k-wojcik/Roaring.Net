using System;
using System.Collections;
using System.Collections.Generic;

namespace Roaring.Net.CRoaring;

internal sealed class Roaring64Enumerator : IEnumerator<ulong>, IEnumerable<ulong>
{
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

    internal Roaring64Enumerator(IntPtr bitmap)
    {
        IntPtr ptr = NativeMethods.roaring64_iterator_create(bitmap);
        if (ptr == IntPtr.Zero)
        {
            throw new InvalidOperationException(ExceptionMessages.UnableToAllocateBitmapIterator);
        }

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

    private void Dispose(bool _)
    {
        if (_isDisposed)
        {
            return;
        }

        NativeMethods.roaring64_iterator_free(_iterator);
        _isDisposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public IEnumerator<ulong> GetEnumerator() => this;

    ~Roaring64Enumerator() => Dispose(false);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}