using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring32BitmapTests;

public class RemoveTests
{
    public class Remove
    {
        [Fact]
        public void Remove_BitmapIsEmpty_DoesNotRemoveValue()
        {
            // Arrange
            using Roaring32BitmapTestObject testObject = Roaring32BitmapTestObjectFactory.Default.GetEmpty();

            // Act
            testObject.Bitmap.Remove(uint.MaxValue);

            // Assert
            Assert.Empty(testObject.Bitmap.Values);
        }

        [Fact]
        public void Remove_BitmapWithValue_RemovesValueFromBitmap()
        {
            // Arrange
            using Roaring32BitmapTestObject testObject = Roaring32BitmapTestObjectFactory.Default.GetDefault();
            var removedValue = testObject.Values.First();

            // Act
            testObject.Bitmap.Remove(removedValue);

            // Assert
            Assert.Equal((uint)(testObject.Values.Length - 1), testObject.Bitmap.Count);
        }
    }

    public class RemoveMany
    {
        [Fact]
        public void RemoveMany_Span_BitmapWithValues_RemovesValuesFromBitmap()
        {
            // Arrange
            using Roaring32BitmapTestObject testObject = Roaring32BitmapTestObjectFactory.Default.GetFromValues(new uint[] { 1, 2, 3, 4, 5 });
            Span<uint> values = [1U, 3U, 5U];

            // Act
            testObject.Bitmap.RemoveMany(values);

            // Assert
            Assert.Equal(new[] { 2U, 4U }, testObject.Bitmap.Values.ToArray());
        }

        [Fact]
        public void RemoveMany_ReadOnlySpan_BitmapWithValues_RemovesValuesFromBitmap()
        {
            // Arrange
            using Roaring32BitmapTestObject testObject = Roaring32BitmapTestObjectFactory.Default.GetFromValues(new uint[] { 1, 2, 3, 4, 5 });
            ReadOnlySpan<uint> values = [1U, 3U, 5U];

            // Act
            testObject.Bitmap.RemoveMany(values);

            // Assert
            Assert.Equal(new[] { 2U, 4U }, testObject.Bitmap.Values.ToArray());
        }

        [Fact]
        public void RemoveMany_ReadOnlyMemory_BitmapWithValues_RemovesValuesFromBitmap()
        {
            // Arrange
            using Roaring32BitmapTestObject testObject = Roaring32BitmapTestObjectFactory.Default.GetFromValues(new uint[] { 1, 2, 3, 4, 5 });
            ReadOnlyMemory<uint> values = new uint[] { 1U, 3U, 5U };

            // Act
            testObject.Bitmap.RemoveMany(values);

            // Assert
            Assert.Equal(new[] { 2U, 4U }, testObject.Bitmap.Values.ToArray());
        }

        [Fact]
        public void RemoveMany_Memory_BitmapWithValues_RemovesValuesFromBitmap()
        {
            // Arrange
            using Roaring32BitmapTestObject testObject = Roaring32BitmapTestObjectFactory.Default.GetFromValues(new uint[] { 1, 2, 3, 4, 5 });
            Memory<uint> values = new uint[] { 1U, 3U, 5U };

            // Act
            testObject.Bitmap.RemoveMany(values);

            // Assert
            Assert.Equal(new[] { 2U, 4U }, testObject.Bitmap.Values.ToArray());
        }

