using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring32BitmapTests;

public class IndexTests
{
    public class TryGetValue
    {
        [Theory]
        [InlineTestObject(new uint[] { 0, 1, 2, 3, 4 }, 0, 0)]
        [InlineTestObject(new uint[] { 0, 1, 2, 3, 4 }, 4, 4)]
        [InlineTestObject(new uint[] { 4, 3, 2, 1, 0 }, 4, 4)]
        [InlineTestObject(new uint[] { 0, 2, 4, 6, 8 }, 2, 4)]
        public void TryGetValue_IndexLessThanBitmapSize_ReturnsTrueAndExpectedValue(uint[] values, uint index, uint expected, IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetFromValues(values);
        
            // Act
            var actualReturn = testObject.ReadOnlyBitmap.TryGetValue(index, out uint actual);
        
            // Assert
            Assert.True(actualReturn);
            Assert.Equal(expected, actual);
        }
    
        [Theory]
        [InlineTestObject(new uint[]{}, 1, 0)]
        [InlineTestObject(new uint[] { 0, 1, 2, 3, 4 }, 5, 0)]
        public void TryGetValue_IndexGreaterThanBitmapSize_ReturnsFalseAndZero(uint[] values, uint index, uint expected, IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetFromValues(values);
        
            // Act
            var actualReturn = testObject.ReadOnlyBitmap.TryGetValue(index, out uint actual);
        
            // Assert
            Assert.False(actualReturn);
            Assert.Equal(expected, actual);
        } 
    }
    
    public class GetIndex
    {
        [Theory]
        [InlineTestObject(new uint[] { 0, 1, 2, 3, 4 }, 0, 0)]
        [InlineTestObject(new uint[] { 0, 1, 2, 3, 4 }, 4, 4)]
        [InlineTestObject(new uint[] { 4, 3, 2, 1, 0 }, 4, 4)]
        [InlineTestObject(new uint[] { 4, 3, 2, 1, 0 }, 5, -1)]
        [InlineTestObject(new uint[] { 0, 2, 4, 6, 8 }, 2, 1)]
        [InlineTestObject(new uint[] { 5, 6, 7, 8, 9 }, 2, -1)]
        [InlineTestObject(new uint[] { 5, 6, 7, 8, 9 }, 7, 2)]
        [InlineTestObject(new uint[] { 5, uint.MaxValue }, uint.MaxValue, 1)]
        public void GetIndex_ForValues_ReturnsIndexOfValue(uint[] values, uint testedValue, long expected, IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetFromValues(values);
        
            // Act
            var actual = testObject.ReadOnlyBitmap.GetIndex(testedValue);
        
            // Assert
            Assert.Equal(expected, actual);
        }
    }
}