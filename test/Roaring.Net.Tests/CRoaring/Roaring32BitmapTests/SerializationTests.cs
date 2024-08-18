using System;
using System.Collections.Generic;
using System.IO;
using Roaring.Net.CRoaring;
using Roaring.Net.Tests.CRoaring.TestData;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring32BitmapTests;

public class SerializationTests
{
    public class GetSerializationBytes
    {
        [Theory]
        [InlineTestObject]
        public void GetSerializationBytes_NotSupportedSerializationFormat_ThrowsArgumentOutOfRangeException(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetEmpty();

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                testObject.ReadOnlyBitmap.GetSerializationBytes((SerializationFormat)int.MaxValue);
            });
        }
        
        [Theory]
        [InlineTestObject(SerializationFormat.Normal, 5)]
        [InlineTestObject(SerializationFormat.Portable, 8)]
        [InlineTestObject(SerializationFormat.Frozen, 4)]
        public void GetSerializationBytes_EmptyBitmap_ReturnsValueGreaterThanZero(SerializationFormat format, int size, IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetEmpty();

            // Act
            var actual = testObject.ReadOnlyBitmap.GetSerializationBytes(format);

            // Assert
            Assert.Equal((nuint)size, actual);
        }

        [Theory]
        [InlineTestObject(SerializationFormat.Normal, 4005)]
        [InlineTestObject(SerializationFormat.Portable, 10008)]
        [InlineTestObject(SerializationFormat.Frozen, 7004)]
        public void GetSerializationBytes_BitmapContainsValues_ReturnsValueGreaterThanZero(SerializationFormat format, int size, IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetDefault();

            // Act
            var actual = testObject.ReadOnlyBitmap.GetSerializationBytes(format);

            // Assert
            Assert.Equal((nuint)size, actual);
        }
    }
    
    public class Serialize
    {
        [Theory]
        [InlineTestObject]
        public void Serialize_NotSupportedSerializationFormat_ThrowsArgumentOutOfRangeException(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetFromValues(SerializationTestBitmap.GetTestBitmapValues().ToArray());

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => testObject.ReadOnlyBitmap.Serialize((SerializationFormat)int.MaxValue));
        }
    }
    
    public class SerializeNormal
    {
        [Theory]
        [InlineTestObject]
        public void Serialize_Deserialize_SelfTest_EqualsToDeserialized(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetFromValues(SerializationTestBitmap.GetTestBitmapValues().ToArray());

            // Act
            var serializedBitmap = testObject.ReadOnlyBitmap.Serialize(SerializationFormat.Normal);

            // Assert
            using var deserializedBitmap = Roaring32Bitmap.Deserialize(serializedBitmap, SerializationFormat.Normal);
            Assert.Equal(testObject.ReadOnlyBitmap.Values, deserializedBitmap.Values);
        }
        
        [Fact]
        public void Serialize_Deserialize_SelfTestWithOptimize_EqualsToDeserialized()
        {
            // Arrange
            using var bitmap = SerializationTestBitmap.GetTestBitmap();

            // Act
            bitmap.Optimize();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Normal);

            // Assert
            using var deserializedBitmap = Roaring32Bitmap.Deserialize(serializedBitmap, SerializationFormat.Normal);
            Assert.Equal(bitmap.Values, deserializedBitmap.Values);
        }
        
        [Theory]
        [InlineTestObject]
        public void Serialize_DeserializeUnsafe_SelfTest_EqualsToDeserialized(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetFromValues(SerializationTestBitmap.GetTestBitmapValues().ToArray());

            // Act
            var serializedBitmap = testObject.ReadOnlyBitmap.Serialize(SerializationFormat.Normal);

            // Assert
            using var deserializedBitmap = Roaring32Bitmap.DeserializeUnsafe(serializedBitmap, SerializationFormat.Normal);
            Assert.Equal(testObject.ReadOnlyBitmap.Values, deserializedBitmap.Values);
        }
        
        [Fact]
        public void Serialize_DeserializeUnsafe_SelfTestWithOptimize_EqualsToDeserialized()
        {
            // Arrange
            using var bitmap = SerializationTestBitmap.GetTestBitmap();

            // Act
            bitmap.Optimize();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Normal);

            // Assert
            using var deserializedBitmap = Roaring32Bitmap.DeserializeUnsafe(serializedBitmap, SerializationFormat.Normal);
            Assert.Equal(bitmap.Values, deserializedBitmap.Values);
        }
    }
    
    public class SerializePortable
    {
        [Theory]
        [InlineTestObject]
        public void Serialize_TestBitmap_EqualsToBitmapFromJava(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetFromValues(SerializationTestBitmap.GetTestBitmapValues().ToArray());

            // Act
            var serializedBitmap = testObject.ReadOnlyBitmap.Serialize(SerializationFormat.Portable);

            // Assert
            var testData = File.ReadAllBytes($"{nameof(CRoaring)}/TestData/bitmapwithoutruns.bin");
            Assert.Equal(testData, serializedBitmap);
        }
        
        [Theory]
        [InlineTestObject]
        public void Serialize_Deserialize_SelfTest_EqualsToDeserialized(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetFromValues(SerializationTestBitmap.GetTestBitmapValues().ToArray());

            // Act
            var serializedBitmap = testObject.ReadOnlyBitmap.Serialize(SerializationFormat.Portable);

            // Assert
            using var deserializedBitmap = Roaring32Bitmap.Deserialize(serializedBitmap, SerializationFormat.Portable);
            Assert.Equal(testObject.ReadOnlyBitmap.Values, deserializedBitmap.Values);
        }
        
        [Theory]
        [InlineTestObject]
        public void Serialize_DeserializeUnsafe_SelfTest_EqualsToDeserialized(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using var testObject = factory.GetFromValues(SerializationTestBitmap.GetTestBitmapValues().ToArray());

            // Act
            var serializedBitmap = testObject.ReadOnlyBitmap.Serialize(SerializationFormat.Portable);

            // Assert
            using var deserializedBitmap = Roaring32Bitmap.DeserializeUnsafe(serializedBitmap, SerializationFormat.Portable);
            Assert.Equal(testObject.ReadOnlyBitmap.Values, deserializedBitmap.Values);
        }
        
        [Fact]
        public void Serialize_TestBitmapWithOptimize_EqualsToBitmapFromJava()
        {
            // Arrange
            using var bitmap = SerializationTestBitmap.GetTestBitmap();

            // Act
            bitmap.Optimize();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Portable);

            // Assert
            var testData = File.ReadAllBytes($"{nameof(CRoaring)}/TestData/bitmapwithruns.bin");
            Assert.Equal(testData, serializedBitmap);
        }
        
        [Fact]
        public void Serialize_Deserialize_SelfTestWithOptimize_EqualsToDeserialized()
        {
            // Arrange
            using var bitmap = SerializationTestBitmap.GetTestBitmap();

            // Act
            bitmap.Optimize();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Portable);

            // Assert
            using var deserializedBitmap = Roaring32Bitmap.Deserialize(serializedBitmap, SerializationFormat.Portable);
            Assert.Equal(bitmap.Values, deserializedBitmap.Values);
        }
        
        [Fact]
        public void Serialize_DeserializeUnsafe_SelfTestWithOptimize_EqualsToDeserialized()
        {
            // Arrange
            using var bitmap = SerializationTestBitmap.GetTestBitmap();

            // Act
            bitmap.Optimize();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Portable);

            // Assert
            using var deserializedBitmap = Roaring32Bitmap.DeserializeUnsafe(serializedBitmap, SerializationFormat.Portable);
            Assert.Equal(bitmap.Values, deserializedBitmap.Values);
        }
    }
    
    
    public class Deserialize
    {
        [Fact]
        public void Deserialize_NotSupportedSerializationFormat_ThrowsArgumentOutOfRangeException()
        {
            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => Roaring32Bitmap.Deserialize([], (SerializationFormat)int.MaxValue));
        }
    }
    
    public class DeserializeNormal
    {
        [Fact]
        public void Deserialize_InvalidDataCannotDeserialize_ThrowsInvalidOperationException()
        {
            // Act && Assert
            Assert.Throws<InvalidOperationException>(() => Roaring32Bitmap.Deserialize([]));
        }
    }
    
    public class DeserializeUnsafe
    {
        [Fact]
        public void DeserializeUnsafe_NotSupportedSerializationFormat_ThrowsArgumentOutOfRangeException()
        {
            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => Roaring32Bitmap.DeserializeUnsafe([], (SerializationFormat)int.MaxValue));
        }
    }

    public class DeserializeUnsafeNormal
    {
        [Fact]
        public void DeserializeUnsafe_InvalidDataCannotDeserialize_ThrowsInvalidOperationException()
        {
            // Act && Assert
            Assert.Throws<InvalidOperationException>(() => Roaring32Bitmap.DeserializeUnsafe([]));
        }
    }
    
    public class DeserializePortable
    {
        [Fact]
        public void Deserialize_DeserializesBitmapFromJava()
        {
            // Arrange
            using var bitmap = SerializationTestBitmap.GetTestBitmap();
            var testData = File.ReadAllBytes($"{nameof(CRoaring)}/TestData/bitmapwithoutruns.bin");
            
            // Act
            var deserializedBitmap = Roaring32Bitmap.Deserialize(testData, SerializationFormat.Portable);

            // Assert
            Assert.Equal(bitmap.AndCount(deserializedBitmap), bitmap.Count);
        }

        [Fact]
        public void Deserialize_WithOptimize_DeserializesBitmapFromJava()
        {
            // Arrange
            using var bitmap = SerializationTestBitmap.GetTestBitmap();
            var testData = File.ReadAllBytes($"{nameof(CRoaring)}/TestData/bitmapwithruns.bin");

            // Act
            var deserializedBitmap = Roaring32Bitmap.Deserialize(testData, SerializationFormat.Portable);

            // Assert
            Assert.Equal(bitmap.AndCount(deserializedBitmap), bitmap.Count);
        }
        
        [Fact]
        public void DeserializeAndInvalidDataCannotDeserialize_ThrowsInvalidOperationException()
        {
            // Act && Assert
            Assert.Throws<InvalidOperationException>(() => Roaring32Bitmap.Deserialize([], SerializationFormat.Portable));
        }
    }
    
    public class DeserializeUnsafePortable
    {
        [Fact]
        public void DeserializeUnsafe_DeserializesBitmapFromJava()
        {
            // Arrange
            using var bitmap = SerializationTestBitmap.GetTestBitmap();
            var testData = File.ReadAllBytes($"{nameof(CRoaring)}/TestData/bitmapwithoutruns.bin");
            
            // Act
            var deserializedBitmap = Roaring32Bitmap.DeserializeUnsafe(testData, SerializationFormat.Portable);

            // Assert
            Assert.Equal(bitmap.AndCount(deserializedBitmap), bitmap.Count);
        }

        [Fact]
        public void DeserializeUnsafe_WithOptimize_DeserializesBitmapFromJava()
        {
            // Arrange
            using var bitmap = SerializationTestBitmap.GetTestBitmap();
            var testData = File.ReadAllBytes($"{nameof(CRoaring)}/TestData/bitmapwithruns.bin");

            // Act
            var deserializedBitmap = Roaring32Bitmap.DeserializeUnsafe(testData, SerializationFormat.Portable);

            // Assert
            Assert.Equal(bitmap.AndCount(deserializedBitmap), bitmap.Count);
        }
        
        [Fact]
        public void DeserializeUnsafe_InvalidDataCannotDeserialize_ThrowsInvalidOperationException()
        {
            // Act
            Assert.Throws<InvalidOperationException>(() => Roaring32Bitmap.DeserializeUnsafe([], SerializationFormat.Portable));
        }
    }
    
    public class GetSerializedSize
    {
        [Theory]
        [InlineData(SerializationFormat.Normal)]
        [InlineData((SerializationFormat)int.MaxValue)]
        public void GetSerializedSize_NotSupportedSerializationFormat_ThrowsArgumentOutOfRangeException(SerializationFormat serializationFormat)
        {
            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => Roaring32Bitmap.GetSerializedSize([], 10, serializationFormat));
        }
        
        [Fact]
        public void GetSerializedSize_Portable_InvalidDataCannotDeserialize_ReturnsZero()
        {
            // Act
            var actual = Roaring32Bitmap.GetSerializedSize([1, 2, 3], uint.MaxValue);
            
            // Assert
            Assert.Equal(0U, actual);
        }
        
        [Fact]
        public void GetSerializedSize_Portable_ReturnsNumberOfBytesOfSerializedBitmapInBuffer()
        {
            // Arrange
            using var bitmap = SerializationTestBitmap.GetTestBitmap();
            var testData = File.ReadAllBytes($"{nameof(CRoaring)}/TestData/bitmapwithoutruns.bin");
            Array.Resize(ref testData, testData.Length + 100);
            
            // Act
            var actual = Roaring32Bitmap.GetSerializedSize(testData, (nuint)testData.Length);

            // Assert
            Assert.Equal((nuint)testData.Length - 100, actual);
        }
    }
}