        [Fact]
        public void RemoveMany_MemorySlice_BitmapWithValues_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            using Roaring32BitmapTestObject testObject = Roaring32BitmapTestObjectFactory.Default.GetFromValues(new uint[] { 1, 2, 3, 4, 5 });
            var values = new uint[] { 1U, 2U, 3U, 4U };

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => testObject.Bitmap.RemoveMany(values.AsMemory(), 3, 2));
        }

        [Fact]
        public void RemoveMany_BitmapIsEmpty_DoesNotRemoveAnyValue()
        {
            // Arrange
            using Roaring32BitmapTestObject testObject = Roaring32BitmapTestObjectFactory.Default.GetEmpty();

            // Act
            testObject.Bitmap.RemoveMany([1, 10, uint.MaxValue]);

            // Assert
            Assert.Empty(testObject.Bitmap.Values);
        }

        [Fact]
        public void RemoveMany_BitmapWithValues_RemovesValuesFromBitmap()
        {
            // Arrange
            using Roaring32BitmapTestObject testObject = Roaring32BitmapTestObjectFactory.Default.GetDefault();
            testObject.Bitmap.Add(uint.MaxValue);
            var removedValues = testObject.Values.Take(10).Append(uint.MaxValue).ToArray();

            // Act
            testObject.Bitmap.RemoveMany(removedValues);

            // Assert
            Assert.Equal((uint)(testObject.Values.Length - removedValues.Length + 1), testObject.Bitmap.Count);
        }
    }

    public class RemoveMany_WithOffset
    {



        [Theory]
        [InlineData(new uint[] { 0, 1, 2, 3, 4 }, 0, 5)]
        [InlineData(new uint[] { 0, 1, 2, 3, 4 }, 4, 1)]
        [InlineData(new uint[] { 0, 1, 2, 3, 4 }, 3, 2)]
        [InlineData(new uint[] { 0, 1, 2, 3, 4 }, 1, 2)]
        [InlineData(new uint[] { 0, 1, 2, 3, 4 }, 0, 0)]
        [InlineData(new uint[] { 1, 2, uint.MaxValue }, 0, 3)]
        public void RemoveMany_WithCorrectOffsetAndCount_RemovesValuesFromBitmap(uint[] values, uint offset, uint count)
        {
            // Arrange
            using Roaring32BitmapTestObject testObject = Roaring32BitmapTestObjectFactory.Default.GetFromValues(values);

            // Act
            testObject.Bitmap.RemoveMany(values, offset, count);

            // Assert
            Assert.Equal(values.Except(values.Skip((int)offset).Take((int)count)).ToArray(),
                testObject.Bitmap.Values.ToArray());
        }

        [Theory]
        [InlineData(new uint[] { 0, 1, 2, 3, 4 }, 0, 6)]
        [InlineData(new uint[] { 0, 1, 2, 3, 4 }, 5, 1)]
        [InlineData(new uint[] { 0, 1, 2, 3, 4 }, 4, 2)]
        public void RemoveMany_OffsetAndCountOutOfAllowedRange_ThrowsArgumentOutOfRangeException(uint[] values,
            uint offset, uint count)
        {
            // Arrange
            using Roaring32BitmapTestObject testObject = Roaring32BitmapTestObjectFactory.Default.GetFromValues(values);

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => { testObject.Bitmap.RemoveMany(values, offset, count); });
        }
    }

    public class TryRemove
    {
        [Fact]
        public void TryRemove_BitmapIsEmpty_DoesNotRemoveValueAndReturnsFalse()
        {
            // Arrange
            using Roaring32BitmapTestObject testObject = Roaring32BitmapTestObjectFactory.Default.GetEmpty();

            // Act
            var actual = testObject.Bitmap.TryRemove(uint.MaxValue);

            // Assert
            Assert.False(actual);
            Assert.Empty(testObject.Bitmap.Values);
        }

        [Fact]
        public void TryRemove_BitmapWithValue_RemovesValueFromBitmapAndReturnsTrue()
        {
            // Arrange
            using Roaring32BitmapTestObject testObject = Roaring32BitmapTestObjectFactory.Default.GetDefault();
            var removedValue = testObject.Values.First();

            // Act
            var actual = testObject.Bitmap.TryRemove(removedValue);

            // Assert
            Assert.True(actual);
            Assert.Equal((uint)(testObject.Values.Length - 1), testObject.Bitmap.Count);
        }
    }

    public class RemoveRange
    {
        [Theory]
        [InlineData(1, 0)]
        [InlineData(10, 5)]
        public void RemoveRange_ArgumentsOutOfAllowedRange_ThrowsArgumentOutOfRangeException(uint start, uint end)
        {
            // Arrange
            using Roaring32BitmapTestObject testObject = Roaring32BitmapTestObjectFactory.Default.GetDefault();

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => testObject.Bitmap.RemoveRange(start, end));
        }

        [Theory]
        [InlineData(0, 100, 0, 0)]
        [InlineData(0, 100, 1, 1)]
        [InlineData(0, 100, 0, 10)]
        [InlineData(200, 300, 0, 10)]
        [InlineData(200, 300, 150, 250)]
        [InlineData(200, 300, 250, 350)]
        [InlineData(200, 300, 100, 350)]
        [InlineData(uint.MaxValue - 100, uint.MaxValue, uint.MaxValue - 1, uint.MaxValue)]
        [InlineData(uint.MaxValue - 100, uint.MaxValue, uint.MaxValue, uint.MaxValue)]
        [InlineData(uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue)]
        [InlineData(uint.MaxValue - 1, uint.MaxValue, uint.MaxValue, uint.MaxValue)]
        public void RemoveRange_CorrectRange_BitmapRemovesRange(uint startTest, uint endTest, uint start, uint end)
        {
            // Arrange
            using Roaring32BitmapTestObject testObject = Roaring32BitmapTestObjectFactory.Default.GetForRange(startTest, endTest);

            // Act
            testObject.Bitmap.RemoveRange(start, end);

            // Assert
            var removedValues = Enumerable.Range((int)start, (int)(end - start + 1)) // 0..10
                .Select(x => (uint)x)
                .ToList();
            IEnumerable<uint> expected = testObject.Values.Except(removedValues);
            var actual = testObject.Bitmap.Values.ToList();

            Assert.Equal(expected, actual);
        }
    }

    public class Mask
    {
        [Fact]
        public void Mask_Range_KeepsOnlyValuesWithinRange()
        {
            // Arrange
            using Roaring32BitmapTestObject testObject = Roaring32BitmapTestObjectFactory.Default.GetFromValues([1u, 3u, 5u, 7u, 8u, 9u]);

            // Act
            testObject.Bitmap.Mask(3, 8);

            // Assert
            Assert.Equal(new[] { 3u, 5u, 7u, 8u }, testObject.Bitmap.Values.ToArray());
        }

        [Fact]
        public void Mask_ZeroRange_KeepsOnlyZeroValue()
        {
            // Arrange
            using Roaring32BitmapTestObject testObject = Roaring32BitmapTestObjectFactory.Default.GetFromValues([1u, 0u, 3u]);

            // Act
            testObject.Bitmap.Mask(0, 0);

            // Assert
            Assert.Equal(new[] { 0u }, testObject.Bitmap.Values.ToArray());
        }

        [Fact]
        public void Mask_SingleValue_RetainsOnlyThatValue()
        {
            // Arrange
            using Roaring32BitmapTestObject testObject = Roaring32BitmapTestObjectFactory.Default.GetFromValues([1u, 2u, 3u]);

            // Act
            testObject.Bitmap.Mask(2, 2);

            // Assert
            Assert.Equal(new[] { 2u }, testObject.Bitmap.Values.ToArray());
        }
    }

    public class Clear
    {
        [Fact]
        public void Clear_BitmapIsEmpty_DoesNotRemoveValues()
        {
            // Arrange
            using Roaring32BitmapTestObject testObject = Roaring32BitmapTestObjectFactory.Default.GetEmpty();

            // Act
            testObject.Bitmap.Clear();

            // Assert
            Assert.Empty(testObject.Bitmap.Values);
        }

        [Fact]
        public void Clear_BitmapHasValues_RemovesAllValuesFromBitmap()
        {
            // Arrange
            using Roaring32BitmapTestObject testObject = Roaring32BitmapTestObjectFactory.Default.GetDefault();

            // Act
            testObject.Bitmap.Clear();

            // Assert
            Assert.Empty(testObject.Bitmap.Values);
        }
    }
}