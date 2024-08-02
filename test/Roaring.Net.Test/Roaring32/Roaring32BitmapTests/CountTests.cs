using System;
using Xunit;

namespace Roaring.Test.Roaring32;

public class CountTests
{
    public class Count
    {
        [Fact]
        public void Count_BitmapContainsValues_ReturnsCardinalityOfValues()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetDefault();

            // Act
            var actual = testObject.Bitmap.Count;

            // Assert
            Assert.Equal((ulong)testObject.Values.Length, actual);
        }

        [Fact]
        public void Count_EmptyBitmap_ReturnsZero()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetEmpty();

            // Act
            var actual = testObject.Bitmap.Count;

            // Assert
            Assert.Equal(0U, actual);
        }
    }
    
    public class CountLessOrEqualTo
    {
        [Theory]
        [InlineData(new uint[] { 0, 1, 2, 3, 4 }, 0, 1)]
        [InlineData(new uint[] { 0, 1, 2, 3, 4 }, 4, 5)]
        [InlineData(new uint[] { 4, 3, 2, 1, 0 }, 4, 5)]
        [InlineData(new uint[] { 4, 3, 2, 1, 0 }, 5, 5)]
        [InlineData(new uint[] { 0, 2, 4, 6, 8 }, 2, 2)]
        [InlineData(new uint[] { 5, 6, 7, 8, 9 }, 2, 0)]
        [InlineData(new uint[] { 5, 6, 7, 8, 9 }, 7, 3)]
        [InlineData(new uint[] { 5, uint.MaxValue }, uint.MaxValue, 2)]
        public void CountLessOrEqualTo_ForValues_ReturnsExpectedNumberOfValues(uint[] values, uint testedValue, uint expected)
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetFromValues(values);
        
            // Act
            var actual = testObject.Bitmap.CountLessOrEqualTo(testedValue);
        
            // Assert
            Assert.Equal(expected, actual);
        }
    }
    
    public class CountRange
    {
        [Theory]
        [InlineData(1, 0)]
        [InlineData(10, 5)]
        public void CountRange_ArgumentsOutOfAllowedRange_ThrowsArgumentOutOfRangeException(uint start, uint end)
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetEmpty();
            
            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => testObject.Bitmap.CountRange(start, end));
        }
        
        [Theory]
        [InlineData(new uint[] { 0, 1, 2, 3, 4 }, 0, 0, 1)]
        [InlineData(new uint[] { 0, 1, 2, 3, 4 }, 0, 5, 5)]
        [InlineData(new uint[] { 0, 2, 4, 6, 8 }, 2, 4, 2)]
        [InlineData(new uint[] { 0, 2, 4, 6, 8 }, 6, 10, 2)]
        [InlineData(new uint[] { 0, 2, 4, 6, 8 }, 7, 8, 1)]
        [InlineData(new uint[] { 0, 1, 2, 3, 4 }, 4, uint.MaxValue, 1)]
        [InlineData(new uint[] { 0, 1, 2, 3, 4, uint.MaxValue }, uint.MinValue, uint.MaxValue, 6)]
        [InlineData(new uint[] { uint.MaxValue - 1, uint.MaxValue }, uint.MinValue, uint.MaxValue, 2)]
        public void CountRange_ForValues_ReturnsExpectedNumberOfValues(uint[] values, uint start, uint end, uint expected)
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetFromValues(values);
        
            // Act
            var actual = testObject.Bitmap.CountRange(start, end);
        
            // Assert
            Assert.Equal(expected, actual);
        }
    }
}