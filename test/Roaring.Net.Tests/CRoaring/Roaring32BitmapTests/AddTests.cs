using System;
using System.Linq;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring32BitmapTests;

public class AddTests
{
    public class Add
    {
        [Fact]
        public void Add_EmptyBitmap_AddsValueToBitmap()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObjectFactory.Default.GetEmpty();
        
            // Act
            testObject.Bitmap.Add(10);
        
            // Assert
            Assert.Single(testObject.Bitmap.Values, value=> value == 10U);
            Assert.Equal(1U, testObject.Bitmap.Count);
        }
    
        [Fact]
        public void Add_BitmapWithValues_AddsValueToBitmap()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObjectFactory.Default.GetDefault();
            var cardinality = testObject.Bitmap.Count;
            Assert.DoesNotContain(testObject.Bitmap.Values, value=> value == 10U);
        
            // Act
            testObject.Bitmap.Add(10);
        
            // Assert
            Assert.Single(testObject.Bitmap.Values, value=> value == 10U);
            Assert.Equal(cardinality + 1, testObject.Bitmap.Count);
        }
    
        [Fact]
        public void Add_AddedValueExistsInBitmap_ValueIsNotAddedToBitmap()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObjectFactory.Default.GetDefault();
            var cardinality = testObject.Bitmap.Count;
            var addedValue = testObject.Bitmap.Values.ToList()[2];
        
            // Act
            testObject.Bitmap.Add(addedValue);
        
            // Assert
            Assert.Single(testObject.Bitmap.Values, value=> value == addedValue);
            Assert.Equal(cardinality, testObject.Bitmap.Count);
        }
    }

    public class AddMany
    {
        [Fact]
        public void AddMany_EmptyBitmap_AddsValuesToBitmap()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObjectFactory.Default.GetEmpty();
        
            // Act
            testObject.Bitmap.AddMany([10, 11]);
        
            // Assert
            Assert.Single(testObject.Bitmap.Values, value=> value == 10U);
            Assert.Single(testObject.Bitmap.Values, value=> value == 11U);
            Assert.Equal(2U, testObject.Bitmap.Count);
        }
    
        [Fact]
        public void AddMany_BitmapWithValues_AddsValueToBitmap()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObjectFactory.Default.GetDefault();
            var cardinality = testObject.Bitmap.Count;
            Assert.DoesNotContain(testObject.Bitmap.Values, value=> value == 10U);
            Assert.DoesNotContain(testObject.Bitmap.Values, value=> value == 11U);
        
            // Act
            testObject.Bitmap.AddMany([10, 11]);
        
            // Assert
            Assert.Single(testObject.Bitmap.Values, value=> value == 10U);
            Assert.Single(testObject.Bitmap.Values, value=> value == 11U);
            Assert.Equal(cardinality + 2, testObject.Bitmap.Count);
        }
    
        [Fact]
        public void AddMany_AddedValueExistsInBitmap_ValueIsNotAddedToBitmap()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObjectFactory.Default.GetDefault();
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
        [InlineData(new uint[] {0, 1, 2, 3, 4}, 0, 5)]
        [InlineData(new uint[] {0, 1, 2, 3, 4}, 4, 1)]
        [InlineData(new uint[] {0, 1, 2, 3, 4}, 3, 2)]
        [InlineData(new uint[] {0, 1, 2, 3, 4}, 1, 2)]
        [InlineData(new uint[] {0, 1, 2, 3, 4}, 0, 0)]
        [InlineData(new uint[] {1, 2, uint.MaxValue}, 0, 3)]
        public void AddMany_WithCorrectOffsetAndCount_AddsValuesToBitmap(uint[] values, uint offset, uint count)
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObjectFactory.Default.GetEmpty();
        
            // Act
            testObject.Bitmap.AddMany(values, offset, count);

            // Assert
            Assert.Equal(values[(int)offset..((int)offset + (int)count)], testObject.Bitmap.Values.ToArray());
        }
    
        [Theory]
        [InlineData(new uint[] {0, 1, 2, 3, 4}, 0, 6)]
        [InlineData(new uint[] {0, 1, 2, 3, 4}, 5, 1)]
        [InlineData(new uint[] {0, 1, 2, 3, 4}, 4, 2)]
        public void AddMany_OffsetAndCountOutOfAllowedRange_ThrowsArgumentOutOfRangeException(uint[] values, uint offset, uint count)
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObjectFactory.Default.GetEmpty();
        
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
            using var testObject = Roaring32BitmapTestObjectFactory.Default.GetEmpty();
        
            // Act
            var actual = testObject.Bitmap.TryAdd(10);
        
            // Assert
            Assert.True(actual);
            Assert.Single(testObject.Bitmap.Values, value=> value == 10U);
            Assert.Equal(1U, testObject.Bitmap.Count);
        }
    
        [Fact]
        public void TryAdd_BitmapWithValues_AddsValueToBitmapAndReturnsTrue()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObjectFactory.Default.GetDefault();
            var cardinality = testObject.Bitmap.Count;
            Assert.DoesNotContain(testObject.Bitmap.Values, value=> value == 10U);
        
            // Act
            var actual = testObject.Bitmap.TryAdd(10);
        
            // Assert
            Assert.True(actual);
            Assert.Single(testObject.Bitmap.Values, value=> value == 10U);
            Assert.Equal(cardinality + 1, testObject.Bitmap.Count);
        }
    
        [Fact]
        public void TryAdd_AddedValueExistsInBitmap_ValueIsNotAddedToBitmapAndReturnsFalse()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObjectFactory.Default.GetDefault();
            var cardinality = testObject.Bitmap.Count;
            var addedValue = testObject.Bitmap.Values.ToList()[2];
        
            // Act
            var actual = testObject.Bitmap.TryAdd(addedValue);
        
            // Assert
            Assert.False(actual);
            Assert.Single(testObject.Bitmap.Values, value=> value == addedValue);
            Assert.Equal(cardinality, testObject.Bitmap.Count);
        }
    }

    public class AddRange
    {
        [Theory]
        [InlineData(1, 0)]
        [InlineData(10, 5)]
        public void AddRange_ArgumentsOutOfAllowedRange_ThrowsArgumentOutOfRangeException(uint start, uint end)
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObjectFactory.Default.GetEmpty();

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => testObject.Bitmap.AddRange(start, end));
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(0, 10)]
        [InlineData(uint.MaxValue - 1, uint.MaxValue)]
        [InlineData(uint.MaxValue, uint.MaxValue)]
        public void AddRange_CorrectRange_BitmapContainsExpectedValues(uint start, uint end)
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObjectFactory.Default.GetEmpty();

            // Act
            testObject.Bitmap.AddRange(start, end);

            // Assert
            var expected = Enumerable.Range((int)start, (int)(end - start + 1)) // 0..10
                .Select(x => (uint)x)
                .ToList();
            var actual = testObject.Bitmap.Values.ToList();

            Assert.Equal(expected, actual);
        }
    }
    
    public class AddOffset
    {
        [Theory]
        [InlineData(new uint[] { }, new uint[] { },10)]
        [InlineData(new uint[] { 0, 1, 2, 3, 4 },new uint[] { 5, 6, 7, 8, 9 }, 5)]
        [InlineData(new uint[] { 0, 1, 2, 3, 4 },new uint[] { 0, 1, 2 }, -2)]
        [InlineData(new uint[] { 0, 1, 2, 3, 4 }, new uint[] { },-5)]
        [InlineData(new uint[] { 0, 2, 4, 6, 8 }, new uint[] { 10, 12, 14, 16, 18 },10)]
        [InlineData(new uint[] { 0, 1, 2, 3, 4, uint.MaxValue }, new uint[] { 1, 2, 3, 4, 5 }, 1)]
        [InlineData(new uint[] { uint.MaxValue - 1, uint.MaxValue }, new uint[] {  uint.MaxValue },1)]
        [InlineData(new uint[] { 0 }, new uint[] {  uint.MaxValue },uint.MaxValue)]
        [InlineData(new uint[] { uint.MaxValue }, new uint[] { 0 },-uint.MaxValue)]
        public void AddOffset_AddsValueToBitmapValues_BitmapContainsExpectedValues(uint[] values, uint[] expected, long offset)
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObjectFactory.Default.GetFromValues(values);

            // Act
            testObject.Bitmap.AddOffset(offset);

            // Assert
            var actual = testObject.Bitmap.Values.ToList();
            Assert.Equal(expected, actual);
        }
    }
}