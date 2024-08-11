using System;

namespace CRoaring.Test.Roaring32;

internal sealed class Roaring32BitmapTestObject : IDisposable
{
    public Roaring32Bitmap Bitmap { get; }
    public uint[] Values { get; }

    private Roaring32BitmapTestObject(Roaring32Bitmap bitmap, uint[] values)
    {
        Bitmap = bitmap;
        Values = values;
    }

    public static Roaring32BitmapTestObject GetDefault() => GetForRange(0, uint.MaxValue, count: 1000);

    public static Roaring32BitmapTestObject GetEmpty() => new(new Roaring32Bitmap(), []);

    public static Roaring32BitmapTestObject GetForCount(uint count) => GetForRange(0, uint.MaxValue, count);
    
    public static Roaring32BitmapTestObject GetFromValues(uint[] values) => new(Roaring32Bitmap.FromValues(values), values);

    public static Roaring32BitmapTestObject GetForRange(uint start, uint end) => GetForRange(start, end, end - start + 1);

    public static Roaring32BitmapTestObject GetForRange(uint start, uint end, uint count)
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
        return new Roaring32BitmapTestObject(bitmap, values);
    }

    public void Dispose()
    {
        Bitmap?.Dispose();
    }
}