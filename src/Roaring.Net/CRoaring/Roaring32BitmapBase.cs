using System;

namespace Roaring.Net.CRoaring;

/// <summary>
/// Represents the base type of 32-bit CRoaring bitmap.
/// </summary>
public abstract class Roaring32BitmapBase : IDisposable
{
    /// <summary>
    /// A pointer to an CRoaring bitmap instance.
    /// </summary>
    protected internal IntPtr Pointer;

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    /// <param name="disposing">A flag indicating whether to dispose managed state. Set to <c>true</c> to dispose managed state, otherwise <c>false</c>.</param>
    protected abstract void Dispose(bool disposing);

    ~Roaring32BitmapBase() => Dispose(false);

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}