using System;
using System.Collections.Generic;
using System.Linq;
using Roaring.Net.CRoaring;
using Roaring.Net.Tests.CRoaring.Roaring32BitmapTests;
using Roaring.Net.Tests.Extensions;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring64BitmapTests;

public class InitializationTests
{
    public class Constructor
    {
        [Fact]
        public void Ctor_NullValues_ThrowsArgumentNullException()
        {
            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => new Roaring64Bitmap((ulong[])null!));
        }

        [Fact]
        public void Ctor_Enumerable_NullValues_ThrowsArgumentNullException()
        {
            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => new Roaring64Bitmap((IEnumerable<ulong>)null!));
        }

        [Fact]
        public void Ctor_Enumerable_InputHasValues_BitmapContainsExpectedValues()
        {
            // Arrange
            IEnumerable<ulong> expected = [1UL, 2UL, 3UL, ulong.MaxValue];

            // Act
            using var uut = new Roaring64Bitmap(expected);

            // Assert
            Assert.Equal(expected, uut.Values.ToArray());
        }

        [Fact]
        public void Ctor_Enumerable_InputIsEmpty_BitmapIsEmpty()
        {
            // Arrange
            IEnumerable<ulong> expected = [];

            // Act
            using var uut = new Roaring64Bitmap(expected);

            // Assert
            Assert.Empty(uut.Values);
        }

        [Fact]
        public void Ctor_ZeroIntPtr_ThrowsInvalidOperationException()
        {
            // Act && Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                using var uut = new Roaring64Bitmap(IntPtr.Zero);
            });
        }

        [Fact]
        public void Ctor_Default_CreatesBitmapWithZeroCapacity()
        {
            // Act
            using var uut = new Roaring64Bitmap();

            // Assert
            var actual = uut.GetSerializationSize();
            Assert.True(actual > 0);
        }

        [Fact]
        public void Ctor_FromValues_InputHasValues_BitmapContainsExpectedValues()
        {
            // Arrange
            var expected = Enumerable.Range(0, 100)
                .Select(x => (ulong)x)
                .Concat([ulong.MaxValue - 1, ulong.MaxValue])
                .ToArray();

            // Act
            using var uut = new Roaring64Bitmap(expected);

            // Assert
            IEnumerable<ulong> actual = uut.Values;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Ctor_FromValues_InputIsEmpty_BitmapIsEmpty()
        {
            // Act
            using var uut = new Roaring64Bitmap([]);

            // Assert
            IEnumerable<ulong> actual = uut.Values;

            Assert.Empty(actual);
        }

        [Fact]
        public void Ctor_FromReadOnlySpan_InputHasValues_BitmapContainsExpectedValues()
        {
            // Arrange
            var expected = new[] { 1UL, 2UL, 3UL, ulong.MaxValue };
            ReadOnlySpan<ulong> values = expected;

            // Act
            using var uut = new Roaring64Bitmap(values);

            // Assert
            Assert.Equal(expected, uut.Values.ToArray());
        }

        [Fact]
        public void Ctor_FromReadOnlyMemory_InputHasValues_BitmapContainsExpectedValues()
        {
            // Arrange
            var expected = new[] { 1UL, 2UL, 3UL, ulong.MaxValue };
            ReadOnlyMemory<ulong> values = expected;

            // Act
            using var uut = new Roaring64Bitmap(values);

            // Assert
            Assert.Equal(expected, uut.Values.ToArray());
        }
    }

    public class FromRange
    {
        [Theory]
        [InlineData(1, 0, 1)]
        [InlineData(10, 5, 1)]
        [InlineData(1, 10, 0)]
        public void FromRange_ArgumentsOutOfAllowedRange_ThrowsArgumentOutOfRangeException(ulong start, ulong end,
            ulong step)
        {
            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => Roaring64Bitmap.FromRange(start, end, step));
        }

        [Theory]
        [InlineData(0, 0, 1)]
        [InlineData(1, 1, 1)]
        [InlineData(10, 100, 2)]
        [InlineData(33, 333, 33)]
        [InlineData(ulong.MaxValue - 1, ulong.MaxValue, 1)]
        [InlineData(ulong.MaxValue, ulong.MaxValue, 1)]
        [InlineData(ulong.MaxValue - 1, ulong.MaxValue, 2)]
        [InlineData(ulong.MaxValue - 2, ulong.MaxValue, 2)]
        [InlineData(ulong.MaxValue - 3, ulong.MaxValue, 3)]
        [InlineData(ulong.MaxValue - 15 * 9, ulong.MaxValue, 15)]
        [InlineData(ulong.MaxValue - 15 * 9, ulong.MaxValue - 1, 15)]
        [InlineData(1, ulong.MaxValue, ulong.MaxValue / 2)]
        public void FromRange_CorrectRange_BitmapContainsExpectedValues(ulong start, ulong end, ulong step)
        {
            // Act
            using var uut = Roaring64Bitmap.FromRange(start, end, step);

            // Assert
            var expected = EnumerableRange.Range(0, (ulong)Math.Floor((end - start) / (double)step) + 1)
                .Select(x => start + x * step)
                .ToList();
            var actual = uut.Values.ToList();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FromRange_StepGreaterThanRange_BitmapContainsMinValueOfRange()
        {
            // Act
            using var uut = Roaring64Bitmap.FromRange(1, 10, 100);

            // Assert
            IEnumerable<ulong> actual = uut.Values;

            Assert.Equal([1], actual);
        }
    }

    public class FromValues
    {
        [Fact]
        public void FromValues_InputHasValues_BitmapContainsExpectedValues()
        {
            // Arrange
            var expected = Enumerable.Range(0, 100)
                .Select(x => (ulong)x)
                .Concat([ulong.MaxValue - 1, ulong.MaxValue])
                .ToArray();

            // Act
            using var uut = Roaring64Bitmap.FromValues(expected);

            // Assert
            IEnumerable<ulong> actual = uut.Values;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FromValues_NullValues_ThrowsArgumentNullException()
        {
            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => Roaring64Bitmap.FromValues((ulong[])null!));
        }

        [Fact]
        public void FromValues_Enumerable_NullValues_ThrowsArgumentNullException()
        {
            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => Roaring64Bitmap.FromValues((IEnumerable<ulong>)null!));
        }

        [Fact]
        public void FromValues_WithOffset_NullValues_ThrowsArgumentNullException()
        {
            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => Roaring64Bitmap.FromValues((ulong[])null!, 0, 0));
        }

        [Fact]
        public void FromValues_ReadOnlySpan_InputHasValues_BitmapContainsExpectedValues()
        {
            // Arrange
            var expected = new[] { 1UL, 2UL, 3UL, ulong.MaxValue };
            ReadOnlySpan<ulong> values = expected;

            // Act
            using var uut = Roaring64Bitmap.FromValues(values);

            // Assert
            Assert.Equal(expected, uut.Values.ToArray());
        }

        [Fact]
        public void FromValues_ReadOnlyMemory_InputHasValues_BitmapContainsExpectedValues()
        {
            // Arrange
            var expected = new[] { 1UL, 2UL, 3UL, ulong.MaxValue };
            ReadOnlyMemory<ulong> values = expected;

            // Act
            using var uut = Roaring64Bitmap.FromValues(values);

            // Assert
            Assert.Equal(expected, uut.Values.ToArray());
        }

        [Fact]
        public void FromValues_Enumerable_InputHasValues_BitmapContainsExpectedValues()
        {
            // Arrange
            List<ulong> expected = [1UL, 2UL, 3UL, ulong.MaxValue];

            // Act
            using var uut = Roaring64Bitmap.FromValues((IEnumerable<ulong>)expected);

            // Assert
            Assert.Equal(expected, uut.Values.ToArray());
        }

        [Fact]
        public void FromValues_InputIsEmpty_BitmapIsEmpty()
        {
            // Act
            using var uut = Roaring64Bitmap.FromValues([]);

            // Assert
            IEnumerable<ulong> actual = uut.Values;

            Assert.Empty(actual);
        }

        [Theory]
        [InlineData(0, 100)]
        [InlineData(99, 1)]
        [InlineData(90, 10)]
        [InlineData(10, 25)]
        [InlineData(0, 0)]
        public void FromValues_WithCorrectOffsetAndCount_BitmapContainsExpectedValues(int offset, int count)
        {
            // Arrange
            var values = Enumerable.Range(0, 100)
                .Select(x => (ulong)x)
                .ToArray();

            // Act
            using var uut = Roaring64Bitmap.FromValues(values, (nuint)offset, (nuint)count);

            // Assert
            IEnumerable<ulong> actual = uut.Values;
            IEnumerable<ulong> expected = values.Skip(offset).Take(count);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 101)]
        [InlineData(100, 1)]
        [InlineData(90, 11)]
        public void FromValues_OffsetAndCountOutOfAllowedRange_ThrowsArgumentOutOfRangeException(uint offset,
            uint count)
        {
            // Arrange
            var values = Enumerable.Range(0, 100)
                .Select(x => (ulong)x)
                .ToArray();

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                using var uut = Roaring64Bitmap.FromValues(values, offset, count);
            });
        }
    }

    public class FromBitmap
    {
        [Fact]
        public void FromBitmap_SourceBitmapIsEmpty_ReturnsEmptyBitmap()
        {
            // Arrange
            using IRoaring32BitmapTestObject sourceBitmap = Roaring32BitmapTestObjectFactory.Default.GetEmpty();

            // Act
            using var uut = Roaring64Bitmap.FromBitmap(sourceBitmap.Bitmap);

            // Assert
            Assert.Empty(uut.Values);
        }

        [Fact]
        public void FromBitmap_SourceBitmapHasValues_ReturnsBitmapWithExpectedValues()
        {
            // Arrange
            using IRoaring32BitmapTestObject sourceBitmap = Roaring32BitmapTestObjectFactory.Default.GetDefault();

            // Act
            using var uut = Roaring64Bitmap.FromBitmap(sourceBitmap.Bitmap);

            // Assert
            Assert.Equal(sourceBitmap.Values.Select(x => (ulong)x).ToArray(), uut.Values.ToArray());
        }
    }

    public class Clone
    {
        [Fact]
        public void Clone_Always_CreatesNewInstanceOfBitmap()
        {
            // Act
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetDefault();

            // Act
            Roaring64Bitmap actual = testObject.Bitmap.Clone();

            // Assert
            Assert.NotEqual(testObject.Bitmap, actual);
            Assert.Equal(testObject.Bitmap.Values, actual.Values);
        }
    }

    public class CloneWithOffset
    {
        [Theory]
        [InlineData(new ulong[] { }, new ulong[] { }, 10)]
        [InlineData(new ulong[] { 0, 1, 2, 3, 4 }, new ulong[] { 5, 6, 7, 8, 9 }, 5)]
        [InlineData(new ulong[] { 0, 2, 4, 6, 8 }, new ulong[] { 10, 12, 14, 16, 18 }, 10)]
        [InlineData(new ulong[] { 0, 1, 2, 3, 4, ulong.MaxValue }, new ulong[] { 1, 2, 3, 4, 5 }, 1)]
        [InlineData(new ulong[] { ulong.MaxValue - 1, ulong.MaxValue }, new ulong[] { ulong.MaxValue }, 1)]
        [InlineData(new ulong[] { 0 }, new ulong[] { ulong.MaxValue }, ulong.MaxValue)]
        public void CloneWithOffset_AddsValueToBitmapValues_ReturnsNewBitmapWithExpectedValues(ulong[] values, ulong[] expected, ulong offset)
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetFromValues(values);

            // Act
            using Roaring64Bitmap actualBitmap = testObject.Bitmap.CloneWithOffset(offset);

            // Assert
            var actual = actualBitmap.Values.ToList();
            Assert.Equal(expected, actual);
            Assert.Equal(testObject.Bitmap.Values, values);
        }
    }

    public class CloneWithNegativeOffset
    {
        [Theory]
        [InlineData(new ulong[] { 0, 1, 2, 3, 4 }, new ulong[] { 0, 1, 2 }, 2)]
        [InlineData(new ulong[] { 0, 1, 2, 3, 4 }, new ulong[] { }, 5)]
        [InlineData(new ulong[] { ulong.MaxValue }, new ulong[] { 0 }, ulong.MaxValue)]
        [InlineData(new ulong[] { ulong.MaxValue }, new ulong[] { ulong.MaxValue - 1 }, 1)]
        public void CloneWithNegativeOffset_SubtractsValueFromBitmapValues_ReturnsNewBitmapWithExpectedValues(ulong[] values, ulong[] expected, ulong offset)
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetFromValues(values);

            // Act
            using Roaring64Bitmap actualBitmap = testObject.Bitmap.CloneWithNegativeOffset(offset);

            // Assert
            var actual = actualBitmap.Values.ToList();
            Assert.Equal(expected, actual);
            Assert.Equal(testObject.Bitmap.Values, values);
        }
    }

    public class OverwriteWith
    {
        [Theory]
        [InlineData(new ulong[] { })]
        [InlineData(new ulong[] { 0 })]
        [InlineData(new ulong[] { 0, 1, 2, 3, 4 })]
        [InlineData(new ulong[] { 0, 2, 4, 6, 8 })]
        [InlineData(new ulong[] { 0, 1, 2, 3, 4, ulong.MaxValue })]
        [InlineData(new ulong[] { ulong.MaxValue - 1, ulong.MaxValue })]
        [InlineData(new ulong[] { ulong.MaxValue })]
        public void OverwriteWith_SourceBitmap_ReplacesDestinationBitmap(ulong[] source)
        {
            // Arrange
            using Roaring64BitmapTestObject sourceObject = Roaring64BitmapTestObjectFactory.Default.GetFromValues(source);
            using Roaring64BitmapTestObject destinationObject = Roaring64BitmapTestObjectFactory.Default.GetFromValues([0, 1, 2, 3, 4, ulong.MaxValue - 1, ulong.MaxValue]);

            // Act
            destinationObject.Bitmap.OverwriteWith(sourceObject.Bitmap);

            // Assert
            Assert.Equal(source, sourceObject.Bitmap.Values);
            Assert.Equal(source, destinationObject.Bitmap.Values);
        }
    }
}