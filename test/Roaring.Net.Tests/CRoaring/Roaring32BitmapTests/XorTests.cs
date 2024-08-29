using System.Collections.Generic;
using System.Linq;
using Roaring.Net.CRoaring;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring32BitmapTests;

public class XorTests
{
    public class Xor
    {
        [Theory]
        [InlineMatrixTestObject(new uint[] { }, new uint[] { })]
        [InlineMatrixTestObject(new uint[] { 1 }, new uint[] { 1 })]
        [InlineMatrixTestObject(new uint[] { 1 }, new uint[] { })]
        [InlineMatrixTestObject(new uint[] { }, new uint[] { 1 })]
        [InlineMatrixTestObject(new uint[] { 0, 1, 2 }, new uint[] { 1, uint.MaxValue })]
        [InlineMatrixTestObject(new uint[] { 0, 1, 2, uint.MaxValue }, new uint[] { 0, 2, uint.MaxValue })]
        public void Xor_BitmapsWithDifferentValues_ReturnsSymmetricDifferenceOfBitmaps(uint[] values1, uint[] values2,
            TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject1 = matrix.X.GetFromValues(values1);
            using IRoaring32BitmapTestObject testObject2 = matrix.Y.GetFromValues(values2);

            // Act
            using Roaring32Bitmap actual = testObject1.ReadOnlyBitmap.Xor(testObject2.Bitmap);

            // Assert
            Assert.Equal(values1.Union(values2).Except(values1.Intersect(values2)), actual.Values);
        }
    }

    public class XorCount
    {
        [Theory]
        [InlineMatrixTestObject(new uint[] { }, new uint[] { })]
        [InlineMatrixTestObject(new uint[] { 1 }, new uint[] { 1 })]
        [InlineMatrixTestObject(new uint[] { 1 }, new uint[] { })]
        [InlineMatrixTestObject(new uint[] { }, new uint[] { 1 })]
        [InlineMatrixTestObject(new uint[] { 0, 1, 2 }, new uint[] { 1, uint.MaxValue })]
        [InlineMatrixTestObject(new uint[] { 0, 1, 2, uint.MaxValue }, new uint[] { 0, 2, uint.MaxValue })]
        public void XorCount_BitmapsWithDifferentValues_ReturnsSymmetricDifferenceOfBitmaps(uint[] values1, uint[] values2,
            TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject1 = matrix.X.GetFromValues(values1);
            using IRoaring32BitmapTestObject testObject2 = matrix.Y.GetFromValues(values2);

            // Act
            var actual = testObject1.ReadOnlyBitmap.XorCount(testObject2.Bitmap);

            // Assert
            Assert.Equal((uint)values1.Union(values2).Except(values1.Intersect(values2)).Count(), actual);
        }
    }

    public class IXor
    {
        [Theory]
        [InlineData(new uint[] { }, new uint[] { })]
        [InlineData(new uint[] { 1 }, new uint[] { 1 })]
        [InlineData(new uint[] { 1 }, new uint[] { })]
        [InlineData(new uint[] { }, new uint[] { 1 })]
        [InlineData(new uint[] { 0, 1, 2 }, new uint[] { 1, uint.MaxValue })]
        [InlineData(new uint[] { 0, 1, 2, uint.MaxValue }, new uint[] { 0, 2, uint.MaxValue })]
        public void IXor_BitmapsWithDifferentValues_ReturnsSymmetricDifferenceOfBitmaps(uint[] values1, uint[] values2)
        {
            // Arrange
            using Roaring32BitmapTestObject testObject1 = Roaring32BitmapTestObjectFactory.Default.GetFromValues(values1);
            using Roaring32BitmapTestObject testObject2 = Roaring32BitmapTestObjectFactory.Default.GetFromValues(values2);

            // Act
            testObject1.Bitmap.IXor(testObject2.Bitmap);

            // Assert
            Assert.Equal(values1.Union(values2).Except(values1.Intersect(values2)), testObject1.Bitmap.Values);
        }
    }

