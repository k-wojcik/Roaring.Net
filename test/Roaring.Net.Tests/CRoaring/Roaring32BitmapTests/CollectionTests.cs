using System;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring32BitmapTests;

public class CollectionTests
{
    public class ToArray
    {
        [Theory]
        [InlineTestObject(new uint[] { })]
        [InlineTestObject(new uint[] { uint.MaxValue })]
        [InlineTestObject(new uint[] { 0, 1, 2, 3, 4 })]
        public void ToArray_Always_ReturnsArrayWithExpectedValues(uint[] expected, IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetFromValues(expected);

            // Act
            var actual = testObject.ReadOnlyBitmap.ToArray();

            // Assert
            Assert.Equal(expected, actual);
        }
    }

    public class CopyTo
    {
        [Theory]
        [InlineTestObject(new uint[] { })]
        [InlineTestObject(new uint[] { uint.MaxValue })]
        [InlineTestObject(new uint[] { 0, 1, 2, 3, 4 })]
        public void CopyTo_OutputCollectionSizeEqualToNumberOfValues_ReturnsFilledCollection(uint[] expected, IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetFromValues(expected);
            uint[] actual = new uint[expected.Length];

            // Act
            testObject.ReadOnlyBitmap.CopyTo(actual);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineTestObject]
        public void CopyTo_OutputCollectionIsEmptyWhenBitmapHasValues_ThrowsArgumentOutOfRangeException(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetDefault();
            uint[] actual = [];

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                testObject.ReadOnlyBitmap.CopyTo(actual);
            });
        }

        [Theory]
        [InlineTestObject]
        public void CopyTo_OutputCollectionSizeLowerThanNumberOfValues_ThrowsArgumentOutOfRangeException(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            var input = new uint[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            using IRoaring32BitmapTestObject testObject = factory.GetFromValues(input);

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                testObject.ReadOnlyBitmap.CopyTo(new uint[input.Length - 5]);
            });
        }

        [Theory]
        [InlineTestObject]
        public void CopyTo_OutputCollectionSizeGreaterThanNumberOfValues_ReturnsFilledCollectionFromBeginning(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            var expected = new uint[] { 0, 1, 2, 3, 4, 0, 0, 0, 0, 0 };
            var input = expected[..5];
            using IRoaring32BitmapTestObject testObject = factory.GetFromValues(input);
            uint[] actual = new uint[input.Length + 5];

            // Act
            testObject.ReadOnlyBitmap.CopyTo(actual);

            // Assert
            Assert.Equal(expected, actual);
        }
    }

    public class Take
    {
        [Theory]
        [InlineTestObject(20, new uint[] { }, new uint[] { })]
        [InlineTestObject(20, new uint[] { 0, 1, 2, 3, 4 }, new uint[] { 0, 1, 2, 3, 4 })]
        [InlineTestObject(2, new uint[] { 0, 1, 2, 3, 4 }, new uint[] { 0, 1 })]
        [InlineTestObject(0, new uint[] { 0, 1, 2, 3, 4 }, new uint[] { })]
        [InlineTestObject(1, new uint[] { uint.MaxValue }, new uint[] { uint.MaxValue })]
        public void Take_ForCount_ReturnsForValuesLimitedToCount(uint count, uint[] values, uint[] expected, IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetFromValues(values);

            // Act
            var actual = testObject.ReadOnlyBitmap.Take(count);

            // Assert
            Assert.Equal(expected, actual);
        }
    }

}