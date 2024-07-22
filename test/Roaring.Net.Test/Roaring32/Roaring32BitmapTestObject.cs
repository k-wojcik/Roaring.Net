using System;

namespace Roaring.Test.Roaring32;

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

    public static Roaring32BitmapTestObject GetEmpty() => GetForRange(0, uint.MaxValue, count: 0);

    public static Roaring32BitmapTestObject GetForCount(uint count) => GetForRange(0, uint.MaxValue, count);

    public static Roaring32BitmapTestObject GetForRange(uint start, uint end, uint count)
    {
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, "Start cannot be greater then end.");
        }

        if (end - start < count)
        {
            throw new ArgumentOutOfRangeException(nameof(count), count,
                "Count cannot be greater then start-end range.");
        }

        uint[] values;
        if (count > 0)
        {
            uint step = (end - start) / count;

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