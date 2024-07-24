using System;
using System.Linq;
using Xunit;

namespace Roaring.Test.Roaring32;

public class ContainsTests
{
    [Fact]
    public void Contains_EmptyBitmap_ReturnFalse()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetEmpty();
        
        // Act
        var actual = testObject.Bitmap.Contains(10);
        
        // Assert
        Assert.False(actual);
    }
    
    [Fact]
    public void Contains_BitmapHasValue_ReturnsTrue()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetDefault();
        var value = testObject.Values.First();
        
        // Act
        var actual = testObject.Bitmap.Contains(value);
        
        // Assert
        Assert.True(actual);
    }
    
    [Fact]
    public void Contains_BitmapDoesNotHaveValue_ReturnsFalse()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetDefault();
        Assert.DoesNotContain(testObject.Bitmap.Values, value=> value == 10U);
        
        // Act
        var actual = testObject.Bitmap.Contains(10U);
        
        // Assert
        Assert.False(actual);
    }
    
    
    [Theory]
    [InlineData(1, 0)]
    [InlineData(10, 5)]
    public void ContainsRange_ArgumentsOutOfAllowedRange_ThrowsArgumentOutOfRangeException(uint min, uint max)
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetEmpty();
        
        // Act && Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => testObject.Bitmap.ContainsRange(min, max));
    }
    
    [Theory]
    [InlineData(0, 0, 0, 0, true)]
    [InlineData(0, 1, 0, 0, true)]
    [InlineData(0, 100, 0, 0, true)]
    [InlineData(0,100, 1, 1, true)]
    [InlineData(0, 100, 0, 10, true)]
    [InlineData(200, 300, 0, 10, false)]
    [InlineData(200, 300, 150, 250, false)]
    [InlineData(200, 300, 250, 350, false)]
    [InlineData(200, 300, 100, 350, false)]
    [InlineData(uint.MaxValue - 100,uint.MaxValue, uint.MaxValue - 1, uint.MaxValue, true)]
    [InlineData(uint.MaxValue - 100,uint.MaxValue, uint.MaxValue, uint.MaxValue, true)]
    public void ContainsRange_CorrectRange_ReturnsExpectedResult(uint minTest, uint maxTest, uint min, uint max, bool expected)
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetForRange(minTest, maxTest);
        
        // Act
        var actual = testObject.Bitmap.ContainsRange(min, max);

        // Assert
        Assert.Equal(expected, actual);
    }
}