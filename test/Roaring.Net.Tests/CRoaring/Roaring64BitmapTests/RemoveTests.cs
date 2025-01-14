using System;
using System.Collections.Generic;
using System.Linq;
using Roaring.Net.CRoaring;
using Roaring.Net.Tests.Extensions;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring64BitmapTests;

public class RemoveTests
{
    public class Remove
    {
        [Fact]
        public void Remove_BitmapIsEmpty_DoesNotRemoveValue()
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetEmpty();

            // Act
            testObject.Bitmap.Remove(ulong.MaxValue);

            // Assert
            Assert.Empty(testObject.Bitmap.Values);
        }

        [Fact]
        public void Remove_BitmapWithValue_RemovesValueFromBitmap()
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetDefault();
            var removedValue = testObject.Values.First();

            // Act
            testObject.Bitmap.Remove(removedValue);

            // Assert
            Assert.Equal((ulong)(testObject.Values.Length - 1), testObject.Bitmap.Count);
        }
    }

    public class RemoveMany
    {
        [Fact]
        public void RemoveMany_BitmapIsEmpty_DoesNotRemoveAnyValue()
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetEmpty();

            // Act
            testObject.Bitmap.RemoveMany([1, 10, ulong.MaxValue]);

            // Assert
            Assert.Empty(testObject.Bitmap.Values);
        }

        [Fact]
        public void RemoveMany_BitmapWithValues_RemovesValuesFromBitmap()
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetDefault();
            testObject.Bitmap.Add(ulong.MaxValue);
            var removedValues = testObject.Values.Take(10).Append(ulong.MaxValue).ToArray();

            // Act
            testObject.Bitmap.RemoveMany(removedValues);

            // Assert
            Assert.Equal((ulong)(testObject.Values.Length - removedValues.Length + 1), testObject.Bitmap.Count);
        }
    }

    public class RemoveMany_WithOffset
    {
        [Theory]
        [InlineData(new ulong[] { 0, 1, 2, 3, 4 }, 0, 5)]
        [InlineData(new ulong[] { 0, 1, 2, 3, 4 }, 4, 1)]
        [InlineData(new ulong[] { 0, 1, 2, 3, 4 }, 3, 2)]
        [InlineData(new ulong[] { 0, 1, 2, 3, 4 }, 1, 2)]
        [InlineData(new ulong[] { 0, 1, 2, 3, 4 }, 0, 0)]
        [InlineData(new ulong[] { 1, 2, ulong.MaxValue }, 0, 3)]
        public void RemoveMany_WithCorrectOffsetAndCount_RemovesValuesFromBitmap(ulong[] values, uint offset, uint count)
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetFromValues(values);

            // Act
            testObject.Bitmap.RemoveMany(values, offset, count);

            // Assert
            Assert.Equal(values.Except(values.Skip((int)offset).Take((int)count)).ToArray(),
                testObject.Bitmap.Values.ToArray());
        }

        [Theory]
        [InlineData(new ulong[] { 0, 1, 2, 3, 4 }, 0, 6)]
        [InlineData(new ulong[] { 0, 1, 2, 3, 4 }, 5, 1)]
        [InlineData(new ulong[] { 0, 1, 2, 3, 4 }, 4, 2)]
        public void RemoveMany_OffsetAndCountOutOfAllowedRange_ThrowsArgumentOutOfRangeException(ulong[] values,
            uint offset, uint count)
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetFromValues(values);

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
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetEmpty();

            // Act
            var actual = testObject.Bitmap.TryRemove(ulong.MaxValue);

            // Assert
            Assert.False(actual);
            Assert.Empty(testObject.Bitmap.Values);
        }

        [Fact]
        public void TryRemove_BitmapWithValue_RemovesValueFromBitmapAndReturnsTrue()
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetDefault();
            var removedValue = testObject.Values.First();

            // Act
            var actual = testObject.Bitmap.TryRemove(removedValue);

            // Assert
            Assert.True(actual);
            Assert.Equal((ulong)(testObject.Values.Length - 1), testObject.Bitmap.Count);
        }
    }

    public class RemoveRange
    {
        [Theory]
        [InlineData(1, 0)]
        [InlineData(10, 5)]
        public void RemoveRange_ArgumentsOutOfAllowedRange_ThrowsArgumentOutOfRangeException(ulong start, ulong end)
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetDefault();

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
        [InlineData(ulong.MaxValue - 100, ulong.MaxValue, ulong.MaxValue - 1, ulong.MaxValue)]
        [InlineData(ulong.MaxValue - 100, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue)]
        [InlineData(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue)]
        [InlineData(ulong.MaxValue - 1, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue)]
        public void RemoveRange_CorrectRange_BitmapRemovesRange(ulong startTest, ulong endTest, ulong start, ulong end)
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetForRange(startTest, endTest);

            // Act
            testObject.Bitmap.RemoveRange(start, end);

            // Assert
            var removedValues = EnumerableRange.Range(start, (end - start + 1)) // 0..10
                .Select(x => x)
                .ToList();
            IEnumerable<ulong> expected = testObject.Values.Except(removedValues);
            var actual = testObject.Bitmap.Values.ToList();

            Assert.Equal(expected, actual);
        }
    }

    public class RemoveBulk
    {
        [Fact]
        public void RemoveBulk_DifferentBitmaps_ThrowsArgumentException()
        {
            // Arrange
            using Roaring64BitmapTestObject testObject1 = Roaring64BitmapTestObjectFactory.Default.GetEmpty();
            using Roaring64BitmapTestObject testObject2 = Roaring64BitmapTestObjectFactory.Default.GetEmpty();
            using var context = BulkContext64.For(testObject1.Bitmap);

            // Act && Assert
            Assert.Throws<ArgumentException>(() =>
            {
                testObject2.Bitmap.RemoveBulk(context, 10);
            });
        }

        [Fact]
        public void RemoveBulk_EmptyBitmap_RemovesValueFromBitmap()
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetFromValues([10]);
            using var context = BulkContext64.For(testObject.Bitmap);

            // Act
            testObject.Bitmap.RemoveBulk(context, 10);

            // Assert
            Assert.Equal(0U, testObject.Bitmap.Count);
        }

        [Fact]
        public void RemoveBulk_BitmapWithValues_RemovesValueFromBitmap()
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetDefault();
            var cardinality = testObject.Bitmap.Count;
            Assert.Contains(testObject.Bitmap.Values, value => value == 0U);
            using var context = BulkContext64.For(testObject.Bitmap);

            // Act
            testObject.Bitmap.RemoveBulk(context, 0);

            // Assert
            Assert.DoesNotContain(testObject.Bitmap.Values, value => value == 0U);
            Assert.Equal(cardinality - 1, testObject.Bitmap.Count);
        }

        [Fact]
        public void RemoveBulk_RemovedValueNotExistInBitmap_ValueIsNotRemovesFromBitmap()
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetDefault();
            var cardinality = testObject.Bitmap.Count;
            Assert.DoesNotContain(testObject.Bitmap.Values, value => value == 10U);
            using var context = BulkContext64.For(testObject.Bitmap);

            // Act
            testObject.Bitmap.RemoveBulk(context, 10U);

            // Assert
            Assert.Equal(cardinality, testObject.Bitmap.Count);
        }
    }

    public class Clear
    {
        [Fact]
        public void Clear_BitmapIsEmpty_DoesNotRemoveValues()
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetEmpty();

            // Act
            testObject.Bitmap.Clear();

            // Assert
            Assert.Empty(testObject.Bitmap.Values);
        }

        [Fact]
        public void Clear_BitmapHasValues_RemovesAllValuesFromBitmap()
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetDefault();

            // Act
            testObject.Bitmap.Clear();

            // Assert
            Assert.Empty(testObject.Bitmap.Values);
        }
    }
}