using System.Linq;
using Roaring.Net.CRoaring;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring32BitmapTests;

public class OrTests
{
    public class Or
    {
        [Theory]
        [InlineMatrixTestObject(new uint[] { }, new uint[] { })]
        [InlineMatrixTestObject(new uint[] { 1 }, new uint[] { 1 })]
        [InlineMatrixTestObject(new uint[] { 1 }, new uint[] { })]
        [InlineMatrixTestObject(new uint[] { }, new uint[] { 1 })]
        [InlineMatrixTestObject(new uint[] { 0, 1, 2 }, new uint[] { 1, uint.MaxValue })]
        [InlineMatrixTestObject(new uint[] { 0, 1, 2, uint.MaxValue }, new uint[] { 0, 2, uint.MaxValue })]
        public void Or_BitmapsWithDifferentValues_ReturnsUnionOfBitmaps(uint[] values1, uint[] values2,
            TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject1 = matrix.X.GetFromValues(values1);
            using IRoaring32BitmapTestObject testObject2 = matrix.Y.GetFromValues(values2);

            // Act
            using Roaring32Bitmap actual = testObject1.ReadOnlyBitmap.Or(testObject2.Bitmap);

            // Assert
            Assert.Equal(values1.Union(values2), actual.Values);
        }
    }

    public class OrCount
    {
        [Theory]
        [InlineMatrixTestObject(new uint[] { }, new uint[] { })]
        [InlineMatrixTestObject(new uint[] { 1 }, new uint[] { 1 })]
        [InlineMatrixTestObject(new uint[] { 1 }, new uint[] { })]
        [InlineMatrixTestObject(new uint[] { }, new uint[] { 1 })]
        [InlineMatrixTestObject(new uint[] { 0, 1, 2 }, new uint[] { 1, uint.MaxValue })]
        [InlineMatrixTestObject(new uint[] { 0, 1, 2, uint.MaxValue }, new uint[] { 0, 2, uint.MaxValue })]
        public void Or_BitmapsWithDifferentValues_ReturnsCountAfterUnionOfBitmaps(uint[] values1, uint[] values2,
            TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject1 = matrix.X.GetFromValues(values1);
            using IRoaring32BitmapTestObject testObject2 = matrix.Y.GetFromValues(values2);

            // Act
            var actual = testObject1.ReadOnlyBitmap.OrCount(testObject2.Bitmap);

            // Assert
            Assert.Equal((ulong)values1.Union(values2).Count(), actual);
        }
    }

    public class OrMany
    {
        [Theory]
        [InlineMatrixTestObject(new uint[] { }, new uint[] { }, new uint[] { })]
        [InlineMatrixTestObject(new uint[] { 1 }, new uint[] { 1 }, new uint[] { 1 })]
        [InlineMatrixTestObject(new uint[] { 1 }, new uint[] { }, new uint[] { })]
        [InlineMatrixTestObject(new uint[] { }, new uint[] { 1 }, new uint[] { })]
        [InlineMatrixTestObject(new uint[] { }, new uint[] { }, new uint[] { 1 })]
        [InlineMatrixTestObject(new uint[] { 0, 1, 2 }, new uint[] { 1, uint.MaxValue }, new uint[] { 3, 5 })]
        [InlineMatrixTestObject(new uint[] { 0, 1, 2, uint.MaxValue }, new uint[] { 0, 2, uint.MaxValue }, new uint[] { 5, uint.MaxValue })]
        public void OrMany_BitmapsWithDifferentValues_ReturnsUnionOfBitmaps(uint[] values1, uint[] values2, uint[] values3,
            TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject1 = matrix.X.GetFromValues(values1);
            using IRoaring32BitmapTestObject testObject2 = matrix.Y.GetFromValues(values2);
            using IRoaring32BitmapTestObject testObject3 = matrix.Z.GetFromValues(values3);

            // Act
            Roaring32Bitmap actual = testObject1.ReadOnlyBitmap.OrMany([testObject2.Bitmap, testObject3.Bitmap]);

            // Assert
            Assert.Equal(values1.Union(values2).Union(values3).Order(), actual.Values);
        }
    }

