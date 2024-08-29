using System;
using System.Runtime.InteropServices;

namespace Roaring.Net.CRoaring;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct Statistics
{
    public readonly uint ContainerCount;
    public readonly uint ArrayContainerCount;
    public readonly uint RunContainerCount;
    public readonly uint BitsetContainerCount;

    public readonly uint ArrayContainerValuesCount;
    public readonly uint RunContainerValuesCount;
    public readonly uint BitsetContainerValuesCount;

    public readonly uint ArrayContainerBytes;
    public readonly uint RunContainerBytes;
    public readonly uint BitsetContainerBytes;

    public readonly uint MaxValue;
    public readonly uint MinValue;

    [Obsolete]
    private readonly ulong ValueSum;

    public readonly ulong Count;
}