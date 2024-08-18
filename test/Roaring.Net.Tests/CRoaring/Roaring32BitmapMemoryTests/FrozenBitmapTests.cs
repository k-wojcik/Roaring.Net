using System;
using Roaring.Net.CRoaring;
using Roaring.Net.Tests.CRoaring.TestData;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring32BitmapMemoryTests;

public class FrozenBitmapTests
{
    public class ToFrozen
    {
        [Fact]
        public void ToFrozen_Disposed_ThrowsObjectDisposedException()
        {
            // Arrange
            var bitmapMemory = new Roaring32BitmapMemory(10);
            bitmapMemory.Dispose();
            
            // Act && Assert
            Assert.Throws<ObjectDisposedException>(() =>
            {
                bitmapMemory.ToFrozen();
            });
        }
        
        [Fact]
        public void ToFrozen_MemorySizeEqualToZero_ThrowsArgumentOutOfRangeException()
        {
            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                using var bitmapMemory = new Roaring32BitmapMemory(0);
            });
        }
        
        [Fact]
        public void ToFrozen_NotSupportedSerializationFormat_ThrowsArgumentOutOfRangeException()
        {
            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                using var bitmapMemory = new Roaring32BitmapMemory(10);
                bitmapMemory.ToFrozen((SerializationFormat)int.MaxValue);
            });
        }
        
        [Fact]
        public void ToFrozen_InvalidDataCannotDeserialize_ThrowsInvalidOperationException()
        {
            // Act && Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                using var bitmapMemory = new Roaring32BitmapMemory(1);
                bitmapMemory.ToFrozen();
            });
        }
        
        [Fact]
        public void ToFrozen_FromFrozen_ReturnsValidFrozenBitmap()
        {
            // Arrange
            using var bitmap = SerializationTestBitmap.GetTestBitmap();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Frozen);
            
            // Act
            using var bitmapMemory = new Roaring32BitmapMemory((nuint)serializedBitmap.Length);
            serializedBitmap.CopyTo(bitmapMemory.AsSpan());
            using var frozenBitmap = bitmapMemory.ToFrozen();
            
            // Assert
            Assert.Equal(bitmap.Values, frozenBitmap.Values);
            Assert.True(bitmap.IsValid());
        }
        
        [Fact]
        public void ToFrozen_FromPortable_ReturnsValidFrozenBitmap()
        {
            // Arrange
            using var bitmap = SerializationTestBitmap.GetTestBitmap();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Portable);
            
            // Act
            using var bitmapMemory = new Roaring32BitmapMemory((nuint)serializedBitmap.Length);
            serializedBitmap.CopyTo(bitmapMemory.AsSpan());
            using var frozenBitmap = bitmapMemory.ToFrozen(SerializationFormat.Portable);
            
            // Assert
            Assert.Equal(bitmap.Values, frozenBitmap.Values);
            Assert.True(bitmap.IsValid());
        }
    }
}