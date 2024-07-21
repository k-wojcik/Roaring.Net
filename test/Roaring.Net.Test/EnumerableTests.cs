using Xunit;

namespace Roaring.Test;

public class EnumerableTests
{
    
    [Fact]
    public void Values_BitmapContainsValues_EnumeratesBitmap()
    {
        uint[] values = [1, 2, 3, 4, 5, 100, 1000];

        using var result = Roaring32Bitmap.FromValues(values);
        Assert.Equal(result.Values, values);
    }
    
    [Fact]
    public void Values_BitmapIsEmpty_BreaksEnumeration()
    {
        uint[] values = [];

        using var result = Roaring32Bitmap.FromValues(values);
        Assert.Empty(result.Values);
    }
}