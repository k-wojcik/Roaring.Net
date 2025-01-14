using System;
using System.Runtime.InteropServices;

namespace Roaring.Net.CRoaring;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal unsafe struct BulkContext64Internal
{
    public fixed byte high_bytes[6];

    public readonly IntPtr leaf;
}