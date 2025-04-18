using Roaring.Net.CRoaring;

namespace Roaring.Net.Tests.CRoaring.FrozenRoaring64BitmapTests;

internal class FrozenRoaring64BitmapTestObjectFactory : Roaring64BitmapTestObjectFactoryBase<FrozenRoaring64Bitmap, FrozenRoaring64BitmapTestObject>
{
    public static FrozenRoaring64BitmapTestObjectFactory Default { get; } = new();

    protected override FrozenRoaring64BitmapTestObject CreateBitmapObject(Roaring64Bitmap bitmap, ulong[] values)
    {
        FrozenRoaring64Bitmap frozenBitmap = bitmap.ToFrozen();
        bitmap.Dispose();
        return new FrozenRoaring64BitmapTestObject(frozenBitmap, values);
    }
}