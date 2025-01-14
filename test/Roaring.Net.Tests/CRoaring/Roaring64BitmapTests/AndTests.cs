using System.Linq;
using Roaring.Net.CRoaring;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring64BitmapTests;

public class AndTests
{
    public class And
    {
        [Theory]
        [InlineMatrixTestObject(new ulong[] { }, new ulong[] { })]
        [InlineMatrixTestObject(new ulong[] { 1 }, new ulong[] { 1 })]
        [InlineMatrixTestObject(new ulong[] { 1 }, new ulong[] { })]
        [InlineMatrixTestObject(new ulong[] { }, new ulong[] { 1 })]
        [InlineMatrixTestObject(new ulong[] { 0, 1, 2 }, new ulong[] { 1, ulong.MaxValue })]
        [InlineMatrixTestObject(new ulong[] { 0, 1, 2, ulong.MaxValue }, new ulong[] { 0, 2, ulong.MaxValue })]
        public void And_BitmapsWithDifferentValues_ReturnsIntersectionOfBitmaps(ulong[] values1, ulong[] values2,
            TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject1 = matrix.X.GetFromValues(values1);
            using IRoaring64BitmapTestObject testObject2 = matrix.Y.GetFromValues(values2);

            // Act
            using Roaring64Bitmap actual = testObject1.ReadOnlyBitmap.And(testObject2.Bitmap);

            // Assert
            Assert.Equal(values1.Intersect(values2), actual.Values);
        }
    }

    public class AndCount
    {
        [Theory]
        [InlineMatrixTestObject(new ulong[] { }, new ulong[] { })]
        [InlineMatrixTestObject(new ulong[] { 1 }, new ulong[] { 1 })]
        [InlineMatrixTestObject(new ulong[] { 1 }, new ulong[] { })]
        [InlineMatrixTestObject(new ulong[] { }, new ulong[] { 1 })]
        [InlineMatrixTestObject(new ulong[] { 0, 1, 2 }, new ulong[] { 1, ulong.MaxValue })]
        [InlineMatrixTestObject(new ulong[] { 0, 1, 2, ulong.MaxValue }, new ulong[] { 0, 2, ulong.MaxValue })]
        public void AndCount_BitmapsWithDifferentValues_ReturnsCountAfterIntersectionOfBitmaps(ulong[] values1, ulong[] values2,
            TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject1 = matrix.X.GetFromValues(values1);
            using IRoaring64BitmapTestObject testObject2 = matrix.Y.GetFromValues(values2);

            // Act
            var actual = testObject1.ReadOnlyBitmap.AndCount(testObject2.Bitmap);

            // Assert
            Assert.Equal((ulong)values1.Intersect(values2).LongCount(), actual);
        }
    }

    public class IAnd
    {
        [Theory]
        [InlineData(new ulong[] { }, new ulong[] { })]
        [InlineData(new ulong[] { 1 }, new ulong[] { 1 })]
        [InlineData(new ulong[] { 1 }, new ulong[] { })]
        [InlineData(new ulong[] { }, new ulong[] { 1 })]
        [InlineData(new ulong[] { 0, 1, 2 }, new ulong[] { 1, ulong.MaxValue })]
        [InlineData(new ulong[] { 0, 1, 2, ulong.MaxValue }, new ulong[] { 0, 2, ulong.MaxValue })]
        public void IAnd_BitmapsWithDifferentValues_ReturnsIntersectionOfBitmaps(ulong[] values1, ulong[] values2)
        {
            // Arrange
            using Roaring64BitmapTestObject testObject1 = Roaring64BitmapTestObjectFactory.Default.GetFromValues(values1);
            using Roaring64BitmapTestObject testObject2 = Roaring64BitmapTestObjectFactory.Default.GetFromValues(values2);

            // Act
            testObject1.Bitmap.IAnd(testObject2.Bitmap);

            // Assert
            Assert.Equal(values1.Intersect(values2), testObject1.Bitmap.Values);
        }
    }

