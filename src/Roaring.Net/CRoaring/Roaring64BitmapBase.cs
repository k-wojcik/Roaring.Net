using System;

namespace Roaring.Net.CRoaring;

/// <summary>
/// Base class for 64-bit roaring bitmaps.
/// </summary>
public abstract class Roaring64BitmapBase : IDisposable
{
    /// <summary>
    /// A pointer to a CRoaring bitmap instance.
    /// </summary>
    [Obsolete("Use the public bitmap API or UseHandle for native interop instead. This field is maintained for binary compatibility only and will be removed in future versions.")]
    protected internal IntPtr Pointer;

    private SafeRoaring64BitmapHandle? _handle;

    internal SafeRoaring64BitmapHandle Handle => _handle ?? throw new ObjectDisposedException(GetType().FullName);

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

        SafeRoaring64BitmapHandle handle = Handle;
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
    /// Initializes a new instance of the <see cref="Roaring64BitmapBase"/> class.
    /// </summary>
    protected Roaring64BitmapBase()
    {
    }

    internal Roaring64BitmapBase(SafeRoaring64BitmapHandle handle)
    {
        _handle = handle;
        UpdateLegacyPointer();
    }

    internal static SafeRoaring64BitmapHandle CreateHandle(IntPtr pointer)
    {
        if (pointer == IntPtr.Zero)
        {
            throw new InvalidOperationException(ExceptionMessages.UnableToAllocateBitmap);
        }

        try
        {
            return new SafeRoaring64BitmapHandle(pointer);
        }
        catch
        {
            NativeMethods.roaring64_bitmap_free(pointer);
            throw;
        }
    }

    internal void ReplaceHandle(SafeRoaring64BitmapHandle handle)
    {
        if (ReferenceEquals(_handle, handle))
        {
            UpdateLegacyPointer();
            return;
        }

        SafeRoaring64BitmapHandle? previous = _handle;
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
