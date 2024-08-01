using System;
using Xunit;

namespace Roaring.Test.Roaring32;

public class NotTests
{
    public class Not_ForWholeBitmap
    {
        [Fact]
        public void Not_EmptyBitmap_NegatesValuesInBitmap()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetEmpty();
            
            // Act
            using var actual = testObject.Bitmap.Not();
            
            // Assert
            Assert.All([uint.MinValue, 1U, 2U, 1000U, uint.MaxValue], value =>
            {
                Assert.True(actual.Contains(value));
            });
            Assert.Equal(0U, testObject.Bitmap.And(actual).Count);
            Assert.Equal(uint.MaxValue + 1UL, actual.Count);
        }
        
        [Fact]
        public void Not_NotEmptyBitmap_NegatesValuesInBitmap()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetFromValues([0, 3, uint.MaxValue]);
            
            // Act
            using var actual = testObject.Bitmap.Not();
            
            // Assert
            Assert.All(testObject.Values, value =>
            {
                Assert.False(actual.Contains(value));
            });
            
            Assert.All([1U, 2U, 1000U], value =>
            {
                Assert.True(actual.Contains(value));
            });
            Assert.Equal(0U, testObject.Bitmap.And(actual).Count);
            Assert.Equal(uint.MaxValue - 2UL, actual.Count);
        }
    }
    
    public class INot_ForWholeBitmap
    {
        [Fact]
        public void INot_EmptyBitmap_NegatesValuesInBitmap()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetEmpty();
            
            // Act
            testObject.Bitmap.INot();
            
            // Assert
            Assert.All([uint.MinValue, 1U, 2U, 1000U, uint.MaxValue], value =>
            {
                Assert.True(testObject.Bitmap.Contains(value));
            });
            Assert.Equal(uint.MaxValue + 1UL, testObject.Bitmap.Count);
        }
        
        [Fact]
        public void INot_NotEmptyBitmap_NegatesValuesInBitmap()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetFromValues([0, 3, uint.MaxValue]);
            
            // Act
            testObject.Bitmap.INot();
            
            // Assert
            Assert.All(testObject.Values, value =>
            {
                Assert.False(testObject.Bitmap.Contains(value));
            });
            Assert.All([1U, 2U, 1000U], value =>
            {
                Assert.True(testObject.Bitmap.Contains(value));
            });
            Assert.Equal(uint.MaxValue - 2UL, testObject.Bitmap.Count);
        }
    }
    
    public class Not_ForRange
    {
        [Theory]
        [InlineData(1, 0)]
        [InlineData(10, 5)]
        public void Not_ArgumentsOutOfAllowedRange_ThrowsArgumentOutOfRangeException(uint start, uint end)
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetEmpty();

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => testObject.Bitmap.Not(start, end));
        }
        
        [Fact]
        public void Not_EmptyBitmap_NegatesValuesInBitmap()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetEmpty();
            
            // Act
            using var actual = testObject.Bitmap.Not(0, 3);
            
            // Assert
            Assert.All([0U, 1U, 2U, 3U], value =>
            {
                Assert.True(actual.Contains(value));
            });
            Assert.All([4U, uint.MaxValue], value =>
            {
                Assert.False(actual.Contains(value));
            });
            Assert.Equal(4UL, actual.Count);
        }
        
        [Fact]
        public void Not_NotEmptyBitmap_NegatesValuesInBitmap()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetFromValues([0, 3, uint.MaxValue]);
            
            // Act
            using var actual = testObject.Bitmap.Not(0, 4);
            
            // Assert
            Assert.All([1U, 2U, 4U, uint.MaxValue], value =>
            {
                Assert.True(actual.Contains(value));
            });
            Assert.All([0U, 3U], value =>
            {
                Assert.False(actual.Contains(value));
            });
            Assert.Equal(4UL, actual.Count);
        }
    }
    
    public class INot_ForRange
    {
        [Theory]
        [InlineData(1, 0)]
        [InlineData(10, 5)]
        public void INot_ArgumentsOutOfAllowedRange_ThrowsArgumentOutOfRangeException(uint start, uint end)
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetEmpty();

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => testObject.Bitmap.INot(start, end));
        }
        
        [Fact]
        public void INot_EmptyBitmap_NegatesValuesInBitmap()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetEmpty();
            
            // Act
            testObject.Bitmap.INot(0, 3);
            
            // Assert
            Assert.All([0U, 1U, 2U, 3U], value =>
            {
                Assert.True(testObject.Bitmap.Contains(value));
            });
            Assert.All([4U, uint.MaxValue], value =>
            {
                Assert.False(testObject.Bitmap.Contains(value));
            });
            Assert.Equal(4UL, testObject.Bitmap.Count);
        }
        
        [Fact]
        public void INot_NotEmptyBitmap_NegatesValuesInBitmap()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetFromValues([0, 3, uint.MaxValue]);
            
            // Act
            testObject.Bitmap.INot(0, 4);
            
            // Assert
            Assert.All([1U, 2U, 4U, uint.MaxValue], value =>
            {
                Assert.True(testObject.Bitmap.Contains(value));
            });
            Assert.All([0U, 3U], value =>
            {
                Assert.False(testObject.Bitmap.Contains(value));
            });
            Assert.Equal(4UL, testObject.Bitmap.Count);
        }
    }
}