    public class AndNot
    {
        [Theory]
        [InlineMatrixTestObject(new ulong[] { }, new ulong[] { })]
        [InlineMatrixTestObject(new ulong[] { 1 }, new ulong[] { 1 })]
        [InlineMatrixTestObject(new ulong[] { 1 }, new ulong[] { })]
        [InlineMatrixTestObject(new ulong[] { }, new ulong[] { 1 })]
        [InlineMatrixTestObject(new ulong[] { 0, 1, 2 }, new ulong[] { 1, ulong.MaxValue })]
        [InlineMatrixTestObject(new ulong[] { 0, 1, 2, ulong.MaxValue }, new ulong[] { 0, 2, ulong.MaxValue })]
        public void AndNot_BitmapsWithDifferentValues_ReturnsDifferenceOfBitmaps(ulong[] values1, ulong[] values2,
            TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject1 = matrix.X.GetFromValues(values1);
            using IRoaring64BitmapTestObject testObject2 = matrix.Y.GetFromValues(values2);

            // Act
            using Roaring64Bitmap actual = testObject1.ReadOnlyBitmap.AndNot(testObject2.Bitmap);

            // Assert
            Assert.Equal(values1.Except(values2), actual.Values);
        }
    }

    public class AndNotCount
    {
        [Theory]
        [InlineMatrixTestObject(new ulong[] { }, new ulong[] { })]
        [InlineMatrixTestObject(new ulong[] { 1 }, new ulong[] { 1 })]
        [InlineMatrixTestObject(new ulong[] { 1 }, new ulong[] { })]
        [InlineMatrixTestObject(new ulong[] { }, new ulong[] { 1 })]
        [InlineMatrixTestObject(new ulong[] { 0, 1, 2 }, new ulong[] { 1, ulong.MaxValue })]
        [InlineMatrixTestObject(new ulong[] { 0, 1, 2, ulong.MaxValue }, new ulong[] { 0, 2, ulong.MaxValue })]
        public void AndNotCount_BitmapsWithDifferentValues_ReturnsCountAfterDifferenceOfBitmaps(ulong[] values1, ulong[] values2,
            TestObjectMatrix<IRoaring64BitmapTestObjectFactory, IRoaring64BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject1 = matrix.X.GetFromValues(values1);
            using IRoaring64BitmapTestObject testObject2 = matrix.Y.GetFromValues(values2);

            // Act
            var actual = testObject1.ReadOnlyBitmap.AndNotCount(testObject2.Bitmap);

            // Assert
            Assert.Equal((ulong)values1.Except(values2).LongCount(), actual);
        }
    }

    public class IAndNot
    {
        [Theory]
        [InlineData(new ulong[] { }, new ulong[] { })]
        [InlineData(new ulong[] { 1 }, new ulong[] { 1 })]
        [InlineData(new ulong[] { 1 }, new ulong[] { })]
        [InlineData(new ulong[] { }, new ulong[] { 1 })]
        [InlineData(new ulong[] { 0, 1, 2 }, new ulong[] { 1, ulong.MaxValue })]
        [InlineData(new ulong[] { 0, 1, 2, ulong.MaxValue }, new ulong[] { 0, 2, ulong.MaxValue })]
        public void IAndNot_BitmapsWithDifferentValues_ReturnsDifferenceOfBitmaps(ulong[] values1, ulong[] values2)
        {
            // Arrange
            using Roaring64BitmapTestObject testObject1 = Roaring64BitmapTestObjectFactory.Default.GetFromValues(values1);
            using Roaring64BitmapTestObject testObject2 = Roaring64BitmapTestObjectFactory.Default.GetFromValues(values2);

            // Act
            testObject1.Bitmap.IAndNot(testObject2.Bitmap);

            // Assert
            Assert.Equal(values1.Except(values2), testObject1.Bitmap.Values);
        }
    }
}