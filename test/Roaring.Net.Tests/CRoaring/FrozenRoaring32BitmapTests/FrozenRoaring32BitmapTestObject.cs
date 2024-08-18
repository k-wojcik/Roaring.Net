using Roaring.Net.CRoaring;

namespace Roaring.Net.Tests.CRoaring.FrozenRoaring32BitmapTests;

internal sealed class FrozenRoaring32BitmapTestObject : IRoaring32BitmapTestObject
{
    IReadOnlyRoaring32Bitmap IRoaring32BitmapTestObject.ReadOnlyBitmap => Bitmap;
    Roaring32BitmapBase IRoaring32BitmapTestObject.Bitmap => Bitmap;
    public FrozenRoaring32Bitmap Bitmap { get; private set; }
    public uint[] Values { get; }
    
    public FrozenRoaring32BitmapTestObject(FrozenRoaring32Bitmap bitmap, uint[] values)
    {
        Bitmap = bitmap;
        Values = values;
    }
    
    public void Dispose()
    {
        Bitmap?.Dispose();
        Bitmap = null!;
    }
}