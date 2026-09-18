using System;
using System.Collections.Generic;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring32BitmapTests;

public class CountTests
{
    public class Count
    {
        [Theory]
        [InlineTestObject]
        public void Count_BitmapContainsValues_ReturnsCardinalityOfValues(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetDefault();

            // Act
            var actual = testObject.ReadOnlyBitmap.Count;

            // Assert
            Assert.Equal((ulong)testObject.Values.Length, actual);
        }

        [Theory]
        [InlineTestObject]
        public void Count_EmptyBitmap_ReturnsZero(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetEmpty();

            // Act
            var actual = testObject.ReadOnlyBitmap.Count;

            // Assert
            Assert.Equal(0U, actual);
        }
    }

    public class CountLessOrEqualTo
    {
        [Theory]
        [InlineTestObject(new uint[] { 0, 1, 2, 3, 4 }, 0, 1)]
        [InlineTestObject(new uint[] { 0, 1, 2, 3, 4 }, 4, 5)]
        [InlineTestObject(new uint[] { 4, 3, 2, 1, 0 }, 4, 5)]
        [InlineTestObject(new uint[] { 4, 3, 2, 1, 0 }, 5, 5)]
        [InlineTestObject(new uint[] { 0, 2, 4, 6, 8 }, 2, 2)]
        [InlineTestObject(new uint[] { 5, 6, 7, 8, 9 }, 2, 0)]
        [InlineTestObject(new uint[] { 5, 6, 7, 8, 9 }, 4, 0)]
        [InlineTestObject(new uint[] { 5, 6, 7, 8, 9 }, 5, 1)]
        [InlineTestObject(new uint[] { 5, 6, 7, 8, 9 }, 7, 3)]
        [InlineTestObject(new uint[] { 5, uint.MaxValue }, uint.MaxValue, 2)]
        public void CountLessOrEqualTo_ForValues_ReturnsExpectedNumberOfValues(uint[] values, uint testedValue, uint expected, IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetFromValues(values);

            // Act
            var actual = testObject.ReadOnlyBitmap.CountLessOrEqualTo(testedValue);

            // Assert
            Assert.Equal(expected, actual);
        }
    }

    public class CountManyLessOrEqualTo
    {
        [Fact]
        public void CountManyLessOrEqualTo_NullValues_ThrowsArgumentNullException()
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = Roaring32BitmapTestObjectFactory.Default.GetDefault();

            // Act && Assert
            Assert.Throws<ArgumentNullException>(() =>
                testObject.ReadOnlyBitmap.CountManyLessOrEqualTo((uint[])null!));
        }

        [Fact]
        public void CountManyLessOrEqualTo_Enumerable_NullValues_ThrowsArgumentNullException()
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = Roaring32BitmapTestObjectFactory.Default.GetDefault();

