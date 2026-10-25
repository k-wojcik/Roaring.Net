using System;
using Roaring.Net.CRoaring;
using Xunit;

#pragma warning disable CS0618

namespace Roaring.Net.Tests.CRoaring.Roaring32BitmapTests;

public class UseHandleTests
{
    private sealed class TestBitmap : Roaring32Bitmap
    {
        public TestBitmap(uint[] values) : base(values)
        {
        }

        public void UseHandleForTest(Action<IntPtr> action) => UseHandle(action);

        public TResult UseHandleForTest<TResult>(Func<IntPtr, TResult> action) => UseHandle(action);
    }

    [Fact]
    public void UseHandle_Callback_ReceivesCurrentPointer()
    {
        // Arrange
        using var bitmap = new TestBitmap([1, 2, 3]);
        var expected = bitmap.Handle.DangerousGetHandle();
        var actual = IntPtr.Zero;

        // Act
        bitmap.UseHandleForTest(pointer =>
        {
            actual = pointer;
        });

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void UseHandle_Callback_ReturnsCallbackResult()
    {
        // Arrange
        using var bitmap = new TestBitmap([1, 2, 3]);

        // Act
        var actual = bitmap.UseHandleForTest(pointer => pointer == IntPtr.Zero ? 0 : 42);

        // Assert
        Assert.Equal(42, actual);
    }

    [Fact]
    public void UseHandle_NullAction_ThrowsArgumentNullException()
    {
        // Arrange
        using var bitmap = new TestBitmap([1, 2, 3]);

        // Act && Assert
        Assert.Throws<ArgumentNullException>(() => bitmap.UseHandleForTest((Action<IntPtr>)null!));
        Assert.Throws<ArgumentNullException>(() => bitmap.UseHandleForTest((Func<IntPtr, int>)null!));
    }

    [Fact]
    public void UseHandle_DisposedBitmap_ThrowsObjectDisposedExceptionWithoutInvokingCallback()
    {
        // Arrange
        var bitmap = new TestBitmap([1, 2, 3]);
        bitmap.Dispose();
        var invoked = false;

        // Act && Assert
        Assert.Throws<ObjectDisposedException>(() =>
        {
            bitmap.UseHandleForTest(_ =>
            {
                invoked = true;
            });
        });

        Assert.False(invoked);
    }

    [Fact]
    public void UseHandle_CallbackThrows_PropagatesExceptionAndKeepsBitmapUsable()
    {
        // Arrange
        using var bitmap = new TestBitmap([1, 2, 3]);

        // Act && Assert
        Assert.Throws<InvalidOperationException>(() =>
        {
            bitmap.UseHandleForTest(_ =>
            {
                throw new InvalidOperationException();
            });
        });

        Assert.Equal([1U, 2U, 3U], bitmap.ToArray());
    }

    [Fact]
    public void UseHandle_CallbackDisposesBitmap_CompletesAndReleasesReference()
    {
        // Arrange
        var bitmap = new TestBitmap([1, 2, 3]);
        var invoked = false;

        // Act
        bitmap.UseHandleForTest(_ =>
        {
            invoked = true;
            bitmap.Dispose();
        });

        // Assert
        Assert.True(invoked);
        Assert.Equal(IntPtr.Zero, bitmap.Pointer);
        Assert.Throws<ObjectDisposedException>(() =>
        {
            _ = bitmap.Handle;
        });
    }
}
