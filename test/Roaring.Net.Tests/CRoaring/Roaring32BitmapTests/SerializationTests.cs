using System;
using System.IO;
using Roaring.Net.CRoaring;
using Roaring.Net.Tests.CRoaring.TestData;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring32BitmapTests;

public class SerializationTests
{
    public class GetSerializationSize
    {
        [Theory]
        [InlineTestObject]
        public void GetSerializationSize_NotSupportedSerializationFormat_ThrowsArgumentOutOfRangeException(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetEmpty();

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                testObject.ReadOnlyBitmap.GetSerializationSize((SerializationFormat)int.MaxValue);
            });
        }

        [Theory]
        [InlineTestObject(SerializationFormat.Normal, 5)]
        [InlineTestObject(SerializationFormat.Portable, 8)]
        [InlineTestObject(SerializationFormat.Frozen, 4)]
        public void GetSerializationSize_EmptyBitmap_ReturnsValueGreaterThanZero(SerializationFormat format, int size, IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetEmpty();

            // Act
            var actual = testObject.ReadOnlyBitmap.GetSerializationSize(format);

            // Assert
            Assert.Equal((nuint)size, actual);
        }

        [Theory]
        [InlineTestObject(SerializationFormat.Normal, 4005)]
        [InlineTestObject(SerializationFormat.Portable, 10008)]
        [InlineTestObject(SerializationFormat.Frozen, 7004)]
        public void GetSerializationSize_BitmapContainsValues_ReturnsValueGreaterThanZero(SerializationFormat format, int size, IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetDefault();

            // Act
            var actual = testObject.ReadOnlyBitmap.GetSerializationSize(format);

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
            using IRoaring32BitmapTestObject testObject = factory.GetFromValues(SerializationTestBitmap.GetTestBitmapValues().ToArray());

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
            using IRoaring32BitmapTestObject testObject = factory.GetFromValues(SerializationTestBitmap.GetTestBitmapValues().ToArray());

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
            using Roaring32Bitmap bitmap = SerializationTestBitmap.GetTestBitmap();

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
            using IRoaring32BitmapTestObject testObject = factory.GetFromValues(SerializationTestBitmap.GetTestBitmapValues().ToArray());

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
            using Roaring32Bitmap bitmap = SerializationTestBitmap.GetTestBitmap();

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
            using IRoaring32BitmapTestObject testObject = factory.GetFromValues(SerializationTestBitmap.GetTestBitmapValues().ToArray());

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
            using IRoaring32BitmapTestObject testObject = factory.GetFromValues(SerializationTestBitmap.GetTestBitmapValues().ToArray());

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
            using IRoaring32BitmapTestObject testObject = factory.GetFromValues(SerializationTestBitmap.GetTestBitmapValues().ToArray());

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
            using Roaring32Bitmap bitmap = SerializationTestBitmap.GetTestBitmap();

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
            using Roaring32Bitmap bitmap = SerializationTestBitmap.GetTestBitmap();

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
            using Roaring32Bitmap bitmap = SerializationTestBitmap.GetTestBitmap();

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
        public void Deserialize_NullBuffer_ThrowsArgumentNullException()
        {
            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => Roaring32Bitmap.Deserialize(null!));
        }

        [Fact]
        public void Deserialize_NotSupportedSerializationFormat_ThrowsArgumentOutOfRangeException()
        {
            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => Roaring32Bitmap.Deserialize([1, 2, 3], (SerializationFormat)int.MaxValue));
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
        public void DeserializeUnsafe_NullBuffer_ThrowsArgumentNullException()
        {
            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => Roaring32Bitmap.DeserializeUnsafe(null!));
        }

        [Fact]
        public void DeserializeUnsafe_NotSupportedSerializationFormat_ThrowsArgumentOutOfRangeException()
        {
            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => Roaring32Bitmap.DeserializeUnsafe([1, 2, 3], (SerializationFormat)int.MaxValue));
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
            using Roaring32Bitmap bitmap = SerializationTestBitmap.GetTestBitmap();
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
            using Roaring32Bitmap bitmap = SerializationTestBitmap.GetTestBitmap();
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
            using Roaring32Bitmap bitmap = SerializationTestBitmap.GetTestBitmap();
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
            using Roaring32Bitmap bitmap = SerializationTestBitmap.GetTestBitmap();
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

    public class GetDeserializationSize
    {
        [Fact]
        public void GetDeserializationSize_NullBuffer_ThrowsArgumentNullException()
        {
            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => Roaring32Bitmap.GetDeserializationSize(null!, 10));
        }

        [Theory]
        [InlineData(SerializationFormat.Normal)]
        [InlineData((SerializationFormat)int.MaxValue)]
        public void GetDeserializationSize_NotSupportedSerializationFormat_ThrowsArgumentOutOfRangeException(SerializationFormat serializationFormat)
        {
            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => Roaring32Bitmap.GetDeserializationSize([1, 2, 3], 10, serializationFormat));
        }

        [Fact]
        public void GetDeserializationSize_Portable_InvalidDataCannotDeserialize_ReturnsZero()
        {
            // Act
            var actual = Roaring32Bitmap.GetDeserializationSize([1, 2, 3], uint.MaxValue);

            // Assert
            Assert.Equal(0U, actual);
        }

        [Fact]
        public void GetDeserializationSize_Portable_ReturnsNumberOfBytesOfSerializedBitmapInBuffer()
        {
            // Arrange
            using Roaring32Bitmap bitmap = SerializationTestBitmap.GetTestBitmap();
            var testData = File.ReadAllBytes($"{nameof(CRoaring)}/TestData/bitmapwithoutruns.bin");
            Array.Resize(ref testData, testData.Length + 100);

            // Act
            var actual = Roaring32Bitmap.GetDeserializationSize(testData, (nuint)testData.Length);

            // Assert
            Assert.Equal((nuint)testData.Length - 100, actual);
        }
    }

    public class SerializeSpan
    {
        [Theory]
        [InlineTestObject]
        public void Serialize_NotSupportedSerializationFormat_ThrowsArgumentOutOfRangeException(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetFromValues(SerializationTestBitmap.GetTestBitmapValues().ToArray());

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => testObject.ReadOnlyBitmap.Serialize(new byte[1], (SerializationFormat)int.MaxValue));
        }

        [Theory]
        [InlineTestObject(SerializationFormat.Normal)]
        [InlineTestObject(SerializationFormat.Portable)]
        [InlineTestObject(SerializationFormat.Frozen)]
        public void Serialize_DestinationTooSmall_ThrowsArgumentOutOfRangeException(SerializationFormat format, IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetFromValues(SerializationTestBitmap.GetTestBitmapValues().ToArray());
            var destination = new byte[(int)testObject.ReadOnlyBitmap.GetSerializationSize(format) - 1];

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => testObject.ReadOnlyBitmap.Serialize(destination, format));
        }

        [Theory]
        [InlineTestObject(SerializationFormat.Normal)]
        [InlineTestObject(SerializationFormat.Portable)]
        [InlineTestObject(SerializationFormat.Frozen)]
        public void Serialize_WritesSerializedBitmapToDestination_EqualsToSerialize(SerializationFormat format, IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetFromValues(SerializationTestBitmap.GetTestBitmapValues().ToArray());
            var destination = new byte[testObject.ReadOnlyBitmap.GetSerializationSize(format)];

            // Act
            testObject.ReadOnlyBitmap.Serialize(destination, format);

            // Assert
            Assert.Equal(testObject.ReadOnlyBitmap.Serialize(format), destination);
        }
    }

    public class DeserializeSpan
    {
        [Fact]
        public void Deserialize_NotSupportedSerializationFormat_ThrowsArgumentOutOfRangeException()
        {
            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => Roaring32Bitmap.Deserialize([1, 2, 3], (SerializationFormat)int.MaxValue));
        }

        [Fact]
        public void Deserialize_InvalidDataCannotDeserialize_ThrowsInvalidOperationException()
        {
            // Act && Assert
            Assert.Throws<InvalidOperationException>(() => Roaring32Bitmap.Deserialize([]));
        }

        [Fact]
        public void Deserialize_DeserializesBitmapFromJava()
        {
            // Arrange
            using Roaring32Bitmap bitmap = SerializationTestBitmap.GetTestBitmap();
            var testData = File.ReadAllBytes($"{nameof(CRoaring)}/TestData/bitmapwithoutruns.bin");

            // Act
            var deserializedBitmap = Roaring32Bitmap.Deserialize((ReadOnlySpan<byte>)testData, SerializationFormat.Portable);

            // Assert
            Assert.Equal(bitmap.AndCount(deserializedBitmap), bitmap.Count);
        }
    }

    public class DeserializeUnsafeSpan
    {
        [Fact]
        public void DeserializeUnsafe_NotSupportedSerializationFormat_ThrowsArgumentOutOfRangeException()
        {
            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => Roaring32Bitmap.DeserializeUnsafe([1, 2, 3], (SerializationFormat)int.MaxValue));
        }

        [Fact]
        public void DeserializeUnsafe_DeserializesBitmapFromJava()
        {
            // Arrange
            using Roaring32Bitmap bitmap = SerializationTestBitmap.GetTestBitmap();
            var testData = File.ReadAllBytes($"{nameof(CRoaring)}/TestData/bitmapwithoutruns.bin");

            // Act
            var deserializedBitmap = Roaring32Bitmap.DeserializeUnsafe((ReadOnlySpan<byte>)testData, SerializationFormat.Portable);

            // Assert
            Assert.Equal(bitmap.AndCount(deserializedBitmap), bitmap.Count);
        }
    }

    public class GetDeserializationSizeSpan
    {
        [Theory]
        [InlineData(SerializationFormat.Normal)]
        [InlineData((SerializationFormat)int.MaxValue)]
        public void GetDeserializationSize_NotSupportedSerializationFormat_ThrowsArgumentOutOfRangeException(SerializationFormat serializationFormat)
        {
            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => Roaring32Bitmap.GetDeserializationSize([1, 2, 3], 10, serializationFormat));
        }

        [Fact]
        public void GetDeserializationSize_Portable_InvalidDataCannotDeserialize_ReturnsZero()
        {
            // Act
            var actual = Roaring32Bitmap.GetDeserializationSize([1, 2, 3], uint.MaxValue);

            // Assert
            Assert.Equal(0U, actual);
        }

        [Fact]
        public void GetDeserializationSize_Portable_ReturnsNumberOfBytesOfSerializedBitmapInBuffer()
        {
            // Arrange
            using Roaring32Bitmap bitmap = SerializationTestBitmap.GetTestBitmap();
            var testData = File.ReadAllBytes($"{nameof(CRoaring)}/TestData/bitmapwithoutruns.bin");
            Array.Resize(ref testData, testData.Length + 100);

            // Act
            var actual = Roaring32Bitmap.GetDeserializationSize((ReadOnlySpan<byte>)testData, (nuint)testData.Length);

            // Assert
            Assert.Equal((nuint)testData.Length - 100, actual);
        }
    }
}