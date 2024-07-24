using System;
using System.Linq;
using Xunit;

namespace Roaring.Test.Roaring32;

public class RemoveTests
{
    [Fact]
    public void Remove_BitmapIsEmpty_DoesNotRemoveValue()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetEmpty();
        
        // Act
        testObject.Bitmap.Remove(uint.MaxValue);
        
        // Assert
        Assert.Empty(testObject.Bitmap.Values);
    }
    
    [Fact]
    public void Remove_BitmapWithValue_RemovesValueFromBitmap()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetDefault();
        var removedValue = testObject.Values.First();
        
        // Act
        testObject.Bitmap.Remove(removedValue);
        
        // Assert
        Assert.Equal((uint)(testObject.Values.Length - 1), testObject.Bitmap.Cardinality);
    }
    
    [Fact]
    public void RemoveMany_BitmapIsEmpty_DoesNotRemoveAnyValue()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetEmpty();
        
        // Act
        testObject.Bitmap.RemoveMany([1, 10, uint.MaxValue]);
        
        // Assert
        Assert.Empty(testObject.Bitmap.Values);
    }
    
    [Fact]
    public void RemoveMany_BitmapWithValues_RemovesValuesFromBitmap()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetDefault();
        testObject.Bitmap.Add(uint.MaxValue);
        var removedValues = testObject.Values.Take(10).Append(uint.MaxValue).ToArray();
        
        // Act
        testObject.Bitmap.RemoveMany(removedValues);
        
        // Assert
        Assert.Equal((uint)(testObject.Values.Length - removedValues.Length + 1), testObject.Bitmap.Cardinality);
    }
    
    [Theory]
    [InlineData(new uint[] {0, 1, 2, 3, 4}, 0, 5)]
    [InlineData(new uint[] {0, 1, 2, 3, 4},4, 1)]
    [InlineData(new uint[] {0, 1, 2, 3, 4},3, 2)]
    [InlineData(new uint[] {0, 1, 2, 3, 4},1, 2)]
    [InlineData(new uint[] {0, 1, 2, 3, 4},0, 0)]
    [InlineData(new uint[] {1, 2, uint.MaxValue},0, 3)]
    public void RemoveMany_WithCorrectOffsetAndCount_RemovesValuesFromBitmap(uint[] values, uint offset, uint count)
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetFromValues(values);
        
        // Act
        testObject.Bitmap.RemoveMany(values, offset, count);

        // Assert
        Assert.Equal(values.Except(values.Skip((int)offset).Take((int)count)).ToArray(), testObject.Bitmap.Values.ToArray());
    }
    
    [Theory]
    [InlineData(new uint[] {0, 1, 2, 3, 4},0, 6)]
    [InlineData(new uint[] {0, 1, 2, 3, 4},5, 1)]
    [InlineData(new uint[] {0, 1, 2, 3, 4},4, 2)]
    public void RemoveMany_OffsetAndCountOutOfAllowedRange_ThrowsArgumentOutOfRangeException(uint[] values, uint offset, uint count)
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetFromValues(values);
        
        // Act && Assert
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            testObject.Bitmap.RemoveMany(values, offset, count);
        });
    }
    
    [Fact]
    public void TryRemove_BitmapIsEmpty_DoesNotRemoveValueAndReturnsFalse()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetEmpty();
        
        // Act
        var actual = testObject.Bitmap.TryRemove(uint.MaxValue);
        
        // Assert
        Assert.False(actual);
        Assert.Empty(testObject.Bitmap.Values);
    }

    [Fact]
    public void Remove_BitmapWithValue_RemovesValueFromBitmapAndReturnsTrue()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetDefault();
        var removedValue = testObject.Values.First();

        // Act
        var actual = testObject.Bitmap.TryRemove(removedValue);

        // Assert
        Assert.True(actual);
        Assert.Equal((uint)(testObject.Values.Length - 1), testObject.Bitmap.Cardinality);
    }
    
    [Theory]
    [InlineData(1, 0)]
    [InlineData(10, 5)]
    public void RemoveRange_ArgumentsOutOfAllowedRange_ThrowsArgumentOutOfRangeException(uint min, uint max)
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetDefault();
        
        // Act && Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => testObject.Bitmap.RemoveRange(min, max));
    }
    
    [Theory]
    [InlineData(0, 100, 0, 0)]
    [InlineData(0,100, 1, 1)]
    [InlineData(0, 100, 0, 10)]
    [InlineData(200, 300, 0, 10)]
    [InlineData(200, 300, 150, 250)]
    [InlineData(200, 300, 250, 350)]
    [InlineData(200, 300, 100, 350)]
    [InlineData(uint.MaxValue - 100,uint.MaxValue, uint.MaxValue - 1, uint.MaxValue)]
    [InlineData(uint.MaxValue - 100,uint.MaxValue, uint.MaxValue, uint.MaxValue)]
    public void RemoveRange_CorrectRange_BitmapRemovesRange(uint minTest, uint maxTest, uint min, uint max)
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetForRange(minTest, maxTest);
        
        // Act
        testObject.Bitmap.RemoveRange(min, max);

        // Assert
        var removedValues = Enumerable.Range((int)min, (int)(max - min + 1)) // 0..10
            .Select(x=> (uint)x)
            .ToList();
        var expected = testObject.Values.Except(removedValues);
        var actual = testObject.Bitmap.Values.ToList();
        
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void Clear_BitmapIsEmpty_DoesNotRemoveValues()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetEmpty();
        
        // Act
        testObject.Bitmap.Clear();
        
        // Assert
        Assert.Empty(testObject.Bitmap.Values);
    }
    
    [Fact]
    public void Clear_BitmapHasValues_RemovesAllValuesFromBitmap()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetDefault();
        
        // Act
        testObject.Bitmap.Clear();
        
        // Assert
        Assert.Empty(testObject.Bitmap.Values);
    }
}