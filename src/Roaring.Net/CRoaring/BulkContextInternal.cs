using System;
using System.Runtime.InteropServices;

namespace Roaring.Net.CRoaring;

[StructLayout(LayoutKind.Sequential)]
internal struct BulkContextInternal
{
    public readonly IntPtr container;

    public readonly int idx;

    public readonly ushort key;

    public readonly byte typecode;
}