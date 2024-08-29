using System.Linq;
using System.Runtime.InteropServices;
using Roaring.Net.CRoaring;
using Roaring.Net.Tests.CRoaring.Roaring32BitmapTests;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.FrozenRoaring32BitmapTests;

public unsafe class ConvertTests
{
    public class ToBitmap
    {
        [Fact]
        public void ToBitmap_Always_CreatesNewInstanceOfBitmap()
        {
            // Act
            using FrozenRoaring32BitmapTestObject testObject = FrozenRoaring32BitmapTestObjectFactory.Default.GetDefault();

            // Act
            Roaring32Bitmap actual = testObject.Bitmap.ToBitmap();

            // Assert
            Assert.Equal(testObject.Bitmap.Values, actual.Values);
        }

        [Fact]
        public void ToBitmap_DestroyOriginalBitmap_NewBitmapIsSillUsable()
        {
            // Act
            FrozenRoaring32BitmapTestObject testObject = FrozenRoaring32BitmapTestObjectFactory.Default.GetDefault();
            var expectedValues = testObject.Bitmap.Values.ToList();

            // Act
            Roaring32Bitmap actual = testObject.Bitmap.ToBitmap();
            NativeMemory.Clear(testObject.Bitmap.Memory.MemoryPtr, testObject.Bitmap.Memory.Size);
            testObject.Dispose();

            // Assert
            Assert.Equal(expectedValues, actual.Values);

            using Roaring32BitmapTestObject opBitmap = Roaring32BitmapTestObjectFactory.Default.GetForCount(100);
            actual.IOr(opBitmap.Bitmap);
            Assert.Equal(expectedValues.Concat(opBitmap.Values).OrderBy(x => x).Distinct(), actual.Values);

            actual.Clear();
            Assert.True(actual.IsEmpty);
        }
    }

    public class ToBitmapWithOffset
    {
        [Theory]
        [InlineData(new uint[] { }, new uint[] { }, 10)]
        [InlineData(new uint[] { 0, 1, 2, 3, 4 }, new uint[] { 5, 6, 7, 8, 9 }, 5)]
        [InlineData(new uint[] { 0, 1, 2, 3, 4 }, new uint[] { 0, 1, 2 }, -2)]
        [InlineData(new uint[] { 0, 1, 2, 3, 4 }, new uint[] { }, -5)]
        [InlineData(new uint[] { 0, 2, 4, 6, 8 }, new uint[] { 10, 12, 14, 16, 18 }, 10)]
        [InlineData(new uint[] { 0, 1, 2, 3, 4, uint.MaxValue }, new uint[] { 1, 2, 3, 4, 5 }, 1)]
        [InlineData(new uint[] { uint.MaxValue - 1, uint.MaxValue }, new uint[] { uint.MaxValue }, 1)]
        [InlineData(new uint[] { 0 }, new uint[] { uint.MaxValue }, uint.MaxValue)]
        [InlineData(new uint[] { uint.MaxValue }, new uint[] { 0 }, -uint.MaxValue)]
        public void ToBitmapWithOffset_AddsValueToBitmapValues_ReturnsNewBitmapWithExpectedValues(uint[] values,
            uint[] expected, long offset)
        {
            // Arrange
            using FrozenRoaring32BitmapTestObject testObject = FrozenRoaring32BitmapTestObjectFactory.Default.GetFromValues(values);

            // Act
            using Roaring32Bitmap actualBitmap = testObject.Bitmap.ToBitmapWithOffset(offset);

            // Assert
            var actual = actualBitmap.Values.ToList();
            Assert.Equal(expected, actual);
            Assert.Equal(testObject.Bitmap.Values, values);
        }

        [Fact]
        public void ToBitmapWithOffset_DestroyOriginalBitmap_NewBitmapIsSillUsable()
        {
            // Act
            FrozenRoaring32BitmapTestObject testObject = FrozenRoaring32BitmapTestObjectFactory.Default.GetDefault();
            var expectedValues = testObject.Bitmap.Values.ToList();

            // Act
            Roaring32Bitmap actual = testObject.Bitmap.ToBitmapWithOffset(0);
            testObject.Dispose();

            // Assert
            Assert.Equal(expectedValues, actual.Values);

            using Roaring32BitmapTestObject opBitmap = Roaring32BitmapTestObjectFactory.Default.GetForCount(100);
            actual.IOr(opBitmap.Bitmap);
            Assert.Equal(expectedValues.Concat(opBitmap.Values).OrderBy(x => x).Distinct(), actual.Values);

            actual.Clear();
            Assert.True(actual.IsEmpty);
        }
    }
}