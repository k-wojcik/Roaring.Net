using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Roaring.Net.CRoaring;

/// <summary>
/// Represents the CRoaring bulk context used to speed up some operations. <br/>
/// Context used with `*Bulk()` methods, can only be used with one bitmap object. <br/>
/// Any modification to a bitmap (other than by `*Bulk()` methods with the context)
/// will invalidate any contexts associated with that bitmap. <br/>
/// <a href="https://github.com/RoaringBitmap/CRoaring/blob/bfba0296178e98ca12a9a3f44514c2e9a0a8ac6e/include/roaring/roaring64.h#L34">Wrapped type roaring64_bulk_context_t</a>
/// </summary>
public sealed unsafe class BulkContext64 : IDisposable
{
    internal Roaring64BitmapBase Bitmap { get; }

    internal SafeBulkContextHandle Handle { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BulkContext64"/> class.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the context will be used.</param>
    public BulkContext64(Roaring64BitmapBase bitmap)
    {
        var size = Marshal.SizeOf<BulkContext64Internal>();
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
    /// Initializes a new instance of the <see cref="BulkContext64"/> class.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the context will be used.</param>
    /// <returns>Context for the passed bitmap,</returns>
    public static BulkContext64 For(Roaring64BitmapBase bitmap) => new(bitmap);

    /// <inheritdoc />
    public void Dispose()
    {
        Handle.Dispose();
    }
}
