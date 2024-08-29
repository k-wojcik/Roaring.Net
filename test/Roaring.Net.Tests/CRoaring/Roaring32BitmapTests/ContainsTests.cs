using System;
using System.Linq;
using Roaring.Net.CRoaring;
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
    
    public class ContainsBulk
    {
        [Theory]
        [InlineMatrixTestObject]
        public void ContainsBulk_DifferentBitmaps_ThrowsArgumentException(TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using var testObject1 = matrix.X.GetEmpty();
            using var testObject2 = matrix.Y.GetEmpty();
            using var context = BulkContext.For(testObject1.Bitmap);
            
            // Act && Assert
            Assert.Throws<ArgumentException>(() =>
            {
                testObject2.ReadOnlyBitmap.ContainsBulk(context, 10);
            });
        }
        
        [Theory]
        [InlineTestObject]
        public void ContainsBulk_EmptyBitmap_ReturnFalse(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetEmpty();
            using var context = BulkContext.For(testObject.Bitmap);

            // Act
            var actual = testObject.ReadOnlyBitmap.ContainsBulk(context, 10);

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineTestObject]
        public void ContainsBulk_BitmapHasValue_ReturnsTrue(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetDefault();
            var value = testObject.Values.First();
            using var context = BulkContext.For(testObject.Bitmap);

            // Act
            var actual = testObject.ReadOnlyBitmap.ContainsBulk(context, value);

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineTestObject]
        public void ContainsBulk_BitmapDoesNotHaveValue_ReturnsFalse(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetDefault();
            Assert.DoesNotContain(testObject.Values, value => value == 10U);
            using var context = BulkContext.For(testObject.Bitmap);

            // Act
            var actual = testObject.ReadOnlyBitmap.ContainsBulk(context, 10U);

            // Assert
            Assert.False(actual);
        }
    }
}