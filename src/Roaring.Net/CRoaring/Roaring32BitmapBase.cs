using System;

namespace Roaring.Net.CRoaring;

/// <summary>
/// Base class for 32-bit roaring bitmaps.
/// </summary>
public abstract class Roaring32BitmapBase : IDisposable
{
    /// <summary>
    /// A pointer to a CRoaring bitmap instance.
    /// </summary>
    [Obsolete("Use the public bitmap API or UseHandle for native interop instead. This field is maintained for binary compatibility only and will be removed in future versions.")]
    protected internal IntPtr Pointer;

    private SafeRoaring32BitmapHandle? _handle;

    internal SafeRoaring32BitmapHandle Handle => _handle ?? throw new ObjectDisposedException(GetType().FullName);

    /// <summary>
    /// Invokes the specified delegate with the native bitmap pointer while keeping the underlying native bitmap alive.
    /// </summary>
    /// <param name="action">The delegate to invoke.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="action"/> is <c>null</c>.</exception>
    /// <exception cref="ObjectDisposedException">Thrown when the bitmap has been disposed.</exception>
    /// <remarks>
    /// The pointer is valid only for the duration of the delegate invocation. Do not store it for later use.
    /// Disposing the bitmap while the delegate is running defers releasing the native bitmap until the delegate returns.
    /// This method does not make the bitmap thread-safe.
    /// </remarks>
    protected void UseHandle(Action<IntPtr> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        UseHandle(pointer =>
        {
            action(pointer);
            return true;
        });
    }

    /// <summary>
    /// Invokes the specified delegate with the native bitmap pointer while keeping the underlying native bitmap alive.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the delegate.</typeparam>
    /// <param name="action">The delegate to invoke.</param>
    /// <returns>The result returned by the delegate.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="action"/> is <c>null</c>.</exception>
    /// <exception cref="ObjectDisposedException">Thrown when the bitmap has been disposed.</exception>
    /// <remarks>
    /// The pointer is valid only for the duration of the delegate invocation. Do not store it for later use.
    /// Disposing the bitmap while the delegate is running defers releasing the native bitmap until the delegate returns.
    /// This method does not make the bitmap thread-safe.
    /// </remarks>
    protected TResult UseHandle<TResult>(Func<IntPtr, TResult> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        SafeRoaring32BitmapHandle handle = Handle;
        var addRef = false;
        try
        {
            handle.DangerousAddRef(ref addRef);
            return action(handle.DangerousGetHandle());
        }
        finally
        {
            if (addRef)
            {
                handle.DangerousRelease();
            }
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Roaring32BitmapBase"/> class.
    /// </summary>
    protected Roaring32BitmapBase()
    {
    }

    internal Roaring32BitmapBase(SafeRoaring32BitmapHandle handle)
    {
        _handle = handle;
        UpdateLegacyPointer();
    }

    internal static SafeRoaring32BitmapHandle CreateHandle(IntPtr pointer)
    {
        if (pointer == IntPtr.Zero)
        {
            throw new InvalidOperationException(ExceptionMessages.UnableToAllocateBitmap);
        }

        try
        {
            return new SafeRoaring32BitmapHandle(pointer);
        }
        catch
        {
            NativeMethods.roaring_bitmap_free(pointer);
            throw;
        }
    }

    internal void ReplaceHandle(SafeRoaring32BitmapHandle handle)
    {
        if (ReferenceEquals(_handle, handle))
        {
            UpdateLegacyPointer();
            return;
        }

        SafeRoaring32BitmapHandle? previous = _handle;
        _handle = handle;
        previous?.Dispose();
        UpdateLegacyPointer();
    }

    private void UpdateLegacyPointer()
    {
#pragma warning disable CS0618
        Pointer = _handle is not null && !_handle.IsClosed ? _handle.DangerousGetHandle() : IntPtr.Zero;
#pragma warning restore CS0618
    }

    /// <summary>
    /// Releases the resources used by the bitmap.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release managed resources; otherwise, <c>false</c>.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        _handle?.Dispose();
        _handle = null;
        UpdateLegacyPointer();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
