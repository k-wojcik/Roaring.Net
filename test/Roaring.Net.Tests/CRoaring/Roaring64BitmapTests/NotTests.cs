using System;
using System.Collections.Generic;
using Roaring.Net.CRoaring;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring64BitmapTests;

public class NotTests
{
    /* unavailable due to CRoaring performance issues
    public class Not_ForWholeBitmap
    {
        [Theory]
        [InlineTestObject]
        public void Not_EmptyBitmap_NegatesValuesInBitmap(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetEmpty();

            // Act
            using Roaring64Bitmap actual = testObject.ReadOnlyBitmap.Not();

            // Assert
            Assert.All([ulong.MinValue, 1U, 2U, 1000U, ulong.MaxValue], value =>
            {
                Assert.True(actual.Contains(value));
            });
            Assert.Equal(0U, testObject.ReadOnlyBitmap.And(actual).Count);
            Assert.Equal(ulong.MaxValue, actual.Count);
        }

        [Theory]
        [InlineTestObject]
        public void Not_NotEmptyBitmap_NegatesValuesInBitmap(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetFromValues([0, 3, ulong.MaxValue]);

            // Act
            using Roaring64Bitmap actual = testObject.ReadOnlyBitmap.Not();

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
            Assert.Equal(ulong.MaxValue - 2UL, actual.Count);
        }
    }
        public class INot_ForWholeBitmap
        {
            [Fact]
            public void INot_EmptyBitmap_NegatesValuesInBitmap()
            {
                // Arrange
                using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetEmpty();

                // Act
                testObject.Bitmap.INot();

                // Assert
                Assert.All([ulong.MinValue, 1U, 2U, 1000U, ulong.MaxValue], value =>
                {
                    Assert.True(testObject.Bitmap.Contains(value));
                });
                Assert.Equal(ulong.MaxValue, testObject.Bitmap.Count);
            }

            [Fact]
            public void INot_NotEmptyBitmap_NegatesValuesInBitmap()
            {
                // Arrange
                using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetFromValues([0, 3, ulong.MaxValue]);

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
                Assert.Equal(ulong.MaxValue - 2UL, testObject.Bitmap.Count);
            }
        }
        */

    public class NotRange
    {
        [Theory]
        [InlineTestObject(1, 0)]
        [InlineTestObject(10, 5)]
        public void NotRange_ArgumentsOutOfAllowedRange_ThrowsArgumentOutOfRangeException(ulong start, ulong end,
            IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetEmpty();

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => testObject.ReadOnlyBitmap.NotRange(start, end));
        }

        [Theory]
        [InlineTestObject]
        public void NotRange_EmptyBitmap_NegatesValuesInBitmap(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetEmpty();

            // Act
            using Roaring64Bitmap actual = testObject.ReadOnlyBitmap.NotRange(0, 3);

            // Assert
            Assert.All([0U, 1U, 2U, 3U], value => { Assert.True(actual.Contains(value)); });
            Assert.All([4U, ulong.MaxValue], value => { Assert.False(actual.Contains(value)); });
            Assert.Equal(4UL, actual.Count);
        }

        public static IEnumerable<object[]> TestData() => RangeTestData();

        [Theory]
        [MemberData(nameof(TestData))]
        public void NotRange_NotEmptyBitmap_NegatesValuesInBitmap(ulong[] values, ulong start, ulong end,
            ulong[] expectedContains, ulong[] expectedNotContains)
        {
            // Arrange
            using var bitmap = new Roaring64Bitmap(values);

            // Act
            using Roaring64Bitmap actual = bitmap.NotRange(start, end);

            // Assert
            Assert.All(expectedContains, value => { Assert.True(actual.Contains(value)); });
            Assert.All(expectedNotContains, value => { Assert.False(actual.Contains(value)); });
        }
    }

    public class INotRange
    {
        [Theory]
        [InlineData(1, 0)]
        [InlineData(10, 5)]
        public void INotRange_ArgumentsOutOfAllowedRange_ThrowsArgumentOutOfRangeException(ulong start, ulong end)
        {
            // Arrange
            using var bitmap = new Roaring64Bitmap();

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => bitmap.INotRange(start, end));
        }

        [Fact]
        public void INotRange_EmptyBitmap_NegatesValuesInBitmap()
        {
            // Arrange
            using var bitmap = new Roaring64Bitmap();

            // Act
            bitmap.INotRange(0, 3);

            // Assert
            Assert.All([0U, 1U, 2U, 3U], value => { Assert.True(bitmap.Contains(value)); });
            Assert.All([4U, ulong.MaxValue], value => { Assert.False(bitmap.Contains(value)); });
            Assert.Equal(4UL, bitmap.Count);
        }

        public static IEnumerable<object[]> TestData() => RangeTestData();

        [Theory]
        [MemberData(nameof(TestData))]
        public void INotRange_NotEmptyBitmap_NegatesValuesInBitmap(ulong[] values, ulong start, ulong end,
            ulong[] expectedContains, ulong[] expectedNotContains)
        {
            // Arrange
            using var bitmap = new Roaring64Bitmap(values);

            // Act
            bitmap.INotRange(start, end);

            // Assert
            Assert.All(expectedContains, value => { Assert.True(bitmap.Contains(value)); });
            Assert.All(expectedNotContains, value => { Assert.False(bitmap.Contains(value)); });
        }
    }

    private static IEnumerable<object[]> RangeTestData()
    {
        yield return [new ulong[] { 0, 1, 2, 3, 4 }, 0, 0, new ulong[] { 1, 2, 3, 4 }, new ulong[] { 0 }];
        yield return [new ulong[] { 0, 1, 2, 3, 4 }, 0, 4, new ulong[] { }, new ulong[] { 0, 1, 2, 3, 4, 5 }];
        yield return
            [new ulong[] { 0, 2, 4, 6, 8 }, 2, 4, new ulong[] { 0, 3, 6, 8 }, new ulong[] { 1, 2, 4, 5, 7, 9 }];
        yield return
            [new ulong[] { 0, 2, 4, 6, 8 }, 6, 10, new ulong[] { 0, 2, 4, 7, 9, 10 }, new ulong[] { 6, 8, 11 }];
        // yield return [new ulong[] { 0, 1, 2, 3, 4 }, 4, ulong.MaxValue, new ulong[] { 0, 1, 2, 3, 5, 6, ulong.MaxValue }, new ulong[] { 4 }];
        // yield return [new ulong[] { 0, 1, 2, 3, 4, ulong.MaxValue }, ulong.MinValue, ulong.MaxValue, new ulong[] { 5, 6, 7 }, new ulong[] { 0, 1, 2, 3, 4, ulong.MaxValue }];
        // yield return [new ulong[] { ulong.MaxValue - 2, ulong.MaxValue }, ulong.MinValue, ulong.MaxValue, new ulong[] { ulong.MaxValue - 1 }, new ulong[] { ulong.MaxValue - 2, ulong.MaxValue }];
    }
}