using System.Linq;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring32BitmapTests;

public class MetricsTests
{
    public class GetJaccardIndex
    {
        [Theory]
        [InlineMatrixTestObject]
        public void GetJaccardIndex_ForNotEmptyBitmaps_ReturnsJaccardIndex(TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject1 = matrix.X.GetFromValues([1, 2, 3, int.MaxValue]);
            using IRoaring32BitmapTestObject testObject2 = matrix.Y.GetFromValues([1, 2, 3, 5]);

            // Act
            var actual = testObject1.ReadOnlyBitmap.GetJaccardIndex(testObject2.Bitmap);

            // Assert
            var intersectCount = testObject1.Values.Intersect(testObject2.Values).Count();
            var unionCount = testObject1.Values.Union(testObject2.Values).Count();

            Assert.Equal(intersectCount / (double)unionCount, actual);
        }

        [Theory]
        [InlineMatrixTestObject]
        public void GetJaccardIndex_ForEmptyBitmaps_ReturnsNaN(TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject1 = matrix.X.GetEmpty();
            using IRoaring32BitmapTestObject testObject2 = matrix.Y.GetEmpty();

            // Act
            var actual = testObject1.ReadOnlyBitmap.GetJaccardIndex(testObject2.Bitmap);

            // Assert
            Assert.Equal(double.NaN, actual);
        }
    }

    public class IsEmpty
    {
        [Theory]
        [InlineTestObject]
        public void IsEmpty_BitmapContainsValues_ReturnsFalse(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetDefault();

            // Act
            var actual = testObject.ReadOnlyBitmap.IsEmpty;

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineTestObject]
        public void IsEmpty_EmptyBitmap_ReturnsTrue(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetEmpty();

            // Act
            var actual = testObject.ReadOnlyBitmap.IsEmpty;

            // Assert
            Assert.True(actual);
        }
    }

    public class Min
    {
        [Theory]
        [InlineTestObject]
        public void Min_BitmapContainsValues_ReturnsMinValue(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetForRange(start: 10, end: uint.MaxValue, count: 1000);

            // Act
            var actual = testObject.ReadOnlyBitmap.Min;

            // Assert
            Assert.Equal(10U, actual);
        }

        [Theory]
        [InlineTestObject]
        public void Min_EmptyBitmap_ReturnsNull(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetEmpty();

            // Act
            uint? actual = testObject.ReadOnlyBitmap.Min;

            // Assert
            Assert.Null(actual);
        }
    }

    public class Max
    {
        [Theory]
        [InlineTestObject]
        public void Max_BitmapContainsValues_ReturnsMaxValue(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetForRange(start: 0, end: 1001, count: 1001);

            // Act
            var actual = testObject.ReadOnlyBitmap.Max;

            // Assert
            Assert.Equal(1000U, actual);
        }

        [Theory]
        [InlineTestObject]
        public void Max_EmptyBitmap_ReturnsNull(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetEmpty();

            // Act
            uint? actual = testObject.ReadOnlyBitmap.Max;

            // Assert
            Assert.Null(actual);
        }
    }
}