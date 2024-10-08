﻿using System;
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
            using Roaring32Bitmap bitmap = SerializationTestBitmap.GetTestBitmap();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Frozen);
            var bitmapMemory = new Roaring32BitmapMemory((nuint)serializedBitmap.Length);
            serializedBitmap.CopyTo(bitmapMemory.AsSpan());
            using FrozenRoaring32Bitmap referencedBitmap = bitmapMemory.ToFrozen();

            // Act && Assert
            bitmapMemory.Dispose();
            bitmapMemory.AsSpan();
        }

        [Fact]
        public void Dispose_NoReferencesToBitmap_DisposeAndThrowObjectDisposedException()
        {
            // Arrange
            using Roaring32Bitmap bitmap = SerializationTestBitmap.GetTestBitmap();
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
            using Roaring32Bitmap bitmap = SerializationTestBitmap.GetTestBitmap();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Frozen);
            var bitmapMemory = new Roaring32BitmapMemory((nuint)serializedBitmap.Length);
            serializedBitmap.CopyTo(bitmapMemory.AsSpan());
            FrozenRoaring32Bitmap referencedBitmap = bitmapMemory.ToFrozen();

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
            using Roaring32Bitmap bitmap = SerializationTestBitmap.GetTestBitmap();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Frozen);
            var bitmapMemory = new Roaring32BitmapMemory((nuint)serializedBitmap.Length);
            serializedBitmap.CopyTo(bitmapMemory.AsSpan());
            FrozenRoaring32Bitmap referencedBitmap1 = bitmapMemory.ToFrozen();
            using FrozenRoaring32Bitmap referencedBitmap2 = bitmapMemory.ToFrozen();

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
            using Roaring32Bitmap bitmap = SerializationTestBitmap.GetTestBitmap();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Frozen);
            var bitmapMemory = new Roaring32BitmapMemory((nuint)serializedBitmap.Length);

            // Act && Assert
            bitmapMemory.Dispose();
            bitmapMemory.Dispose();
        }
    }
}