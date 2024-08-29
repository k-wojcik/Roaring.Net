using System;
using System.Reflection;
using Roaring.Net.CRoaring;
using Roaring.Net.Tests.CRoaring.TestData;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.FrozenRoaring32BitmapTests;

public class DisposeTests
{
    public class Dispose
    {
        [Fact]
        public void Dispose_InvokedMoreThanOnce_BlocksRedundantCalls()
        {
            // Arrange
            using Roaring32Bitmap bitmap = SerializationTestBitmap.GetTestBitmap();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Frozen);
            using var bitmapMemory = new Roaring32BitmapMemory((nuint)serializedBitmap.Length);
            serializedBitmap.CopyTo(bitmapMemory.AsSpan());
            FrozenRoaring32Bitmap frozenBitmap = bitmapMemory.ToFrozen();

            // Act && Assert
            frozenBitmap.Dispose();
            frozenBitmap.Dispose();
        }

        [Fact]
        public void Dispose_MemoryObject_ReleasesMemoryObject()
        {
            // Arrange
            using Roaring32Bitmap bitmap = SerializationTestBitmap.GetTestBitmap();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Frozen);
            var bitmapMemory = new Roaring32BitmapMemory((nuint)serializedBitmap.Length);
            serializedBitmap.CopyTo(bitmapMemory.AsSpan());
            FrozenRoaring32Bitmap frozenBitmap = bitmapMemory.ToFrozen();
            bitmapMemory.Dispose();

            // Act && Assert
            frozenBitmap.Dispose();

            // Act
            Assert.Throws<ObjectDisposedException>(() => bitmapMemory.AsSpan());
        }
    }

    public class Finalizer
    {
        [Fact]
        public void Finalizer_InvokedMoreThanOnce_BlocksRedundantCalls()
        {
            // Arrange
            using Roaring32Bitmap bitmap = SerializationTestBitmap.GetTestBitmap();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Frozen);
            using var bitmapMemory = new Roaring32BitmapMemory((nuint)serializedBitmap.Length);
            serializedBitmap.CopyTo(bitmapMemory.AsSpan());
            using FrozenRoaring32Bitmap frozenBitmap = bitmapMemory.ToFrozen();
            MethodInfo? finalizer = frozenBitmap.GetType().GetMethod("Finalize", BindingFlags.Instance | BindingFlags.NonPublic);

            // Act && Assert
            Assert.NotNull(finalizer);
            finalizer.Invoke(frozenBitmap, null);
            finalizer.Invoke(frozenBitmap, null);
        }

        [Fact]
        public void Finalizer_MemoryObject_ReleasesMemoryObject()
        {
            // Arrange
            using Roaring32Bitmap bitmap = SerializationTestBitmap.GetTestBitmap();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Frozen);
            var bitmapMemory = new Roaring32BitmapMemory((nuint)serializedBitmap.Length);
            serializedBitmap.CopyTo(bitmapMemory.AsSpan());
            using FrozenRoaring32Bitmap frozenBitmap = bitmapMemory.ToFrozen();
            bitmapMemory.Dispose();
            MethodInfo? finalizer = frozenBitmap.GetType().GetMethod("Finalize", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(finalizer);

            // Act 
            finalizer.Invoke(frozenBitmap, null);

            // Act
            Assert.Throws<ObjectDisposedException>(() => bitmapMemory.AsSpan());
        }
    }
}