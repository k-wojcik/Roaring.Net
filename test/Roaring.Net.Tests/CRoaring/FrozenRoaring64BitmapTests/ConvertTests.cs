using System.Linq;
using System.Runtime.InteropServices;
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
}