using System;
using Roaring.Net.CRoaring;
using Roaring.Net.Tests.CRoaring.TestData;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring32BitmapMemoryTests;

public class DisposeTests
{
    public class Dispose
    {
        [Fact]
        public void Dispose_HasReferenceToBitmap_DoesNotDisposeAndNotThrowObjectDisposedException()
        {   
            // Arrange
            using var bitmap = SerializationTestBitmap.GetTestBitmap();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Frozen);
            var bitmapMemory = new Roaring32BitmapMemory((nuint)serializedBitmap.Length);
            serializedBitmap.CopyTo(bitmapMemory.AsSpan());
            using var referencedBitmap = bitmapMemory.ToFrozen();

            // Act && Assert
            bitmapMemory.Dispose();
            bitmapMemory.AsSpan();
        }
        
        [Fact]
        public void Dispose_NoReferencesToBitmap_DisposeAndThrowObjectDisposedException()
        {   
            // Arrange
            using var bitmap = SerializationTestBitmap.GetTestBitmap();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Frozen);
            var bitmapMemory = new Roaring32BitmapMemory((nuint)serializedBitmap.Length);

            // Act
            bitmapMemory.Dispose();
            
            // Assert
            Assert.Throws<ObjectDisposedException>(() => bitmapMemory.AsSpan());
        }
        
        [Fact]
        public void Dispose_ReferenceBitmapHasBeenDisposedAndNoOtherReferencesToBitmap_DisposeAndThrowObjectDisposedException()
        {   
            // Arrange
            using var bitmap = SerializationTestBitmap.GetTestBitmap();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Frozen);
            var bitmapMemory = new Roaring32BitmapMemory((nuint)serializedBitmap.Length);
            serializedBitmap.CopyTo(bitmapMemory.AsSpan());
            var referencedBitmap = bitmapMemory.ToFrozen();

            // Act
            referencedBitmap.Dispose();
            bitmapMemory.Dispose();
            
            // Assert
            Assert.Throws<ObjectDisposedException>(() => bitmapMemory.AsSpan());
        }
        
        [Fact]
        public void Dispose_ReferenceBitmapHasBeenDisposedAndHasOtherReferencesToBitmap_DoesNotDisposeAndNotThrowObjectDisposedException()
        {   
            // Arrange
            using var bitmap = SerializationTestBitmap.GetTestBitmap();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Frozen);
            var bitmapMemory = new Roaring32BitmapMemory((nuint)serializedBitmap.Length);
            serializedBitmap.CopyTo(bitmapMemory.AsSpan());
            var referencedBitmap1 = bitmapMemory.ToFrozen();
            using var referencedBitmap2 = bitmapMemory.ToFrozen();

            // Act
            referencedBitmap1.Dispose();
            bitmapMemory.Dispose();
            
            // Assert
            bitmapMemory.AsSpan();
        }
         
        [Fact]
        public void Dispose_InvokedMoreThanOnce_BlocksRedundantCalls()
        {   
            // Arrange
            using var bitmap = SerializationTestBitmap.GetTestBitmap();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Frozen);
            var bitmapMemory = new Roaring32BitmapMemory((nuint)serializedBitmap.Length);

            // Act && Assert
            bitmapMemory.Dispose();
            bitmapMemory.Dispose();
        }
    }
}