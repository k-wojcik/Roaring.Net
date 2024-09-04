using System;
using System.Runtime.InteropServices;

namespace Roaring.Net.CRoaring;

/// <summary>
/// Stores statistics about the bitmap.
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct Statistics
{
    /// <summary>
    /// The total number of containers.
    /// </summary>
    public readonly uint ContainerCount;

    /// <summary>
    /// The number of array containers.
    /// </summary>
    public readonly uint ArrayContainerCount;

    /// <summary>
    /// The number of run containers.
    /// </summary>
    public readonly uint RunContainerCount;

    /// <summary>
    /// The number of bitset containers.
    /// </summary>
    public readonly uint BitsetContainerCount;

    /// <summary>
    /// The number of values in the array containers.
    /// </summary>
    public readonly uint ArrayContainerValuesCount;

    /// <summary>
    /// The number of values in the run containers.
    /// </summary>
    public readonly uint RunContainerValuesCount;

    /// <summary>
    /// The number of values in the bitset containers.
    /// </summary>
    public readonly uint BitsetContainerValuesCount;

    /// <summary>
    /// The number of bytes used by the array containers.
    /// </summary>
    public readonly uint ArrayContainerBytes;

    /// <summary>
    /// The number of bytes used by the run containers.
    /// </summary>
    public readonly uint RunContainerBytes;

    /// <summary>
    /// The number of bytes used by the bitset containers.
    /// </summary>
    public readonly uint BitsetContainerBytes;

    /// <summary>
    /// The minimum value in the bitmap.
    /// </summary>
    public readonly uint MaxValue;

    /// <summary>
    /// The maximum value in the bitmap.
    /// </summary>
    public readonly uint MinValue;

    [Obsolete]
    private readonly ulong ValueSum;

    /// <summary>
    /// The total number of values in the bitmap.
    /// </summary>
    public readonly ulong Count;
}