            // Act && Assert
            Assert.Throws<ArgumentNullException>(() =>
                testObject.ReadOnlyBitmap.CountManyLessOrEqualTo((IEnumerable<uint>)null!));
        }

        [Theory]
        [InlineTestObject(new uint[] { }, new uint[] { }, new ulong[] { })]
        [InlineTestObject(new uint[] { 0, 1, 2, 3, 4 }, new uint[] { }, new ulong[] { })]
        [InlineTestObject(new uint[] { 0, 1, 2, 3, 4 }, new uint[] { 0, 1, 2, 3, 4 }, new ulong[] { 1, 2, 3, 4, 5 })]
        [InlineTestObject(new uint[] { 0, 1, 2, 3, 4 }, new uint[] { 1, 2, 3, 4, 5 }, new ulong[] { 2, 3, 4, 5, 5 })]
        [InlineTestObject(new uint[] { 0, 2, 4, 6, 8 }, new uint[] { 5, 6, 7, 8, 9 }, new ulong[] { 3, 4, 4, 5, 5 })]
        [InlineTestObject(new uint[] { 10, 11, 12 }, new uint[] { 0, 1, 2, 3, 4 }, new ulong[] { 0, 0, 0, 0, 0 })]
        [InlineTestObject(new uint[] { 0, 1, 2, 3, 4, uint.MaxValue }, new uint[] { 0, 1, 2, 3, 4, uint.MaxValue }, new ulong[] { 1, 2, 3, 4, 5, 6 })]
        [InlineTestObject(new uint[] { uint.MaxValue - 1, uint.MaxValue }, new uint[] { uint.MaxValue - 1, uint.MaxValue }, new ulong[] { 1, 2 })]
        public void CountManyLessOrEqualTo_ForValues_ReturnsExpectedNumberOfValues(uint[] values, uint[] testedValues, ulong[] expected, IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetFromValues(values);

            // Act
            var actual = testObject.ReadOnlyBitmap.CountManyLessOrEqualTo(testedValues);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineTestObject(new uint[] { 0, 1, 2, 3, 4 }, new uint[] { 1, 2, 3, 4, 5 }, new ulong[] { 2, 3, 4, 5, 5 })]
        public void CountManyLessOrEqualTo_Span_ReturnsExpectedNumberOfValues(uint[] values, uint[] testedValues, ulong[] expected, IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetFromValues(values);
            Span<uint> span = testedValues;

            // Act
            var actual = testObject.ReadOnlyBitmap.CountManyLessOrEqualTo(span);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineTestObject(new uint[] { 0, 1, 2, 3, 4 }, new uint[] { 1, 2, 3, 4, 5 }, new ulong[] { 2, 3, 4, 5, 5 })]
        public void CountManyLessOrEqualTo_Memory_ReturnsExpectedNumberOfValues(uint[] values, uint[] testedValues, ulong[] expected, IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetFromValues(values);
            Memory<uint> memory = testedValues;

            // Act
            var actual = testObject.ReadOnlyBitmap.CountManyLessOrEqualTo(memory);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineTestObject(new uint[] { 0, 1, 2, 3, 4 }, new uint[] { 1, 2, 3, 4, 5 }, new ulong[] { 2, 3, 4, 5, 5 })]
        public void CountManyLessOrEqualTo_IEnumerable_ReturnsExpectedNumberOfValues(uint[] values, uint[] testedValues, ulong[] expected,
            IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetFromValues(values);
            IEnumerable<uint> enumerable = testedValues;

            // Act
            var actual = testObject.ReadOnlyBitmap.CountManyLessOrEqualTo(enumerable);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineTestObject(new uint[] { }, new uint[] { }, new ulong[] { })]
        [InlineTestObject(new uint[] { 0, 1, 2, 3, 4 }, new uint[] { }, new ulong[] { })]
        [InlineTestObject(new uint[] { 0, 1, 2, 3, 4 }, new uint[] { 0, 1, 2, 3, 4 }, new ulong[] { 1, 2, 3, 4, 5 })]
        [InlineTestObject(new uint[] { 0, 1, 2, 3, 4 }, new uint[] { 1, 2, 3, 4, 5 }, new ulong[] { 2, 3, 4, 5, 5 })]
        [InlineTestObject(new uint[] { 0, 2, 4, 6, 8 }, new uint[] { 5, 6, 7, 8, 9 }, new ulong[] { 3, 4, 4, 5, 5 })]
        [InlineTestObject(new uint[] { 10, 11, 12 }, new uint[] { 0, 1, 2, 3, 4 }, new ulong[] { 0, 0, 0, 0, 0 })]
        [InlineTestObject(new uint[] { 0, 1, 2, 3, 4, uint.MaxValue }, new uint[] { 0, 1, 2, 3, 4, uint.MaxValue }, new ulong[] { 1, 2, 3, 4, 5, 6 })]
        [InlineTestObject(new uint[] { uint.MaxValue - 1, uint.MaxValue }, new uint[] { uint.MaxValue - 1, uint.MaxValue }, new ulong[] { 1, 2 })]
        public void CountManyLessOrEqualTo_Destination_ForValues_ReturnsExpectedNumberOfValues(uint[] values, uint[] testedValues, ulong[] expected,
            IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetFromValues(values);
            ReadOnlySpan<uint> span = testedValues;
            var destination = new ulong[testedValues.Length];

            // Act
            testObject.ReadOnlyBitmap.CountManyLessOrEqualTo(span, destination);

            // Assert
            Assert.Equal(expected, destination);
        }

        [Theory]
        [InlineTestObject(new uint[] { 0, 1, 2, 3, 4 }, new uint[] { 1, 2, 3, 4, 5 })]
        public void CountManyLessOrEqualTo_Destination_DestinationTooSmall_ThrowsArgumentOutOfRangeException(uint[] values, uint[] testedValues,
            IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetFromValues(values);
            var destination = new ulong[testedValues.Length - 1];

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                testObject.ReadOnlyBitmap.CountManyLessOrEqualTo(testedValues.AsSpan(), destination.AsSpan()));
        }
    }

    public class CountRange
    {
        [Theory]
        [InlineTestObject(1, 0)]
        [InlineTestObject(10, 5)]
        public void CountRange_ArgumentsOutOfAllowedRange_ThrowsArgumentOutOfRangeException(uint start, uint end, IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetEmpty();

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => testObject.ReadOnlyBitmap.CountRange(start, end));
        }

        [Theory]
        [InlineTestObject(new uint[] { 0, 1, 2, 3, 4 }, 0, 0, 1)]
        [InlineTestObject(new uint[] { 0, 1, 2, 3, 4 }, 0, 5, 5)]
        [InlineTestObject(new uint[] { 0, 2, 4, 6, 8 }, 2, 4, 2)]
        [InlineTestObject(new uint[] { 0, 2, 4, 6, 8 }, 6, 10, 2)]
        [InlineTestObject(new uint[] { 0, 2, 4, 6, 8 }, 7, 8, 1)]
        [InlineTestObject(new uint[] { 0, 1, 2, 3, 4 }, 4, uint.MaxValue, 1)]
        [InlineTestObject(new uint[] { 0, 1, 2, 3, 4, uint.MaxValue }, uint.MinValue, uint.MaxValue, 6)]
        [InlineTestObject(new uint[] { uint.MaxValue - 1, uint.MaxValue }, uint.MinValue, uint.MaxValue, 2)]
        public void CountRange_ForValues_ReturnsExpectedNumberOfValues(uint[] values, uint start, uint end, uint expected, IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetFromValues(values);

            // Act
            var actual = testObject.ReadOnlyBitmap.CountRange(start, end);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}