﻿using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Roaring.Test.Roaring32;

public class Roaring32BitmapTests
{
    [Fact]
    public void TestNot()
    {
        uint[] values = [1, 2, 3, 4, 5, 100, 1000];
        uint max = values.Max() + 1;

        using var source = Roaring32Bitmap.FromValues(values);
        using var result = source.Not(0, max);
        for (uint i = 0; i < max; i++)
        {
            if (values.Contains(i))
                Assert.False(result.Contains(i));
            else
                Assert.True(result.Contains(i));
        }
    }

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
    public void TestAnd()
    {
        uint[] values1 = [1, 2, 3, 4, 5, 100, 1000];
        uint[] values2 = [1, 2, 3, 4, 5, 100, 1000];
        uint[] values3 = [3, 4, 5, 7, 100, 1020];

        using var source1 = Roaring32Bitmap.FromValues(values1);
        using var source2 = Roaring32Bitmap.FromValues(values2);
        using var source3 = Roaring32Bitmap.FromValues(values3);
        using var result1 = source1.And(source2);
        using var result2 = source2.And(source3);
        using var result3 = result1.And(source3);
        Assert.Equal(result1.Count, AndCount(values1, values2));
        Assert.Equal(result2.Count, AndCount(values2, values3));
        Assert.Equal(result3.Count, AndCount(values1, values2, values3));
    }

    [Fact]
    public void TestIAnd()
    {
        uint[] values1 = [1, 2, 3, 4, 5, 100, 1000];
        uint[] values2 = [1, 2, 3, 4, 5, 100, 1000];
        uint[] values3 = [3, 4, 5, 7, 100, 1020];

        using var result = Roaring32Bitmap.FromValues(values1);
        using var source1 = Roaring32Bitmap.FromValues(values2);
        using var source2 = Roaring32Bitmap.FromValues(values3);
        result.IAnd(source1);
        result.IAnd(source2);
        Assert.Equal(result.Count, AndCount(values1, values2, values3));
    }

    [Fact]
    public void TestAndNot()
    {
        uint[] values1 = [1, 2, 3, 4, 5, 100, 1000];
        uint[] values2 = [1, 2, 3, 4, 5, 100, 1000];
        uint[] values3 = [3, 4, 5, 7, 100, 1020];

        using var source1 = Roaring32Bitmap.FromValues(values1);
        using var source2 = Roaring32Bitmap.FromValues(values2);
        using var source3 = Roaring32Bitmap.FromValues(values3);
        using var result1 = source1.AndNot(source2);
        using var result2 = source2.AndNot(source3);
        using var result3 = result1.AndNot(source3);
        Assert.Equal(result1.Count, AndNotCount(values1, values2));
        Assert.Equal(result2.Count, AndNotCount(values2, values3));
        Assert.Equal(result3.Count, AndNotCount(values1, values2, values3));
    }

    [Fact]
    public void TestIAndNot()
    {
        uint[] values1 = [1, 2, 3, 4, 5, 100, 1000];
        uint[] values2 = [1, 2, 3, 4, 5, 100, 1000];
        uint[] values3 = [3, 4, 5, 7, 100, 1020];

        using var result = Roaring32Bitmap.FromValues(values1);
        using var source1 = Roaring32Bitmap.FromValues(values2);
        using var source2 = Roaring32Bitmap.FromValues(values3);
        result.IAndNot(source1);
        result.IAndNot(source2);
        Assert.Equal(result.Count, AndNotCount(values1, values2, values3));
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

    [Fact]
    public void TestStats()
    {
        var bitmap = new Roaring32Bitmap();
        bitmap.AddMany([1, 2, 3, 4, 6, 7]);
        bitmap.AddMany([999991, 999992, 999993, 999994, 999996, 999997]);
        var stats = bitmap.GetStatistics();

        Assert.Equal(bitmap.Count, stats.Cardinality);
        Assert.Equal(2U, stats.ContainerCount);
        Assert.Equal(2U, stats.ArrayContainerCount);
        Assert.Equal(0U, stats.RunContainerCount);
        Assert.Equal(0U, stats.BitsetContainerCount);
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