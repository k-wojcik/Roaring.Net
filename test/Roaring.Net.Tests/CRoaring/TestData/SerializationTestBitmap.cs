using System.Collections.Generic;
using Roaring.Net.CRoaring;

namespace Roaring.Net.Tests.CRoaring.TestData;

internal static class SerializationTestBitmap
{
    public static List<uint> GetTestBitmapValues()
    {
        var values = new List<uint>();
        for (uint k = 0; k < 100000; k+= 1000) {
            values.Add(k);
        }
        for (uint k = 100000; k < 200000; ++k) {
            values.Add(3*k);
        }
        for (uint k = 700000; k < 800000; ++k) {
            values.Add(k);
        }

        return values;
    }
    
    public static Roaring32Bitmap GetTestBitmap()
    {
        var values = GetTestBitmapValues();
        var bitmap = new Roaring32Bitmap();

        foreach (var value in values)
        { 
            bitmap.Add(value);
        }
        
        return bitmap;
    }
}