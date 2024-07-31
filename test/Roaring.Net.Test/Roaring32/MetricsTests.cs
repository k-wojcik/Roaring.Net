using System.Linq;
using Xunit;

namespace Roaring.Test.Roaring32;

public class MetricsTests
{
    public class GetJaccardIndex
    {
        [Fact]
        public void GetJaccardIndex_ForNotEmptyBitmaps_ReturnsJaccardIndex()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues([1, 2, 3, int.MaxValue]);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues([1, 2, 3, 5]);

            // Act
            var actual = testObject1.Bitmap.GetJaccardIndex(testObject2.Bitmap);

            // Assert
            var intersectCount = testObject1.Values.Intersect(testObject2.Values).Count();
            var unionCount = testObject1.Values.Union(testObject2.Values).Count();
            
            Assert.Equal(intersectCount / (double)unionCount, actual);
        }
        
        [Fact]
        public void GetJaccardIndex_ForEmptyBitmaps_ReturnsNaN()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetEmpty();
            using var testObject2 = Roaring32BitmapTestObject.GetEmpty();

            // Act
            var actual = testObject1.Bitmap.GetJaccardIndex(testObject2.Bitmap);

            // Assert
            Assert.Equal(double.NaN, actual);
        }
    }

    public class IsEmpty
    {
        [Fact]
        public void IsEmpty_BitmapContainsValues_ReturnsFalse()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetDefault();

            // Act
            var actual = testObject.Bitmap.IsEmpty;

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void IsEmpty_EmptyBitmap_ReturnsTrue()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetEmpty();

            // Act
            var actual = testObject.Bitmap.IsEmpty;

            // Assert
            Assert.True(actual);
        }
    }

    public class Min
    {
        [Fact]
        public void Min_BitmapContainsValues_ReturnsMinValue()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetForRange(start: 10, end: uint.MaxValue, count: 1000);

            // Act
            var actual = testObject.Bitmap.Min;

            // Assert
            Assert.Equal(10U, actual);
        }

        [Fact]
        public void Min_EmptyBitmap_ReturnsNull()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetEmpty();

            // Act
            uint? actual = testObject.Bitmap.Min;

            // Assert
            Assert.Null(actual);
        }
    }

    public class Max
    {
        [Fact]
        public void Max_BitmapContainsValues_ReturnsMaxValue()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetForRange(start: 0, end: 1001, count: 1001);

            // Act
            var actual = testObject.Bitmap.Max;

            // Assert
            Assert.Equal(1000U, actual);
        }

        [Fact]
        public void Max_EmptyBitmap_ReturnsNull()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetEmpty();

            // Act
            uint? actual = testObject.Bitmap.Max;

            // Assert
            Assert.Null(actual);
        }
    }

    public class SerializedBytes
    {
        [Fact]
        public void SerializedBytes_EmptyBitmap_ReturnsValueGreaterThanZero()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetEmpty();

            // Act
            var actual = testObject.Bitmap.SerializedBytes;

            // Assert
            Assert.True(actual > 0);
        }

        [Fact]
        public void SerializedBytes_BitmapContainsValues_ReturnsValueGreaterThanZero()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetDefault();

            // Act
            var actual = testObject.Bitmap.SerializedBytes;

            // Assert
            Assert.True(actual > 0);
        }
    }

    public class PortableSerializedBytes
    {
        [Fact]
        public void PortableSerializedBytes_EmptyBitmap_ReturnsValueGreaterThanZero()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetEmpty();

            // Act
            var actual = testObject.Bitmap.PortableSerializedBytes;

            // Assert
            Assert.True(actual > 0);
        }

        [Fact]
        public void PortableSerializedBytes_BitmapContainsValues_ReturnsValueGreaterThanZero()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetDefault();

            // Act
            var actual = testObject.Bitmap.PortableSerializedBytes;

            // Assert
            Assert.True(actual > 0);
        }
    }
}