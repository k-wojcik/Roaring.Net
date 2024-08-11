using System;
using System.Linq;
using Xunit;

namespace CRoaring.Test.Roaring32;

public class RemoveTests
{
    public class Remove
    {



        [Fact]
        public void Remove_BitmapIsEmpty_DoesNotRemoveValue()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetEmpty();

            // Act
            testObject.Bitmap.Remove(uint.MaxValue);

            // Assert
            Assert.Empty(testObject.Bitmap.Values);
        }

        [Fact]
        public void Remove_BitmapWithValue_RemovesValueFromBitmap()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetDefault();
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
        public void RemoveMany_BitmapIsEmpty_DoesNotRemoveAnyValue()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetEmpty();

            // Act
            testObject.Bitmap.RemoveMany([1, 10, uint.MaxValue]);

            // Assert
            Assert.Empty(testObject.Bitmap.Values);
        }

        [Fact]
        public void RemoveMany_BitmapWithValues_RemovesValuesFromBitmap()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetDefault();
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
            using var testObject = Roaring32BitmapTestObject.GetFromValues(values);

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
            using var testObject = Roaring32BitmapTestObject.GetFromValues(values);

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
            using var testObject = Roaring32BitmapTestObject.GetEmpty();

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
            using var testObject = Roaring32BitmapTestObject.GetDefault();
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
            using var testObject = Roaring32BitmapTestObject.GetDefault();

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
            using var testObject = Roaring32BitmapTestObject.GetForRange(startTest, endTest);

            // Act
            testObject.Bitmap.RemoveRange(start, end);

            // Assert
            var removedValues = Enumerable.Range((int)start, (int)(end - start + 1)) // 0..10
                .Select(x => (uint)x)
                .ToList();
            var expected = testObject.Values.Except(removedValues);
            var actual = testObject.Bitmap.Values.ToList();

            Assert.Equal(expected, actual);
        }
    }

    public class Clear
    {
        [Fact]
        public void Clear_BitmapIsEmpty_DoesNotRemoveValues()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetEmpty();

            // Act
            testObject.Bitmap.Clear();

            // Assert
            Assert.Empty(testObject.Bitmap.Values);
        }

        [Fact]
        public void Clear_BitmapHasValues_RemovesAllValuesFromBitmap()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetDefault();

            // Act
            testObject.Bitmap.Clear();

            // Assert
            Assert.Empty(testObject.Bitmap.Values);
        }
    }
}