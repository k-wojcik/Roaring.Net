using System;
using System.Collections.Generic;

namespace Roaring.Net.CRoaring;

/// <summary>
/// Represents a 32-bit CRoaring frozen bitmap. <br/>
/// <a href="https://github.com/RoaringBitmap/CRoaring/blob/60d0e97fa021b04f8a6ad50e3877ca16d988c80e/include/roaring/roaring.h#L694-L711"> "Frozen" serialization format.</a>
/// </summary>
public unsafe class FrozenRoaring32Bitmap : Roaring32BitmapBase, IReadOnlyRoaring32Bitmap
{
    internal Roaring32BitmapMemory Memory { get; }

    private readonly Roaring32Bitmap _bitmap;
    private bool _isDisposed;

    /// <summary>
    /// Gets the number of elements (cardinality) contained in the <see cref="FrozenRoaring32Bitmap"/>.
    /// </summary>
    /// <returns>The number of elements contained in the <see cref="FrozenRoaring32Bitmap"/>.</returns>
    public ulong Count => _bitmap.Count;

    /// <summary>
    /// Gets a value indicating that <see cref="Roaring32Bitmap"/> is empty (cardinality is zero).
    /// </summary>
    /// <returns><c>true</c> if <see cref="Roaring32Bitmap"/> is empty (cardinality is zero); otherwise, <c>false</c>.</returns>
    public bool IsEmpty => _bitmap.IsEmpty;

    /// <summary>
    /// Gets the minimum value in the <see cref="Roaring32Bitmap"/>.
    /// </summary>
    /// <returns>The minimum value in the <see cref="Roaring32Bitmap"/> or <c>null</c> when the bitmap is empty.</returns>    
    public uint? Min => _bitmap.Min;

    /// <summary>
    /// Gets the maximum value in the <see cref="Roaring32Bitmap"/>.
    /// </summary>
    /// <returns>The maximum value in the <see cref="Roaring32Bitmap"/> or <see langword="null"/> when the bitmap is empty.</returns>
    public uint? Max => _bitmap.Max;

    internal FrozenRoaring32Bitmap(Roaring32Bitmap bitmap)
    {
        nuint size = bitmap.GetSerializationBytes(SerializationFormat.Frozen);
        Memory = new Roaring32BitmapMemory(size, shared: false);
        _bitmap = bitmap.GetFrozenView(size, Memory.MemoryPtr);

        Pointer = _bitmap.Pointer;
    }

    internal FrozenRoaring32Bitmap(IntPtr pointer, Roaring32BitmapMemory memory)
    {
        Pointer = pointer;
        Memory = memory;

        _bitmap = new Roaring32Bitmap(pointer);
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        _bitmap.Dispose();
        Memory.Release(this);

        _isDisposed = true;
    }

    /// <inheritdoc />
    ~FrozenRoaring32Bitmap() => Dispose(false);

    /// <summary>
    /// Checks if a value is present in the bitmap.
    /// </summary>
    /// <param name="value">A value for which the check will be performed.</param>
    /// <returns><c>true</c> if a value exists in the bitmap; otherwise, <c>false</c>.</returns>
    public bool Contains(uint value) => _bitmap.Contains(value);

    /// <summary>
    /// Checks if the values for the given range are present in the bitmap.
    /// </summary>
    /// <param name="start">Start of range (inclusive).</param>
    /// <param name="end">End of range (inclusive).</param>
    /// <returns><c>true</c> if all values from the given range exist in the bitmap; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when arguments have invalid values.</exception>
    public bool ContainsRange(uint start, uint end) => _bitmap.ContainsRange(start, end);

    /// <summary>
    /// Checks if a value is present in the bitmap using context from a previous bulk operation to optimize the checking process.
    /// </summary>
    /// <param name="context">A context that stores information between `*Bulk` method calls.</param>
    /// <param name="value">A value for which the check will be performed.</param>
    /// <exception cref="ArgumentException">Thrown when context belongs to another bitmap.</exception>
    /// <remarks>
    /// To take advantage of this optimization, the caller should call this method sequentially with values
    /// with the same "key" (high 16 bits of the value).
    /// </remarks>
    public bool ContainsBulk(BulkContext context, uint value)
    {
        if (context.Bitmap != this)
        {
            throw new ArgumentException(ExceptionMessages.BulkContextBelongsToOtherBitmap, nameof(context));
        }

        return NativeMethods.roaring_bitmap_contains_bulk(Pointer, context.Pointer, value);
    }

