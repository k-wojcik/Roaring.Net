
using Roaring.Net.CRoaring;
using System;

namespace Roaring.Net.Tests.CRoaring;

public interface IRoaring32BitmapTestObject : IDisposable
{
    internal IReadOnlyRoaring32Bitmap ReadOnlyBitmap { get; }
    public Roaring32BitmapBase Bitmap { get; }
    public uint[] Values { get; }
}