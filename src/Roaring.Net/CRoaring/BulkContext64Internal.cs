using System;
using System.Runtime.InteropServices;

namespace Roaring.Net.CRoaring;

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct BulkContext64Internal
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public byte[] high_bytes;

    public IntPtr leaf;
}