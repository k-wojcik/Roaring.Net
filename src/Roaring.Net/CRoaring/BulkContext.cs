using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Roaring.Net.CRoaring;

public sealed unsafe class BulkContext : IDisposable
{
    internal Roaring32BitmapBase Bitmap { get; }
    
    internal readonly IntPtr Pointer;

    private bool _isDisposed;

    public BulkContext(Roaring32BitmapBase bitmap)
    {
        var size = Marshal.SizeOf<BulkContextInternal>();
        Pointer = Marshal.AllocHGlobal(size);
        Unsafe.InitBlockUnaligned(Pointer.ToPointer(), 0, (uint)size);
        Bitmap = bitmap;
    }

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

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~BulkContext()
    {
        Dispose(false);
    }
}