    public class OrManyHeap
    {
        [Theory]
        [InlineMatrixTestObject(new uint[] { }, new uint[] { }, new uint[] { })]
        [InlineMatrixTestObject(new uint[] { 1 }, new uint[] { 1 }, new uint[] { 1 })]
        [InlineMatrixTestObject(new uint[] { 1 }, new uint[] { }, new uint[] { })]
        [InlineMatrixTestObject(new uint[] { }, new uint[] { 1 }, new uint[] { })]
        [InlineMatrixTestObject(new uint[] { }, new uint[] { }, new uint[] { 1 })]
        [InlineMatrixTestObject(new uint[] { 0, 1, 2 }, new uint[] { 1, uint.MaxValue }, new uint[] { 3, 5 })]
        [InlineMatrixTestObject(new uint[] { 0, 1, 2, uint.MaxValue }, new uint[] { 0, 2, uint.MaxValue }, new uint[] { 5, uint.MaxValue })]
        public void OrManyHeap_BitmapsWithDifferentValues_ReturnsUnionOfBitmaps(uint[] values1, uint[] values2, uint[] values3,
            TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject1 = matrix.X.GetFromValues(values1);
            using IRoaring32BitmapTestObject testObject2 = matrix.Y.GetFromValues(values2);
            using IRoaring32BitmapTestObject testObject3 = matrix.Z.GetFromValues(values3);

            // Act
            Roaring32Bitmap actual = testObject1.ReadOnlyBitmap.OrManyHeap([testObject2.Bitmap, testObject3.Bitmap]);

            // Assert
            Assert.Equal(values1.Union(values2).Union(values3).Order(), actual.Values);
        }
    }

    public class IOr
    {
        [Theory]
        [InlineData(new uint[] { }, new uint[] { })]
        [InlineData(new uint[] { 1 }, new uint[] { 1 })]
        [InlineData(new uint[] { 1 }, new uint[] { })]
        [InlineData(new uint[] { }, new uint[] { 1 })]
        [InlineData(new uint[] { 0, 1, 2 }, new uint[] { 1, uint.MaxValue })]
        [InlineData(new uint[] { 0, 1, 2, uint.MaxValue }, new uint[] { 0, 2, uint.MaxValue })]
        public void IOr_BitmapsWithDifferentValues_ReturnsUnionOfBitmaps(uint[] values1, uint[] values2)
        {
            // Arrange
            using Roaring32BitmapTestObject testObject1 = Roaring32BitmapTestObjectFactory.Default.GetFromValues(values1);
            using Roaring32BitmapTestObject testObject2 = Roaring32BitmapTestObjectFactory.Default.GetFromValues(values2);

            // Act
            testObject1.Bitmap.IOr(testObject2.Bitmap);

            // Assert
            Assert.Equal(values1.Union(values2), testObject1.Bitmap.Values);
        }
    }

    public class LazyOr
    {
        [Theory]
        [InlineMatrixTestObject(new uint[] { }, new uint[] { })]
        [InlineMatrixTestObject(new uint[] { 1 }, new uint[] { 1 })]
        [InlineMatrixTestObject(new uint[] { 1 }, new uint[] { })]
        [InlineMatrixTestObject(new uint[] { }, new uint[] { 1 })]
        [InlineMatrixTestObject(new uint[] { 0, 1, 2 }, new uint[] { 1, uint.MaxValue })]
        [InlineMatrixTestObject(new uint[] { 0, 1, 2, uint.MaxValue }, new uint[] { 0, 2, uint.MaxValue })]
        public void LazyOr_BitmapsWithDifferentValues_ReturnsUnionOfBitmaps(uint[] values1, uint[] values2,
            TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject1 = matrix.X.GetFromValues(values1);
            using IRoaring32BitmapTestObject testObject2 = matrix.Y.GetFromValues(values2);

            // Act
            using Roaring32Bitmap actual = testObject1.ReadOnlyBitmap.LazyOr(testObject2.Bitmap, true);
            actual.RepairAfterLazy();

            // Assert
            Assert.Equal(values1.Union(values2), actual.Values);
        }
    }

    public class ILazyOr
    {
        [Theory]
        [InlineData(new uint[] { }, new uint[] { })]
        [InlineData(new uint[] { 1 }, new uint[] { 1 })]
        [InlineData(new uint[] { 1 }, new uint[] { })]
        [InlineData(new uint[] { }, new uint[] { 1 })]
        [InlineData(new uint[] { 0, 1, 2 }, new uint[] { 1, uint.MaxValue })]
        [InlineData(new uint[] { 0, 1, 2, uint.MaxValue }, new uint[] { 0, 2, uint.MaxValue })]
        public void IOr_BitmapsWithDifferentValues_ReturnsUnionOfBitmaps(uint[] values1, uint[] values2)
        {
            // Arrange
            using Roaring32BitmapTestObject testObject1 = Roaring32BitmapTestObjectFactory.Default.GetFromValues(values1);
            using Roaring32BitmapTestObject testObject2 = Roaring32BitmapTestObjectFactory.Default.GetFromValues(values2);

            // Act
            testObject1.Bitmap.ILazyOr(testObject2.Bitmap, true);
            testObject1.Bitmap.RepairAfterLazy();

            // Assert
            Assert.Equal(values1.Union(values2), testObject1.Bitmap.Values);
        }
    }
}