using System;
using System.Linq;
using Xunit;

namespace Roaring.Test.Roaring32;

public class CreationTests
{
    [Fact]
    public void Ctor_Default_CreatesBitmapWithZeroCapacity()
    {
        // Act
        using var uut = new Roaring32Bitmap();

        // Assert
        var actual = uut.SerializedBytes;
        Assert.True(actual > 0);
    }

    [Fact]
    public void Ctor_WithCapacity_CreatesBitmapWithPriviedCapacity()
    {
        // Act
        using var uut = new Roaring32Bitmap(1000);

        // Assert
        var actual = uut.SerializedBytes;
        Assert.True(actual > 0);
    }
    
    [Theory]
    [InlineData(0, 0, 1)]
    [InlineData(1, 0, 1)]
    [InlineData(1, 10, 0)]
    public void FromRange_ArgumentsOutOfAllowedRange_ThrowsArgumentOutOfRangeException(uint min, uint max, uint step)
    {
        // Act && Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => Roaring32Bitmap.FromRange(min, max, step));
    }
    
    [Fact]
    public void FromRange_CorrectRange_BitmapContainsExpectedValues()
    {
        // Act
        using var uut = Roaring32Bitmap.FromRange(10, 100, 2);

        // Assert
        var expected = Enumerable.Range(0, 45)
            .Select(x=> 10 + (uint)x * 2)
            .ToList();
        var actual = uut.Values.ToList();
        
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void FromRange_StepGreaterThanRange_BitmapContainsMinValueOfRange()
    {
        // Act
        using var uut = Roaring32Bitmap.FromRange(1, 10, 100);

        // Assert
        var actual = uut.Values;
        
        Assert.Equal([1], actual);
    }
    
    [Fact]
    public void FromValues_InputHasValues_BitmapContainsExpectedValues()
    {
        // Arrange
        var expected = Enumerable.Range(0, 100)
            .Select(x=> (uint)x)
            .ToArray();
        
        // Act
        using var uut = Roaring32Bitmap.FromValues(expected);

        // Assert
        var actual = uut.Values;
        
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void FromValues_InputIsEmpty_BitmapIsEmpty()
    {
        // Act
        using var uut = Roaring32Bitmap.FromValues([]);

        // Assert
        var actual = uut.Values;
        
        Assert.Empty(actual);
    }
    
    [Theory]
    [InlineData(0, 100)]
    [InlineData(99, 1)]
    [InlineData(90, 10)]
    [InlineData(10, 25)]
    [InlineData(0, 0)]
    public void FromValues_WithCorrectOffsetAndCount_BitmapContainsExpectedValues(uint offset, uint count)
    {
        // Arrange
        var values = Enumerable.Range(0, 100)
            .Select(x=> (uint)x)
            .ToArray();
        
        // Act
        using var uut = Roaring32Bitmap.FromValues(values, offset, count);

        // Assert
        var actual = uut.Values;
        var expected = values.Skip((int)offset).Take((int)count);
        
        Assert.Equal(expected, actual);
    }
    
    [Theory]
    [InlineData(0, 101)]
    [InlineData(100, 1)]
    [InlineData(90, 11)]
    public void FromValues_OffsetAndCountOutOfAllowedRange_ThrowsArgumentOutOfRangeException(uint offset, uint count)
    {
        // Arrange
        var values = Enumerable.Range(0, 100)
            .Select(x=> (uint)x)
            .ToArray();
        
        // Act && Assert
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            using var uut = Roaring32Bitmap.FromValues(values, offset, count);
        });
    }
    
    [Fact]
    public void Clone_Always_CreatesNewInstanceOfBitmap()
    {
        // Act
        using var testObject = Roaring32BitmapTestObject.GetDefault();

        // Act
        var actual = testObject.Bitmap.Clone();
        
        // Assert
        Assert.NotEqual(testObject.Bitmap, actual);
        Assert.Equal(testObject.Bitmap.Values, actual.Values);
    }
}