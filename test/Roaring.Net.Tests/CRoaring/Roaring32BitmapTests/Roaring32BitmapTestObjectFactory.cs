using Roaring.Net.CRoaring;

namespace Roaring.Net.Tests.CRoaring.Roaring32BitmapTests;

internal class Roaring32BitmapTestObjectFactory : Roaring32BitmapTestObjectFactoryBase<Roaring32Bitmap, Roaring32BitmapTestObject>
{
    public static Roaring32BitmapTestObjectFactory Default { get; } = new();

    protected override Roaring32BitmapTestObject CreateBitmapObject(Roaring32Bitmap bitmap, uint[] values)
    {
        return new Roaring32BitmapTestObject(bitmap, values);
    }
}