using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Roaring.Test.Roaring32;

public class Roaring32BitmapTests
{
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
}