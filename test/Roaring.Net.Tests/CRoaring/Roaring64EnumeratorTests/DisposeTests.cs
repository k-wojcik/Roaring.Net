using System;
using System.Collections.Generic;
using Roaring.Net.CRoaring;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring64EnumeratorTests;

public class DisposeTests
{
    public class Lifetime
    {
        [Fact]
        public void Dispose_InvokedMoreThanOnce_BlocksRedundantCalls()
        {
            // Arrange
            using var bitmap = new Roaring64Bitmap(new ulong[] { 1 });
            IEnumerator<ulong> enumerator = bitmap.Values.GetEnumerator();

            // Act && Assert
            enumerator.Dispose();
            enumerator.Dispose();
        }

        [Fact]
        public void MoveNext_BitmapDisposedBeforeIteration_IteratesAllValues()
        {
            // Arrange
            using var bitmap = new Roaring64Bitmap(new ulong[] { 1, 2, 3 });
            using IEnumerator<ulong> enumerator = bitmap.Values.GetEnumerator();

            // Act
            bitmap.Dispose();

            var values = new List<ulong>();
            while (enumerator.MoveNext())
            {
                values.Add(enumerator.Current);
            }

            // Assert
            Assert.Equal(new ulong[] { 1, 2, 3 }, values);
        }

        [Fact]
        public void MoveNext_EnumeratorDisposed_ThrowsObjectDisposedException()
        {
            // Arrange
            using var bitmap = new Roaring64Bitmap(new ulong[] { 1 });
            IEnumerator<ulong> enumerator = bitmap.Values.GetEnumerator();
            enumerator.Dispose();

            // Act && Assert
            Assert.Throws<ObjectDisposedException>(() => enumerator.MoveNext());
        }

        [Fact]
        public void Current_EnumeratorDisposed_ThrowsObjectDisposedException()
        {
            // Arrange
            using var bitmap = new Roaring64Bitmap(new ulong[] { 1 });
            IEnumerator<ulong> enumerator = bitmap.Values.GetEnumerator();
            enumerator.Dispose();

            // Act && Assert
            Assert.Throws<ObjectDisposedException>(() => _ = enumerator.Current);
        }
    }
}