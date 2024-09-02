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
        Memory = new Roaring32BitmapMemory(size);
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
    /// Counts the number of values in the given range of values.
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

    public Roaring32Bitmap And(Roaring32BitmapBase bitmap) => _bitmap.And(bitmap);

    public ulong AndCount(Roaring32BitmapBase bitmap) => _bitmap.AndCount(bitmap);

    public Roaring32Bitmap AndNot(Roaring32BitmapBase bitmap) => _bitmap.AndNot(bitmap);

    public ulong AndNotCount(Roaring32BitmapBase bitmap) => _bitmap.AndNotCount(bitmap);

    public Roaring32Bitmap Or(Roaring32BitmapBase bitmap) => _bitmap.Or(bitmap);

    public ulong OrCount(Roaring32BitmapBase bitmap) => _bitmap.OrCount(bitmap);

    public Roaring32Bitmap OrMany(Roaring32BitmapBase[] bitmaps) => _bitmap.OrMany(bitmaps);

    public Roaring32Bitmap OrManyHeap(Roaring32BitmapBase[] bitmaps) => _bitmap.OrManyHeap(bitmaps);

    public Roaring32Bitmap LazyOr(Roaring32BitmapBase bitmap, bool bitsetConversion) => _bitmap.LazyOr(bitmap, bitsetConversion);

    public Roaring32Bitmap Xor(Roaring32BitmapBase bitmap) => _bitmap.Xor(bitmap);

    public ulong XorCount(Roaring32BitmapBase bitmap) => _bitmap.XorCount(bitmap);

    public Roaring32Bitmap XorMany(params Roaring32BitmapBase[] bitmaps) => _bitmap.XorMany(bitmaps);

    public Roaring32Bitmap LazyXor(Roaring32BitmapBase bitmap) => _bitmap.LazyXor(bitmap);

    public bool Overlaps(Roaring32BitmapBase bitmap) => _bitmap.Overlaps(bitmap);

    public bool OverlapsRange(uint start, uint end) => _bitmap.OverlapsRange(start, end);

    public double GetJaccardIndex(Roaring32BitmapBase bitmap) => _bitmap.GetJaccardIndex(bitmap);

    public void CopyTo(uint[] buffer) => _bitmap.CopyTo(buffer);

    public IEnumerable<uint> Values => _bitmap.Values;

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

    public nuint GetSerializationBytes(SerializationFormat format = SerializationFormat.Normal) => _bitmap.GetSerializationBytes(format);

    public byte[] Serialize(SerializationFormat format = SerializationFormat.Normal) => _bitmap.Serialize(format);

    public uint[] Take(ulong count) => _bitmap.Take(count);

    public Statistics GetStatistics() => _bitmap.GetStatistics();

    public bool IsValid() => _bitmap.IsValid();

    public bool IsValid(out string? reason) => _bitmap.IsValid(out reason);
}