    /// <summary>
    /// Compares the equality of bitmaps based on the values they contain.
    /// </summary>
    /// <param name="bitmap">Bitmap with which equality will be compared.</param>
    /// <returns><c>true</c> if both bitmaps have the same values; otherwise, <c>false</c>.</returns>
    public bool ValueEquals(Roaring32BitmapBase? bitmap) => _bitmap.ValueEquals(bitmap);

    /// <summary>
    /// Checks if the current bitmap is a subset of the <paramref name="bitmap"/>. 
    /// </summary>
    /// <param name="bitmap"></param>
    /// <returns><c>true</c> if current bitmaps is a subset of the <paramref name="bitmap"/>; otherwise, <c>false</c>.</returns>
    public bool IsSubsetOf(Roaring32BitmapBase? bitmap) => _bitmap.IsSubsetOf(bitmap);

    /// <summary>
    /// Checks if the current bitmap is a proper subset of the <paramref name="bitmap"/>. 
    /// </summary>
    /// <param name="bitmap"></param>
    /// <returns><c>true</c> if current bitmaps is a proper subset of the <paramref name="bitmap"/>; otherwise, <c>false</c>.</returns>
    public bool IsProperSubsetOf(Roaring32BitmapBase? bitmap) => _bitmap.IsProperSubsetOf(bitmap);

    /// <summary>
    /// Checks if the current bitmap is a superset of the <paramref name="bitmap"/>. 
    /// </summary>
    /// <param name="bitmap"></param>
    /// <returns><c>true</c> if current bitmaps is a superset of the <paramref name="bitmap"/>; otherwise, <c>false</c>.</returns>
    public bool IsSupersetOf(Roaring32BitmapBase? bitmap) => _bitmap.IsSupersetOf(bitmap);

    /// <summary>
    /// Checks if the current bitmap is a proper superset of the <paramref name="bitmap"/>. 
    /// </summary>
    /// <param name="bitmap"></param>
    /// <returns><c>true</c> if current bitmaps is a proper superset of the <paramref name="bitmap"/>; otherwise, <c>false</c>.</returns>
    public bool IsProperSupersetOf(Roaring32BitmapBase? bitmap) => _bitmap.IsProperSupersetOf(bitmap);

    /// <summary>
    /// Tries to get a value from the bitmap located at the given <paramref name="index"/> (rank).
    /// </summary>
    /// <param name="index">The index (rank) for which the value will be retrieved. Index values start from 0.</param>
    /// <param name="value">Retrieved value. <c>0</c> if value does not exist in the bitmap.</param>
    /// <returns><c>true</c> if a value exists in the bitmap; otherwise, <c>false</c>.</returns>
    public bool TryGetValue(uint index, out uint value) => _bitmap.TryGetValue(index, out value);

    /// <summary>
    /// Gets the index (rank) for the given value.
    /// </summary>
    /// <param name="value">The value for which the index will be retrieved.</param>
    /// <returns><c>-1</c> if a <paramref name="value"/> does not exist in the bitmap; otherwise, index (rank) of the <paramref name="value"/>.</returns>
    public long GetIndex(uint value) => _bitmap.GetIndex(value);

    /// <summary>
    /// Counts number of values less than or equal to <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value for which data will be counted.</param>
    /// <returns>The number of values that are less than or equal to the <paramref name="value"/>.</returns>
    public ulong CountLessOrEqualTo(uint value) => _bitmap.CountLessOrEqualTo(value);

    /// <summary>
    /// Counts number of values less than or equal to for each element of <paramref name="values"/>.
    /// </summary>
    /// <param name="values">An ascending sorted set of tested values.</param>
    /// <returns>The number values that are less than or equal to the value from <paramref name="values"/> placed under the same index.</returns>
    public ulong[] CountManyLessOrEqualTo(uint[] values) => _bitmap.CountManyLessOrEqualTo(values);

    /// <summary>
    /// Counts number of values in the given range of values.
    /// </summary>
    /// <param name="start">Start of range (inclusive).</param>
    /// <param name="end">End of range (inclusive).</param>
    /// <returns>The number of values in the given range of values.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when arguments have invalid values.</exception>
    public ulong CountRange(uint start, uint end) => _bitmap.CountRange(start, end);

