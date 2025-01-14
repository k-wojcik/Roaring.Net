using System;
using System.Linq;
using Roaring.Net.CRoaring;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring64BitmapTests;

public class AddTests
{
    public class Add
    {
        [Fact]
        public void Add_EmptyBitmap_AddsValueToBitmap()
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetEmpty();

            // Act
            testObject.Bitmap.Add(10);

            // Assert
            Assert.Single(testObject.Bitmap.Values, value => value == 10U);
            Assert.Equal(1U, testObject.Bitmap.Count);
        }

        [Fact]
        public void Add_BitmapWithValues_AddsValueToBitmap()
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetDefault();
            var cardinality = testObject.Bitmap.Count;
            Assert.DoesNotContain(testObject.Bitmap.Values, value => value == 10U);

            // Act
            testObject.Bitmap.Add(10);

            // Assert
            Assert.Single(testObject.Bitmap.Values, value => value == 10U);
            Assert.Equal(cardinality + 1, testObject.Bitmap.Count);
        }

        [Fact]
        public void Add_AddedValueExistsInBitmap_ValueIsNotAddedToBitmap()
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetDefault();
            var cardinality = testObject.Bitmap.Count;
            var addedValue = testObject.Bitmap.Values.ToList()[2];

            // Act
            testObject.Bitmap.Add(addedValue);

            // Assert
            Assert.Single(testObject.Bitmap.Values, value => value == addedValue);
            Assert.Equal(cardinality, testObject.Bitmap.Count);
        }
    }

    public class AddMany
    {
        [Fact]
        public void AddMany_EmptyBitmap_AddsValuesToBitmap()
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetEmpty();

            // Act
            testObject.Bitmap.AddMany([10, 11]);

            // Assert
            Assert.Single(testObject.Bitmap.Values, value => value == 10U);
            Assert.Single(testObject.Bitmap.Values, value => value == 11U);
            Assert.Equal(2U, testObject.Bitmap.Count);
        }

        [Fact]
        public void AddMany_BitmapWithValues_AddsValueToBitmap()
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetDefault();
            var cardinality = testObject.Bitmap.Count;
            Assert.DoesNotContain(testObject.Bitmap.Values, value => value == 10U);
            Assert.DoesNotContain(testObject.Bitmap.Values, value => value == 11U);

            // Act
            testObject.Bitmap.AddMany([10, 11]);

            // Assert
            Assert.Single(testObject.Bitmap.Values, value => value == 10U);
            Assert.Single(testObject.Bitmap.Values, value => value == 11U);
            Assert.Equal(cardinality + 2, testObject.Bitmap.Count);
        }

        [Fact]
        public void AddMany_AddedValueExistsInBitmap_ValueIsNotAddedToBitmap()
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetDefault();
            var cardinality = testObject.Bitmap.Count;
            var addedValues = testObject.Bitmap.Values.ToList()[2..10].ToArray();

            // Act
            testObject.Bitmap.AddMany(addedValues);

            // Assert
            Assert.Contains(testObject.Bitmap.Values, value => addedValues.Contains(value));
            Assert.Equal(cardinality, testObject.Bitmap.Count);
        }
    }

    public class AddMany_WithOffset
    {
        [Theory]
        [InlineData(new ulong[] { 0, 1, 2, 3, 4 }, 0, 5)]
        [InlineData(new ulong[] { 0, 1, 2, 3, 4 }, 4, 1)]
        [InlineData(new ulong[] { 0, 1, 2, 3, 4 }, 3, 2)]
        [InlineData(new ulong[] { 0, 1, 2, 3, 4 }, 1, 2)]
        [InlineData(new ulong[] { 0, 1, 2, 3, 4 }, 0, 0)]
        [InlineData(new ulong[] { 1, 2, ulong.MaxValue }, 0, 3)]
        public void AddMany_WithCorrectOffsetAndCount_AddsValuesToBitmap(ulong[] values, uint offset, uint count)
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetEmpty();

            // Act
            testObject.Bitmap.AddMany(values, offset, count);

            // Assert
            Assert.Equal(values[(int)offset..((int)offset + (int)count)], testObject.Bitmap.Values.ToArray());
        }

        [Theory]
        [InlineData(new ulong[] { 0, 1, 2, 3, 4 }, 0, 6)]
        [InlineData(new ulong[] { 0, 1, 2, 3, 4 }, 5, 1)]
        [InlineData(new ulong[] { 0, 1, 2, 3, 4 }, 4, 2)]
        public void AddMany_OffsetAndCountOutOfAllowedRange_ThrowsArgumentOutOfRangeException(ulong[] values, uint offset, uint count)
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetEmpty();

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                testObject.Bitmap.AddMany(values, offset, count);
            });
        }
    }

    public class TryAdd
    {
        [Fact]
        public void TryAdd_EmptyBitmap_AddsValueToBitmapAndReturnsTrue()
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetEmpty();

            // Act
            var actual = testObject.Bitmap.TryAdd(10);

            // Assert
            Assert.True(actual);
            Assert.Single(testObject.Bitmap.Values, value => value == 10U);
            Assert.Equal(1U, testObject.Bitmap.Count);
        }

        [Fact]
        public void TryAdd_BitmapWithValues_AddsValueToBitmapAndReturnsTrue()
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetDefault();
            var cardinality = testObject.Bitmap.Count;
            Assert.DoesNotContain(testObject.Bitmap.Values, value => value == 10U);

            // Act
            var actual = testObject.Bitmap.TryAdd(10);

            // Assert
            Assert.True(actual);
            Assert.Single(testObject.Bitmap.Values, value => value == 10U);
            Assert.Equal(cardinality + 1, testObject.Bitmap.Count);
        }

        [Fact]
        public void TryAdd_AddedValueExistsInBitmap_ValueIsNotAddedToBitmapAndReturnsFalse()
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetDefault();
            var cardinality = testObject.Bitmap.Count;
            var addedValue = testObject.Bitmap.Values.ToList()[2];

            // Act
            var actual = testObject.Bitmap.TryAdd(addedValue);

            // Assert
            Assert.False(actual);
            Assert.Single(testObject.Bitmap.Values, value => value == addedValue);
            Assert.Equal(cardinality, testObject.Bitmap.Count);
        }
    }

    public class AddRange
    {
        [Theory]
        [InlineData(1, 0)]
        [InlineData(10, 5)]
        public void AddRange_ArgumentsOutOfAllowedRange_ThrowsArgumentOutOfRangeException(ulong start, ulong end)
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetEmpty();

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => testObject.Bitmap.AddRange(start, end));
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(0, 10)]
        [InlineData(ulong.MaxValue - 1, ulong.MaxValue)]
        [InlineData(ulong.MaxValue, ulong.MaxValue)]
        public void AddRange_CorrectRange_BitmapContainsExpectedValues(ulong start, ulong end)
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetEmpty();

            // Act
            testObject.Bitmap.AddRange(start, end);

            // Assert
            var expected = Enumerable.Range((int)start, (int)(end - start + 1)) // 0..10
                .Select(x => (ulong)x)
                .ToList();
            var actual = testObject.Bitmap.Values.ToList();

            Assert.Equal(expected, actual);
        }
    }

    public class AddBulk
    {
        [Fact]
        public void AddBulk_DifferentBitmaps_ThrowsArgumentException()
        {
            // Arrange
            using Roaring64BitmapTestObject testObject1 = Roaring64BitmapTestObjectFactory.Default.GetEmpty();
            using Roaring64BitmapTestObject testObject2 = Roaring64BitmapTestObjectFactory.Default.GetEmpty();
            using var context = BulkContext64.For(testObject1.Bitmap);

            // Act && Assert
            Assert.Throws<ArgumentException>(() =>
            {
                testObject2.Bitmap.AddBulk(context, 10);
            });
        }

        [Fact]
        public void AddBulk_EmptyBitmap_AddsValueToBitmap()
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetEmpty();
            using var context = BulkContext64.For(testObject.Bitmap);

            // Act
            testObject.Bitmap.AddBulk(context, 10);

            // Assert
            Assert.Single(testObject.Bitmap.Values, value => value == 10U);
            Assert.Equal(1U, testObject.Bitmap.Count);
        }

        [Fact]
        public void AddBulk_BitmapWithValues_AddsValueToBitmap()
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetDefault();
            var cardinality = testObject.Bitmap.Count;
            Assert.DoesNotContain(testObject.Bitmap.Values, value => value == 10U);
            using var context = BulkContext64.For(testObject.Bitmap);

            // Act
            testObject.Bitmap.AddBulk(context, 10);

            // Assert
            Assert.Single(testObject.Bitmap.Values, value => value == 10U);
            Assert.Equal(cardinality + 1, testObject.Bitmap.Count);
        }

        [Fact]
        public void AddBulk_AddedValueExistsInBitmap_ValueIsNotAddedToBitmap()
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetDefault();
            var cardinality = testObject.Bitmap.Count;
            var addedValue = testObject.Bitmap.Values.ToList()[2];
            using var context = BulkContext64.For(testObject.Bitmap);

            // Act
            testObject.Bitmap.AddBulk(context, addedValue);

            // Assert
            Assert.Single(testObject.Bitmap.Values, value => value == addedValue);
            Assert.Equal(cardinality, testObject.Bitmap.Count);
        }
    }
}