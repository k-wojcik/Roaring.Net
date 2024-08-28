using System;
using System.Runtime.InteropServices;

namespace Roaring.Net.CRoaring;

public sealed class BulkContext : IDisposable
{
    internal Roaring32BitmapBase Bitmap { get; }
    
    internal readonly IntPtr Pointer;

    private bool _isDisposed;

    public BulkContext(Roaring32BitmapBase bitmap)
    {
        Pointer = Marshal.AllocHGlobal(Marshal.SizeOf<BulkContextInternal>());
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