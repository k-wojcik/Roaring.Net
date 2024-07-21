using System;
using System.Collections;
using System.Collections.Generic;

namespace Roaring
{
    internal sealed unsafe class Roaring32Enumerator : IEnumerator<uint>, IEnumerable<uint>
    {
        private readonly NativeMethods.Iterator* _iterator;
        private bool _isFirst, _isDisposed;

        public uint Current => _iterator->current_value;
        object IEnumerator.Current => Current;

        public Roaring32Enumerator(IntPtr bitmap)
        {
            _iterator = (NativeMethods.Iterator*)NativeMethods.roaring_iterator_create(bitmap);
            _isFirst = true;
        }

        public bool MoveNext()
        {
            if (_isFirst)
            {
                _isFirst = false;
                return _iterator->has_value;
            }

            return NativeMethods.roaring_uint32_iterator_advance(new IntPtr(_iterator));
        }

        public void Reset()
        {
            throw new InvalidOperationException();
        }

        private void Dispose(bool isDisposing)
        {
            if (_isDisposed) return;

            NativeMethods.roaring_uint32_iterator_free(new IntPtr(_iterator));
            _isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerator<uint> GetEnumerator()
        {
            return this;
        }

        ~Roaring32Enumerator() => Dispose(false);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}