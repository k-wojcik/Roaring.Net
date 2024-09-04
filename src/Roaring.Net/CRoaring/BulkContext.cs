using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Roaring.Net.CRoaring;

/// <summary>
/// Represents the CRoaring bulk context used to speed up some operations. <br/>
/// Context used with `*Bulk()` methods, can only be used with one bitmap object. <br/>
/// Any modification to a bitmap (other than by `*Bulk()` methods with the context)
/// will invalidate any contexts associated with that bitmap. <br/>
/// <a href="https://github.com/RoaringBitmap/CRoaring/pull/363">Introduce roaring_bitmap_*_bulk operations in CRoaring</a> <br/>
/// <a href="https://github.com/RoaringBitmap/CRoaring/blob/60d0e97fa021b04f8a6ad50e3877ca16d988c80e/include/roaring/roaring.h#L333">Wrapped type roaring_bulk_context_t</a>
/// </summary>
public sealed unsafe class BulkContext : IDisposable
{
    internal Roaring32BitmapBase Bitmap { get; }

    internal readonly IntPtr Pointer;

    private bool _isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="BulkContext"/> class.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the context will be used.</param>
    public BulkContext(Roaring32BitmapBase bitmap)
    {
        var size = Marshal.SizeOf<BulkContextInternal>();
        Pointer = Marshal.AllocHGlobal(size);
        Unsafe.InitBlockUnaligned(Pointer.ToPointer(), 0, (uint)size);
        Bitmap = bitmap;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BulkContext"/> class.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the context will be used.</param>
    /// <returns>Context for the passed bitmap,</returns>
    public static BulkContext For(Roaring32BitmapBase bitmap) => new(bitmap);

    private void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        Marshal.FreeHGlobal(Pointer);

        _isDisposed = true;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Finalizer.
    /// </summary>
    ~BulkContext()
    {
        Dispose(false);
    }
}