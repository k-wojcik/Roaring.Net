using System;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring32BitmapTests;

public class CompareTests
{
    public class ValueEquals
    {
        [Theory]
        [InlineTestObject]
        public void ValueEquals_SameBitmap_ReturnsTrue(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetEmpty();

            // Act
            var actual = testObject.ReadOnlyBitmap.ValueEquals(testObject.Bitmap);

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineMatrixTestObject]
        public void ValueEquals_BitmapsHaveSameValues_ReturnsTrue(TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using var testObject1 = matrix.X.GetFromValues([0, 10]);
            using var testObject2 = matrix.Y.GetFromValues([0, 10]);

            // Act
            var actual = testObject1.ReadOnlyBitmap.ValueEquals(testObject2.Bitmap);

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineMatrixTestObject]
        public void ValueEquals_BitmapsHaveDifferentValues_ReturnsFalse(TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using var testObject1 = matrix.X.GetFromValues([0, 10]);
            using var testObject2 = matrix.Y.GetFromValues([0, 10, 20]);

            // Act
            var actual = testObject1.ReadOnlyBitmap.ValueEquals(testObject2.Bitmap);

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineTestObject]
        public void ValueEquals_BitmapIsNull_ReturnsFalse(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject1 = factory.GetEmpty();

            // Act
            var actual = testObject1.ReadOnlyBitmap.ValueEquals(null);

            // Assert
            Assert.False(actual);
        }
    }

    public class IsSubsetOf
    {
        [Theory]
        [InlineTestObject]
        public void IsSubsetOf_BitmapIsNull_ReturnsFalse(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetEmpty();

            // Act
            var actual = testObject.ReadOnlyBitmap.IsSubsetOf(null);

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineTestObject]
        public void IsSubsetOf_SameBitmap_ReturnsTrue(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetEmpty();

            // Act
            var actual = testObject.ReadOnlyBitmap.IsSubsetOf(testObject.Bitmap);

            // Assert
            Assert.True(actual);
        }
        
        [Theory]
        [InlineMatrixTestObject]
        public void IsSubsetOf_EmptyBitmaps_ReturnsFalse(TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using var testObject1 = matrix.X.GetEmpty();
            using var testObject2 = matrix.Y.GetEmpty();

            // Act
            var actual = testObject1.ReadOnlyBitmap.IsSubsetOf(testObject2.Bitmap);

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineMatrixTestObject]
        public void IsSubsetOf_BitmapsHaveSameValues_ReturnsTrue(TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using var testObject1 = matrix.X.GetFromValues([0, 10]);
            using var testObject2 = matrix.Y.GetFromValues([0, 10]);

            // Act
            var actual = testObject1.ReadOnlyBitmap.IsSubsetOf(testObject2.Bitmap);

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineMatrixTestObject]
        public void IsSubsetOf_SecondBitmapContainsAllValuesOfBitmap_ReturnsTrue(TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using var testObject1 = matrix.X.GetFromValues([0, 10]);
            using var testObject2 = matrix.Y.GetFromValues([0, 10, 20]);
        
            // Act
            var actual = testObject1.ReadOnlyBitmap.IsSubsetOf(testObject2.Bitmap);
        
            // Assert
            Assert.True(actual);
        }
        
        [Theory]
        [InlineMatrixTestObject]
        public void IsSubsetOf_SecondBitmapNotContainsSomeValuesOfBitmap_ReturnsFalse(TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using var testObject1 = matrix.X.GetFromValues([0, 10]);
            using var testObject2 = matrix.Y.GetFromValues([0]);
        
            // Act
            var actual = testObject1.ReadOnlyBitmap.IsSubsetOf(testObject2.Bitmap);
        
            // Assert
            Assert.False(actual);
        }
    }

    public class IsProperSubsetOf
    {
        [Theory]
        [InlineTestObject]
        public void IsProperSubsetOf_BitmapIsNull_ReturnsFalse(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetEmpty();

            // Act
            var actual = testObject.ReadOnlyBitmap.IsProperSubsetOf(null);

            // Assert
            Assert.False(actual);
        }
        
        [Theory]
        [InlineTestObject]
        public void IsProperSubsetOf_SameBitmap_ReturnsFalse(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetFromValues([1]);

            // Act
            var actual = testObject.ReadOnlyBitmap.IsProperSubsetOf(testObject.Bitmap);

            // Assert
            Assert.False(actual);
        }
        