    /// <summary>
    /// Creates a new negated bitmap based on the values in the current bitmap. 
    /// </summary>
    /// <returns>Instance of the <see cref="Roaring32Bitmap"/> class with negated values of current bitmap.</returns>
    public Roaring32Bitmap Not() => _bitmap.Not();

    /// <summary>
    /// Creates a new negated bitmap based on the values in the current bitmap for the given range of values. 
    /// </summary>
    /// <param name="start">Start of range (inclusive).</param>
    /// <param name="end">End of range (inclusive).</param>
    /// <returns>Instance of the <see cref="Roaring32Bitmap"/> class with negated values of current bitmap in the given range of values.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when arguments have invalid values.</exception>
    /// <remarks>Values outside the range are left unchanged.</remarks>
    public Roaring32Bitmap NotRange(uint start, uint end) => _bitmap.NotRange(start, end);

    /// <summary>
    /// Creates a intersection between the current bitmap and the <paramref name="bitmap"/> given in the parameter.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the intersection will be performed.</param>
    /// <returns><see cref="Roaring32Bitmap"/> with the result of intersection of two bitmaps.</returns>
    /// <remarks>
    /// Performance hints:
    /// <list type="bullet">
    /// <item>if you are computing the intersection between several bitmaps, two-by-two, it is best to start with the smallest bitmap,</item>
    /// </list>
    /// </remarks>
    public Roaring32Bitmap And(Roaring32BitmapBase bitmap) => _bitmap.And(bitmap);

    /// <summary>
    /// Intersects the current bitmap with the <paramref name="bitmap"/> given in the parameter and returns the number of values contained in the resulting bitmap.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the intersection will be performed.</param>
    public ulong AndCount(Roaring32BitmapBase bitmap) => _bitmap.AndCount(bitmap);

    /// <summary>
    /// Creates a difference between the current bitmap and the <paramref name="bitmap"/> given in the parameter.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the difference will be performed.</param>
    /// <returns><see cref="Roaring32Bitmap"/> with the result of difference of two bitmaps.</returns>
    public Roaring32Bitmap AndNot(Roaring32BitmapBase bitmap) => _bitmap.AndNot(bitmap);

    /// <summary>
    /// Creates a difference between the current bitmap and the <paramref name="bitmap"/> given in the parameter and returns the number of values contained in the resulting bitmap.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the difference will be performed.</param>
    /// <returns>Number of values contained in the resulting bitmap after difference.</returns>
    public ulong AndNotCount(Roaring32BitmapBase bitmap) => _bitmap.AndNotCount(bitmap);

    /// <summary>
    /// Creates a union between the current bitmap and the <paramref name="bitmap"/> given in the parameter.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the union will be performed.</param>
    /// <returns><see cref="Roaring32Bitmap"/> with the result of union two bitmaps.</returns>
    public Roaring32Bitmap Or(Roaring32BitmapBase bitmap) => _bitmap.Or(bitmap);

    /// <summary>
    /// Creates a union between the current bitmap and the <paramref name="bitmap"/> given in the parameter and returns the number of values contained in the resulting bitmap.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the union will be performed.</param>
    /// <returns>Number of values contained in the resulting bitmap after union.</returns>
    public ulong OrCount(Roaring32BitmapBase bitmap) => _bitmap.OrCount(bitmap);

    /// <summary>
    /// Creates a union between the current bitmap and the <paramref name="bitmaps"/> given in the parameter.
    /// </summary>
    /// <param name="bitmaps">Bitmaps with which the union will be performed.</param>
    /// <returns><see cref="Roaring32Bitmap"/> with the result of union of many bitmaps.</returns>
    /// <remarks>This method may be slower than <see cref="OrManyHeap"/> in some cases.</remarks>
    public Roaring32Bitmap OrMany(Roaring32BitmapBase[] bitmaps) => _bitmap.OrMany(bitmaps);

    /// <summary>
    /// Creates a union between the current bitmap and the <paramref name="bitmaps"/> given in the parameter using a heap.
    /// </summary>
    /// <param name="bitmaps">Bitmaps with which the union will be performed.</param>
    /// <returns><see cref="Roaring32Bitmap"/> with the result of union of many bitmaps.</returns>
    /// <remarks>This method may be faster than <see cref="OrMany"/> in some cases.</remarks>
    public Roaring32Bitmap OrManyHeap(Roaring32BitmapBase[] bitmaps) => _bitmap.OrManyHeap(bitmaps);

