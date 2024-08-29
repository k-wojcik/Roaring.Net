using System.Linq;
using Roaring.Net.CRoaring;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring32BitmapTests;

public class AndTests
{
    public class And
    {
        [Theory]
        [InlineMatrixTestObject(new uint[] { }, new uint[] { })]
        [InlineMatrixTestObject(new uint[] { 1 }, new uint[] { 1 })]
        [InlineMatrixTestObject(new uint[] { 1 }, new uint[] { })]
        [InlineMatrixTestObject(new uint[] { }, new uint[] { 1 })]
        [InlineMatrixTestObject(new uint[] { 0, 1, 2 }, new uint[] { 1, uint.MaxValue })]
        [InlineMatrixTestObject(new uint[] { 0, 1, 2, uint.MaxValue }, new uint[] { 0, 2, uint.MaxValue })]
        public void And_BitmapsWithDifferentValues_ReturnsIntersectionOfBitmaps(uint[] values1, uint[] values2,
            TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject1 = matrix.X.GetFromValues(values1);
            using IRoaring32BitmapTestObject testObject2 = matrix.Y.GetFromValues(values2);

            // Act
            using Roaring32Bitmap actual = testObject1.ReadOnlyBitmap.And(testObject2.Bitmap);

            // Assert
            Assert.Equal(values1.Intersect(values2), actual.Values);
        }
    }

    public class AndCount
    {
        [Theory]
        [InlineMatrixTestObject(new uint[] { }, new uint[] { })]
        [InlineMatrixTestObject(new uint[] { 1 }, new uint[] { 1 })]
        [InlineMatrixTestObject(new uint[] { 1 }, new uint[] { })]
        [InlineMatrixTestObject(new uint[] { }, new uint[] { 1 })]
        [InlineMatrixTestObject(new uint[] { 0, 1, 2 }, new uint[] { 1, uint.MaxValue })]
        [InlineMatrixTestObject(new uint[] { 0, 1, 2, uint.MaxValue }, new uint[] { 0, 2, uint.MaxValue })]
        public void AndCount_BitmapsWithDifferentValues_ReturnsCountAfterIntersectionOfBitmaps(uint[] values1, uint[] values2,
            TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject1 = matrix.X.GetFromValues(values1);
            using IRoaring32BitmapTestObject testObject2 = matrix.Y.GetFromValues(values2);

            // Act
            var actual = testObject1.ReadOnlyBitmap.AndCount(testObject2.Bitmap);

            // Assert
            Assert.Equal((uint)values1.Intersect(values2).Count(), actual);
        }
    }

    public class IAnd
    {
        [Theory]
        [InlineData(new uint[] { }, new uint[] { })]
        [InlineData(new uint[] { 1 }, new uint[] { 1 })]
        [InlineData(new uint[] { 1 }, new uint[] { })]
        [InlineData(new uint[] { }, new uint[] { 1 })]
        [InlineData(new uint[] { 0, 1, 2 }, new uint[] { 1, uint.MaxValue })]
        [InlineData(new uint[] { 0, 1, 2, uint.MaxValue }, new uint[] { 0, 2, uint.MaxValue })]
        public void IAnd_BitmapsWithDifferentValues_ReturnsIntersectionOfBitmaps(uint[] values1, uint[] values2)
        {
            // Arrange
            using Roaring32BitmapTestObject testObject1 = Roaring32BitmapTestObjectFactory.Default.GetFromValues(values1);
            using Roaring32BitmapTestObject testObject2 = Roaring32BitmapTestObjectFactory.Default.GetFromValues(values2);

            // Act
            testObject1.Bitmap.IAnd(testObject2.Bitmap);

            // Assert
            Assert.Equal(values1.Intersect(values2), testObject1.Bitmap.Values);
        }
    }

    public class AndNot
    {
        [Theory]
        [InlineMatrixTestObject(new uint[] { }, new uint[] { })]
        [InlineMatrixTestObject(new uint[] { 1 }, new uint[] { 1 })]
        [InlineMatrixTestObject(new uint[] { 1 }, new uint[] { })]
        [InlineMatrixTestObject(new uint[] { }, new uint[] { 1 })]
        [InlineMatrixTestObject(new uint[] { 0, 1, 2 }, new uint[] { 1, uint.MaxValue })]
        [InlineMatrixTestObject(new uint[] { 0, 1, 2, uint.MaxValue }, new uint[] { 0, 2, uint.MaxValue })]
        public void AndNot_BitmapsWithDifferentValues_ReturnsDifferenceOfBitmaps(uint[] values1, uint[] values2,
            TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject1 = matrix.X.GetFromValues(values1);
            using IRoaring32BitmapTestObject testObject2 = matrix.Y.GetFromValues(values2);

            // Act
            using Roaring32Bitmap actual = testObject1.ReadOnlyBitmap.AndNot(testObject2.Bitmap);

            // Assert
            Assert.Equal(values1.Except(values2), actual.Values);
        }
    }

    public class AndNotCount
    {
        [Theory]
        [InlineMatrixTestObject(new uint[] { }, new uint[] { })]
        [InlineMatrixTestObject(new uint[] { 1 }, new uint[] { 1 })]
        [InlineMatrixTestObject(new uint[] { 1 }, new uint[] { })]
        [InlineMatrixTestObject(new uint[] { }, new uint[] { 1 })]
        [InlineMatrixTestObject(new uint[] { 0, 1, 2 }, new uint[] { 1, uint.MaxValue })]
        [InlineMatrixTestObject(new uint[] { 0, 1, 2, uint.MaxValue }, new uint[] { 0, 2, uint.MaxValue })]
        public void AndNotCount_BitmapsWithDifferentValues_ReturnsCountAfterDifferenceOfBitmaps(uint[] values1, uint[] values2,
            TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject1 = matrix.X.GetFromValues(values1);
            using IRoaring32BitmapTestObject testObject2 = matrix.Y.GetFromValues(values2);

            // Act
            var actual = testObject1.ReadOnlyBitmap.AndNotCount(testObject2.Bitmap);

            // Assert
            Assert.Equal((uint)values1.Except(values2).Count(), actual);
        }
    }

    public class IAndNot
    {
        [Theory]
        [InlineData(new uint[] { }, new uint[] { })]
        [InlineData(new uint[] { 1 }, new uint[] { 1 })]
        [InlineData(new uint[] { 1 }, new uint[] { })]
        [InlineData(new uint[] { }, new uint[] { 1 })]
        [InlineData(new uint[] { 0, 1, 2 }, new uint[] { 1, uint.MaxValue })]
        [InlineData(new uint[] { 0, 1, 2, uint.MaxValue }, new uint[] { 0, 2, uint.MaxValue })]
        public void IAndNot_BitmapsWithDifferentValues_ReturnsDifferenceOfBitmaps(uint[] values1, uint[] values2)
        {
            // Arrange
            using Roaring32BitmapTestObject testObject1 = Roaring32BitmapTestObjectFactory.Default.GetFromValues(values1);
            using Roaring32BitmapTestObject testObject2 = Roaring32BitmapTestObjectFactory.Default.GetFromValues(values2);

            // Act
            testObject1.Bitmap.IAndNot(testObject2.Bitmap);

            // Assert
            Assert.Equal(values1.Except(values2), testObject1.Bitmap.Values);
        }
    }
}