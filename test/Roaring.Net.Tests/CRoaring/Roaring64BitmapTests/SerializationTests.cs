using System;
using System.IO;
using Roaring.Net.CRoaring;
using Roaring.Net.Tests.CRoaring.TestData;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring64BitmapTests;

public class SerializationTests
{
    public class GetSerializationSize
    {
        [Theory]
        [InlineTestObject]
        public void GetSerializationSize_NotSupportedSerializationFormat_ThrowsArgumentOutOfRangeException(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetEmpty();

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                testObject.ReadOnlyBitmap.GetSerializationSize((SerializationFormat)int.MaxValue);
            });
        }

        [Theory]
        [InlineTestObject(SerializationFormat.Portable, 8)]
        public void GetSerializationSize_EmptyBitmap_ReturnsValueGreaterThanZero(SerializationFormat format, int size, IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetEmpty();

            // Act
            var actual = testObject.ReadOnlyBitmap.GetSerializationSize(format);

            // Assert
            Assert.Equal((nuint)size, actual);
        }

        [Theory]
        [InlineTestObject(SerializationFormat.Portable, 22008)]
        public void GetSerializationSize_BitmapContainsValues_ReturnsValueGreaterThanZero(SerializationFormat format, int size, IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetDefault();

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
        public void Serialize_NotSupportedSerializationFormat_ThrowsArgumentOutOfRangeException(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetFromValues(SerializationTestBitmap.GetTestBitmap64Values().ToArray());

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => testObject.ReadOnlyBitmap.Serialize((SerializationFormat)int.MaxValue));
        }
    }

    public class SerializePortable
    {
        [Theory]
        [InlineTestObject]
        public void Serialize_TestBitmap_EqualsToBitmapFromJava(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetFromValues(SerializationTestBitmap.GetTestBitmap64Values().ToArray());

            // Act
            var serializedBitmap = testObject.ReadOnlyBitmap.Serialize(SerializationFormat.Portable);

            // Assert
            var testData = File.ReadAllBytes($"{nameof(CRoaring)}/TestData/bitmap64withoutruns.bin");
            Assert.Equal(testData, serializedBitmap);
        }

        [Theory]
        [InlineTestObject]
        public void Serialize_Deserialize_SelfTest_EqualsToDeserialized(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetFromValues(SerializationTestBitmap.GetTestBitmap64Values().ToArray());

            // Act
            var serializedBitmap = testObject.ReadOnlyBitmap.Serialize(SerializationFormat.Portable);

            // Assert
            using var deserializedBitmap = Roaring64Bitmap.Deserialize(serializedBitmap, SerializationFormat.Portable);
            Assert.Equal(testObject.ReadOnlyBitmap.Values, deserializedBitmap.Values);
        }

        [Fact]
        public void Serialize_TestBitmapWithOptimize_EqualsToBitmapFromJava()
        {
            // Arrange
            using Roaring64Bitmap bitmap = SerializationTestBitmap.GetTestBitmap64();

            // Act
            bitmap.Optimize();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Portable);

            // Assert
            var testData = File.ReadAllBytes($"{nameof(CRoaring)}/TestData/bitmap64withruns.bin");
            Assert.Equal(testData, serializedBitmap);
        }

        [Fact]
        public void Serialize_Deserialize_SelfTestWithOptimize_EqualsToDeserialized()
        {
            // Arrange
            using Roaring64Bitmap bitmap = SerializationTestBitmap.GetTestBitmap64();

            // Act
            bitmap.Optimize();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Portable);

            // Assert
            using var deserializedBitmap = Roaring64Bitmap.Deserialize(serializedBitmap, SerializationFormat.Portable);
            Assert.Equal(bitmap.Values, deserializedBitmap.Values);
        }
    }

    public class Deserialize
    {
        [Fact]
        public void Deserialize_NullBuffer_ThrowsArgumentNullException()
        {
            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => Roaring64Bitmap.Deserialize(null!, SerializationFormat.Portable));
        }

        [Fact]
        public void Deserialize_NotSupportedSerializationFormat_ThrowsArgumentOutOfRangeException()
        {
            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => Roaring64Bitmap.Deserialize([], (SerializationFormat)int.MaxValue));
        }
    }

    public class DeserializeNormal
    {
        [Fact]
        public void Deserialize_InvalidDataCannotDeserialize_ThrowsInvalidOperationException()
        {
            // Act && Assert
            Assert.Throws<InvalidOperationException>(() => Roaring64Bitmap.Deserialize([]));
        }
    }

    public class DeserializePortable
    {
        [Fact]
        public void Deserialize_DeserializesBitmapFromJava()
        {
            // Arrange
            using Roaring64Bitmap bitmap = SerializationTestBitmap.GetTestBitmap64();
            var testData = File.ReadAllBytes($"{nameof(CRoaring)}/TestData/bitmap64withoutruns.bin");

            // Act
            var deserializedBitmap = Roaring64Bitmap.Deserialize(testData, SerializationFormat.Portable);

            // Assert
            Assert.Equal(bitmap.AndCount(deserializedBitmap), bitmap.Count);
        }

        [Fact]
        public void Deserialize_WithOptimize_DeserializesBitmapFromJava()
        {
            // Arrange
            using Roaring64Bitmap bitmap = SerializationTestBitmap.GetTestBitmap64();
            bitmap.Optimize();
            File.WriteAllBytes($"{nameof(CRoaring)}/TestData/bitmap64withruns.bin", bitmap.Serialize());
            var testData = File.ReadAllBytes($"{nameof(CRoaring)}/TestData/bitmap64withruns.bin");

            // Act
            var deserializedBitmap = Roaring64Bitmap.Deserialize(testData, SerializationFormat.Portable);

            // Assert
            Assert.Equal(bitmap.AndCount(deserializedBitmap), bitmap.Count);
        }

        [Fact]
        public void DeserializeAndInvalidDataCannotDeserialize_ThrowsInvalidOperationException()
        {
            // Act && Assert
            Assert.Throws<InvalidOperationException>(() => Roaring64Bitmap.Deserialize([], SerializationFormat.Portable));
        }
    }

    public class GetDeserializationSize
    {
        [Fact]
        public void GetDeserializationSize_NullBuffer_ThrowsArgumentNullException()
        {
            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => Roaring64Bitmap.GetDeserializationSize(null!, 10));
        }

        [Theory]
        [InlineData(SerializationFormat.Normal)]
        [InlineData((SerializationFormat)int.MaxValue)]
        public void GetDeserializationSize_NotSupportedSerializationFormat_ThrowsArgumentOutOfRangeException(SerializationFormat serializationFormat)
        {
            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => Roaring64Bitmap.GetDeserializationSize([1, 2, 3], 10, serializationFormat));
        }

        [Fact]
        public void GetDeserializationSize_Portable_InvalidDataCannotDeserialize_ReturnsZero()
        {
            // Act
            var actual = Roaring64Bitmap.GetDeserializationSize([1, 2, 3], uint.MaxValue);

            // Assert
            Assert.Equal(0U, actual);
        }

        [Fact]
        public void GetDeserializationSize_Portable_ReturnsNumberOfBytesOfSerializedBitmapInBuffer()
        {
            // Arrange
            using Roaring64Bitmap bitmap = SerializationTestBitmap.GetTestBitmap64();
            var testData = File.ReadAllBytes($"{nameof(CRoaring)}/TestData/bitmap64withoutruns.bin");
            Array.Resize(ref testData, testData.Length + 100);

            // Act
            var actual = Roaring64Bitmap.GetDeserializationSize(testData, (nuint)testData.Length);

            // Assert
            Assert.Equal((nuint)testData.Length - 100, actual);
        }
    }

    public class SerializeSpan
    {
        [Theory]
        [InlineTestObject]
        public void Serialize_NotSupportedSerializationFormat_ThrowsArgumentOutOfRangeException(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetFromValues(SerializationTestBitmap.GetTestBitmap64Values().ToArray());

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => testObject.ReadOnlyBitmap.Serialize(new byte[1], (SerializationFormat)int.MaxValue));
        }

        [Theory]
        [InlineTestObject(SerializationFormat.Portable)]
        [InlineTestObject(SerializationFormat.Frozen)]
        public void Serialize_DestinationTooSmall_ThrowsArgumentOutOfRangeException(SerializationFormat format, IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetFromValues(SerializationTestBitmap.GetTestBitmap64Values().ToArray());
            var destination = new byte[(int)testObject.ReadOnlyBitmap.GetSerializationSize(format) - 1];

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => testObject.ReadOnlyBitmap.Serialize(destination, format));
        }

        [Theory]
        [InlineTestObject(SerializationFormat.Portable)]
        [InlineTestObject(SerializationFormat.Frozen)]
        public void Serialize_WritesSerializedBitmapToDestination_EqualsToSerialize(SerializationFormat format, IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetFromValues(SerializationTestBitmap.GetTestBitmap64Values().ToArray());
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
            Assert.Throws<ArgumentOutOfRangeException>(() => Roaring64Bitmap.Deserialize((ReadOnlySpan<byte>)[], (SerializationFormat)int.MaxValue));
        }

        [Fact]
        public void Deserialize_InvalidDataCannotDeserialize_ThrowsInvalidOperationException()
        {
            // Act && Assert
            Assert.Throws<InvalidOperationException>(() => Roaring64Bitmap.Deserialize((ReadOnlySpan<byte>)[]));
        }

        [Fact]
        public void Deserialize_DeserializesBitmapFromJava()
        {
            // Arrange
            using Roaring64Bitmap bitmap = SerializationTestBitmap.GetTestBitmap64();
            var testData = File.ReadAllBytes($"{nameof(CRoaring)}/TestData/bitmap64withoutruns.bin");

            // Act
            var deserializedBitmap = Roaring64Bitmap.Deserialize((ReadOnlySpan<byte>)testData, SerializationFormat.Portable);

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
            Assert.Throws<ArgumentOutOfRangeException>(() => Roaring64Bitmap.GetDeserializationSize((ReadOnlySpan<byte>)[1, 2, 3], 10, serializationFormat));
        }

        [Fact]
        public void GetDeserializationSize_Portable_InvalidDataCannotDeserialize_ReturnsZero()
        {
            // Act
            var actual = Roaring64Bitmap.GetDeserializationSize((ReadOnlySpan<byte>)[1, 2, 3], uint.MaxValue);

            // Assert
            Assert.Equal(0U, actual);
        }

        [Fact]
        public void GetDeserializationSize_Portable_ReturnsNumberOfBytesOfSerializedBitmapInBuffer()
        {
            // Arrange
            using Roaring64Bitmap bitmap = SerializationTestBitmap.GetTestBitmap64();
            var testData = File.ReadAllBytes($"{nameof(CRoaring)}/TestData/bitmap64withoutruns.bin");
            Array.Resize(ref testData, testData.Length + 100);

            // Act
            var actual = Roaring64Bitmap.GetDeserializationSize((ReadOnlySpan<byte>)testData, (nuint)testData.Length);

            // Assert
            Assert.Equal((nuint)testData.Length - 100, actual);
        }
    }
}