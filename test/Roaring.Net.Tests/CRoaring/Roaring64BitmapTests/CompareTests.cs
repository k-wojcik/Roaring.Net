using System;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring64BitmapTests;

public class CompareTests
{
    public class ValueEquals
    {
        [Theory]
        [InlineTestObject]
        public void ValueEquals_SameBitmap_ReturnsTrue(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetEmpty();

            // Act
            var actual = testObject.ReadOnlyBitmap.ValueEquals(testObject.Bitmap);

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineMatrixTestObject]
        public void ValueEquals_BitmapsHaveSameValues_ReturnsTrue(TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject1 = matrix.X.GetFromValues([0, 10]);
            using IRoaring64BitmapTestObject testObject2 = matrix.Y.GetFromValues([0, 10]);

            // Act
            var actual = testObject1.ReadOnlyBitmap.ValueEquals(testObject2.Bitmap);

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineMatrixTestObject]
        public void ValueEquals_BitmapsHaveDifferentValues_ReturnsFalse(TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject1 = matrix.X.GetFromValues([0, 10]);
            using IRoaring64BitmapTestObject testObject2 = matrix.Y.GetFromValues([0, 10, 20]);

            // Act
            var actual = testObject1.ReadOnlyBitmap.ValueEquals(testObject2.Bitmap);

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineTestObject]
        public void ValueEquals_BitmapIsNull_ReturnsFalse(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject1 = factory.GetEmpty();

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
        public void IsSubsetOf_BitmapIsNull_ReturnsFalse(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetEmpty();

            // Act
            var actual = testObject.ReadOnlyBitmap.IsSubsetOf(null);

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineTestObject]
        public void IsSubsetOf_SameBitmap_ReturnsTrue(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetEmpty();

            // Act
            var actual = testObject.ReadOnlyBitmap.IsSubsetOf(testObject.Bitmap);

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineMatrixTestObject]
        public void IsSubsetOf_EmptyBitmaps_ReturnsFalse(TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject1 = matrix.X.GetEmpty();
            using IRoaring64BitmapTestObject testObject2 = matrix.Y.GetEmpty();

            // Act
            var actual = testObject1.ReadOnlyBitmap.IsSubsetOf(testObject2.Bitmap);

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineMatrixTestObject]
        public void IsSubsetOf_BitmapsHaveSameValues_ReturnsTrue(TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject1 = matrix.X.GetFromValues([0, 10]);
            using IRoaring64BitmapTestObject testObject2 = matrix.Y.GetFromValues([0, 10]);

            // Act
            var actual = testObject1.ReadOnlyBitmap.IsSubsetOf(testObject2.Bitmap);

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineMatrixTestObject]
        public void IsSubsetOf_SecondBitmapContainsAllValuesOfBitmap_ReturnsTrue(TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject1 = matrix.X.GetFromValues([0, 10]);
            using IRoaring64BitmapTestObject testObject2 = matrix.Y.GetFromValues([0, 10, 20]);

            // Act
            var actual = testObject1.ReadOnlyBitmap.IsSubsetOf(testObject2.Bitmap);

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineMatrixTestObject]
        public void IsSubsetOf_SecondBitmapNotContainsSomeValuesOfBitmap_ReturnsFalse(TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject1 = matrix.X.GetFromValues([0, 10]);
            using IRoaring64BitmapTestObject testObject2 = matrix.Y.GetFromValues([0]);

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
        public void IsProperSubsetOf_BitmapIsNull_ReturnsFalse(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetEmpty();

            // Act
            var actual = testObject.ReadOnlyBitmap.IsProperSubsetOf(null);

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineTestObject]
        public void IsProperSubsetOf_SameBitmap_ReturnsFalse(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetFromValues([1]);

            // Act
            var actual = testObject.ReadOnlyBitmap.IsProperSubsetOf(testObject.Bitmap);

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineMatrixTestObject]
        public void IsProperSubsetOf_EmptyBitmaps_ReturnsFalse(TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject1 = matrix.X.GetEmpty();
            using IRoaring64BitmapTestObject testObject2 = matrix.Y.GetEmpty();

            // Act
            var actual = testObject1.ReadOnlyBitmap.IsProperSubsetOf(testObject2.Bitmap);

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineMatrixTestObject]
        public void IsProperSubsetOf_BitmapsHaveSameValues_ReturnsFalse(TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject1 = matrix.X.GetFromValues([0, 10]);
            using IRoaring64BitmapTestObject testObject2 = matrix.Y.GetFromValues([0, 10]);

            // Act
            var actual = testObject1.ReadOnlyBitmap.IsProperSubsetOf(testObject2.Bitmap);

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineMatrixTestObject]
        public void IsProperSubsetOf_SecondBitmapContainsAllValuesOfBitmap_ReturnsTrue(TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject1 = matrix.X.GetFromValues([0, 10]);
            using IRoaring64BitmapTestObject testObject2 = matrix.Y.GetFromValues([0, 10, 20]);

            // Act
            var actual = testObject1.ReadOnlyBitmap.IsProperSubsetOf(testObject2.Bitmap);

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineMatrixTestObject]
        public void IsProperSubsetOf_SecondBitmapNotContainsSomeValuesOfBitmap_ReturnsFalse(TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject1 = matrix.X.GetFromValues([0, 10]);
            using IRoaring64BitmapTestObject testObject2 = matrix.Y.GetFromValues([0]);

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
        public void IsSupersetOf_BitmapIsNull_ReturnsFalse(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetEmpty();

            // Act
            var actual = testObject.ReadOnlyBitmap.IsSupersetOf(null);

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineTestObject]
        public void IsSupersetOf_SameBitmap_ReturnsTrue(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetEmpty();

            // Act
            var actual = testObject.ReadOnlyBitmap.IsSupersetOf(testObject.Bitmap);

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineMatrixTestObject]
        public void IsSupersetOf_EmptyBitmaps_ReturnsTrue(TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject1 = matrix.X.GetEmpty();
            using IRoaring64BitmapTestObject testObject2 = matrix.Y.GetEmpty();

            // Act
            var actual = testObject1.ReadOnlyBitmap.IsSupersetOf(testObject2.Bitmap);

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineMatrixTestObject]
        public void IsSupersetOf_BitmapsHaveSameValues_ReturnsTrue(TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject1 = matrix.X.GetFromValues([0, 10]);
            using IRoaring64BitmapTestObject testObject2 = matrix.Y.GetFromValues([0, 10]);

            // Act
            var actual = testObject1.ReadOnlyBitmap.IsSubsetOf(testObject2.Bitmap);

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineMatrixTestObject]
        public void IsSupersetOf_BitmapContainsAllValuesOfSecondBitmap_ReturnsTrue(TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject1 = matrix.X.GetFromValues([0, 10, 20]);
            using IRoaring64BitmapTestObject testObject2 = matrix.Y.GetFromValues([0, 10]);

            // Act
            var actual = testObject1.ReadOnlyBitmap.IsSupersetOf(testObject2.Bitmap);

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineMatrixTestObject]
        public void IsSupersetOf_BitmapNotContainsSomeValuesOfSecondBitmap_ReturnsFalse(TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject1 = matrix.X.GetFromValues([0, 10]);
            using IRoaring64BitmapTestObject testObject2 = matrix.Y.GetFromValues([0, 10, 20]);

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
        public void IsProperSupersetOf_BitmapIsNull_ReturnsFalse(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetEmpty();

            // Act
            var actual = testObject.ReadOnlyBitmap.IsProperSupersetOf(null);

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineTestObject]
        public void IsProperSupersetOf_SameBitmap_ReturnsFalse(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetEmpty();

            // Act
            var actual = testObject.ReadOnlyBitmap.IsProperSupersetOf(testObject.Bitmap);

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineMatrixTestObject]
        public void IsProperSupersetOf_EmptyBitmaps_ReturnsTrue(TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject1 = matrix.X.GetEmpty();
            using IRoaring64BitmapTestObject testObject2 = matrix.Y.GetEmpty();

            // Act
            var actual = testObject1.ReadOnlyBitmap.IsProperSupersetOf(testObject2.Bitmap);

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineMatrixTestObject]
        public void IsProperSupersetOf_BitmapsHaveSameValues_ReturnsFalse(TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject1 = matrix.X.GetFromValues([0, 10]);
            using IRoaring64BitmapTestObject testObject2 = matrix.Y.GetFromValues([0, 10]);

            // Act
            var actual = testObject1.ReadOnlyBitmap.IsProperSupersetOf(testObject2.Bitmap);

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineMatrixTestObject]
        public void IsProperSupersetOf_BitmapContainsAllValuesOfSecondBitmap_ReturnsTrue(TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject1 = matrix.X.GetFromValues([0, 10, 20]);
            using IRoaring64BitmapTestObject testObject2 = matrix.Y.GetFromValues([0, 10]);

            // Act
            var actual = testObject1.ReadOnlyBitmap.IsProperSupersetOf(testObject2.Bitmap);

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineMatrixTestObject]
        public void IsProperSupersetOf_BitmapNotContainsSomeValuesOfSecondBitmap_ReturnsFalse(TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject1 = matrix.X.GetFromValues([0, 10]);
            using IRoaring64BitmapTestObject testObject2 = matrix.Y.GetFromValues([0, 10, 20]);

            // Act
            var actual = testObject1.ReadOnlyBitmap.IsProperSupersetOf(testObject2.Bitmap);

            // Assert
            Assert.False(actual);
        }
    }

    public class Overlaps
    {
        [Theory]
        [InlineMatrixTestObject(new ulong[] { 0, 10, ulong.MaxValue, 1 }, new ulong[] { 5, 10, 11 })]
        [InlineMatrixTestObject(new ulong[] { 0 }, new ulong[] { 0 })]
        [InlineMatrixTestObject(new ulong[] { 0, ulong.MaxValue }, new ulong[] { 0, ulong.MaxValue })]
        public void Overlaps_IntersectsWithAtLeastOneValue_ReturnsTrue(
            ulong[] values1,
            ulong[] values2,
            TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject1 = matrix.X.GetFromValues(values1);
            using IRoaring64BitmapTestObject testObject2 = matrix.Y.GetFromValues(values2);

            // Act
            var actual = testObject1.ReadOnlyBitmap.Overlaps(testObject2.Bitmap);

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineMatrixTestObject(new ulong[] { }, new ulong[] { })]
        [InlineMatrixTestObject(new ulong[] { 0, 10, ulong.MaxValue, 1 }, new ulong[] { 5, 12, 11 })]
        [InlineMatrixTestObject(new ulong[] { }, new ulong[] { 5, 12, 11 })]
        [InlineMatrixTestObject(new ulong[] { 0, 10, ulong.MaxValue, 1 }, new ulong[] { })]
        public void Overlaps_NoValuesIntersects_ReturnsFalse(
            ulong[] values1,
            ulong[] values2,
            TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject1 = matrix.X.GetFromValues(values1);
            using IRoaring64BitmapTestObject testObject2 = matrix.Y.GetFromValues(values2);

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
        public void OverlapsRange_ArgumentsOutOfAllowedRange_ThrowsArgumentOutOfRangeException(ulong start, ulong end, IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetEmpty();

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => testObject.ReadOnlyBitmap.OverlapsRange(start, end));
        }

        [Theory]
        [InlineTestObject(new ulong[] { 0, 10, ulong.MaxValue }, 0, 0)]
        [InlineTestObject(new ulong[] { 0, 10, ulong.MaxValue }, 0, ulong.MaxValue)]
        [InlineTestObject(new ulong[] { 0, 10, ulong.MaxValue }, 0, 9)]
        [InlineTestObject(new ulong[] { 0, 10, ulong.MaxValue }, 1, 10)]
        [InlineTestObject(new ulong[] { 0, 10, ulong.MaxValue - 1 }, ulong.MaxValue - 1, ulong.MaxValue)]
        [InlineTestObject(new ulong[] { 0, 10, ulong.MaxValue }, ulong.MaxValue - 1, ulong.MaxValue)]
        [InlineTestObject(new ulong[] { 0, 10, ulong.MaxValue }, ulong.MaxValue, ulong.MaxValue)]
        public void OverlapsRange_IntersectsWithAtLeastOneValue_ReturnsTrue(ulong[] values, ulong start, ulong end, IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetFromValues(values);

            // Act
            var actual = testObject.ReadOnlyBitmap.OverlapsRange(start, end);

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineTestObject(new ulong[] { 0, 10, ulong.MaxValue }, 1, 9)]
        [InlineTestObject(new ulong[] { 5, 10, ulong.MaxValue }, 0, 4)]
        [InlineTestObject(new ulong[] { 0, 10, ulong.MaxValue }, 11, ulong.MaxValue - 1)]
        [InlineTestObject(new ulong[] { 0, 10, ulong.MaxValue - 1 }, ulong.MaxValue, ulong.MaxValue)]
        [InlineTestObject(new ulong[] { }, ulong.MaxValue, ulong.MaxValue)]
        public void OverlapsRange_NoValuesIntersects_ReturnsFalse(ulong[] values, ulong start, ulong end, IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetFromValues(values);

            // Act
            var actual = testObject.ReadOnlyBitmap.OverlapsRange(start, end);

            // Assert
            Assert.False(actual);
        }
    }
}