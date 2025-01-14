using System.Linq;
using Roaring.Net.CRoaring;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring64BitmapTests;

public class OrTests
{
    public class Or
    {
        [Theory]
        [InlineMatrixTestObject(new ulong[] { }, new ulong[] { })]
        [InlineMatrixTestObject(new ulong[] { 1 }, new ulong[] { 1 })]
        [InlineMatrixTestObject(new ulong[] { 1 }, new ulong[] { })]
        [InlineMatrixTestObject(new ulong[] { }, new ulong[] { 1 })]
        [InlineMatrixTestObject(new ulong[] { 0, 1, 2 }, new ulong[] { 1, ulong.MaxValue })]
        [InlineMatrixTestObject(new ulong[] { 0, 1, 2, ulong.MaxValue }, new ulong[] { 0, 2, ulong.MaxValue })]
        public void Or_BitmapsWithDifferentValues_ReturnsUnionOfBitmaps(ulong[] values1, ulong[] values2,
            TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject1 = matrix.X.GetFromValues(values1);
            using IRoaring64BitmapTestObject testObject2 = matrix.Y.GetFromValues(values2);

            // Act
            using Roaring64Bitmap actual = testObject1.ReadOnlyBitmap.Or(testObject2.Bitmap);

            // Assert
            Assert.Equal(values1.Union(values2), actual.Values);
        }
    }

    public class OrCount
    {
        [Theory]
        [InlineMatrixTestObject(new ulong[] { }, new ulong[] { })]
        [InlineMatrixTestObject(new ulong[] { 1 }, new ulong[] { 1 })]
        [InlineMatrixTestObject(new ulong[] { 1 }, new ulong[] { })]
        [InlineMatrixTestObject(new ulong[] { }, new ulong[] { 1 })]
        [InlineMatrixTestObject(new ulong[] { 0, 1, 2 }, new ulong[] { 1, ulong.MaxValue })]
        [InlineMatrixTestObject(new ulong[] { 0, 1, 2, ulong.MaxValue }, new ulong[] { 0, 2, ulong.MaxValue })]
        public void Or_BitmapsWithDifferentValues_ReturnsCountAfterUnionOfBitmaps(ulong[] values1, ulong[] values2,
            TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject1 = matrix.X.GetFromValues(values1);
            using IRoaring64BitmapTestObject testObject2 = matrix.Y.GetFromValues(values2);

            // Act
            var actual = testObject1.ReadOnlyBitmap.OrCount(testObject2.Bitmap);

            // Assert
            Assert.Equal((ulong)values1.Union(values2).LongCount(), actual);
        }
    }

    public class IOr
    {
        [Theory]
        [InlineData(new ulong[] { }, new ulong[] { })]
        [InlineData(new ulong[] { 1 }, new ulong[] { 1 })]
        [InlineData(new ulong[] { 1 }, new ulong[] { })]
        [InlineData(new ulong[] { }, new ulong[] { 1 })]
        [InlineData(new ulong[] { 0, 1, 2 }, new ulong[] { 1, ulong.MaxValue })]
        [InlineData(new ulong[] { 0, 1, 2, ulong.MaxValue }, new ulong[] { 0, 2, ulong.MaxValue })]
        public void IOr_BitmapsWithDifferentValues_ReturnsUnionOfBitmaps(ulong[] values1, ulong[] values2)
        {
            // Arrange
            using Roaring64BitmapTestObject testObject1 = Roaring64BitmapTestObjectFactory.Default.GetFromValues(values1);
            using Roaring64BitmapTestObject testObject2 = Roaring64BitmapTestObjectFactory.Default.GetFromValues(values2);

            // Act
            testObject1.Bitmap.IOr(testObject2.Bitmap);

            // Assert
            Assert.Equal(values1.Union(values2), testObject1.Bitmap.Values);
        }
    }
}