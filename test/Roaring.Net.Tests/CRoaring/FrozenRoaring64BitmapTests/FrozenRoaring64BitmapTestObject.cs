using Roaring.Net.CRoaring;

namespace Roaring.Net.Tests.CRoaring.FrozenRoaring64BitmapTests;

internal sealed class FrozenRoaring64BitmapTestObject : IRoaring64BitmapTestObject
{
    IReadOnlyRoaring64Bitmap IRoaring64BitmapTestObject.ReadOnlyBitmap => Bitmap;
    Roaring64BitmapBase IRoaring64BitmapTestObject.Bitmap => Bitmap;
    public FrozenRoaring64Bitmap Bitmap { get; private set; }
    public ulong[] Values { get; }

    public FrozenRoaring64BitmapTestObject(FrozenRoaring64Bitmap bitmap, ulong[] values)
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