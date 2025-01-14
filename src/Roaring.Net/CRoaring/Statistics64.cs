using System.Runtime.InteropServices;

namespace Roaring.Net.CRoaring;

/// <summary>
/// Stores statistics about the 64-bit bitmap.
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct Statistics64
{
    /// <summary>
    /// The total number of containers.
    /// </summary>
    public readonly ulong ContainerCount;

    /// <summary>
    /// The number of array containers.
    /// </summary>
    public readonly ulong ArrayContainerCount;

    /// <summary>
    /// The number of run containers.
    /// </summary>
    public readonly ulong RunContainerCount;

    /// <summary>
    /// The number of bitset containers.
    /// </summary>
    public readonly ulong BitsetContainerCount;

    /// <summary>
    /// The number of values in the array containers.
    /// </summary>
    public readonly ulong ArrayContainerValuesCount;

    /// <summary>
    /// The number of values in the run containers.
    /// </summary>
    public readonly ulong RunContainerValuesCount;

    /// <summary>
    /// The number of values in the bitset containers.
    /// </summary>
    public readonly ulong BitsetContainerValuesCount;

    /// <summary>
    /// The number of bytes used by the array containers.
    /// </summary>
    public readonly ulong ArrayContainerBytes;

    /// <summary>
    /// The number of bytes used by the run containers.
    /// </summary>
    public readonly ulong RunContainerBytes;

    /// <summary>
    /// The number of bytes used by the bitset containers.
    /// </summary>
    public readonly ulong BitsetContainerBytes;

    /// <summary>
    /// The minimum value in the bitmap.
    /// </summary>
    public readonly ulong MaxValue;

    /// <summary>
    /// The maximum value in the bitmap.
    /// </summary>
    public readonly ulong MinValue;

    /// <summary>
    /// The total number of values in the bitmap.
    /// </summary>
    public readonly ulong Count;
}