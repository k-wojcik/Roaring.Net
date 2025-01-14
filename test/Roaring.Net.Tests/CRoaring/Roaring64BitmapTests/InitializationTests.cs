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
            var actual = uut.GetSerializationBytes();
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
}