using System.Linq;
using Xunit;

namespace Roaring.Test.Roaring32;

public class OrTests
{
    public class Or
    {
        [Theory]
        [InlineData(new uint[]{  }, new uint[]{  })]
        [InlineData(new uint[]{ 1 }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 1 }, new uint[]{  })]
        [InlineData(new uint[]{  }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 0, 1, 2 }, new uint[]{ 1, uint.MaxValue })]
        [InlineData(new uint[]{ 0, 1, 2, uint.MaxValue }, new uint[]{ 0, 2, uint.MaxValue })]
        public void Or_BitmapsWithDifferentValues_ReturnsUnionOfBitmaps(uint[] values1, uint[] values2)
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues(values1);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues(values2);
       
            // Act
            using var actual = testObject1.Bitmap.Or(testObject2.Bitmap);
            
            // Assert
            Assert.Equal(values1.Union(values2), actual.Values);
        }
    }
    
    public class OrCount
    {
        [Theory]
        [InlineData(new uint[]{  }, new uint[]{  })]
        [InlineData(new uint[]{ 1 }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 1 }, new uint[]{  })]
        [InlineData(new uint[]{  }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 0, 1, 2 }, new uint[]{ 1, uint.MaxValue })]
        [InlineData(new uint[]{ 0, 1, 2, uint.MaxValue }, new uint[]{ 0, 2, uint.MaxValue })]
        public void Or_BitmapsWithDifferentValues_ReturnsCountAfterUnionOfBitmaps(uint[] values1, uint[] values2)
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues(values1);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues(values2);
       
            // Act
            var actual = testObject1.Bitmap.OrCount(testObject2.Bitmap);
            
            // Assert
            Assert.Equal((ulong)values1.Union(values2).Count(), actual);
        }
    }
    
    public class OrMany
    {
        [Theory]
        [InlineData(new uint[]{  }, new uint[]{  }, new uint[]{  })]
        [InlineData(new uint[]{ 1 }, new uint[]{ 1 }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 1 }, new uint[]{  }, new uint[]{  })]
        [InlineData(new uint[]{  }, new uint[]{ 1 }, new uint[]{  })]
        [InlineData(new uint[]{  }, new uint[]{  }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 0, 1, 2 }, new uint[]{ 1, uint.MaxValue }, new uint[]{ 3, 5 })]
        [InlineData(new uint[]{ 0, 1, 2, uint.MaxValue }, new uint[]{ 0, 2, uint.MaxValue }, new uint[]{ 5, uint.MaxValue })]
        public void OrMany_BitmapsWithDifferentValues_ReturnsUnionOfBitmaps(uint[] values1, uint[] values2, uint[] values3)
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues(values1);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues(values2);
            using var testObject3 = Roaring32BitmapTestObject.GetFromValues(values3);
       
            // Act
            var actual = testObject1.Bitmap.OrMany([testObject2.Bitmap, testObject3.Bitmap]);
            
            // Assert
            Assert.Equal(values1.Union(values2).Union(values3).Order(), actual.Values);
        }
    }
    
    public class OrManyHeap
    {
        [Theory]
        [InlineData(new uint[]{  }, new uint[]{  }, new uint[]{  })]
        [InlineData(new uint[]{ 1 }, new uint[]{ 1 }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 1 }, new uint[]{  }, new uint[]{  })]
        [InlineData(new uint[]{  }, new uint[]{ 1 }, new uint[]{  })]
        [InlineData(new uint[]{  }, new uint[]{  }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 0, 1, 2 }, new uint[]{ 1, uint.MaxValue }, new uint[]{ 3, 5 })]
        [InlineData(new uint[]{ 0, 1, 2, uint.MaxValue }, new uint[]{ 0, 2, uint.MaxValue }, new uint[]{ 5, uint.MaxValue })]
        public void OrManyHeap_BitmapsWithDifferentValues_ReturnsUnionOfBitmaps(uint[] values1, uint[] values2, uint[] values3)
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues(values1);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues(values2);
            using var testObject3 = Roaring32BitmapTestObject.GetFromValues(values3);
       
            // Act
            var actual = testObject1.Bitmap.OrManyHeap([testObject2.Bitmap, testObject3.Bitmap]);
            
            // Assert
            Assert.Equal(values1.Union(values2).Union(values3).Order(), actual.Values);
        }
    }
    
    public class IOr
    {
        [Theory]
        [InlineData(new uint[]{  }, new uint[]{  })]
        [InlineData(new uint[]{ 1 }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 1 }, new uint[]{  })]
        [InlineData(new uint[]{  }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 0, 1, 2 }, new uint[]{ 1, uint.MaxValue })]
        [InlineData(new uint[]{ 0, 1, 2, uint.MaxValue }, new uint[]{ 0, 2, uint.MaxValue })]
        public void IOr_BitmapsWithDifferentValues_ReturnsUnionOfBitmaps(uint[] values1, uint[] values2)
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues(values1);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues(values2);
       
            // Act
            testObject1.Bitmap.IOr(testObject2.Bitmap);
            
            // Assert
            Assert.Equal(values1.Union(values2), testObject1.Bitmap.Values);
        }
    }
    
    public class LazyOr
    {
        [Theory]
        [InlineData(new uint[]{  }, new uint[]{  })]
        [InlineData(new uint[]{ 1 }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 1 }, new uint[]{  })]
        [InlineData(new uint[]{  }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 0, 1, 2 }, new uint[]{ 1, uint.MaxValue })]
        [InlineData(new uint[]{ 0, 1, 2, uint.MaxValue }, new uint[]{ 0, 2, uint.MaxValue })]
        public void LazyOr_BitmapsWithDifferentValues_ReturnsUnionOfBitmaps(uint[] values1, uint[] values2)
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues(values1);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues(values2);
       
            // Act
            using var actual = testObject1.Bitmap.LazyOr(testObject2.Bitmap, true);
            
            // Assert
            Assert.Equal(values1.Union(values2), actual.Values);
        }
    }
    
    public class ILazyOr
    {
        [Theory]
        [InlineData(new uint[]{  }, new uint[]{  })]
        [InlineData(new uint[]{ 1 }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 1 }, new uint[]{  })]
        [InlineData(new uint[]{  }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 0, 1, 2 }, new uint[]{ 1, uint.MaxValue })]
        [InlineData(new uint[]{ 0, 1, 2, uint.MaxValue }, new uint[]{ 0, 2, uint.MaxValue })]
        public void IOr_BitmapsWithDifferentValues_ReturnsUnionOfBitmaps(uint[] values1, uint[] values2)
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues(values1);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues(values2);
       
            // Act
            testObject1.Bitmap.ILazyOr(testObject2.Bitmap, true);
            
            // Assert
            Assert.Equal(values1.Union(values2), testObject1.Bitmap.Values);
        }
    }
}