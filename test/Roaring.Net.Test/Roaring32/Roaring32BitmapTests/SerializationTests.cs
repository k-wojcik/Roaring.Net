using System;
using System.IO;
using Xunit;

namespace Roaring.Test.Roaring32;

public class SerializationTests
{
    public class SerializePortable
    {
        [Fact]
        public void Serialize_Portable_TestBitmap_EqualsToBitmapFromJava()
        {
            // Arrange
            using var bitmap = GetTestBitmap();

            // Act
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Portable);

            // Assert
            var testData = File.ReadAllBytes("Roaring32/Roaring32BitmapTests/TestData/bitmapwithoutruns.bin");
            Assert.Equal(testData, serializedBitmap);
        }
        
        [Fact]
        public void Serialize_Deserialize_SelfTest_Portable_EqualsToDeserialized()
        {
            // Arrange
            using var bitmap = GetTestBitmap();

            // Act
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Portable);

            // Assert
            using var deserializedBitmap = Roaring32Bitmap.Deserialize(serializedBitmap, SerializationFormat.Portable);
            Assert.Equal(bitmap.Values, deserializedBitmap.Values);
        }
        
        [Fact]
        public void Serialize_PortableWithOptimize_TestBitmap_EqualsToBitmapFromJava()
        {
            // Arrange
            using var bitmap = GetTestBitmap();

            // Act
            bitmap.Optimize();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Portable);

            // Assert
            var testData = File.ReadAllBytes("Roaring32/Roaring32BitmapTests/TestData/bitmapwithruns.bin");
            Assert.Equal(testData, serializedBitmap);
        }
        
        [Fact]
        public void Serialize_Deserialize_SelfTest_PortableWithOptimize_EqualsToDeserialized()
        {
            // Arrange
            using var bitmap = GetTestBitmap();

            // Act
            bitmap.Optimize();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Portable);

            // Assert
            using var deserializedBitmap = Roaring32Bitmap.Deserialize(serializedBitmap, SerializationFormat.Portable);
            Assert.Equal(bitmap.Values, deserializedBitmap.Values);
        }
    }
    
    public class Serialize
    {
        [Fact]
        public void Serialize_NotSupportedSerializationFormat_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            using var bitmap = GetTestBitmap();

            // Act
            Assert.Throws<ArgumentOutOfRangeException>(() => bitmap.Serialize((SerializationFormat)int.MaxValue));
        }
        
        [Fact]
        public void Serialize_Deserialize_SelfTest_EqualsToDeserialized()
        {
            // Arrange
            using var bitmap = GetTestBitmap();

            // Act
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Normal);

            // Assert
            using var deserializedBitmap = Roaring32Bitmap.Deserialize(serializedBitmap, SerializationFormat.Normal);
            Assert.Equal(bitmap.Values, deserializedBitmap.Values);
        }
        
        [Fact]
        public void Serialize_Deserialize_SelfTest_WithOptimize__EqualsToDeserialized()
        {
            // Arrange
            using var bitmap = GetTestBitmap();

            // Act
            bitmap.Optimize();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Normal);

            // Assert
            using var deserializedBitmap = Roaring32Bitmap.Deserialize(serializedBitmap, SerializationFormat.Normal);
            Assert.Equal(bitmap.Values, deserializedBitmap.Values);
        }
    }
    
    public class Deserialize
    {
        [Fact]
        public void Deserialize_NotSupportedSerializationFormat_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            using var bitmap = GetTestBitmap();

            // Act
            Assert.Throws<ArgumentOutOfRangeException>(() => Roaring32Bitmap.Deserialize([], (SerializationFormat)int.MaxValue));
        }
        
        [Fact]
        public void Deserialize_InvalidDataCannotDeserialize_ThrowsInvalidOperationException()
        {
            // Arrange
            using var bitmap = GetTestBitmap();

            // Act
            Assert.Throws<InvalidOperationException>(() => Roaring32Bitmap.Deserialize([]));
        }
    }
    
    public class DeserializePortable
    {
        [Fact]
        public void Deserialize_Portable_DeserializesBitmapFromJava()
        {
            // Arrange
            using var bitmap = GetTestBitmap();
            var testData = File.ReadAllBytes("Roaring32/Roaring32BitmapTests/TestData/bitmapwithoutruns.bin");
            
            // Act
            var deserializedBitmap = Roaring32Bitmap.Deserialize(testData, SerializationFormat.Portable);

            // Assert
            Assert.Equal(bitmap.AndCount(deserializedBitmap), bitmap.Count);
        }

        [Fact]
        public void Deserialize_PortableWithOptimize_DeserializesBitmapFromJava()
        {
            // Arrange
            using var bitmap = GetTestBitmap();
            var testData = File.ReadAllBytes("Roaring32/Roaring32BitmapTests/TestData/bitmapwithruns.bin");

            // Act
            var deserializedBitmap = Roaring32Bitmap.Deserialize(testData, SerializationFormat.Portable);

            // Assert
            Assert.Equal(bitmap.AndCount(deserializedBitmap), bitmap.Count);
        }
    }
    
    private static Roaring32Bitmap GetTestBitmap()
    {
        var bitmap = new Roaring32Bitmap();
        for (uint k = 0; k < 100000; k+= 1000) {
            bitmap.Add(k);
        }
        for (uint k = 100000; k < 200000; ++k) {
            bitmap.Add(3*k);
        }
        for (uint k = 700000; k < 800000; ++k) {
            bitmap.Add(k);
        }

        return bitmap;
    }
}