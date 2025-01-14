using System;
using System.Collections.Generic;
using System.Linq;
using Roaring.Net.CRoaring;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring32BitmapTests;

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
                using var uut = new Roaring32Bitmap(IntPtr.Zero);
            });
        }

        [Fact]
        public void Ctor_Default_CreatesBitmapWithZeroCapacity()
        {
            // Act
            using var uut = new Roaring32Bitmap();

            // Assert
            var actual = uut.GetSerializationBytes();
            Assert.True(actual > 0);
        }

        [Fact]
        public void Ctor_WithCapacity_CreatesBitmapWithGivenCapacity()
        {
            // Act
            using var uut = new Roaring32Bitmap(1000U);

            // Assert
            var actual = uut.GetSerializationBytes();
            Assert.True(actual > 0);
        }

        [Fact]
        public void Ctor_FromValues_InputHasValues_BitmapContainsExpectedValues()
        {
            // Arrange
            var expected = Enumerable.Range(0, 100)
                .Select(x => (uint)x)
                .Concat([uint.MaxValue - 1, uint.MaxValue])
                .ToArray();

            // Act
            using var uut = new Roaring32Bitmap(expected);

            // Assert
            IEnumerable<uint> actual = uut.Values;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Ctor_FromValues_InputIsEmpty_BitmapIsEmpty()
        {
            // Act
            using var uut = new Roaring32Bitmap([]);

            // Assert
            IEnumerable<uint> actual = uut.Values;

            Assert.Empty(actual);
        }
    }

    public class FromRange
    {
        [Theory]
        [InlineData(1, 0, 1)]
        [InlineData(10, 5, 1)]
        [InlineData(1, 10, 0)]
        public void FromRange_ArgumentsOutOfAllowedRange_ThrowsArgumentOutOfRangeException(uint start, uint end,
            uint step)
        {
            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => Roaring32Bitmap.FromRange(start, end, step));
        }

        [Theory]
        [InlineData(0, 0, 1)]
        [InlineData(1, 1, 1)]
        [InlineData(10, 100, 2)]
        [InlineData(33, 333, 33)]
        [InlineData(uint.MaxValue - 1, uint.MaxValue, 1)]
        [InlineData(uint.MaxValue, uint.MaxValue, 1)]
        [InlineData(uint.MaxValue - 3, uint.MaxValue, 3)]
        [InlineData(uint.MaxValue - 15 * 9, uint.MaxValue, 15)]
        [InlineData(1, uint.MaxValue, uint.MaxValue / 2)]
        public void FromRange_CorrectRange_BitmapContainsExpectedValues(uint start, uint end, uint step)
        {
            // Act
            using var uut = Roaring32Bitmap.FromRange(start, end, step);

            // Assert
            var expected = Enumerable.Range(0, (int)Math.Floor((end - start) / (double)step) + 1)
                .Select(x => start + (uint)x * step)
                .ToList();
            var actual = uut.Values.ToList();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FromRange_StepGreaterThanRange_BitmapContainsMinValueOfRange()
        {
            // Act
            using var uut = Roaring32Bitmap.FromRange(1, 10, 100);

            // Assert
            IEnumerable<uint> actual = uut.Values;

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
                .Select(x => (uint)x)
                .Concat([uint.MaxValue - 1, uint.MaxValue])
                .ToArray();

            // Act
            using var uut = Roaring32Bitmap.FromValues(expected);

            // Assert
            IEnumerable<uint> actual = uut.Values;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FromValues_InputIsEmpty_BitmapIsEmpty()
        {
            // Act
            using var uut = Roaring32Bitmap.FromValues([]);

            // Assert
            IEnumerable<uint> actual = uut.Values;

            Assert.Empty(actual);
        }

        [Theory]
        [InlineData(0, 100)]
        [InlineData(99, 1)]
        [InlineData(90, 10)]
        [InlineData(10, 25)]
        [InlineData(0, 0)]
        public void FromValues_WithCorrectOffsetAndCount_BitmapContainsExpectedValues(uint offset, uint count)
        {
            // Arrange
            var values = Enumerable.Range(0, 100)
                .Select(x => (uint)x)
                .ToArray();

            // Act
            using var uut = Roaring32Bitmap.FromValues(values, offset, count);

            // Assert
            IEnumerable<uint> actual = uut.Values;
            IEnumerable<uint> expected = values.Skip((int)offset).Take((int)count);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 101)]
        [InlineData(100, 1)]
        [InlineData(90, 11)]
        public void FromValues_OffsetAndCountOutOfAllowedRange_ThrowsArgumentOutOfRangeException(uint offset, uint count)
        {
            // Arrange
            var values = Enumerable.Range(0, 100)
                .Select(x => (uint)x)
                .ToArray();

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                using var uut = Roaring32Bitmap.FromValues(values, offset, count);
            });
        }
    }

    public class Clone
    {
        [Fact]
        public void Clone_Always_CreatesNewInstanceOfBitmap()
        {
            // Act
            using Roaring32BitmapTestObject testObject = Roaring32BitmapTestObjectFactory.Default.GetDefault();

            // Act
            Roaring32Bitmap actual = testObject.Bitmap.Clone();

            // Assert
            Assert.NotEqual(testObject.Bitmap, actual);
            Assert.Equal(testObject.Bitmap.Values, actual.Values);
        }
    }

    public class CloneWithOffset
    {
        [Theory]
        [InlineData(new uint[] { }, new uint[] { }, 10)]
        [InlineData(new uint[] { 0, 1, 2, 3, 4 }, new uint[] { 5, 6, 7, 8, 9 }, 5)]
        [InlineData(new uint[] { 0, 1, 2, 3, 4 }, new uint[] { 0, 1, 2 }, -2)]
        [InlineData(new uint[] { 0, 1, 2, 3, 4 }, new uint[] { }, -5)]
        [InlineData(new uint[] { 0, 2, 4, 6, 8 }, new uint[] { 10, 12, 14, 16, 18 }, 10)]
        [InlineData(new uint[] { 0, 1, 2, 3, 4, uint.MaxValue }, new uint[] { 1, 2, 3, 4, 5 }, 1)]
        [InlineData(new uint[] { uint.MaxValue - 1, uint.MaxValue }, new uint[] { uint.MaxValue }, 1)]
        [InlineData(new uint[] { 0 }, new uint[] { uint.MaxValue }, uint.MaxValue)]
        [InlineData(new uint[] { uint.MaxValue }, new uint[] { 0 }, -uint.MaxValue)]
        public void CloneWithOffset_AddsValueToBitmapValues_ReturnsNewBitmapWithExpectedValues(uint[] values, uint[] expected, long offset)
        {
            // Arrange
            using Roaring32BitmapTestObject testObject = Roaring32BitmapTestObjectFactory.Default.GetFromValues(values);

            // Act
            using Roaring32Bitmap actualBitmap = testObject.Bitmap.CloneWithOffset(offset);

            // Assert
            var actual = actualBitmap.Values.ToList();
            Assert.Equal(expected, actual);
            Assert.Equal(testObject.Bitmap.Values, values);
        }
    }

    public class OverwriteWith
    {
        [Theory]
        [InlineData(new uint[] { })]
        [InlineData(new uint[] { 0 })]
        [InlineData(new uint[] { 0, 1, 2, 3, 4 })]
        [InlineData(new uint[] { 0, 2, 4, 6, 8 })]
        [InlineData(new uint[] { 0, 1, 2, 3, 4, uint.MaxValue })]
        [InlineData(new uint[] { uint.MaxValue - 1, uint.MaxValue })]
        [InlineData(new uint[] { uint.MaxValue })]
        public void OverwriteWith_SourceBitmap_ReplacesDestinationBitmapAndReturnsTrue(uint[] source)
        {
            // Arrange
            using Roaring32BitmapTestObject sourceObject = Roaring32BitmapTestObjectFactory.Default.GetFromValues(source);
            using Roaring32BitmapTestObject destinationObject = Roaring32BitmapTestObjectFactory.Default.GetFromValues([0, 1, 2, 3, 4, uint.MaxValue - 1, uint.MaxValue]);

            // Act
            var actual = destinationObject.Bitmap.OverwriteWith(sourceObject.Bitmap);

            // Assert
            Assert.True(actual);
            Assert.Equal(source, sourceObject.Bitmap.Values);
            Assert.Equal(source, destinationObject.Bitmap.Values);
        }
    }
}