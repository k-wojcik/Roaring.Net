using System.Collections.Generic;
using System.Linq;
using Roaring.Net.CRoaring;

namespace Roaring.Net.Tests.CRoaring.TestData;

internal static class SerializationTestBitmap
{
    public static List<uint> GetTestBitmapValues()
    {
        var values = new List<uint>();
        for (uint k = 0; k < 100000; k += 1000)
        {
            values.Add(k);
        }

        for (uint k = 100000; k < 200000; ++k)
        {
            values.Add(3 * k);
        }

        for (uint k = 700000; k < 800000; ++k)
        {
            values.Add(k);
        }

        return values;
    }

    public static List<ulong> GetTestBitmap64Values() => GetTestBitmapValues().Select(x => (ulong)x).ToList();

    public static Roaring32Bitmap GetTestBitmap()
    {
        List<uint> values = GetTestBitmapValues();
        var bitmap = new Roaring32Bitmap();

        foreach (var value in values)
        {
            bitmap.Add(value);
        }

        return bitmap;
    }

    public static Roaring64Bitmap GetTestBitmap64()
    {
        List<ulong> values = GetTestBitmap64Values();
        var bitmap = new Roaring64Bitmap();

        foreach (var value in values)
        {
            bitmap.Add(value);
        }

        return bitmap;
    }
}