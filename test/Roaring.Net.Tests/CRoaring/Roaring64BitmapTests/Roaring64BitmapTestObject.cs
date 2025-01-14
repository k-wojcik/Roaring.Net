using Roaring.Net.CRoaring;

namespace Roaring.Net.Tests.CRoaring.Roaring64BitmapTests;

internal sealed class Roaring64BitmapTestObject : IRoaring64BitmapTestObject
{
    IReadOnlyRoaring64Bitmap IRoaring64BitmapTestObject.ReadOnlyBitmap => Bitmap;
    Roaring64BitmapBase IRoaring64BitmapTestObject.Bitmap => Bitmap;
    public Roaring64Bitmap Bitmap { get; }
    public ulong[] Values { get; }

    public Roaring64BitmapTestObject(Roaring64Bitmap bitmap, ulong[] values)
    {
        Bitmap = bitmap;
        Values = values;
    }

    public void Dispose()
    {
        Bitmap.Dispose();
    }
}