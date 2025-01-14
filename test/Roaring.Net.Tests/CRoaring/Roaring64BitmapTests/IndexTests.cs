using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring64BitmapTests;

public class IndexTests
{
    public class TryGetValue
    {
        [Theory]
        [InlineTestObject(new ulong[] { 0, 1, 2, 3, 4 }, 0, 0)]
        [InlineTestObject(new ulong[] { 0, 1, 2, 3, 4 }, 4, 4)]
        [InlineTestObject(new ulong[] { 4, 3, 2, 1, 0 }, 4, 4)]
        [InlineTestObject(new ulong[] { 0, 2, 4, 6, 8 }, 2, 4)]
        [InlineTestObject(new ulong[] { 50, 100, 200, 300, 400 }, 2, 200)]
        public void TryGetValue_IndexLessThanBitmapSize_ReturnsTrueAndExpectedValue(ulong[] values, ulong index, ulong expected, IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetFromValues(values);

            // Act
            var actualReturn = testObject.ReadOnlyBitmap.TryGetValue(index, out ulong actual);

            // Assert
            Assert.True(actualReturn);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineTestObject(new ulong[] { }, 1, 0)]
        [InlineTestObject(new ulong[] { 0, 1, 2, 3, 4 }, 5, 0)]
        public void TryGetValue_IndexGreaterThanBitmapSize_ReturnsFalseAndZero(ulong[] values, ulong index, ulong expected, IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetFromValues(values);

            // Act
            var actualReturn = testObject.ReadOnlyBitmap.TryGetValue(index, out ulong actual);

            // Assert
            Assert.False(actualReturn);
            Assert.Equal(expected, actual);
        }
    }

    public class TryGetIndex
    {
        [Theory]
        [InlineTestObject(new ulong[] { 0, 1, 2, 3, 4 }, 0, 0, true)]
        [InlineTestObject(new ulong[] { 0, 1, 2, 3, 4 }, 4, 4, true)]
        [InlineTestObject(new ulong[] { 4, 3, 2, 1, 0 }, 4, 4, true)]
        [InlineTestObject(new ulong[] { 4, 3, 2, 1, 0 }, 5, 0, false)]
        [InlineTestObject(new ulong[] { 0, 2, 4, 6, 8 }, 2, 1, true)]
        [InlineTestObject(new ulong[] { 5, 6, 7, 8, 9 }, 2, 0, false)]
        [InlineTestObject(new ulong[] { 5, 6, 7, 8, 9 }, 7, 2, true)]
        [InlineTestObject(new ulong[] { 5, ulong.MaxValue }, ulong.MaxValue, 1, true)]
        public void GetIndex_ForValues_ReturnsIndexOfValue(ulong[] values, ulong testedValue, ulong expected, bool expectedReturn, IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetFromValues(values);

            // Act
            var actualReturn = testObject.ReadOnlyBitmap.TryGetIndex(testedValue, out ulong actual);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(expectedReturn, actualReturn);
        }
    }
}