using System.Linq;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring32BitmapTests;

public class XorTests
{
    public class Xor
    {
        [Theory]
        [InlineData(new uint[]{  }, new uint[]{  })]
        [InlineData(new uint[]{ 1 }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 1 }, new uint[]{  })]
        [InlineData(new uint[]{  }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 0, 1, 2 }, new uint[]{ 1, uint.MaxValue })]
        [InlineData(new uint[]{ 0, 1, 2, uint.MaxValue }, new uint[]{ 0, 2, uint.MaxValue })]
        public void Xor_BitmapsWithDifferentValues_ReturnsSymmetricDifferenceOfBitmaps(uint[] values1, uint[] values2)
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues(values1);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues(values2);
       
            // Act
            using var actual = testObject1.Bitmap.Xor(testObject2.Bitmap);
            
            // Assert
            Assert.Equal(values1.Union(values2).Except(values1.Intersect(values2)), actual.Values);
        }
    }
    
    public class XorCount
    {
        [Theory]
        [InlineData(new uint[]{  }, new uint[]{  })]
        [InlineData(new uint[]{ 1 }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 1 }, new uint[]{  })]
        [InlineData(new uint[]{  }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 0, 1, 2 }, new uint[]{ 1, uint.MaxValue })]
        [InlineData(new uint[]{ 0, 1, 2, uint.MaxValue }, new uint[]{ 0, 2, uint.MaxValue })]
        public void XorCount_BitmapsWithDifferentValues_ReturnsSymmetricDifferenceOfBitmaps(uint[] values1, uint[] values2)
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues(values1);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues(values2);
       
            // Act
            var actual = testObject1.Bitmap.XorCount(testObject2.Bitmap);
            
            // Assert
            Assert.Equal((uint)values1.Union(values2).Except(values1.Intersect(values2)).Count(), actual);
        }
    }
    
    public class IXor
    {
        [Theory]
        [InlineData(new uint[]{  }, new uint[]{  })]
        [InlineData(new uint[]{ 1 }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 1 }, new uint[]{  })]
        [InlineData(new uint[]{  }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 0, 1, 2 }, new uint[]{ 1, uint.MaxValue })]
        [InlineData(new uint[]{ 0, 1, 2, uint.MaxValue }, new uint[]{ 0, 2, uint.MaxValue })]
        public void IXor_BitmapsWithDifferentValues_ReturnsSymmetricDifferenceOfBitmaps(uint[] values1, uint[] values2)
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues(values1);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues(values2);
       
            // Act
            testObject1.Bitmap.IXor(testObject2.Bitmap);
            
            // Assert
            Assert.Equal(values1.Union(values2).Except(values1.Intersect(values2)), testObject1.Bitmap.Values);
        }
    }
    
    public class XorMany
    {
        [Theory]
        [InlineData(new uint[]{  }, new uint[]{  }, new uint[]{  })]
        [InlineData(new uint[]{ 1 }, new uint[]{ 1 }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 1 }, new uint[]{  }, new uint[]{  })]
        [InlineData(new uint[]{  }, new uint[]{ 1 }, new uint[]{  })]
        [InlineData(new uint[]{  }, new uint[]{  }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 0, 1, 2 }, new uint[]{ 1, uint.MaxValue }, new uint[]{ 3, 5 })]
        [InlineData(new uint[]{ 0, 1, 2, uint.MaxValue }, new uint[]{ 0, 2, uint.MaxValue }, new uint[]{ 5, uint.MaxValue })]
        public void XorMany_BitmapsWithDifferentValues_ReturnsSymmetricDifferenceOfBitmaps(uint[] values1, uint[] values2, uint[] values3)
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues(values1);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues(values2);
            using var testObject3 = Roaring32BitmapTestObject.GetFromValues(values3);
       
            // Act
            var actual = testObject1.Bitmap.XorMany([testObject2.Bitmap, testObject3.Bitmap]);
            
            // Assert
            var tempSet = values1.Union(values2).Except(values1.Intersect(values2)).ToList();
            var expectedSet = values3.Union(tempSet).Except(values3.Intersect(tempSet));
            Assert.Equal(expectedSet.Order(), actual.Values);
        }
    }
    
    public class LazyXor
    {
        [Theory]
        [InlineData(new uint[]{  }, new uint[]{  })]
        [InlineData(new uint[]{ 1 }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 1 }, new uint[]{  })]
        [InlineData(new uint[]{  }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 0, 1, 2 }, new uint[]{ 1, uint.MaxValue })]
        [InlineData(new uint[]{ 0, 1, 2, uint.MaxValue }, new uint[]{ 0, 2, uint.MaxValue })]
        public void Xor_BitmapsWithDifferentValues_ReturnsSymmetricDifferenceOfBitmaps(uint[] values1, uint[] values2)
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues(values1);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues(values2);
       
            // Act
            using var actual = testObject1.Bitmap.LazyXor(testObject2.Bitmap);
            
            // Assert
            Assert.Equal(values1.Union(values2).Except(values1.Intersect(values2)), actual.Values);
        }   
    }
    public class ILazyXor
    {
        [Theory]
        [InlineData(new uint[]{  }, new uint[]{  })]
        [InlineData(new uint[]{ 1 }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 1 }, new uint[]{  })]
        [InlineData(new uint[]{  }, new uint[]{ 1 })]
        [InlineData(new uint[]{ 0, 1, 2 }, new uint[]{ 1, uint.MaxValue })]
        [InlineData(new uint[]{ 0, 1, 2, uint.MaxValue }, new uint[]{ 0, 2, uint.MaxValue })]
        public void Xor_BitmapsWithDifferentValues_ReturnsSymmetricDifferenceOfBitmaps(uint[] values1, uint[] values2)
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues(values1);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues(values2);
       
            // Act
            testObject1.Bitmap.ILazyXor(testObject2.Bitmap);
            
            // Assert
            Assert.Equal(values1.Union(values2).Except(values1.Intersect(values2)), testObject1.Bitmap.Values);
        }   
    }
}