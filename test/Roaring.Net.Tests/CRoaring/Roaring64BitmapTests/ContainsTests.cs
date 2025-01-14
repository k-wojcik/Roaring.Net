using System;
using System.Linq;
using Roaring.Net.CRoaring;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring64BitmapTests;

public class ContainsTests
{
    public class Contains
    {
        [Theory]
        [InlineTestObject]
        public void Contains_EmptyBitmap_ReturnFalse(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetEmpty();

            // Act
            var actual = testObject.ReadOnlyBitmap.Contains(10);

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineTestObject]
        public void Contains_BitmapHasValue_ReturnsTrue(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetDefault();
            var value = testObject.Values.First();

            // Act
            var actual = testObject.ReadOnlyBitmap.Contains(value);

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineTestObject]
        public void Contains_BitmapDoesNotHaveValue_ReturnsFalse(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetDefault();
            Assert.DoesNotContain(testObject.Values, value => value == 10U);

            // Act
            var actual = testObject.ReadOnlyBitmap.Contains(10U);

            // Assert
            Assert.False(actual);
        }
    }

    public class ContainsRange
    {
        [Theory]
        [InlineTestObject(1, 0)]
        [InlineTestObject(10, 5)]
        public void ContainsRange_ArgumentsOutOfAllowedRange_ThrowsArgumentOutOfRangeException(uint start, uint end, IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetEmpty();

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => testObject.ReadOnlyBitmap.ContainsRange(start, end));
        }

        [Theory]
        [InlineTestObject(0, 0, 0, 0, true)]
        [InlineTestObject(0, 1, 0, 0, true)]
        [InlineTestObject(0, 100, 0, 0, true)]
        [InlineTestObject(0, 100, 1, 1, true)]
        [InlineTestObject(0, 100, 0, 10, true)]
        [InlineTestObject(200, 300, 0, 10, false)]
        [InlineTestObject(200, 300, 150, 250, false)]
        [InlineTestObject(200, 300, 250, 350, false)]
        [InlineTestObject(200, 300, 100, 350, false)]
        [InlineTestObject(ulong.MaxValue - 100, ulong.MaxValue, ulong.MaxValue - 1, ulong.MaxValue, true)]
        [InlineTestObject(ulong.MaxValue - 100, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, true)]
        [InlineTestObject(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, true)]
        [InlineTestObject(ulong.MaxValue - 1, ulong.MaxValue - 1, ulong.MaxValue, ulong.MaxValue, false)]
        public void ContainsRange_CorrectRange_ReturnsExpectedResult(ulong startTest, ulong endTest, ulong start, ulong end, bool expected, IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetForRange(startTest, endTest);

            // Act
            var actual = testObject.ReadOnlyBitmap.ContainsRange(start, end);

            // Assert
            Assert.Equal(expected, actual);
        }
    }

    public class ContainsBulk
    {
        [Theory]
        [InlineMatrixTestObject]
        public void ContainsBulk_DifferentBitmaps_ThrowsArgumentException(TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject1 = matrix.X.GetEmpty();
            using IRoaring64BitmapTestObject testObject2 = matrix.Y.GetEmpty();
            using var context = BulkContext64.For(testObject1.Bitmap);

            // Act && Assert
            Assert.Throws<ArgumentException>(() =>
            {
                testObject2.ReadOnlyBitmap.ContainsBulk(context, 10);
            });
        }

        [Theory]
        [InlineTestObject]
        public void ContainsBulk_EmptyBitmap_ReturnFalse(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetEmpty();
            using var context = BulkContext64.For(testObject.Bitmap);

            // Act
            var actual = testObject.ReadOnlyBitmap.ContainsBulk(context, 10);

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineTestObject]
        public void ContainsBulk_BitmapHasValue_ReturnsTrue(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetDefault();
            var value = testObject.Values.First();
            using var context = BulkContext64.For(testObject.Bitmap);

            // Act
            var actual = testObject.ReadOnlyBitmap.ContainsBulk(context, value);

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineTestObject]
        public void ContainsBulk_BitmapDoesNotHaveValue_ReturnsFalse(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetDefault();
            Assert.DoesNotContain(testObject.Values, value => value == 10U);
            using var context = BulkContext64.For(testObject.Bitmap);

            // Act
            var actual = testObject.ReadOnlyBitmap.ContainsBulk(context, 10U);

            // Assert
            Assert.False(actual);
        }
    }
}