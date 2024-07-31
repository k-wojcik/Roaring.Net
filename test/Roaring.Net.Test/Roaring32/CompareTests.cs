using Xunit;

namespace Roaring.Test.Roaring32;

public class CompareTests
{
    public class Equals
    {
        [Fact]
        public void Equals_SameBitmap_ReturnsTrue()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetEmpty();

            // Act
            var actual = testObject.Bitmap.Equals(testObject.Bitmap);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void Equals_BitmapsHaveSameValues_ReturnsTrue()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues([0, 10]);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues([0, 10]);

            // Act
            var actual = testObject1.Bitmap.Equals(testObject2.Bitmap);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void Equals_BitmapsHaveDifferentValues_ReturnsFalse()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues([0, 10]);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues([0, 10, 20]);

            // Act
            var actual = testObject1.Bitmap.Equals(testObject2.Bitmap);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void Equals_BitmapIsNull_ReturnsFalse()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetEmpty();

            // Act
            var actual = testObject1.Bitmap.Equals(null);

            // Assert
            Assert.False(actual);
        }
    }

    public class IsSubset
    {
        [Fact]
        public void IsSubset_BitmapIsNull_ReturnsFalse()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetEmpty();

            // Act
            var actual = testObject.Bitmap.IsSubset(null);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void IsSubset_SameBitmap_ReturnsTrue()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetEmpty();

            // Act
            var actual = testObject.Bitmap.IsSubset(testObject.Bitmap);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsSubset_BitmapsHaveSameValues_ReturnsTrue()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues([0, 10]);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues([0, 10]);

            // Act
            var actual = testObject1.Bitmap.IsSubset(testObject2.Bitmap);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsSubset_BitmapsIsSubsetOfSecondBitmap_ReturnsTrue()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues([0, 10]);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues([0, 10, 20]);
        
            // Act
            var actual = testObject1.Bitmap.IsSubset(testObject2.Bitmap);
        
            // Assert
            Assert.True(actual);
        }
        
        [Fact]
        public void IsSubset_SomeValuesMissingInSecondBitmap_ReturnsFalse()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues([0, 10]);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues([0]);
        
            // Act
            var actual = testObject1.Bitmap.IsSubset(testObject2.Bitmap);
        
            // Assert
            Assert.False(actual);
        }
    }

    public class IsStrictSubset
    {
        [Fact]
        public void IsStrictSubset_BitmapIsNull_ReturnsFalse()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetEmpty();

            // Act
            var actual = testObject.Bitmap.IsStrictSubset(null);

            // Assert
            Assert.False(actual);
        }
        
        [Fact]
        public void IsStrictSubset_SameBitmap_ReturnsFalse()
        {
            // Arrange
            using var testObject = Roaring32BitmapTestObject.GetEmpty();

            // Act
            var actual = testObject.Bitmap.IsStrictSubset(testObject.Bitmap);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void IsStrictSubset_BitmapsHaveSameValues_ReturnsFalse()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues([0, 10]);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues([0, 10]);

            // Act
            var actual = testObject1.Bitmap.IsStrictSubset(testObject2.Bitmap);

            // Assert
            Assert.False(actual);
        }
    
        [Fact]
        public void IsStrictSubset_BitmapsIsSubsetOfSecondBitmap_ReturnsTrue()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues([0, 10]);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues([0, 10, 20]);
        
            // Act
            var actual = testObject1.Bitmap.IsStrictSubset(testObject2.Bitmap);
        
            // Assert
            Assert.True(actual);
        }
    
        [Fact]
        public void IsStrictSubset_SomeValuesMissingInSecondBitmap_ReturnsFalse()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues([0, 10]);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues([0]);
        
            // Act
            var actual = testObject1.Bitmap.IsStrictSubset(testObject2.Bitmap);
        
            // Assert
            Assert.False(actual);
        }
    }
    
    public class HasIntersects
    {
        [Fact]
        public void HasIntersection_IntersectsWithAtLeastOneValue_ReturnsTrue()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues([0, 10, uint.MaxValue, 1]);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues([5, 10, 11]);
        
            // Act
            var actual = testObject1.Bitmap.HasIntersection(testObject2.Bitmap);
        
            // Assert
            Assert.True(actual);
        }
        
        [Fact]
        public void HasIntersection_NoValuesIntersects_ReturnsFalse()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetFromValues([0, 10, uint.MaxValue, 1]);
            using var testObject2 = Roaring32BitmapTestObject.GetFromValues([5, 12, 11]);
        
            // Act
            var actual = testObject1.Bitmap.HasIntersection(testObject2.Bitmap);
        
            // Assert
            Assert.False(actual);
        }
    }
}