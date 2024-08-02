using System;
using Xunit;

namespace Roaring.Test.Roaring32;

public class CompareTests
{
    public class ValueEquals
    {
        [Fact]
        public void ValueEquals_SameBitmap_ReturnsTrue()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetEmpty();

            // Act
            var actual = testObject.Bitmap.ValueEquals(testObject.Bitmap);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void ValueEquals_BitmapsHaveSameValues_ReturnsTrue()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues([0, 10]);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues([0, 10]);

            // Act
            var actual = testObject1.Bitmap.ValueEquals(testObject2.Bitmap);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void ValueEquals_BitmapsHaveDifferentValues_ReturnsFalse()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues([0, 10]);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues([0, 10, 20]);

            // Act
            var actual = testObject1.Bitmap.ValueEquals(testObject2.Bitmap);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void ValueEquals_BitmapIsNull_ReturnsFalse()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetEmpty();

            // Act
            var actual = testObject1.Bitmap.ValueEquals(null);

            // Assert
            Assert.False(actual);
        }
    }

    public class IsSubsetOf
    {
        [Fact]
        public void IsSubsetOf_BitmapIsNull_ReturnsFalse()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetEmpty();

            // Act
            var actual = testObject.Bitmap.IsSubsetOf(null);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void IsSubsetOf_SameBitmap_ReturnsTrue()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetEmpty();

            // Act
            var actual = testObject.Bitmap.IsSubsetOf(testObject.Bitmap);

            // Assert
            Assert.True(actual);
        }
        
        [Fact]
        public void IsSubsetOf_EmptyBitmaps_ReturnsFalse()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetEmpty();
            using var testObject2 = Roaring32BitmapTestObject.GetEmpty();

            // Act
            var actual = testObject1.Bitmap.IsSubsetOf(testObject2.Bitmap);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsSubsetOf_BitmapsHaveSameValues_ReturnsTrue()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues([0, 10]);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues([0, 10]);

            // Act
            var actual = testObject1.Bitmap.IsSubsetOf(testObject2.Bitmap);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsSubsetOf_SecondBitmapContainsAllValuesOfBitmap_ReturnsTrue()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues([0, 10]);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues([0, 10, 20]);
        
            // Act
            var actual = testObject1.Bitmap.IsSubsetOf(testObject2.Bitmap);
        
            // Assert
            Assert.True(actual);
        }
        
        [Fact]
        public void IsSubsetOf_SecondBitmapNotContainsSomeValuesOfBitmap_ReturnsFalse()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues([0, 10]);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues([0]);
        
            // Act
            var actual = testObject1.Bitmap.IsSubsetOf(testObject2.Bitmap);
        
            // Assert
            Assert.False(actual);
        }
    }

    public class IsProperSubsetOf
    {
        [Fact]
        public void IsProperSubsetOf_BitmapIsNull_ReturnsFalse()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetEmpty();

            // Act
            var actual = testObject.Bitmap.IsProperSubsetOf(null);

            // Assert
            Assert.False(actual);
        }
        
        [Fact]
        public void IsProperSubsetOf_SameBitmap_ReturnsFalse()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetFromValues([1]);

            // Act
            var actual = testObject.Bitmap.IsProperSubsetOf(testObject.Bitmap);

            // Assert
            Assert.False(actual);
        }
        
        [Fact]
        public void IsProperSubsetOf_EmptyBitmaps_ReturnsFalse()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetEmpty();
            using var testObject2 = Roaring32BitmapTestObject.GetEmpty();

            // Act
            var actual = testObject1.Bitmap.IsProperSubsetOf(testObject2.Bitmap);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void IsProperSubsetOf_BitmapsHaveSameValues_ReturnsFalse()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues([0, 10]);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues([0, 10]);

            // Act
            var actual = testObject1.Bitmap.IsProperSubsetOf(testObject2.Bitmap);

            // Assert
            Assert.False(actual);
        }
    
        [Fact]
        public void IsProperSubsetOf_SecondBitmapContainsAllValuesOfBitmap_ReturnsTrue()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues([0, 10]);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues([0, 10, 20]);
        
            // Act
            var actual = testObject1.Bitmap.IsProperSubsetOf(testObject2.Bitmap);
        
            // Assert
            Assert.True(actual);
        }
    
        [Fact]
        public void IsProperSubsetOf_SecondBitmapNotContainsSomeValuesOfBitmap_ReturnsFalse()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues([0, 10]);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues([0]);
        
            // Act
            var actual = testObject1.Bitmap.IsProperSubsetOf(testObject2.Bitmap);
        
            // Assert
            Assert.False(actual);
        }
    }
    
    public class IsSupersetOf
    {
        [Fact]
        public void IsSupersetOf_BitmapIsNull_ReturnsFalse()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetEmpty();

            // Act
            var actual = testObject.Bitmap.IsSupersetOf(null);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void IsSupersetOf_SameBitmap_ReturnsTrue()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetEmpty();

            // Act
            var actual = testObject.Bitmap.IsSupersetOf(testObject.Bitmap);

            // Assert
            Assert.True(actual);
        }
        
        [Fact]
        public void IsSupersetOf_EmptyBitmaps_ReturnsTrue()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetEmpty();
            using var testObject2 = Roaring32BitmapTestObject.GetEmpty();

            // Act
            var actual = testObject1.Bitmap.IsSupersetOf(testObject2.Bitmap);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsSupersetOf_BitmapsHaveSameValues_ReturnsTrue()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues([0, 10]);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues([0, 10]);

            // Act
            var actual = testObject1.Bitmap.IsSubsetOf(testObject2.Bitmap);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsSupersetOf_BitmapContainsAllValuesOfSecondBitmap_ReturnsTrue()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues([0, 10, 20 ]);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues([0, 10]);
        
            // Act
            var actual = testObject1.Bitmap.IsSupersetOf(testObject2.Bitmap);
        
            // Assert
            Assert.True(actual);
        }
        
        [Fact]
        public void IsSupersetOf_BitmapNotContainsSomeValuesOfSecondBitmap_ReturnsFalse()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues([0, 10]);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues([0, 10, 20]);
        
            // Act
            var actual = testObject1.Bitmap.IsSupersetOf(testObject2.Bitmap);
        
            // Assert
            Assert.False(actual);
        }
    }
    
    public class IsProperSupersetOf
    {
        [Fact]
        public void IsProperSupersetOf_BitmapIsNull_ReturnsFalse()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetEmpty();

            // Act
            var actual = testObject.Bitmap.IsProperSupersetOf(null);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void IsProperSupersetOf_SameBitmap_ReturnsFalse()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetEmpty();

            // Act
            var actual = testObject.Bitmap.IsProperSupersetOf(testObject.Bitmap);

            // Assert
            Assert.False(actual);
        }
        
        [Fact]
        public void IsProperSupersetOf_EmptyBitmaps_ReturnsTrue()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetEmpty();
            using var testObject2 = Roaring32BitmapTestObject.GetEmpty();

            // Act
            var actual = testObject1.Bitmap.IsProperSupersetOf(testObject2.Bitmap);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void IsProperSupersetOf_BitmapsHaveSameValues_ReturnsFalse()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues([0, 10]);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues([0, 10]);

            // Act
            var actual = testObject1.Bitmap.IsProperSupersetOf(testObject2.Bitmap);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void IsProperSupersetOf_BitmapContainsAllValuesOfSecondBitmap_ReturnsTrue()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues([0, 10, 20]);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues([0, 10]);
        
            // Act
            var actual = testObject1.Bitmap.IsProperSupersetOf(testObject2.Bitmap);
        
            // Assert
            Assert.True(actual);
        }
        
        [Fact]
        public void IsProperSupersetOf_BitmapNotContainsSomeValuesOfSecondBitmap_ReturnsFalse()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues([0, 10]);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues([0, 10, 20]);
        
            // Act
            var actual = testObject1.Bitmap.IsProperSupersetOf(testObject2.Bitmap);
        
            // Assert
            Assert.False(actual);
        }
    }
    
    public class Overlaps
    {
        [Fact]
        public void Overlaps_IntersectsWithAtLeastOneValue_ReturnsTrue()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues([0, 10, uint.MaxValue, 1]);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues([5, 10, 11]);
        
            // Act
            var actual = testObject1.Bitmap.Overlaps(testObject2.Bitmap);
        
            // Assert
            Assert.True(actual);
        }
        
        [Fact]
        public void Overlaps_NoValuesIntersects_ReturnsFalse()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues([0, 10, uint.MaxValue, 1]);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues([5, 12, 11]);
        
            // Act
            var actual = testObject1.Bitmap.Overlaps(testObject2.Bitmap);
        
            // Assert
            Assert.False(actual);
        }
    }
      
    public class OverlapsRange
    {
        [Theory]
        [InlineData(1, 0)]
        [InlineData(10, 5)]
        public void OverlapsRange_ArgumentsOutOfAllowedRange_ThrowsArgumentOutOfRangeException(uint start, uint end)
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetEmpty();
            
            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => testObject.Bitmap.OverlapsRange(start, end));
        }
        
        [Theory]
        [InlineData(new uint[] {0, 10, uint.MaxValue}, 0, 0)]
        [InlineData(new uint[] {0, 10, uint.MaxValue}, 0, uint.MaxValue)]
        [InlineData(new uint[] {0, 10, uint.MaxValue}, 0, 9)]
        [InlineData(new uint[] {0, 10, uint.MaxValue}, 1, 10)]
        [InlineData(new uint[] {0, 10, uint.MaxValue - 1}, uint.MaxValue - 1, uint.MaxValue)]
        [InlineData(new uint[] {0, 10, uint.MaxValue}, uint.MaxValue - 1, uint.MaxValue)]
        [InlineData(new uint[] {0, 10, uint.MaxValue}, uint.MaxValue, uint.MaxValue)]
        public void OverlapsRange_IntersectsWithAtLeastOneValue_ReturnsTrue(uint[] values, uint start, uint end)
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetFromValues(values);
        
            // Act
            var actual = testObject.Bitmap.OverlapsRange(start, end);
        
            // Assert
            Assert.True(actual);
        }
        
        [Theory]
        [InlineData(new uint[] { 0, 10, uint.MaxValue}, 1, 9)]
        [InlineData(new uint[] { 5, 10, uint.MaxValue}, 0, 4)]
        [InlineData(new uint[] { 0, 10, uint.MaxValue}, 11, uint.MaxValue - 1)]
        [InlineData(new uint[] { 0, 10, uint.MaxValue - 1}, uint.MaxValue, uint.MaxValue)]
        public void OverlapsRange_NoValuesIntersects_ReturnsFalse(uint[] values, uint start, uint end)
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetFromValues(values);
        
            // Act
            var actual = testObject.Bitmap.OverlapsRange(start, end);
        
            // Assert
            Assert.False(actual);
        }
    }
}