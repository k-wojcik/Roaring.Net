using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Roaring.Test.Roaring32;

public class Roaring32BitmapTests
{
    [Fact]
    public void TestOr()
    {
        uint[] values1 = [1, 2, 3, 4, 5, 100, 1000];
        uint[] values2 = [1, 2, 3, 4, 5, 100, 1000];
        uint[] values3 = [3, 4, 5, 7, 100, 1020];

        using var source1 = Roaring32Bitmap.FromValues(values1);
        using var source2 = Roaring32Bitmap.FromValues(values2);
        using var source3 = Roaring32Bitmap.FromValues(values3);
        using var result1 = source1.Or(source2);
        using var result2 = source2.Or(source3);
        using var result3 = result1.Or(source3);
        Assert.Equal(result1.Count, OrCount(values1, values2));
        Assert.Equal(result2.Count, OrCount(values2, values3));
        Assert.Equal(result3.Count, OrCount(values1, values2, values3));
    }

    [Fact]
    public void TestIOr()
    {
        uint[] values1 = [1, 2, 3, 4, 5, 100, 1000];
        uint[] values2 = [1, 2, 3, 4, 5, 100, 1000];
        uint[] values3 = [3, 4, 5, 7, 100, 1020];

        using var result = Roaring32Bitmap.FromValues(values1);
        using var source1 = Roaring32Bitmap.FromValues(values2);
        using var source2 = Roaring32Bitmap.FromValues(values3);
        result.IOr(source1);
        result.IOr(source2);
        Assert.Equal(result.Count, OrCount(values1, values2, values3));
    }

    [Fact]
    public void TestXor()
    {
        uint[] values1 = [1, 2, 3, 4, 5, 100, 1000];
        uint[] values2 = [1, 2, 3, 4, 5, 100, 1000];
        uint[] values3 = [3, 4, 5, 7, 100, 1020];

        using var source1 = Roaring32Bitmap.FromValues(values1);
        using var source2 = Roaring32Bitmap.FromValues(values2);
        using var source3 = Roaring32Bitmap.FromValues(values3);
        using var result1 = source1.Xor(source2);
        using var result2 = source2.Xor(source3);
        using var result3 = result1.Xor(source3);
        Assert.Equal(result1.Count, XorCount(values1, values2));
        Assert.Equal(result2.Count, XorCount(values2, values3));
        Assert.Equal(result3.Count, XorCount(values1, values2, values3));
    }

    [Fact]
    public void TestIXor()
    {
        uint[] values1 = [1, 2, 3, 4, 5, 100, 1000];
        uint[] values2 = [1, 2, 3, 4, 5, 100, 1000];
        uint[] values3 = [3, 4, 5, 7, 100, 1020];

        using var result = Roaring32Bitmap.FromValues(values1);
        using var source1 = Roaring32Bitmap.FromValues(values2);
        using var source2 = Roaring32Bitmap.FromValues(values3);
        result.IXor(source1);
        result.IXor(source2);
        Assert.Equal(result.Count, XorCount(values1, values2, values3));
    }

    [Fact]
    public void TestSerialization()
    {
        using var rb1 = new Roaring32Bitmap();
        rb1.AddMany([1, 2, 3, 4, 5, 100, 1000]);
        rb1.Optimize();

        var s1 = rb1.Serialize(SerializationFormat.Normal);
        var s2 = rb1.Serialize(SerializationFormat.Portable);

        using var rb2 = Roaring32Bitmap.Deserialize(s1, SerializationFormat.Normal);
        using var rb3 = Roaring32Bitmap.Deserialize(s2, SerializationFormat.Portable);
        Assert.True(rb1.Equals(rb2));
        Assert.True(rb1.Equals(rb3));
    }

    private static ulong OrCount(params IEnumerable<uint>[] values)
    {
        var set = values[0];
        for (int i = 1; i < values.Length; i++)
            set = set.Union(values[i]);
        return (ulong)set.LongCount();
    }

    private static ulong AndCount(params IEnumerable<uint>[] values)
    {
        var set = values[0];
        for (int i = 1; i < values.Length; i++)
            set = set.Intersect(values[i]);
        return (ulong)set.LongCount();
    }

    private static ulong AndNotCount(params IEnumerable<uint>[] values)
    {
        var set = values[0];
        for (int i = 1; i < values.Length; i++)
            set = set.Except(values[i]);
        return (ulong)set.LongCount();
    }

    private static ulong XorCount(params IEnumerable<uint>[] values)
    {
        var set = values[0];
        for (int i = 1; i < values.Length; i++)
            set = set.Except(values[i]).Union(values[i].Except(set));
        return (ulong)set.LongCount();
    }
}