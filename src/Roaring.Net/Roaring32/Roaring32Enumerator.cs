﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Roaring;

internal sealed unsafe class Roaring32Enumerator : IEnumerator<uint>, IEnumerable<uint>
{
    private readonly NativeMethods.Iterator* _iterator;
    private bool _isFirst;
    private bool _isDisposed;

    public uint Current
    {
        get
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("Cannot access a disposed enumerator.");
            }

            return _iterator->current_value;
        }
    }

    object IEnumerator.Current => Current;

    public Roaring32Enumerator(IntPtr bitmap)
    {
        _iterator = (NativeMethods.Iterator*)NativeMethods.roaring_iterator_create(bitmap);
        _isFirst = true;
    }

    public bool MoveNext()
    {
        if (_isDisposed)
        {
            throw new ObjectDisposedException("Cannot access a disposed enumerator.");
        }

        if (_isFirst)
        {
            _isFirst = false;
            return _iterator->has_value;
        }

        return NativeMethods.roaring_uint32_iterator_advance(new IntPtr(_iterator));
    }

    public void Reset() => throw new NotSupportedException();

    private void Dispose(bool _)
    {
        if (_isDisposed)
        {
            return;
        }

        NativeMethods.roaring_uint32_iterator_free(new IntPtr(_iterator));
        _isDisposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public IEnumerator<uint> GetEnumerator() => this;

    ~Roaring32Enumerator() => Dispose(false);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}