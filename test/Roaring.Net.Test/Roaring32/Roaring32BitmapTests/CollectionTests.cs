using System.Linq;
using Xunit;

namespace CRoaring.Test.Roaring32;

public class CollectionTests
{
    public class ToArray
    {
        [Theory]
        [InlineData(new uint[]{})]
        [InlineData(new uint[]{uint.MaxValue})]
        [InlineData(new uint[] { 0, 1, 2, 3, 4 })]
        public void ToArray_Always_ReturnsArrayWithExpectedValues(uint[] expected)
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetFromValues(expected);
        
            // Act
            var actual = testObject.Bitmap.ToArray();
        
            // Assert
            Assert.Equal(expected, actual);
        }
    }
    
    public class CopyTo
    {
        [Theory]
        [InlineData(new uint[]{})]
        [InlineData(new uint[]{uint.MaxValue})]
        [InlineData(new uint[] { 0, 1, 2, 3, 4 })]
        public void CopyTo_OutputCollectionSizeEqualToNumberOfValues_ReturnsFilledCollection(uint[] expected)
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetFromValues(expected);
            uint[] actual = new uint[expected.Length];
            
            // Act
            testObject.Bitmap.CopyTo(actual);
        
            // Assert
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void CopyTo_OutputCollectionIsEmpty_ReturnsEmptyCollection()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetFromValues([ 0, 1, 2, 3, 4 ]);
            uint[] actual = [];
            
            // Act
            testObject.Bitmap.CopyTo(actual);
        
            // Assert
            Assert.Empty(actual);
        }
        
        [Fact]
        public void CopyTo_OutputCollectionSizeGreaterThanNumberOfValues_ReturnsFilledCollectionFromBeginning()
        {
            // Arrange
            var expected = new uint[]{0, 1, 2, 3, 4, 0, 0, 0, 0, 0};
            var input = expected[..5];
            using var testObject = Roaring32BitmapTestObject.GetFromValues(input);
            uint[] actual = new uint[input.Length + 5];
            
            // Act
            testObject.Bitmap.CopyTo(actual);
        
            // Assert
            Assert.Equal(expected, actual);
        }
    }
    
    public class Take
    {
        [Theory]
        [InlineData(20, new uint[]{}, new uint[]{})]
        [InlineData(20, new uint[] { 0, 1, 2, 3, 4 }, new uint[] { 0, 1, 2, 3, 4 })]
        [InlineData(2, new uint[] { 0, 1, 2, 3, 4 }, new uint[] { 0 ,1 })]
        [InlineData(0, new uint[] { 0, 1, 2, 3, 4 }, new uint[] {  })]
        [InlineData(1, new uint[] { uint.MaxValue }, new uint[] { uint.MaxValue  })]
        public void Take_ForCount_ReturnsForValuesLimitedToCount(uint count, uint[] values, uint[] expected)
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetFromValues(values);
        
            // Act
            var actual = testObject.Bitmap.Take(count);
        
            // Assert
            Assert.Equal(expected, actual);
        }
    }
    
}