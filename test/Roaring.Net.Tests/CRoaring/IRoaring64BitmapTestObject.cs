
using Roaring.Net.CRoaring;
using System;

namespace Roaring.Net.Tests.CRoaring;

public interface IRoaring64BitmapTestObject : IDisposable
{
    internal IReadOnlyRoaring64Bitmap ReadOnlyBitmap { get; }
    Roaring64BitmapBase Bitmap { get; }
    ulong[] Values { get; }
}