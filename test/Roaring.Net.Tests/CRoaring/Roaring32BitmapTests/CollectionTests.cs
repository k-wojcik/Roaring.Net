﻿using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring32BitmapTests;

public class CollectionTests
{
    public class ToArray
    {
        [Theory]
        [InlineTestObject(new uint[]{})]
        [InlineTestObject(new uint[]{uint.MaxValue})]
        [InlineTestObject(new uint[] { 0, 1, 2, 3, 4 })]
        public void ToArray_Always_ReturnsArrayWithExpectedValues(uint[] expected, IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetFromValues(expected);
        
            // Act
            var actual = testObject.ReadOnlyBitmap.ToArray();
        
            // Assert
            Assert.Equal(expected, actual);
        }
    }
    
    public class CopyTo
    {
        [Theory]
        [InlineTestObject(new uint[]{})]
        [InlineTestObject(new uint[]{uint.MaxValue})]
        [InlineTestObject(new uint[] { 0, 1, 2, 3, 4 })]
        public void CopyTo_OutputCollectionSizeEqualToNumberOfValues_ReturnsFilledCollection(uint[] expected, IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetFromValues(expected);
            uint[] actual = new uint[expected.Length];
            
            // Act
            testObject.ReadOnlyBitmap.CopyTo(actual);
        
            // Assert
            Assert.Equal(expected, actual);
        }
        
        [Theory]
        [InlineTestObject]
        public void CopyTo_OutputCollectionIsEmpty_ReturnsEmptyCollection(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetFromValues([ 0, 1, 2, 3, 4 ]);
            uint[] actual = [];
            
            // Act
            testObject.ReadOnlyBitmap.CopyTo(actual);
        
            // Assert
            Assert.Empty(actual);
        }
        
        [Theory]
        [InlineTestObject]
        public void CopyTo_OutputCollectionSizeGreaterThanNumberOfValues_ReturnsFilledCollectionFromBeginning(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            var expected = new uint[]{0, 1, 2, 3, 4, 0, 0, 0, 0, 0};
            var input = expected[..5];
            using var testObject = factory.GetFromValues(input);
            uint[] actual = new uint[input.Length + 5];
            
            // Act
            testObject.ReadOnlyBitmap.CopyTo(actual);
        
            // Assert
            Assert.Equal(expected, actual);
        }
    }
    
    public class Take
    {
        [Theory]
        [InlineTestObject(20, new uint[]{}, new uint[]{})]
        [InlineTestObject(20, new uint[] { 0, 1, 2, 3, 4 }, new uint[] { 0, 1, 2, 3, 4 })]
        [InlineTestObject(2, new uint[] { 0, 1, 2, 3, 4 }, new uint[] { 0 ,1 })]
        [InlineTestObject(0, new uint[] { 0, 1, 2, 3, 4 }, new uint[] {  })]
        [InlineTestObject(1, new uint[] { uint.MaxValue }, new uint[] { uint.MaxValue  })]
        public void Take_ForCount_ReturnsForValuesLimitedToCount(uint count, uint[] values, uint[] expected, IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetFromValues(values);
        
            // Act
            var actual = testObject.ReadOnlyBitmap.Take(count);
        
            // Assert
            Assert.Equal(expected, actual);
        }
    }
    
}