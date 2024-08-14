using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring32BitmapTests;

public class IndexTests
{
    public class TryGetValue
    {
        [Theory]
        [InlineData(new uint[] { 0, 1, 2, 3, 4 }, 0, 0)]
        [InlineData(new uint[] { 0, 1, 2, 3, 4 }, 4, 4)]
        [InlineData(new uint[] { 4, 3, 2, 1, 0 }, 4, 4)]
        [InlineData(new uint[] { 0, 2, 4, 6, 8 }, 2, 4)]
        public void TryGetValue_IndexLessThanBitmapSize_ReturnsTrueAndExpectedValue(uint[] values, uint index, uint expected)
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetFromValues(values);
        
            // Act
            var actualReturn = testObject.Bitmap.TryGetValue(index, out uint actual);
        
            // Assert
            Assert.True(actualReturn);
            Assert.Equal(expected, actual);
        }
    
        [Theory]
        [InlineData(new uint[]{}, 1, 0)]
        [InlineData(new uint[] { 0, 1, 2, 3, 4 }, 5, 0)]
        public void TryGetValue_IndexGreaterThanBitmapSize_ReturnsFalseAndZero(uint[] values, uint index, uint expected)
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetFromValues(values);
        
            // Act
            var actualReturn = testObject.Bitmap.TryGetValue(index, out uint actual);
        
            // Assert
            Assert.False(actualReturn);
            Assert.Equal(expected, actual);
        } 
    }
    
    public class GetIndex
    {
        [Theory]
        [InlineData(new uint[] { 0, 1, 2, 3, 4 }, 0, 0)]
        [InlineData(new uint[] { 0, 1, 2, 3, 4 }, 4, 4)]
        [InlineData(new uint[] { 4, 3, 2, 1, 0 }, 4, 4)]
        [InlineData(new uint[] { 4, 3, 2, 1, 0 }, 5, -1)]
        [InlineData(new uint[] { 0, 2, 4, 6, 8 }, 2, 1)]
        [InlineData(new uint[] { 5, 6, 7, 8, 9 }, 2, -1)]
        [InlineData(new uint[] { 5, 6, 7, 8, 9 }, 7, 2)]
        [InlineData(new uint[] { 5, uint.MaxValue }, uint.MaxValue, 1)]
        public void GetIndex_ForValues_ReturnsIndexOfValue(uint[] values, uint testedValue, long expected)
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetFromValues(values);
        
            // Act
            var actual = testObject.Bitmap.GetIndex(testedValue);
        
            // Assert
            Assert.Equal(expected, actual);
        }
    }
}