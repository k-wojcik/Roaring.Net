using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Roaring.Net.CRoaring;
using Roaring.Net.Tests.CRoaring.TestData;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.FrozenRoaring64BitmapTests;

public class DisposeTests
{
    public class Dispose
    {
        [Fact]
        public void Dispose_InvokedMoreThanOnce_BlocksRedundantCalls()
        {
            // Arrange
            using Roaring64Bitmap bitmap = SerializationTestBitmap.GetTestBitmap64();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Frozen);
            using var bitmapMemory = new Roaring64BitmapMemory((nuint)serializedBitmap.Length);
            serializedBitmap.CopyTo(bitmapMemory.AsSpan());
            FrozenRoaring64Bitmap frozenBitmap = bitmapMemory.ToFrozen();

            // Act && Assert
            frozenBitmap.Dispose();
            frozenBitmap.Dispose();
        }

        [Fact]
        public void Dispose_MemoryObject_ReleasesMemoryObject()
        {
            // Arrange
            using Roaring64Bitmap bitmap = SerializationTestBitmap.GetTestBitmap64();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Frozen);
            var bitmapMemory = new Roaring64BitmapMemory((nuint)serializedBitmap.Length);
            serializedBitmap.CopyTo(bitmapMemory.AsSpan());
            FrozenRoaring64Bitmap frozenBitmap = bitmapMemory.ToFrozen();
            bitmapMemory.Dispose();

            // Act && Assert
            frozenBitmap.Dispose();

