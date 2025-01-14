using System;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring64BitmapTests;

public class CountTests
{
    public class Count
    {
        [Theory]
        [InlineTestObject]
        public void Count_BitmapContainsValues_ReturnsCardinalityOfValues(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetDefault();

            // Act
            var actual = testObject.ReadOnlyBitmap.Count;

            // Assert
            Assert.Equal((ulong)testObject.Values.Length, actual);
        }

        [Theory]
        [InlineTestObject]
        public void Count_EmptyBitmap_ReturnsZero(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetEmpty();

            // Act
            var actual = testObject.ReadOnlyBitmap.Count;

            // Assert
            Assert.Equal(0U, actual);
        }
    }

    public class CountLessOrEqualTo
    {
        [Theory]
        [InlineTestObject(new ulong[] { 0, 1, 2, 3, 4 }, 0, 1)]
        [InlineTestObject(new ulong[] { 0, 1, 2, 3, 4 }, 4, 5)]
        [InlineTestObject(new ulong[] { 4, 3, 2, 1, 0 }, 4, 5)]
        [InlineTestObject(new ulong[] { 4, 3, 2, 1, 0 }, 5, 5)]
        [InlineTestObject(new ulong[] { 0, 2, 4, 6, 8 }, 2, 2)]
        [InlineTestObject(new ulong[] { 5, 6, 7, 8, 9 }, 2, 0)]
        [InlineTestObject(new ulong[] { 5, 6, 7, 8, 9 }, 4, 0)]
        [InlineTestObject(new ulong[] { 5, 6, 7, 8, 9 }, 5, 1)]
        [InlineTestObject(new ulong[] { 5, 6, 7, 8, 9 }, 7, 3)]
        [InlineTestObject(new ulong[] { 5, ulong.MaxValue }, ulong.MaxValue, 2)]
        public void CountLessOrEqualTo_ForValues_ReturnsExpectedNumberOfValues(ulong[] values, ulong testedValue, ulong expected, IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetFromValues(values);

            // Act
            var actual = testObject.ReadOnlyBitmap.CountLessOrEqualTo(testedValue);

            // Assert
            Assert.Equal(expected, actual);
        }
    }

    public class CountRange
    {
        [Theory]
        [InlineTestObject(1, 0)]
        [InlineTestObject(10, 5)]
        public void CountRange_ArgumentsOutOfAllowedRange_ThrowsArgumentOutOfRangeException(uint start, uint end, IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetEmpty();

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => testObject.ReadOnlyBitmap.CountRange(start, end));
        }

        [Theory]
        [InlineTestObject(new ulong[] { 0, 1, 2, 3, 4 }, 0, 0, 1)]
        [InlineTestObject(new ulong[] { 0, 1, 2, 3, 4 }, 0, 5, 5)]
        [InlineTestObject(new ulong[] { 0, 2, 4, 6, 8 }, 2, 4, 2)]
        [InlineTestObject(new ulong[] { 0, 2, 4, 6, 8 }, 6, 10, 2)]
        [InlineTestObject(new ulong[] { 0, 2, 4, 6, 8 }, 7, 8, 1)]
        [InlineTestObject(new ulong[] { 0, 1, 2, 3, 4 }, 4, ulong.MaxValue, 1)]
        [InlineTestObject(new ulong[] { 0, 1, 2, 3, 4, ulong.MaxValue }, ulong.MinValue, ulong.MaxValue, 6)]
        [InlineTestObject(new ulong[] { ulong.MaxValue - 1, ulong.MaxValue }, ulong.MinValue, ulong.MaxValue, 2)]
        public void CountRange_ForValues_ReturnsExpectedNumberOfValues(ulong[] values, ulong start, ulong end, ulong expected, IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetFromValues(values);

            // Act
            var actual = testObject.ReadOnlyBitmap.CountRange(start, end);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}