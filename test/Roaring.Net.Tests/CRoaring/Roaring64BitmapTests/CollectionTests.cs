using System;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring64BitmapTests;

public class CollectionTests
{
    public class ToArray
    {
        [Theory]
        [InlineTestObject(new ulong[] { })]
        [InlineTestObject(new ulong[] { ulong.MaxValue })]
        [InlineTestObject(new ulong[] { 0, 1, 2, 3, 4 })]
        public void ToArray_Always_ReturnsArrayWithExpectedValues(ulong[] expected, IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetFromValues(expected);

            // Act
            var actual = testObject.ReadOnlyBitmap.ToArray();

            // Assert
            Assert.Equal(expected, actual);
        }
    }

    public class CopyToArray
    {
        [Theory]
        [InlineTestObject(new ulong[] { })]
        [InlineTestObject(new ulong[] { ulong.MaxValue })]
        [InlineTestObject(new ulong[] { 0, 1, 2, 3, 4 })]
        public void CopyTo_OutputCollectionSizeEqualToNumberOfValues_ReturnsFilledCollection(ulong[] expected, IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetFromValues(expected);
            ulong[] actual = new ulong[expected.Length];

            // Act
            testObject.ReadOnlyBitmap.CopyTo(actual);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineTestObject]
        public void CopyTo_OutputCollectionIsEmptyWhenBitmapHasValues_ThrowsArgumentOutOfRangeException(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetDefault();
            ulong[] actual = [];

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                testObject.ReadOnlyBitmap.CopyTo(actual);
            });
        }

        [Theory]
        [InlineTestObject]
        public void CopyTo_OutputCollectionSizeLowerThanNumberOfValues_ThrowsArgumentOutOfRangeException(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            var input = new ulong[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            using IRoaring64BitmapTestObject testObject = factory.GetFromValues(input);

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                testObject.ReadOnlyBitmap.CopyTo(new ulong[input.Length - 5]);
            });
        }

        [Theory]
        [InlineTestObject]
        public void CopyTo_OutputCollectionSizeGreaterThanNumberOfValues_ReturnsFilledCollectionFromBeginning(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            var expected = new ulong[] { 0, 1, 2, 3, 4, 0, 0, 0, 0, 0 };
            var input = expected[..5];
            using IRoaring64BitmapTestObject testObject = factory.GetFromValues(input);
            ulong[] actual = new ulong[input.Length + 5];

            // Act
            testObject.ReadOnlyBitmap.CopyTo(actual);

            // Assert
            Assert.Equal(expected, actual);
        }
    }

    public class CopyToMemory
    {
        [Theory]
        [InlineTestObject(new ulong[] { })]
        [InlineTestObject(new ulong[] { ulong.MaxValue })]
        [InlineTestObject(new ulong[] { 0, 1, 2, 3, 4 })]
        public void CopyTo_OutputCollectionSizeEqualToNumberOfValues_ReturnsFilledCollection(ulong[] expected, IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetFromValues(expected);
            ulong[] actual = new ulong[expected.Length];

            // Act
            testObject.ReadOnlyBitmap.CopyTo(actual.AsMemory());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineTestObject]
        public void CopyTo_OutputCollectionIsEmptyWhenBitmapHasValues_ThrowsArgumentOutOfRangeException(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetDefault();
            ulong[] actual = [];

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                testObject.ReadOnlyBitmap.CopyTo(actual.AsMemory());
            });
        }

        [Theory]
        [InlineTestObject]
        public void CopyTo_OutputCollectionSizeLowerThanNumberOfValues_ThrowsArgumentOutOfRangeException(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            var input = new ulong[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            using IRoaring64BitmapTestObject testObject = factory.GetFromValues(input);

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                testObject.ReadOnlyBitmap.CopyTo(new ulong[input.Length - 5].AsMemory());
            });
        }

        [Theory]
        [InlineTestObject]
        public void CopyTo_OutputCollectionSizeGreaterThanNumberOfValues_ReturnsFilledCollectionFromBeginning(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            var expected = new ulong[] { 0, 1, 2, 3, 4, 0, 0, 0, 0, 0 };
            var input = expected[..5];
            using IRoaring64BitmapTestObject testObject = factory.GetFromValues(input);
            ulong[] actual = new ulong[input.Length + 5];

            // Act
            testObject.ReadOnlyBitmap.CopyTo(actual.AsMemory());

            // Assert
            Assert.Equal(expected, actual);
        }
    }

    public class CopyToSpan
    {
        [Theory]
        [InlineTestObject(new ulong[] { })]
        [InlineTestObject(new ulong[] { ulong.MaxValue })]
        [InlineTestObject(new ulong[] { 0, 1, 2, 3, 4 })]
        public void CopyTo_OutputCollectionSizeEqualToNumberOfValues_ReturnsFilledCollection(ulong[] expected, IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetFromValues(expected);
            ulong[] actual = new ulong[expected.Length];

            // Act
            testObject.ReadOnlyBitmap.CopyTo(actual.AsSpan());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineTestObject]
        public void CopyTo_OutputCollectionIsEmptyWhenBitmapHasValues_ThrowsArgumentOutOfRangeException(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetDefault();
            ulong[] actual = [];

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                testObject.ReadOnlyBitmap.CopyTo(actual.AsSpan());
            });
        }

        [Theory]
        [InlineTestObject]
        public void CopyTo_OutputCollectionSizeLowerThanNumberOfValues_ThrowsArgumentOutOfRangeException(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            var input = new ulong[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            using IRoaring64BitmapTestObject testObject = factory.GetFromValues(input);

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                testObject.ReadOnlyBitmap.CopyTo(new ulong[input.Length - 5].AsSpan());
            });
        }

        [Theory]
        [InlineTestObject]
        public void CopyTo_OutputCollectionSizeGreaterThanNumberOfValues_ReturnsFilledCollectionFromBeginning(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            var expected = new ulong[] { 0, 1, 2, 3, 4, 0, 0, 0, 0, 0 };
            var input = expected[..5];
            using IRoaring64BitmapTestObject testObject = factory.GetFromValues(input);
            ulong[] actual = new ulong[input.Length + 5];

            // Act
            testObject.ReadOnlyBitmap.CopyTo(actual.AsSpan());

            // Assert
            Assert.Equal(expected, actual);
        }
    }

    public class Take
    {
        [Theory]
        [InlineTestObject(20, new ulong[] { }, new ulong[] { })]
        [InlineTestObject(20, new ulong[] { 0, 1, 2, 3, 4 }, new ulong[] { 0, 1, 2, 3, 4 })]
        [InlineTestObject(2, new ulong[] { 0, 1, 2, 3, 4 }, new ulong[] { 0, 1 })]
        [InlineTestObject(0, new ulong[] { 0, 1, 2, 3, 4 }, new ulong[] { })]
        [InlineTestObject(1, new ulong[] { ulong.MaxValue }, new ulong[] { ulong.MaxValue })]
        public void Take_ForCount_ReturnsForValuesLimitedToCount(ulong count, ulong[] values, ulong[] expected, IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetFromValues(values);

            // Act
            var actual = testObject.ReadOnlyBitmap.Take(count);

            // Assert
            Assert.Equal(expected, actual);
        }
    }

}