    public class XorMany
    {
        [Theory]
        [InlineMatrixTestObject(new uint[] { }, new uint[] { }, new uint[] { })]
        [InlineMatrixTestObject(new uint[] { 1 }, new uint[] { 1 }, new uint[] { 1 })]
        [InlineMatrixTestObject(new uint[] { 1 }, new uint[] { }, new uint[] { })]
        [InlineMatrixTestObject(new uint[] { }, new uint[] { 1 }, new uint[] { })]
        [InlineMatrixTestObject(new uint[] { }, new uint[] { }, new uint[] { 1 })]
        [InlineMatrixTestObject(new uint[] { 0, 1, 2 }, new uint[] { 1, uint.MaxValue }, new uint[] { 3, 5 })]
        [InlineMatrixTestObject(new uint[] { 0, 1, 2, uint.MaxValue }, new uint[] { 0, 2, uint.MaxValue }, new uint[] { 5, uint.MaxValue })]
        public void XorMany_BitmapsWithDifferentValues_ReturnsSymmetricDifferenceOfBitmaps(uint[] values1, uint[] values2, uint[] values3,
            TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject1 = matrix.X.GetFromValues(values1);
            using IRoaring32BitmapTestObject testObject2 = matrix.Y.GetFromValues(values2);
            using IRoaring32BitmapTestObject testObject3 = matrix.Z.GetFromValues(values3);

            // Act
            Roaring32Bitmap actual = testObject1.ReadOnlyBitmap.XorMany([testObject2.Bitmap, testObject3.Bitmap]);

            // Assert
            var tempSet = values1.Union(values2).Except(values1.Intersect(values2)).ToList();
            IEnumerable<uint> expectedSet = values3.Union(tempSet).Except(values3.Intersect(tempSet));
            Assert.Equal(expectedSet.Order(), actual.Values);
        }
    }

    public class LazyXor
    {
        [Theory]
        [InlineMatrixTestObject(new uint[] { }, new uint[] { })]
        [InlineMatrixTestObject(new uint[] { 1 }, new uint[] { 1 })]
        [InlineMatrixTestObject(new uint[] { 0x1 }, new uint[] { })]
        [InlineMatrixTestObject(new uint[] { }, new uint[] { 1 })]
        [InlineMatrixTestObject(new uint[] { 0, 1, 2 }, new uint[] { 1, uint.MaxValue })]
        [InlineMatrixTestObject(new uint[] { 0, 1, 2, uint.MaxValue }, new uint[] { 0, 2, uint.MaxValue })]
        public void Xor_BitmapsWithDifferentValues_ReturnsSymmetricDifferenceOfBitmaps(uint[] values1, uint[] values2,
            TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject1 = matrix.X.GetFromValues(values1);
            using IRoaring32BitmapTestObject testObject2 = matrix.Y.GetFromValues(values2);

            // Act
            using Roaring32Bitmap actual = testObject1.ReadOnlyBitmap.LazyXor(testObject2.Bitmap);
            actual.RepairAfterLazy();

            // Assert
            Assert.Equal(values1.Union(values2).Except(values1.Intersect(values2)), actual.Values);
        }
    }

    public class ILazyXor
    {
        [Theory]
        [InlineData(new uint[] { }, new uint[] { })]
        [InlineData(new uint[] { 1 }, new uint[] { 1 })]
        [InlineData(new uint[] { 1 }, new uint[] { })]
        [InlineData(new uint[] { }, new uint[] { 1 })]
        [InlineData(new uint[] { 0, 1, 2 }, new uint[] { 1, uint.MaxValue })]
        [InlineData(new uint[] { 0, 1, 2, uint.MaxValue }, new uint[] { 0, 2, uint.MaxValue })]
        public void Xor_BitmapsWithDifferentValues_ReturnsSymmetricDifferenceOfBitmaps(uint[] values1, uint[] values2)
        {
            // Arrange
            using Roaring32BitmapTestObject testObject1 = Roaring32BitmapTestObjectFactory.Default.GetFromValues(values1);
            using Roaring32BitmapTestObject testObject2 = Roaring32BitmapTestObjectFactory.Default.GetFromValues(values2);

            // Act
            testObject1.Bitmap.ILazyXor(testObject2.Bitmap);
            testObject1.Bitmap.RepairAfterLazy();

            // Assert
            Assert.Equal(values1.Union(values2).Except(values1.Intersect(values2)), testObject1.Bitmap.Values);
        }
    }
}