        [Theory]
        [InlineMatrixTestObject]
        public void IsProperSubsetOf_EmptyBitmaps_ReturnsFalse(TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using var testObject1 = matrix.X.GetEmpty();
            using var testObject2 = matrix.Y.GetEmpty();

            // Act
            var actual = testObject1.ReadOnlyBitmap.IsProperSubsetOf(testObject2.Bitmap);

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineMatrixTestObject]
        public void IsProperSubsetOf_BitmapsHaveSameValues_ReturnsFalse(TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using var testObject1 = matrix.X.GetFromValues([0, 10]);
            using var testObject2 = matrix.Y.GetFromValues([0, 10]);

            // Act
            var actual = testObject1.ReadOnlyBitmap.IsProperSubsetOf(testObject2.Bitmap);

            // Assert
            Assert.False(actual);
        }
    
        [Theory]
        [InlineMatrixTestObject]
        public void IsProperSubsetOf_SecondBitmapContainsAllValuesOfBitmap_ReturnsTrue(TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using var testObject1 = matrix.X.GetFromValues([0, 10]);
            using var testObject2 = matrix.Y.GetFromValues([0, 10, 20]);
        
            // Act
            var actual = testObject1.ReadOnlyBitmap.IsProperSubsetOf(testObject2.Bitmap);
        
            // Assert
            Assert.True(actual);
        }
    
        [Theory]
        [InlineMatrixTestObject]
        public void IsProperSubsetOf_SecondBitmapNotContainsSomeValuesOfBitmap_ReturnsFalse(TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using var testObject1 = matrix.X.GetFromValues([0, 10]);
            using var testObject2 = matrix.Y.GetFromValues([0]);
        
            // Act
            var actual = testObject1.ReadOnlyBitmap.IsProperSubsetOf(testObject2.Bitmap);
        
            // Assert
            Assert.False(actual);
        }
    }
    
    public class IsSupersetOf
    {
        [Theory]
        [InlineTestObject]
        public void IsSupersetOf_BitmapIsNull_ReturnsFalse(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetEmpty();

            // Act
            var actual = testObject.ReadOnlyBitmap.IsSupersetOf(null);

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineTestObject]
        public void IsSupersetOf_SameBitmap_ReturnsTrue(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetEmpty();

            // Act
            var actual = testObject.ReadOnlyBitmap.IsSupersetOf(testObject.Bitmap);

            // Assert
            Assert.True(actual);
        }
        
        [Theory]
        [InlineMatrixTestObject]
        public void IsSupersetOf_EmptyBitmaps_ReturnsTrue(TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using var testObject1 = matrix.X.GetEmpty();
            using var testObject2 = matrix.Y.GetEmpty();

            // Act
            var actual = testObject1.ReadOnlyBitmap.IsSupersetOf(testObject2.Bitmap);

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineMatrixTestObject]
        public void IsSupersetOf_BitmapsHaveSameValues_ReturnsTrue(TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using var testObject1 = matrix.X.GetFromValues([0, 10]);
            using var testObject2 = matrix.Y.GetFromValues([0, 10]);

            // Act
            var actual = testObject1.ReadOnlyBitmap.IsSubsetOf(testObject2.Bitmap);

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineMatrixTestObject]
        public void IsSupersetOf_BitmapContainsAllValuesOfSecondBitmap_ReturnsTrue(TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using var testObject1 = matrix.X.GetFromValues([0, 10, 20 ]);
            using var testObject2 = matrix.Y.GetFromValues([0, 10]);
        
            // Act
            var actual = testObject1.ReadOnlyBitmap.IsSupersetOf(testObject2.Bitmap);
        
            // Assert
            Assert.True(actual);
        }
        
        [Theory]
        [InlineMatrixTestObject]
        public void IsSupersetOf_BitmapNotContainsSomeValuesOfSecondBitmap_ReturnsFalse(TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using var testObject1 = matrix.X.GetFromValues([0, 10]);
            using var testObject2 = matrix.Y.GetFromValues([0, 10, 20]);
        
            // Act
            var actual = testObject1.ReadOnlyBitmap.IsSupersetOf(testObject2.Bitmap);
        
            // Assert
            Assert.False(actual);
        }
    }
    
    public class IsProperSupersetOf
    {
        [Theory]
        [InlineTestObject]
        public void IsProperSupersetOf_BitmapIsNull_ReturnsFalse(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetEmpty();

            // Act
            var actual = testObject.ReadOnlyBitmap.IsProperSupersetOf(null);

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineTestObject]
        public void IsProperSupersetOf_SameBitmap_ReturnsFalse(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetEmpty();

            // Act
            var actual = testObject.ReadOnlyBitmap.IsProperSupersetOf(testObject.Bitmap);

            // Assert
            Assert.False(actual);
        }
        
