using System;
using System.Threading;
using System.Threading.Tasks;
using Roaring.Net.CRoaring;
using Roaring.Net.Tests.CRoaring.TestData;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring32BitmapMemoryTests;

public class WriteTests
{
    public class Span
    {
        [Fact]
        public void AsSpan_Disposed_ThrowsObjectDisposedException()
        {
            // Arrange
            var bitmapMemory = new Roaring32BitmapMemory(10);
            bitmapMemory.Dispose();
            
            // Act && Assert
            Assert.Throws<ObjectDisposedException>(() =>
            {
                bitmapMemory.AsSpan();
            });
        }
        
        [Fact]
        public void AsSpan_CopiesDataToSpan_ReturnsValidBitmap()
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
    }
    
    public class WriteSpan
    {
        [Fact]
        public void Write_Disposed_ThrowsObjectDisposedException()
        {
            // Arrange
            var bitmapMemory = new Roaring32BitmapMemory(10);
            bitmapMemory.Dispose();
            
            // Act && Assert
            Assert.Throws<ObjectDisposedException>(() =>
            {
                bitmapMemory.Write(Array.Empty<byte>().AsSpan());
            });
        }
        
        [Fact]
        public void Write_WritesReadonlySpan_ReturnsValidBitmap()
        {
            // Arrange
            using var bitmap = SerializationTestBitmap.GetTestBitmap();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Frozen);
            
            // Act
            using var bitmapMemory = new Roaring32BitmapMemory((nuint)serializedBitmap.Length);
            bitmapMemory.Write(serializedBitmap.AsSpan());
            using var frozenBitmap = bitmapMemory.ToFrozen();
            
            // Assert
            Assert.Equal(bitmap.Values, frozenBitmap.Values);
            Assert.True(bitmap.IsValid());
        }
    }
    
    public class WriteSpanAsync
    {
        [Fact]
        public async Task WriteAsync_Disposed_ThrowsObjectDisposedException()
        {
            // Arrange
            var bitmapMemory = new Roaring32BitmapMemory(10);
            bitmapMemory.Dispose();
            
            // Act && Assert
            await Assert.ThrowsAsync<ObjectDisposedException>(async () =>
            {
                await bitmapMemory.WriteAsync(Array.Empty<byte>().AsSpan());
            });
        }
        
        [Fact]
        public async Task WriteAsync_CanceledCancellationToken_ThrowsTaskCanceledException()
        {
            // Arrange
            using var bitmap = SerializationTestBitmap.GetTestBitmap();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Frozen);
            var cts = new CancellationTokenSource();
            await cts.CancelAsync();
            
            using var bitmapMemory = new Roaring32BitmapMemory((nuint)serializedBitmap.Length);
            
            // Act && Assert
            await Assert.ThrowsAsync<TaskCanceledException>(async () =>
            {
                await bitmapMemory.WriteAsync(Array.Empty<byte>().AsSpan(), cts.Token);
            });
        }
        
        [Fact]
        public async Task WriteAsync_WritesReadonlySpan_ReturnsValidBitmap()
        {
            // Arrange
            using var bitmap = SerializationTestBitmap.GetTestBitmap();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Frozen);
            
            // Act
            using var bitmapMemory = new Roaring32BitmapMemory((nuint)serializedBitmap.Length);
            await bitmapMemory.WriteAsync(serializedBitmap.AsSpan());
            using var frozenBitmap = bitmapMemory.ToFrozen();
            
            // Assert
            Assert.Equal(bitmap.Values, frozenBitmap.Values);
            Assert.True(bitmap.IsValid());
        }
    }
    
    public class WriteByteBuffer
    {
        [Fact]
        public void Write_Disposed_ThrowsObjectDisposedException()
        {
            // Arrange
            var bitmapMemory = new Roaring32BitmapMemory(10);
            bitmapMemory.Dispose();
            
            // Act && Assert
            Assert.Throws<ObjectDisposedException>(() =>
            {
                bitmapMemory.Write([], 0, 0);
            });
        }
        
        [Fact]
        public void Write_WritesByteArray_ReturnsValidBitmap()
        {
            // Arrange
            using var bitmap = SerializationTestBitmap.GetTestBitmap();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Frozen);
            
            // Act
            using var bitmapMemory = new Roaring32BitmapMemory((nuint)serializedBitmap.Length);
            bitmapMemory.Write(serializedBitmap, 0, serializedBitmap.Length);
            using var frozenBitmap = bitmapMemory.ToFrozen();
            
            // Assert
            Assert.Equal(bitmap.Values, frozenBitmap.Values);
            Assert.True(bitmap.IsValid());
        }
    }
    
    public class WriteByteBufferAsync
    {
        [Fact]
        public async Task WriteAsync_Disposed_ThrowsObjectDisposedException()
        {
            // Arrange
            var bitmapMemory = new Roaring32BitmapMemory(10);
            bitmapMemory.Dispose();
            
            // Act && Assert
            await Assert.ThrowsAsync<ObjectDisposedException>(async () =>
            {
                await bitmapMemory.WriteAsync([], 0, 0);
            });
        }
        
        [Fact]
        public async Task WriteAsync_CanceledCancellationToken_ThrowsTaskCanceledException()
        {
            // Arrange
            using var bitmap = SerializationTestBitmap.GetTestBitmap();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Frozen);
            var cts = new CancellationTokenSource();
            await cts.CancelAsync();
            
            using var bitmapMemory = new Roaring32BitmapMemory((nuint)serializedBitmap.Length);
            
            // Act && Assert
            await Assert.ThrowsAsync<TaskCanceledException>(async () =>
            {
                await bitmapMemory.WriteAsync(serializedBitmap, 0, serializedBitmap.Length, cts.Token);
            });
        }
        
        [Fact]
        public async Task WriteAsync_WritesByteArray_ReturnsValidBitmap()
        {
            // Arrange
            using var bitmap = SerializationTestBitmap.GetTestBitmap();
            var serializedBitmap = bitmap.Serialize(SerializationFormat.Frozen);
            
            // Act
            using var bitmapMemory = new Roaring32BitmapMemory((nuint)serializedBitmap.Length);
            await bitmapMemory.WriteAsync(serializedBitmap, 0, serializedBitmap.Length);
            using var frozenBitmap = bitmapMemory.ToFrozen();
            
            // Assert
            Assert.Equal(bitmap.Values, frozenBitmap.Values);
            Assert.True(bitmap.IsValid());
        }
    }
}