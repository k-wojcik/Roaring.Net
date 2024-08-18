using Roaring.Net.CRoaring;
using Roaring.Net.Tests.CRoaring.Roaring32BitmapTests;

namespace Roaring.Net.Tests.CRoaring.FrozenRoaring32BitmapTests;

internal class FrozenRoaring32BitmapTestObjectFactory : Roaring32BitmapTestObjectFactoryBase<FrozenRoaring32Bitmap, FrozenRoaring32BitmapTestObject>
{
    public static FrozenRoaring32BitmapTestObjectFactory Default { get; } = new();
    
    protected override FrozenRoaring32BitmapTestObject CreateBitmapObject(Roaring32Bitmap bitmap, uint[] values)
    {
        var frozenBitmap = bitmap.ToFrozen();
        bitmap.Dispose();
        return new FrozenRoaring32BitmapTestObject(frozenBitmap, values);
    }
}