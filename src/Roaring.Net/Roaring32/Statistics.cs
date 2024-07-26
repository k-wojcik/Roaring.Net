using System;
using System.Runtime.InteropServices;

namespace Roaring;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct Statistics
{
    public uint ContainerCount;
    public uint ArrayContainerCount;
    public uint RunContainerCount;
    public uint BitsetContainerCount;

    public uint ArrayContainerValuesCount;
    public uint RunContainerValuesCount;
    public uint BitsetContainerValuesCount;

    public uint ArrayContainerBytes;
    public uint RunContainerBytes;
    public uint BitsetContainerBytes;

    public uint MaxValue;
    public uint MinValue;
    
    [Obsolete]
    private ulong ValueSum;
    
    public ulong Count;
}