            // Act
            Assert.Throws<ObjectDisposedException>(() => bitmapMemory.AsSpan());
        }
    }

    public class Lifetime
    {
        [Fact]
        public void Values_FrozenBitmapDroppedBeforeEnumeration_KeepsFrozenBitmapAlive()
        {
            // Arrange
            IEnumerator<ulong> enumerator = CreateEnumerator(out WeakReference ownerReference, out SafeRoaring64MemoryHandle memoryHandle);

            // Act
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // Assert
            Assert.True(ownerReference.IsAlive);
            Assert.False(memoryHandle.IsClosed);
            Assert.True(enumerator.MoveNext());

            enumerator.Dispose();
        }

        [Fact]
        public void Values_FrozenBitmapDisposedBeforeIteration_IteratesAllValues()
        {
            // Arrange
            using Roaring64Bitmap bitmap = SerializationTestBitmap.GetTestBitmap64();
            FrozenRoaring64Bitmap frozenBitmap = bitmap.ToFrozen();
            var expected = frozenBitmap.ToArray();
            SafeRoaring64MemoryHandle memoryHandle = frozenBitmap.Memory.Handle;
            IEnumerator<ulong> enumerator = frozenBitmap.Values.GetEnumerator();

            // Act
            frozenBitmap.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            List<ulong> actual = new();
            while (enumerator.MoveNext())
            {
                actual.Add(enumerator.Current);
            }

            // Assert
            Assert.Equal(expected, actual);
            Assert.False(memoryHandle.IsClosed);

            enumerator.Dispose();
            Assert.True(memoryHandle.IsClosed);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static IEnumerator<ulong> CreateEnumerator(out WeakReference ownerReference, out SafeRoaring64MemoryHandle memoryHandle)
        {
            using Roaring64Bitmap bitmap = SerializationTestBitmap.GetTestBitmap64();
            FrozenRoaring64Bitmap frozenBitmap = bitmap.ToFrozen();
            ownerReference = new WeakReference(frozenBitmap);
            memoryHandle = frozenBitmap.Memory.Handle;

            return frozenBitmap.Values.GetEnumerator();
        }

        [Fact]
        public void Values_EnumeratorDisposed_ReleasesFrozenBitmapAndMemory()
        {
            // Arrange
            IEnumerator<ulong> enumerator = CreateEnumerator(out WeakReference ownerReference, out SafeRoaring64MemoryHandle memoryHandle);

            // Act
            enumerator.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // Assert
            Assert.False(ownerReference.IsAlive);
            Assert.True(memoryHandle.IsClosed);
            GC.KeepAlive(enumerator);
        }

        [Fact]
        public void Values_TwoEnumerators_DisposingOneKeepsFrozenBitmapAndMemoryAlive()
        {
            // Arrange
            CreateTwoEnumerators(out IEnumerator<ulong> first, out IEnumerator<ulong> second, out WeakReference ownerReference, out SafeRoaring64MemoryHandle memoryHandle);

            // Act
            first.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // Assert
            Assert.True(ownerReference.IsAlive);
            Assert.False(memoryHandle.IsClosed);
            Assert.True(second.MoveNext());

            second.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Assert.False(ownerReference.IsAlive);
            Assert.True(memoryHandle.IsClosed);
            GC.KeepAlive(first);
            GC.KeepAlive(second);
        }

        [Fact]
        public void Values_GarbageCollectedEnumerator_WithLiveFrozen_DoesNotReleaseFrozenBitmapOrMemory()
        {
            // Arrange
            FrozenRoaring64Bitmap frozenBitmap = CreateFrozenWithGarbageCollectedEnumerator(out WeakReference enumeratorReference, out SafeRoaring64MemoryHandle memoryHandle);

            // Act
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // Assert
            Assert.False(enumeratorReference.IsAlive);
            Assert.False(memoryHandle.IsClosed);
            Assert.True(frozenBitmap.Count > 0);

            frozenBitmap.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Assert.True(memoryHandle.IsClosed);
        }

        [Fact]
        public void Values_EnumeratorDisposed_FrozenFromMemory_ReleasesFrozenBitmapAndMemory()
        {
            // Arrange
            Roaring64BitmapMemory bitmapMemory = CreateMemoryWithDisposableEnumerator(out IEnumerator<ulong> enumerator, out WeakReference ownerReference);

            // Act
            enumerator.Dispose();
            bitmapMemory.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // Assert
            Assert.False(ownerReference.IsAlive);
            Assert.True(bitmapMemory.Handle.IsClosed);
            GC.KeepAlive(enumerator);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void CreateTwoEnumerators(out IEnumerator<ulong> first, out IEnumerator<ulong> second, out WeakReference ownerReference, out SafeRoaring64MemoryHandle memoryHandle)
        {
            using Roaring64Bitmap bitmap = SerializationTestBitmap.GetTestBitmap64();
            FrozenRoaring64Bitmap frozenBitmap = bitmap.ToFrozen();
            ownerReference = new WeakReference(frozenBitmap);
            memoryHandle = frozenBitmap.Memory.Handle;

            first = frozenBitmap.Values.GetEnumerator();
            second = frozenBitmap.Values.GetEnumerator();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static FrozenRoaring64Bitmap CreateFrozenWithGarbageCollectedEnumerator(out WeakReference enumeratorReference, out SafeRoaring64MemoryHandle memoryHandle)
        {
            using Roaring64Bitmap bitmap = SerializationTestBitmap.GetTestBitmap64();
            FrozenRoaring64Bitmap frozenBitmap = bitmap.ToFrozen();
            memoryHandle = frozenBitmap.Memory.Handle;
            enumeratorReference = new WeakReference(frozenBitmap.Values.GetEnumerator());

            return frozenBitmap;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Roaring64BitmapMemory CreateMemoryWithDisposableEnumerator(out IEnumerator<ulong> enumerator, out WeakReference ownerReference)
        {
            using Roaring64Bitmap bitmap = SerializationTestBitmap.GetTestBitmap64();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Frozen);
            var bitmapMemory = new Roaring64BitmapMemory((nuint)serializedBitmap.Length);
            serializedBitmap.CopyTo(bitmapMemory.AsSpan());
            FrozenRoaring64Bitmap frozenBitmap = bitmapMemory.ToFrozen();
            ownerReference = new WeakReference(frozenBitmap);
            enumerator = frozenBitmap.Values.GetEnumerator();

            return bitmapMemory;
        }

        [Fact]
        public void GarbageCollectedFrozenBitmap_WithoutReferences_ReleasesOwnedMemory()
        {
            // Arrange
            SafeRoaring64MemoryHandle memoryHandle = CreateFrozenWithOwnedMemory(out WeakReference frozenReference);

            // Act
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // Assert
            Assert.False(frozenReference.IsAlive);
            Assert.True(memoryHandle.IsClosed);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static SafeRoaring64MemoryHandle CreateFrozenWithOwnedMemory(out WeakReference frozenReference)
        {
            using Roaring64Bitmap bitmap = SerializationTestBitmap.GetTestBitmap64();
            FrozenRoaring64Bitmap frozenBitmap = bitmap.ToFrozen();
            frozenReference = new WeakReference(frozenBitmap);

            return frozenBitmap.Memory.Handle;
        }

        [Fact]
        public void GarbageCollectedFrozenBitmap_AfterMemoryDisposed_ReleasesMemory()
        {
            // Arrange
            Roaring64BitmapMemory bitmapMemory = CreateMemoryWithFrozen(out WeakReference frozenReference);

            // Act
            bitmapMemory.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // Assert
            Assert.False(frozenReference.IsAlive);
            Assert.Throws<ObjectDisposedException>(() => bitmapMemory.AsSpan());
        }

        [Fact]
        public void GarbageCollectedEnumerator_AfterMemoryDisposed_ReleasesBitmapReference()
        {
            // Arrange
            Roaring64BitmapMemory bitmapMemory = CreateMemoryWithEnumerator(out WeakReference enumeratorReference);

            // Act
            bitmapMemory.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // Assert
            Assert.False(enumeratorReference.IsAlive);
            Assert.Throws<ObjectDisposedException>(() => bitmapMemory.AsSpan());
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Roaring64BitmapMemory CreateMemoryWithEnumerator(out WeakReference enumeratorReference)
        {
            using Roaring64Bitmap bitmap = SerializationTestBitmap.GetTestBitmap64();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Frozen);
            var bitmapMemory = new Roaring64BitmapMemory((nuint)serializedBitmap.Length);
            serializedBitmap.CopyTo(bitmapMemory.AsSpan());
            FrozenRoaring64Bitmap frozenBitmap = bitmapMemory.ToFrozen();
            enumeratorReference = new WeakReference(frozenBitmap.Values.GetEnumerator());

            return bitmapMemory;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Roaring64BitmapMemory CreateMemoryWithFrozen(out WeakReference frozenReference)
        {
            using Roaring64Bitmap bitmap = SerializationTestBitmap.GetTestBitmap64();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Frozen);
            var bitmapMemory = new Roaring64BitmapMemory((nuint)serializedBitmap.Length);
            serializedBitmap.CopyTo(bitmapMemory.AsSpan());
            FrozenRoaring64Bitmap frozenBitmap = bitmapMemory.ToFrozen();
            frozenReference = new WeakReference(frozenBitmap);

            return bitmapMemory;
        }
    }
}