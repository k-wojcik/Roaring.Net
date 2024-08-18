using System;
using System.Linq;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring32BitmapTests;

public class ContainsTests
{
    public class Contains
    {
        [Theory]
        [InlineTestObject]
        public void Contains_EmptyBitmap_ReturnFalse(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetEmpty();

            // Act
            var actual = testObject.ReadOnlyBitmap.Contains(10);

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineTestObject]
        public void Contains_BitmapHasValue_ReturnsTrue(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetDefault();
            var value = testObject.Values.First();

            // Act
            var actual = testObject.ReadOnlyBitmap.Contains(value);

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineTestObject]
        public void Contains_BitmapDoesNotHaveValue_ReturnsFalse(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetDefault();
            Assert.DoesNotContain(testObject.Values, value => value == 10U);

            // Act
            var actual = testObject.ReadOnlyBitmap.Contains(10U);

            // Assert
            Assert.False(actual);
        }
    }

    public class ContainsRange
    {
        [Theory]
        [InlineTestObject(1, 0)]
        [InlineTestObject(10, 5)]
        public void ContainsRange_ArgumentsOutOfAllowedRange_ThrowsArgumentOutOfRangeException(uint start, uint end, IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetEmpty();
            
            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => testObject.ReadOnlyBitmap.ContainsRange(start, end));
        }
        
        [Theory]
        [InlineTestObject(0, 0, 0, 0, true)]
        [InlineTestObject(0, 1, 0, 0, true)]
        [InlineTestObject(0, 100, 0, 0, true)]
        [InlineTestObject(0,100, 1, 1, true)]
        [InlineTestObject(0, 100, 0, 10, true)]
        [InlineTestObject(200, 300, 0, 10, false)]
        [InlineTestObject(200, 300, 150, 250, false)]
        [InlineTestObject(200, 300, 250, 350, false)]
        [InlineTestObject(200, 300, 100, 350, false)]
        [InlineTestObject(uint.MaxValue - 100,uint.MaxValue, uint.MaxValue - 1, uint.MaxValue, true)]
        [InlineTestObject(uint.MaxValue - 100,uint.MaxValue, uint.MaxValue, uint.MaxValue, true)]
        [InlineTestObject(uint.MaxValue,uint.MaxValue, uint.MaxValue, uint.MaxValue, true)]
        [InlineTestObject(uint.MaxValue - 1,uint.MaxValue - 1, uint.MaxValue, uint.MaxValue, false)]
        public void ContainsRange_CorrectRange_ReturnsExpectedResult(uint startTest, uint endTest, uint start, uint end, bool expected, IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetForRange(startTest, endTest);
            
            // Act
            var actual = testObject.ReadOnlyBitmap.ContainsRange(start, end);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}