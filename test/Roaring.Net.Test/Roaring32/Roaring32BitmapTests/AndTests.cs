using System.Linq;
using Xunit;

namespace Roaring.Test.Roaring32;

public class AndTests
{
    public class And
    {
        [Theory]
        [InlineData(new uint[]{  }, new uint[]{  })]
        [InlineData(new uint[]{ 1 }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 1 }, new uint[]{  })]
        [InlineData(new uint[]{  }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 0, 1, 2 }, new uint[]{ 1, uint.MaxValue })]
        [InlineData(new uint[]{ 0, 1, 2, uint.MaxValue }, new uint[]{ 0, 2, uint.MaxValue })]
        public void Add_BitmapsWithDifferentValues_ReturnsIntersectionOfBitmaps(uint[] values1, uint[] values2)
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues(values1);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues(values2);
       
            // Act
            using var actual = testObject1.Bitmap.And(testObject2.Bitmap);
            
            // Assert
            Assert.Equal(values1.Intersect(values2), actual.Values);
        }
    }
    
    public class AndCardinality
    {
        [Theory]
        [InlineData(new uint[]{  }, new uint[]{  })]
        [InlineData(new uint[]{ 1 }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 1 }, new uint[]{  })]
        [InlineData(new uint[]{  }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 0, 1, 2 }, new uint[]{ 1, uint.MaxValue })]
        [InlineData(new uint[]{ 0, 1, 2, uint.MaxValue }, new uint[]{ 0, 2, uint.MaxValue })]
        public void AndCount_BitmapsWithDifferentValues_ReturnsCountAfterIntersectionOfBitmaps(uint[] values1, uint[] values2)
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues(values1);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues(values2);
       
            // Act
            var actual = testObject1.Bitmap.AndCount(testObject2.Bitmap);
            
            // Assert
            Assert.Equal((uint)values1.Intersect(values2).Count(), actual);
        }
    }
    
    public class IAnd
    {
        [Theory]
        [InlineData(new uint[]{  }, new uint[]{  })]
        [InlineData(new uint[]{ 1 }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 1 }, new uint[]{  })]
        [InlineData(new uint[]{  }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 0, 1, 2 }, new uint[]{ 1, uint.MaxValue })]
        [InlineData(new uint[]{ 0, 1, 2, uint.MaxValue }, new uint[]{ 0, 2, uint.MaxValue })]
        public void IAnd_BitmapsWithDifferentValues_ReturnsIntersectionOfBitmaps(uint[] values1, uint[] values2)
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues(values1);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues(values2);
       
            // Act
            testObject1.Bitmap.IAnd(testObject2.Bitmap);
            
            // Assert
            Assert.Equal(values1.Intersect(values2), testObject1.Bitmap.Values);
        }
    }

    public class AndNot
    {
        [Theory]
        [InlineData(new uint[]{  }, new uint[]{  })]
        [InlineData(new uint[]{ 1 }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 1 }, new uint[]{  })]
        [InlineData(new uint[]{  }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 0, 1, 2 }, new uint[]{ 1, uint.MaxValue })]
        [InlineData(new uint[]{ 0, 1, 2, uint.MaxValue }, new uint[]{ 0, 2, uint.MaxValue })]
        public void AndNot_BitmapsWithDifferentValues_ReturnsDifferenceOfBitmaps(uint[] values1, uint[] values2)
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues(values1);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues(values2);
       
            // Act
            using var actual = testObject1.Bitmap.AndNot(testObject2.Bitmap);
            
            // Assert
            Assert.Equal(values1.Except(values2), actual.Values);
        }
    }
    
    public class AndNotCardinality
    {
        [Theory]
        [InlineData(new uint[]{  }, new uint[]{  })]
        [InlineData(new uint[]{ 1 }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 1 }, new uint[]{  })]
        [InlineData(new uint[]{  }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 0, 1, 2 }, new uint[]{ 1, uint.MaxValue })]
        [InlineData(new uint[]{ 0, 1, 2, uint.MaxValue }, new uint[]{ 0, 2, uint.MaxValue })]
        public void IAndNot_BitmapsWithDifferentValues_ReturnsCountAfterDifferenceOfBitmaps(uint[] values1, uint[] values2)
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues(values1);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues(values2);
       
            // Act
            var actual = testObject1.Bitmap.AndNotCount(testObject2.Bitmap);
            
            // Assert
            Assert.Equal((uint)values1.Except(values2).Count(), actual);
        }
    }

    public class IAndNot
    {
        [Theory]
        [InlineData(new uint[]{  }, new uint[]{  })]
        [InlineData(new uint[]{ 1 }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 1 }, new uint[]{  })]
        [InlineData(new uint[]{  }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 0, 1, 2 }, new uint[]{ 1, uint.MaxValue })]
        [InlineData(new uint[]{ 0, 1, 2, uint.MaxValue }, new uint[]{ 0, 2, uint.MaxValue })]
        public void IAndNot_BitmapsWithDifferentValues_ReturnsDifferenceOfBitmaps(uint[] values1, uint[] values2)
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues(values1);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues(values2);
       
            // Act
            testObject1.Bitmap.IAndNot(testObject2.Bitmap);
            
            // Assert
            Assert.Equal(values1.Except(values2), testObject1.Bitmap.Values);
        }
    }
}