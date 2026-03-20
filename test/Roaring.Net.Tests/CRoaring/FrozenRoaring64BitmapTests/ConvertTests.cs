using System.Linq;
using Roaring.Net.CRoaring;
using Roaring.Net.Tests.CRoaring.Roaring64BitmapTests;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.FrozenRoaring64BitmapTests;

public unsafe class ConvertTests
{
    public class ToBitmap
    {
        [Fact]
        public void ToBitmap_Always_CreatesNewInstanceOfBitmap()
        {
            // Act
            using FrozenRoaring64BitmapTestObject testObject = FrozenRoaring64BitmapTestObjectFactory.Default.GetDefault();

            // Act
            Roaring64Bitmap actual = testObject.Bitmap.ToBitmap();

            // Assert
            Assert.Equal(testObject.Bitmap.Values, actual.Values);
        }

        [Fact]
        public void ToBitmap_DestroyOriginalBitmap_NewBitmapIsSillUsable()
        {
            // Act
            FrozenRoaring64BitmapTestObject testObject = FrozenRoaring64BitmapTestObjectFactory.Default.GetDefault();
            var expectedValues = testObject.Bitmap.Values.ToList();

            // Act
            Roaring64Bitmap actual = testObject.Bitmap.ToBitmap();
            testObject.Dispose();

            // Assert
            Assert.Equal(expectedValues, actual.Values);

            using Roaring64BitmapTestObject opBitmap = Roaring64BitmapTestObjectFactory.Default.GetForCount(100);
            actual.IOr(opBitmap.Bitmap);
            Assert.Equal(expectedValues.Concat(opBitmap.Values).OrderBy(x => x).Distinct(), actual.Values);

            actual.Clear();
            Assert.True(actual.IsEmpty);
        }
    }

    public class ToBitmapWithOffset
    {
        [Theory]
        [InlineData(new ulong[] { }, new ulong[] { }, 10)]
        [InlineData(new ulong[] { 0, 1, 2, 3, 4 }, new ulong[] { 5, 6, 7, 8, 9 }, 5)]
        [InlineData(new ulong[] { 0, 2, 4, 6, 8 }, new ulong[] { 10, 12, 14, 16, 18 }, 10)]
        [InlineData(new ulong[] { 0, 1, 2, 3, 4, ulong.MaxValue }, new ulong[] { 1, 2, 3, 4, 5 }, 1)]
        [InlineData(new ulong[] { ulong.MaxValue - 1, ulong.MaxValue }, new ulong[] { ulong.MaxValue }, 1)]
        [InlineData(new ulong[] { 0 }, new ulong[] { ulong.MaxValue }, ulong.MaxValue)]
        public void ToBitmapWithOffset_AddsValueToBitmapValues_ReturnsNewBitmapWithExpectedValues(ulong[] values, ulong[] expected, ulong offset)
        {
            // Arrange
            using FrozenRoaring64BitmapTestObject testObject = FrozenRoaring64BitmapTestObjectFactory.Default.GetFromValues(values);

            // Act
            using Roaring64Bitmap actualBitmap = testObject.Bitmap.ToBitmapWithOffset(offset);

            // Assert
            var actual = actualBitmap.Values.ToList();
            Assert.Equal(expected, actual);
            Assert.Equal(testObject.Bitmap.Values, values);
        }

        [Fact]
        public void ToBitmapWithOffset_DestroyOriginalBitmap_NewBitmapIsSillUsable()
        {
            // Act
            FrozenRoaring64BitmapTestObject testObject = FrozenRoaring64BitmapTestObjectFactory.Default.GetDefault();
            var expectedValues = testObject.Bitmap.Values.ToList();

            // Act
            Roaring64Bitmap actual = testObject.Bitmap.ToBitmapWithOffset(0);
            testObject.Dispose();

            // Assert
            Assert.Equal(expectedValues, actual.Values);

            using Roaring64BitmapTestObject opBitmap = Roaring64BitmapTestObjectFactory.Default.GetForCount(100);
            actual.IOr(opBitmap.Bitmap);
            Assert.Equal(expectedValues.Concat(opBitmap.Values).OrderBy(x => x).Distinct(), actual.Values);

            actual.Clear();
            Assert.True(actual.IsEmpty);
        }
    }

    public class ToBitmapWithNegativeOffset
    {
        [Theory]
        [InlineData(new ulong[] { 0, 1, 2, 3, 4 }, new ulong[] { 0, 1, 2 }, 2)]
        [InlineData(new ulong[] { 0, 1, 2, 3, 4 }, new ulong[] { }, 5)]
        [InlineData(new ulong[] { ulong.MaxValue }, new ulong[] { 0 }, ulong.MaxValue)]
        [InlineData(new ulong[] { ulong.MaxValue }, new ulong[] { ulong.MaxValue - 1 }, 1)]
        public void ToBitmapWithNegativeOffset_SubtractValueFromBitmapValues_ReturnsNewBitmapWithExpectedValues(ulong[] values, ulong[] expected, ulong offset)
        {
            // Arrange
            using FrozenRoaring64BitmapTestObject testObject = FrozenRoaring64BitmapTestObjectFactory.Default.GetFromValues(values);

            // Act
            using Roaring64Bitmap actualBitmap = testObject.Bitmap.ToBitmapWithNegativeOffset(offset);

            // Assert
            var actual = actualBitmap.Values.ToList();
            Assert.Equal(expected, actual);
            Assert.Equal(testObject.Bitmap.Values, values);
        }

        [Fact]
        public void ToBitmapWithNegativeOffset_DestroyOriginalBitmap_NewBitmapIsSillUsable()
        {
            // Act
            FrozenRoaring64BitmapTestObject testObject = FrozenRoaring64BitmapTestObjectFactory.Default.GetDefault();
            var expectedValues = testObject.Bitmap.Values.ToList();

            // Act
            Roaring64Bitmap actual = testObject.Bitmap.ToBitmapWithNegativeOffset(0);
            testObject.Dispose();

            // Assert
            Assert.Equal(expectedValues, actual.Values);

            using Roaring64BitmapTestObject opBitmap = Roaring64BitmapTestObjectFactory.Default.GetForCount(100);
            actual.IOr(opBitmap.Bitmap);
            Assert.Equal(expectedValues.Concat(opBitmap.Values).OrderBy(x => x).Distinct(), actual.Values);

            actual.Clear();
            Assert.True(actual.IsEmpty);
        }
    }
}