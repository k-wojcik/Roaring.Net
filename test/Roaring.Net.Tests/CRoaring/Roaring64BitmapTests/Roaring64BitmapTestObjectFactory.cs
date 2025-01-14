using Roaring.Net.CRoaring;

namespace Roaring.Net.Tests.CRoaring.Roaring64BitmapTests;

internal class Roaring64BitmapTestObjectFactory : Roaring64BitmapTestObjectFactoryBase<Roaring64Bitmap, Roaring64BitmapTestObject>
{
    public static Roaring64BitmapTestObjectFactory Default { get; } = new();

    protected override Roaring64BitmapTestObject CreateBitmapObject(Roaring64Bitmap bitmap, ulong[] values)
    {
        return new Roaring64BitmapTestObject(bitmap, values);
    }
}