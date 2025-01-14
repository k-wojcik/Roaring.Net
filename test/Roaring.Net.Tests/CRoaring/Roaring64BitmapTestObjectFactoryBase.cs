using System;
using Roaring.Net.CRoaring;

namespace Roaring.Net.Tests.CRoaring;

internal abstract class Roaring64BitmapTestObjectFactoryBase<TBitmap, TTestObject> : IRoaring64BitmapTestObjectFactory<TTestObject>, IRoaring64BitmapTestObjectFactory
    where TBitmap : IReadOnlyRoaring64Bitmap
    where TTestObject : IRoaring64BitmapTestObject
{
    IRoaring64BitmapTestObject IRoaring64BitmapTestObjectFactory.GetDefault() => GetDefault();
    IRoaring64BitmapTestObject IRoaring64BitmapTestObjectFactory.GetEmpty() => GetEmpty();
    IRoaring64BitmapTestObject IRoaring64BitmapTestObjectFactory.GetForCount(ulong count) => GetForCount(count);
    IRoaring64BitmapTestObject IRoaring64BitmapTestObjectFactory.GetFromValues(ulong[] values) => GetFromValues(values);
    IRoaring64BitmapTestObject IRoaring64BitmapTestObjectFactory.GetForRange(ulong start, ulong end) => GetForRange(start, end);
    IRoaring64BitmapTestObject IRoaring64BitmapTestObjectFactory.GetForRange(ulong start, ulong end, ulong count) => GetForRange(start, end, count);

    public virtual TTestObject GetDefault() => GetForRange(0, ulong.MaxValue, count: 1000);
    public virtual TTestObject GetEmpty() => CreateBitmapObject(new Roaring64Bitmap(), []);

    public virtual TTestObject GetForCount(ulong count) => GetForRange(0, ulong.MaxValue, count);

    public virtual TTestObject GetFromValues(ulong[] values) => CreateBitmapObject(Roaring64Bitmap.FromValues(values), values);

    public virtual TTestObject GetForRange(ulong start, ulong end) => GetForRange(start, end, end - start + 1);

    public virtual TTestObject GetForRange(ulong start, ulong end, ulong count)
    {
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, "Start cannot be greater then end.");
        }

        var length = end - start == ulong.MaxValue ? ulong.MaxValue : end - start + 1;
        if (length < count)
        {
            throw new ArgumentOutOfRangeException(nameof(count), count, "Count cannot be greater then start-end range.");
        }

        ulong[] values;
        if (count > 0)
        {
            ulong step = length / count;

            values = new ulong[count];

            for (ulong i = 0; i < count; i++)
            {
                values[i] = start + i * step;
            }
        }
        else
        {
            values = [];
        }

        var bitmap = Roaring64Bitmap.FromValues(values);
        return CreateBitmapObject(bitmap, values);
    }

    protected abstract TTestObject CreateBitmapObject(Roaring64Bitmap bitmap, ulong[] values);
}