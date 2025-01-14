using Roaring.Net.CRoaring;

namespace Roaring.Net.Tests.CRoaring.Roaring32BitmapTests;

internal sealed class Roaring32BitmapTestObject : IRoaring32BitmapTestObject
{
    IReadOnlyRoaring32Bitmap IRoaring32BitmapTestObject.ReadOnlyBitmap => Bitmap;
    Roaring32BitmapBase IRoaring32BitmapTestObject.Bitmap => Bitmap;
    public Roaring32Bitmap Bitmap { get; }
    public uint[] Values { get; }

    public Roaring32BitmapTestObject(Roaring32Bitmap bitmap, uint[] values)
    {
        Bitmap = bitmap;
        Values = values;
    }

    public void Dispose()
    {
        Bitmap.Dispose();
    }
}