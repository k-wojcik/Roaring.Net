using System;

namespace Roaring.Net.CRoaring;

public abstract class Roaring32BitmapBase : IDisposable
{
    protected internal IntPtr Pointer;

    protected abstract void Dispose(bool disposing);

    ~Roaring32BitmapBase() => Dispose(false);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}