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

    internal SafeBulkContextHandle Handle { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BulkContext"/> class.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the context will be used.</param>
    public BulkContext(Roaring32BitmapBase bitmap)
    {
        var size = Marshal.SizeOf<BulkContextInternal>();
        var pointer = Marshal.AllocHGlobal(size);

        try
        {
            Unsafe.InitBlockUnaligned(pointer.ToPointer(), 0, (uint)size);
            Handle = new SafeBulkContextHandle(pointer);
            Bitmap = bitmap;
        }
        catch
        {
            Marshal.FreeHGlobal(pointer);
            throw;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BulkContext"/> class.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the context will be used.</param>
    /// <returns>Context for the passed bitmap,</returns>
    public static BulkContext For(Roaring32BitmapBase bitmap) => new(bitmap);

    /// <inheritdoc />
    public void Dispose()
    {
        Handle.Dispose();
    }
}
