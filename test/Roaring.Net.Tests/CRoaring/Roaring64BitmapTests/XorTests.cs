using System.Collections.Generic;
using System.Linq;
using Roaring.Net.CRoaring;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring64BitmapTests;

public class XorTests
{
    public class Xor
    {
        [Theory]
        [InlineMatrixTestObject(new ulong[] { }, new ulong[] { })]
        [InlineMatrixTestObject(new ulong[] { 1 }, new ulong[] { 1 })]
        [InlineMatrixTestObject(new ulong[] { 1 }, new ulong[] { })]
        [InlineMatrixTestObject(new ulong[] { }, new ulong[] { 1 })]
        [InlineMatrixTestObject(new ulong[] { 0, 1, 2 }, new ulong[] { 1, ulong.MaxValue })]
        [InlineMatrixTestObject(new ulong[] { 0, 1, 2, ulong.MaxValue }, new ulong[] { 0, 2, ulong.MaxValue })]
        public void Xor_BitmapsWithDifferentValues_ReturnsSymmetricDifferenceOfBitmaps(ulong[] values1, ulong[] values2,
            TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject1 = matrix.X.GetFromValues(values1);
            using IRoaring64BitmapTestObject testObject2 = matrix.Y.GetFromValues(values2);

            // Act
            using Roaring64Bitmap actual = testObject1.ReadOnlyBitmap.Xor(testObject2.Bitmap);

            // Assert
            Assert.Equal(values1.Union(values2).Except(values1.Intersect(values2)), actual.Values);
        }
    }

    public class XorCount
    {
        [Theory]
        [InlineMatrixTestObject(new ulong[] { }, new ulong[] { })]
        [InlineMatrixTestObject(new ulong[] { 1 }, new ulong[] { 1 })]
        [InlineMatrixTestObject(new ulong[] { 1 }, new ulong[] { })]
        [InlineMatrixTestObject(new ulong[] { }, new ulong[] { 1 })]
        [InlineMatrixTestObject(new ulong[] { 0, 1, 2 }, new ulong[] { 1, ulong.MaxValue })]
        [InlineMatrixTestObject(new ulong[] { 0, 1, 2, ulong.MaxValue }, new ulong[] { 0, 2, ulong.MaxValue })]
        public void XorCount_BitmapsWithDifferentValues_ReturnsSymmetricDifferenceOfBitmaps(ulong[] values1, ulong[] values2,
            TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject1 = matrix.X.GetFromValues(values1);
            using IRoaring64BitmapTestObject testObject2 = matrix.Y.GetFromValues(values2);

            // Act
            var actual = testObject1.ReadOnlyBitmap.XorCount(testObject2.Bitmap);

            // Assert
            Assert.Equal((uint)values1.Union(values2).Except(values1.Intersect(values2)).Count(), actual);
        }
    }

    public class XorMany
    {
        [Theory]
        [InlineMatrixTestObject(new ulong[] { }, new ulong[] { }, new ulong[] { })]
        [InlineMatrixTestObject(new ulong[] { 1 }, new ulong[] { 1 }, new ulong[] { 1 })]
        [InlineMatrixTestObject(new ulong[] { 1 }, new ulong[] { }, new ulong[] { })]
        [InlineMatrixTestObject(new ulong[] { }, new ulong[] { 1 }, new ulong[] { })]
        [InlineMatrixTestObject(new ulong[] { }, new ulong[] { }, new ulong[] { 1 })]
        [InlineMatrixTestObject(new ulong[] { 0, 1, 2 }, new ulong[] { 1, ulong.MaxValue }, new ulong[] { 3, 5 })]
        [InlineMatrixTestObject(new ulong[] { 0, 1, 2, ulong.MaxValue }, new ulong[] { 0, 2, ulong.MaxValue }, new ulong[] { 5, ulong.MaxValue })]
        public void XorMany_BitmapsWithDifferentValues_ReturnsSymmetricDifferenceOfBitmaps(ulong[] values1, ulong[] values2, ulong[] values3,
            TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject1 = matrix.X.GetFromValues(values1);
            using IRoaring64BitmapTestObject testObject2 = matrix.Y.GetFromValues(values2);
            using IRoaring64BitmapTestObject testObject3 = matrix.Z.GetFromValues(values3);

            // Act
            Roaring64Bitmap actual = testObject1.ReadOnlyBitmap.XorMany([testObject2.Bitmap, testObject3.Bitmap]);

            // Assert
            var tempSet = values1.Union(values2).Except(values1.Intersect(values2)).ToList();
            IEnumerable<ulong> expectedSet = values3.Union(tempSet).Except(values3.Intersect(tempSet));
            Assert.Equal(expectedSet.Order(), actual.Values);
        }
    }

    public class IXor
    {
        [Theory]
        [InlineData(new ulong[] { }, new ulong[] { })]
        [InlineData(new ulong[] { 1 }, new ulong[] { 1 })]
        [InlineData(new ulong[] { 1 }, new ulong[] { })]
        [InlineData(new ulong[] { }, new ulong[] { 1 })]
        [InlineData(new ulong[] { 0, 1, 2 }, new ulong[] { 1, ulong.MaxValue })]
        [InlineData(new ulong[] { 0, 1, 2, ulong.MaxValue }, new ulong[] { 0, 2, ulong.MaxValue })]
        public void IXor_BitmapsWithDifferentValues_ReturnsSymmetricDifferenceOfBitmaps(ulong[] values1, ulong[] values2)
        {
            // Arrange
            using Roaring64BitmapTestObject testObject1 = Roaring64BitmapTestObjectFactory.Default.GetFromValues(values1);
            using Roaring64BitmapTestObject testObject2 = Roaring64BitmapTestObjectFactory.Default.GetFromValues(values2);

            // Act
            testObject1.Bitmap.IXor(testObject2.Bitmap);

            // Assert
            Assert.Equal(values1.Union(values2).Except(values1.Intersect(values2)), testObject1.Bitmap.Values);
        }
    }
}