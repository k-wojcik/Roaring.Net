using System;
using System.Collections.Generic;
using Roaring.Net.CRoaring;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring32BitmapTests;

public class NotTests
{
    public class Not_ForWholeBitmap
    {
        [Theory]
        [InlineTestObject]
        public void Not_EmptyBitmap_NegatesValuesInBitmap(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetEmpty();

            // Act
            using Roaring32Bitmap actual = testObject.ReadOnlyBitmap.Not();

            // Assert
            Assert.All([uint.MinValue, 1U, 2U, 1000U, uint.MaxValue], value =>
            {
                Assert.True(actual.Contains(value));
            });
            Assert.Equal(0U, testObject.ReadOnlyBitmap.And(actual).Count);
            Assert.Equal(uint.MaxValue + 1UL, actual.Count);
        }

        [Theory]
        [InlineTestObject]
        public void Not_NotEmptyBitmap_NegatesValuesInBitmap(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetFromValues([0, 3, uint.MaxValue]);

            // Act
            using Roaring32Bitmap actual = testObject.ReadOnlyBitmap.Not();

            // Assert
            Assert.All(testObject.Values, value =>
            {
                Assert.False(actual.Contains(value));
            });

            Assert.All([1U, 2U, 1000U], value =>
            {
                Assert.True(actual.Contains(value));
            });
            Assert.Equal(0U, testObject.ReadOnlyBitmap.And(actual).Count);
            Assert.Equal(uint.MaxValue - 2UL, actual.Count);
        }
    }

    public class INot_ForWholeBitmap
    {
        [Fact]
        public void INot_EmptyBitmap_NegatesValuesInBitmap()
        {
            // Arrange
            using Roaring32BitmapTestObject testObject = Roaring32BitmapTestObjectFactory.Default.GetEmpty();

            // Act
            testObject.Bitmap.INot();

            // Assert
            Assert.All([uint.MinValue, 1U, 2U, 1000U, uint.MaxValue], value =>
            {
                Assert.True(testObject.Bitmap.Contains(value));
            });
            Assert.Equal(uint.MaxValue + 1UL, testObject.Bitmap.Count);
        }

        [Fact]
        public void INot_NotEmptyBitmap_NegatesValuesInBitmap()
        {
            // Arrange
            using Roaring32BitmapTestObject testObject = Roaring32BitmapTestObjectFactory.Default.GetFromValues([0, 3, uint.MaxValue]);

            // Act
            testObject.Bitmap.INot();

            // Assert
            Assert.All(testObject.Values, value =>
            {
                Assert.False(testObject.Bitmap.Contains(value));
            });
            Assert.All([1U, 2U, 1000U], value =>
            {
                Assert.True(testObject.Bitmap.Contains(value));
            });
            Assert.Equal(uint.MaxValue - 2UL, testObject.Bitmap.Count);
        }
    }

    public class NotRange
    {
        [Theory]
        [InlineTestObject(1, 0)]
        [InlineTestObject(10, 5)]
        public void NotRange_ArgumentsOutOfAllowedRange_ThrowsArgumentOutOfRangeException(uint start, uint end, IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetEmpty();

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => testObject.ReadOnlyBitmap.NotRange(start, end));
        }

        [Theory]
        [InlineTestObject]
        public void NotRange_EmptyBitmap_NegatesValuesInBitmap(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetEmpty();

            // Act
            using Roaring32Bitmap actual = testObject.ReadOnlyBitmap.NotRange(0, 3);

            // Assert
            Assert.All([0U, 1U, 2U, 3U], value =>
            {
                Assert.True(actual.Contains(value));
            });
            Assert.All([4U, uint.MaxValue], value =>
            {
                Assert.False(actual.Contains(value));
            });
            Assert.Equal(4UL, actual.Count);
        }

        public static IEnumerable<object[]> TestData() => RangeTestData();

        [Theory]
        [MemberData(nameof(TestData))]
        public void NotRange_NotEmptyBitmap_NegatesValuesInBitmap(uint[] values, uint start, uint end, uint[] expectedContains, uint[] expectedNotContains)
        {
            // Arrange
            using var bitmap = new Roaring32Bitmap(values);

            // Act
            using Roaring32Bitmap actual = bitmap.NotRange(start, end);

            // Assert
            Assert.All(expectedContains, value =>
            {
                Assert.True(actual.Contains(value));
            });
            Assert.All(expectedNotContains, value =>
            {
                Assert.False(actual.Contains(value));
            });
        }
    }

    public class INotRange
    {
        [Theory]
        [InlineData(1, 0)]
        [InlineData(10, 5)]
        public void INotRange_ArgumentsOutOfAllowedRange_ThrowsArgumentOutOfRangeException(uint start, uint end)
        {
            // Arrange
            using var bitmap = new Roaring32Bitmap();

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => bitmap.INotRange(start, end));
        }

        [Fact]
        public void INotRange_EmptyBitmap_NegatesValuesInBitmap()
        {
            // Arrange
            using var bitmap = new Roaring32Bitmap();

            // Act
            bitmap.INotRange(0, 3);

            // Assert
            Assert.All([0U, 1U, 2U, 3U], value =>
            {
                Assert.True(bitmap.Contains(value));
            });
            Assert.All([4U, uint.MaxValue], value =>
            {
                Assert.False(bitmap.Contains(value));
            });
            Assert.Equal(4UL, bitmap.Count);
        }

        public static IEnumerable<object[]> TestData() => RangeTestData();

        [Theory]
        [MemberData(nameof(TestData))]
        public void INotRange_NotEmptyBitmap_NegatesValuesInBitmap(uint[] values, uint start, uint end, uint[] expectedContains, uint[] expectedNotContains)
        {
            // Arrange
            using var bitmap = new Roaring32Bitmap(values);

            // Act
            bitmap.INotRange(start, end);

            // Assert
            Assert.All(expectedContains, value =>
            {
                Assert.True(bitmap.Contains(value));
            });
            Assert.All(expectedNotContains, value =>
            {
                Assert.False(bitmap.Contains(value));
            });
        }
    }

    private static IEnumerable<object[]> RangeTestData()
    {
        yield return [new uint[] { 0, 1, 2, 3, 4 }, 0, 0, new uint[] { 1, 2, 3, 4 }, new uint[] { 0 }];
        yield return [new uint[] { 0, 1, 2, 3, 4 }, 0, 4, new uint[] { }, new uint[] { 0, 1, 2, 3, 4, 5 }];
        yield return [new uint[] { 0, 2, 4, 6, 8 }, 2, 4, new uint[] { 0, 3, 6, 8 }, new uint[] { 1, 2, 4, 5, 7, 9 }];
        yield return [new uint[] { 0, 2, 4, 6, 8 }, 6, 10, new uint[] { 0, 2, 4, 7, 9, 10 }, new uint[] { 6, 8, 11 }];
        yield return [new uint[] { 0, 1, 2, 3, 4 }, 4, uint.MaxValue, new uint[] { 0, 1, 2, 3, 5, 6, uint.MaxValue }, new uint[] { 4 }];
        yield return [new uint[] { 0, 1, 2, 3, 4, uint.MaxValue }, uint.MinValue, uint.MaxValue, new uint[] { 5, 6, 7 }, new uint[] { 0, 1, 2, 3, 4, uint.MaxValue }];
        yield return [new uint[] { uint.MaxValue - 2, uint.MaxValue }, uint.MinValue, uint.MaxValue, new uint[] { uint.MaxValue - 1 }, new uint[] { uint.MaxValue - 2, uint.MaxValue }];
    }
}