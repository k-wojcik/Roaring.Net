using Xunit;

namespace Roaring.Test.Roaring32;

public class CollectionTests
{
    [Theory]
    [InlineData(new uint[]{})]
    [InlineData(new uint[]{uint.MaxValue})]
    [InlineData(new uint[] { 0, 1, 2, 3, 4 })]
    public void ToArray_Always_ReturnsArrayWithExpectedValues(uint[] expected)
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetFromValues(expected);
        
        // Act
        var actual = testObject.Bitmap.ToArray();
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Theory]
    [InlineData(20, new uint[]{}, new uint[]{})]
    [InlineData(20, new uint[] { 0, 1, 2, 3, 4 }, new uint[] { 0, 1, 2, 3, 4 })]
    [InlineData(2, new uint[] { 0, 1, 2, 3, 4 }, new uint[] { 0 ,1 })]
    [InlineData(0, new uint[] { 0, 1, 2, 3, 4 }, new uint[] {  })]
    [InlineData(1, new uint[] { uint.MaxValue }, new uint[] { uint.MaxValue  })]
    public void Take_ForCount_ReturnsForValuesLimitedToCount(uint count, uint[] values, uint[] expected)
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetFromValues(values);
        
        // Act
        var actual = testObject.Bitmap.Take(count);
        
        // Assert
        Assert.Equal(expected, actual);
    }
}