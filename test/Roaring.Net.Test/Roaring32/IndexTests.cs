﻿using Xunit;

namespace Roaring.Test.Roaring32;

public class IndexTests
{
    [Theory]
    [InlineData(new uint[] { 0, 1, 2, 3, 4 }, 0, 0)]
    [InlineData(new uint[] { 0, 1, 2, 3, 4 }, 4, 4)]
    [InlineData(new uint[] { 4, 3, 2, 1, 0 }, 4, 4)]
    [InlineData(new uint[] { 0, 2, 4, 6, 8 }, 2, 4)]
    public void TryGetValue_IndexLessThanBitmapSize_ReturnsTrueAndExpectedValue(uint[] values, uint index, uint excepted)
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetFromValues(values);
        
        // Act
        var actualReturn = testObject.Bitmap.TryGetValue(index, out uint actual);
        
        // Assert
        Assert.True(actualReturn);
        Assert.Equal(excepted, actual);
    }
    
    [Theory]
    [InlineData(new uint[]{}, 1, 0)]
    [InlineData(new uint[] { 0, 1, 2, 3, 4 }, 5, 0)]
    public void TryGetValue_IndexGreaterThanBitmapSize_ReturnsFalseAndZero(uint[] values, uint index, uint excepted)
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetFromValues(values);
        
        // Act
        var actualReturn = testObject.Bitmap.TryGetValue(index, out uint actual);
        
        // Assert
        Assert.False(actualReturn);
        Assert.Equal(excepted, actual);
    }
    
    [Theory]
    [InlineData(new uint[] { 0, 1, 2, 3, 4 }, 0, 1)]
    [InlineData(new uint[] { 0, 1, 2, 3, 4 }, 4, 5)]
    [InlineData(new uint[] { 4, 3, 2, 1, 0 }, 4, 5)]
    [InlineData(new uint[] { 4, 3, 2, 1, 0 }, 5, 5)]
    [InlineData(new uint[] { 0, 2, 4, 6, 8 }, 2, 2)]
    [InlineData(new uint[] { 5, 6, 7, 8, 9 }, 2, 0)]
    [InlineData(new uint[] { 5, 6, 7, 8, 9 }, 7, 3)]
    [InlineData(new uint[] { 5, uint.MaxValue }, uint.MaxValue, 2)]
    public void CountLessOrEqualTo_ForValues_ReturnsExpectedNumberOfValues(uint[] values, uint testedValue, uint excepted)
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetFromValues(values);
        
        // Act
        var actual = testObject.Bitmap.CountLessOrEqualTo(testedValue);
        
        // Assert
        Assert.Equal(excepted, actual);
    }
    
    [Theory]
    [InlineData(new uint[] { 0, 1, 2, 3, 4 }, 0, 0)]
    [InlineData(new uint[] { 0, 1, 2, 3, 4 }, 4, 4)]
    [InlineData(new uint[] { 4, 3, 2, 1, 0 }, 4, 4)]
    [InlineData(new uint[] { 4, 3, 2, 1, 0 }, 5, -1)]
    [InlineData(new uint[] { 0, 2, 4, 6, 8 }, 2, 1)]
    [InlineData(new uint[] { 5, 6, 7, 8, 9 }, 2, -1)]
    [InlineData(new uint[] { 5, 6, 7, 8, 9 }, 7, 2)]
    [InlineData(new uint[] { 5, uint.MaxValue }, uint.MaxValue, 1)]
    public void GetIndex_ForValues_ReturnsIndexOfValue(uint[] values, uint testedValue, long excepted)
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetFromValues(values);
        
        // Act
        var actual = testObject.Bitmap.GetIndex(testedValue);
        
        // Assert
        Assert.Equal(excepted, actual);
    }
}