    /// <summary>
    /// Creates a union between the current bitmap and the <paramref name="bitmap"/> given in the parameter using lazy algorithm.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the union will be performed.</param>
    /// <param name="bitsetConversion">Flag which determines whether container-container operations force a bitset conversion.</param>
    /// <returns><see cref="Roaring32Bitmap"/> with the result of union two bitmaps.</returns>
    /// <remarks>
    /// You must call <see cref="Roaring32Bitmap.RepairAfterLazy"/> on the resulting bitmap after executing "lazy" computations. <br/>
    /// Lazy operations can be called multiple times in sequence.
    /// </remarks>
    public Roaring32Bitmap LazyOr(Roaring32BitmapBase bitmap, bool bitsetConversion) => _bitmap.LazyOr(bitmap, bitsetConversion);

    /// <summary>
    /// Creates a symmetric difference between the current bitmap and the <paramref name="bitmap"/> given in the parameter.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the symmetric difference will be performed.</param>
    /// <returns><see cref="Roaring32Bitmap"/> with the result of the symmetric difference of two bitmaps.</returns>
    public Roaring32Bitmap Xor(Roaring32BitmapBase bitmap) => _bitmap.Xor(bitmap);

    /// <summary>
    /// Creates a symmetric difference between the current bitmap and the <paramref name="bitmap"/> given in the parameter and returns the number of values contained in the resulting bitmap. 
    /// </summary>
    /// <param name="bitmap">Bitmap with which the symmetric difference will be performed.</param>
    /// <returns>Number of values contained in the resulting bitmap after symmetric difference.</returns>
    public ulong XorCount(Roaring32BitmapBase bitmap) => _bitmap.XorCount(bitmap);

    /// <summary>
    /// Creates a symmetric difference between the current bitmap and the <paramref name="bitmaps"/> given in the parameter.
    /// </summary>
    /// <param name="bitmaps">Bitmaps with which the symmetric difference will be performed.</param>
    /// <returns><see cref="Roaring32Bitmap"/> with the result of the symmetric difference of many bitmaps.</returns>
    public Roaring32Bitmap XorMany(params Roaring32BitmapBase[] bitmaps) => _bitmap.XorMany(bitmaps);

    /// <summary>
    /// Creates a symmetric difference between the current bitmap and the <paramref name="bitmap"/> given in the parameter using lazy algorithm.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the symmetric difference will be performed.</param>
    /// <returns><see cref="Roaring32Bitmap"/> with the result of the symmetric difference of two bitmaps.</returns>
    /// <remarks>
    /// You must call <see cref="Roaring32Bitmap.RepairAfterLazy"/> on the resulting bitmap after executing "lazy" computations. <br/>
    /// Lazy operations can be called multiple times in sequence.
    /// </remarks>
    public Roaring32Bitmap LazyXor(Roaring32BitmapBase bitmap) => _bitmap.LazyXor(bitmap);

    /// <summary>
    /// Checks whether the current bitmaps overlaps (at least one element exists in both bitmaps) the <paramref name="bitmap"/> given in the parameter.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the calculation will be performed.</param>
    /// <returns><c>true</c> if current bitmap overlaps the given bitmap and the bitmaps are not empty; otherwise, <c>false</c>.</returns>
    public bool Overlaps(Roaring32BitmapBase bitmap) => _bitmap.Overlaps(bitmap);

    /// <summary>
    /// Checks whether the current bitmaps overlaps (at least one element exists in range) the range given in the parameters.
    /// </summary>
    /// <param name="start">Start of range (inclusive).</param>
    /// <param name="end">End of range (inclusive).</param>
    /// <returns><c>true</c> if current bitmap overlaps the given range; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when arguments have invalid values.</exception>
    public bool OverlapsRange(uint start, uint end) => _bitmap.OverlapsRange(start, end);

    /// <summary>
    /// Computes the Jaccard index (Tanimoto distance, Jaccard similarity coefficient)
    /// between current bitmap and the <paramref name="bitmap"/> given in the parameter. 
    /// </summary>
    /// <param name="bitmap">Bitmap with which the Jaccard index computation will be performed.</param>
    /// <returns>Value of the Jaccard index.</returns>
    /// <remarks>The Jaccard index is undefined if both bitmaps are empty.</remarks>
    public double GetJaccardIndex(Roaring32BitmapBase bitmap) => _bitmap.GetJaccardIndex(bitmap);