        [Theory]
        [InlineMatrixTestObject]
        public void IsProperSupersetOf_EmptyBitmaps_ReturnsTrue(TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using var testObject1 = matrix.X.GetEmpty();
            using var testObject2 = matrix.Y.GetEmpty();

            // Act
            var actual = testObject1.ReadOnlyBitmap.IsProperSupersetOf(testObject2.Bitmap);

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineMatrixTestObject]
        public void IsProperSupersetOf_BitmapsHaveSameValues_ReturnsFalse(TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using var testObject1 = matrix.X.GetFromValues([0, 10]);
            using var testObject2 = matrix.Y.GetFromValues([0, 10]);

            // Act
            var actual = testObject1.ReadOnlyBitmap.IsProperSupersetOf(testObject2.Bitmap);

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineMatrixTestObject]
        public void IsProperSupersetOf_BitmapContainsAllValuesOfSecondBitmap_ReturnsTrue(TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using var testObject1 = matrix.X.GetFromValues([0, 10, 20]);
            using var testObject2 = matrix.Y.GetFromValues([0, 10]);
        
            // Act
            var actual = testObject1.ReadOnlyBitmap.IsProperSupersetOf(testObject2.Bitmap);
        
            // Assert
            Assert.True(actual);
        }
        
        [Theory]
        [InlineMatrixTestObject]
        public void IsProperSupersetOf_BitmapNotContainsSomeValuesOfSecondBitmap_ReturnsFalse(TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using var testObject1 = matrix.X.GetFromValues([0, 10]);
            using var testObject2 = matrix.Y.GetFromValues([0, 10, 20]);
        
            // Act
            var actual = testObject1.ReadOnlyBitmap.IsProperSupersetOf(testObject2.Bitmap);
        
            // Assert
            Assert.False(actual);
        }
    }
    
    public class Overlaps
    {
        [Theory]
        [InlineMatrixTestObject]
        public void Overlaps_IntersectsWithAtLeastOneValue_ReturnsTrue(TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using var testObject1 = matrix.X.GetFromValues([0, 10, uint.MaxValue, 1]);
            using var testObject2 = matrix.Y.GetFromValues([5, 10, 11]);
        
            // Act
            var actual = testObject1.ReadOnlyBitmap.Overlaps(testObject2.Bitmap);
        
            // Assert
            Assert.True(actual);
        }
        
        [Theory]
        [InlineMatrixTestObject]
        public void Overlaps_NoValuesIntersects_ReturnsFalse(TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using var testObject1 = matrix.X.GetFromValues([0, 10, uint.MaxValue, 1]);
            using var testObject2 = matrix.Y.GetFromValues([5, 12, 11]);
        
            // Act
            var actual = testObject1.ReadOnlyBitmap.Overlaps(testObject2.Bitmap);
        
            // Assert
            Assert.False(actual);
        }
    }
      
    public class OverlapsRange
    {
        [Theory]
        [InlineTestObject(1, 0)]
        [InlineTestObject(10, 5)]
        public void OverlapsRange_ArgumentsOutOfAllowedRange_ThrowsArgumentOutOfRangeException(uint start, uint end, IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetEmpty();
            
            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => testObject.ReadOnlyBitmap.OverlapsRange(start, end));
        }
        
        [Theory]
        [InlineTestObject(new uint[] {0, 10, uint.MaxValue}, 0, 0)]
        [InlineTestObject(new uint[] {0, 10, uint.MaxValue}, 0, uint.MaxValue)]
        [InlineTestObject(new uint[] {0, 10, uint.MaxValue}, 0, 9)]
        [InlineTestObject(new uint[] {0, 10, uint.MaxValue}, 1, 10)]
        [InlineTestObject(new uint[] {0, 10, uint.MaxValue - 1}, uint.MaxValue - 1, uint.MaxValue)]
        [InlineTestObject(new uint[] {0, 10, uint.MaxValue}, uint.MaxValue - 1, uint.MaxValue)]
        [InlineTestObject(new uint[] {0, 10, uint.MaxValue}, uint.MaxValue, uint.MaxValue)]
        public void OverlapsRange_IntersectsWithAtLeastOneValue_ReturnsTrue(uint[] values, uint start, uint end, IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetFromValues(values);
        
            // Act
            var actual = testObject.ReadOnlyBitmap.OverlapsRange(start, end);
        
            // Assert
            Assert.True(actual);
        }
        
        [Theory]
        [InlineTestObject(new uint[] { 0, 10, uint.MaxValue}, 1, 9)]
        [InlineTestObject(new uint[] { 5, 10, uint.MaxValue}, 0, 4)]
        [InlineTestObject(new uint[] { 0, 10, uint.MaxValue}, 11, uint.MaxValue - 1)]
        [InlineTestObject(new uint[] { 0, 10, uint.MaxValue - 1}, uint.MaxValue, uint.MaxValue)]
        public void OverlapsRange_NoValuesIntersects_ReturnsFalse(uint[] values, uint start, uint end, IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetFromValues(values);
        
            // Act
            var actual = testObject.ReadOnlyBitmap.OverlapsRange(start, end);
        
            // Assert
            Assert.False(actual);
        }
    }
}