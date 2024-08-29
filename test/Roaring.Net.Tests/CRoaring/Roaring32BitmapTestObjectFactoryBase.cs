using System;
using Roaring.Net.CRoaring;

namespace Roaring.Net.Tests.CRoaring.Roaring32BitmapTests;

internal abstract class Roaring32BitmapTestObjectFactoryBase<TBitmap, TTestObject> : IRoaring32BitmapTestObjectFactory<TTestObject>, IRoaring32BitmapTestObjectFactory
    where TBitmap : IReadOnlyRoaring32Bitmap
    where TTestObject : IRoaring32BitmapTestObject
{
    IRoaring32BitmapTestObject IRoaring32BitmapTestObjectFactory.GetDefault() => GetDefault();
    IRoaring32BitmapTestObject IRoaring32BitmapTestObjectFactory.GetEmpty() => GetEmpty();
    IRoaring32BitmapTestObject IRoaring32BitmapTestObjectFactory.GetForCount(uint count) => GetForCount(count);
    IRoaring32BitmapTestObject IRoaring32BitmapTestObjectFactory.GetFromValues(uint[] values) => GetFromValues(values);
    IRoaring32BitmapTestObject IRoaring32BitmapTestObjectFactory.GetForRange(uint start, uint end) => GetForRange(start, end);
    IRoaring32BitmapTestObject IRoaring32BitmapTestObjectFactory.GetForRange(uint start, uint end, uint count) => GetForRange(start, end, count);

    public virtual TTestObject GetDefault() => GetForRange(0, uint.MaxValue, count: 1000);
    public virtual TTestObject GetEmpty() => CreateBitmapObject(new Roaring32Bitmap(), []);

    public virtual TTestObject GetForCount(uint count) => GetForRange(0, uint.MaxValue, count);

    public virtual TTestObject GetFromValues(uint[] values) => CreateBitmapObject(Roaring32Bitmap.FromValues(values), values);

    public virtual TTestObject GetForRange(uint start, uint end) => GetForRange(start, end, end - start + 1);

    public virtual TTestObject GetForRange(uint start, uint end, uint count)
    {
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, "Start cannot be greater then end.");
        }

        var length = end - start == uint.MaxValue ? uint.MaxValue : end - start + 1;
        if (length < count)
        {
            throw new ArgumentOutOfRangeException(nameof(count), count, "Count cannot be greater then start-end range.");
        }

        uint[] values;
        if (count > 0)
        {
            uint step = length / count;

            values = new uint[count];

            for (int i = 0; i < count; i++)
            {
                values[i] = start + (uint)i * step;
            }
        }
        else
        {
            values = [];
        }

        var bitmap = Roaring32Bitmap.FromValues(values);
        return CreateBitmapObject(bitmap, values);
    }

    protected abstract TTestObject CreateBitmapObject(Roaring32Bitmap bitmap, uint[] values);
}