    /// <summary>
    /// Writes current bitmap to the <paramref name="buffer"/> given in the parameter.
    /// </summary>
    /// <param name="buffer">The array in which the bitmap will be written.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="buffer"/> size is too small to write the bitmap.</exception>
    public void CopyTo(uint[] buffer) => _bitmap.CopyTo(buffer);

    /// <summary>
    /// Gets enumerator that returns the values contained in the bitmap.
    /// </summary>
    /// <remarks>The values are ordered from smallest to largest.</remarks>
    public IEnumerable<uint> Values => _bitmap.Values;

    /// <summary>
    /// Writes current bitmap to the array.
    /// </summary>
    /// <returns>The array containing the values of the bitmap.</returns>
    public uint[] ToArray() => _bitmap.ToArray();

    /// <summary>
    /// Converts <see cref="FrozenRoaring32Bitmap"/> to the <see cref="Roaring32Bitmap"/>.
    /// </summary>
    /// <returns>Instance of the <see cref="Roaring32Bitmap"/> class with the same values as the current bitmap.</returns>
    /// <exception cref="InvalidOperationException">Thrown when unable to allocate bitmap.</exception>
    public Roaring32Bitmap ToBitmap() => _bitmap.Clone();

    /// <summary>
    /// Converts <see cref="FrozenRoaring32Bitmap"/> to the <see cref="Roaring32Bitmap"/> from the given offset.
    /// </summary>
    /// <returns>Instance of the <see cref="Roaring32Bitmap"/> class with the same values as the current bitmap from the given offset.</returns>
    /// <exception cref="InvalidOperationException">Thrown when unable to allocate bitmap.</exception>
    public Roaring32Bitmap ToBitmapWithOffset(long offset) => _bitmap.CloneWithOffset(offset);

    /// <summary>
    /// Takes the given number of values from the current bitmap and puts them into an array.
    /// </summary>
    /// <param name="count">Number of values to take from the bitmap.</param>
    /// <returns>An array containing the given number of values from the bitmap.</returns>
    /// <remarks>If the bitmap contains fewer values than the given number then the array will be adjusted to the number of values.</remarks>
    public uint[] Take(ulong count) => _bitmap.Take(count);

    /// <summary>
    /// Gets statistics about the bitmap.
    /// </summary>
    /// <returns>Structure containing statistics about the bitmap.</returns>
    public Statistics GetStatistics() => _bitmap.GetStatistics();

    /// <summary>
    /// Performs internal consistency checks.
    /// </summary>
    /// <returns><c>true</c> if the bitmap is consistent; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// It may be useful to call this after deserializing bitmaps from
    /// untrusted sources. If <see cref="IsValid()"/> returns <c>true</c>, then the
    /// bitmap should be consistent and can be trusted not to cause crashes or memory
    /// corruption.
    /// </remarks>
    public bool IsValid() => _bitmap.IsValid();

    /// <summary>
    /// Performs internal consistency checks and returns the cause of inconsistencies.
    /// </summary>
    /// <param name="reason">Reason of inconsistency.</param>
    /// <returns><c>true</c> if the bitmap is consistent; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// It may be useful to call this after deserializing bitmaps from
    /// untrusted sources. If <see cref="IsValid()"/> returns <c>true</c>, then the
    /// bitmap should be consistent and can be trusted not to cause crashes or memory
    /// corruption.
    /// </remarks>
    public bool IsValid(out string? reason) => _bitmap.IsValid(out reason);

    /// <summary>
    /// Gets the number of bytes required for a given serialization format.
    /// </summary>
    /// <param name="format">Serialization type for which we get the number of bytes.</param>
    /// <returns>Number of bytes required for the given serialization format.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when serialization format is not supported.</exception>
    public nuint GetSerializationBytes(SerializationFormat format = SerializationFormat.Normal) => _bitmap.GetSerializationBytes(format);

    /// <summary>
    /// Serializes the current bitmap to the given serialization format.
    /// </summary>
    /// <param name="format">The serialization format to which we serialize the bitmap.</param>
    /// <returns>An array that contains a bitmap in a serialized form.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when serialization format is not supported.</exception>
    public byte[] Serialize(SerializationFormat format = SerializationFormat.Normal) => _bitmap.Serialize(format);
}