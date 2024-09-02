using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Roaring.Net.CRoaring;

/// <summary>
/// Represents a 32-bit CRoaring bitmap.
/// </summary>
public unsafe class Roaring32Bitmap : Roaring32BitmapBase, IReadOnlyRoaring32Bitmap
{
    private bool _isDisposed;

    /// <summary>
    /// Gets the number of elements (cardinality) contained in the <see cref="Roaring32Bitmap"/>.
    /// </summary>
    /// <returns>The number of elements contained in the <see cref="Roaring32Bitmap"/>.</returns>
    public ulong Count => NativeMethods.roaring_bitmap_get_cardinality(Pointer);

    /// <summary>
    /// Gets a value indicating that <see cref="Roaring32Bitmap"/> is empty (cardinality is zero).
    /// </summary>
    /// <returns><c>true</c> if <see cref="Roaring32Bitmap"/> is empty (cardinality is zero); otherwise, <c>false</c>.</returns>
    public bool IsEmpty => NativeMethods.roaring_bitmap_is_empty(Pointer);

    /// <summary>
    /// Gets a value indicating that <see cref="Roaring32Bitmap"/> is has copy-on-write (COW) mode enabled.
    /// </summary>
    /// <returns><c>true</c> if <see cref="Roaring32Bitmap"/> has copy-on-write enabled; otherwise, <c>false</c>.</returns>
    public bool IsCopyOnWrite => NativeMethods.roaring_bitmap_get_copy_on_write(Pointer);

    /// <summary>
    /// Gets the minimum value in the <see cref="Roaring32Bitmap"/>.
    /// </summary>
    /// <returns>The minimum value in the <see cref="Roaring32Bitmap"/> or <c>null</c> when the bitmap is empty.</returns>
    public uint? Min => IsEmpty ? null : NativeMethods.roaring_bitmap_minimum(Pointer);

    /// <summary>
    /// Gets the maximum value in the <see cref="Roaring32Bitmap"/>.
    /// </summary>
    /// <returns>The maximum value in the <see cref="Roaring32Bitmap"/> or <see langword="null"/> when the bitmap is empty.</returns>
    public uint? Max => IsEmpty ? null : NativeMethods.roaring_bitmap_maximum(Pointer);

    /// <summary>
    /// Initializes a new instance of the <see cref="Roaring32Bitmap"/> class.
    /// </summary>
    /// <remarks>Initializes an empty bitmap (with a capacity of 0).</remarks>
    /// <exception cref="InvalidOperationException">Thrown when unable to allocate bitmap.</exception>
    public Roaring32Bitmap() => Pointer = CheckBitmapPointer(NativeMethods.roaring_bitmap_create_with_capacity(0));

    /// <summary>
    /// Initializes a new instance of the <see cref="Roaring32Bitmap"/> class with the given capacity.
    /// </summary>
    /// <param name="capacity">Initial capacity of the bitmap.</param>
    /// <remarks>Capacity is a performance hint indicating how much data should be allocated when creating a bitmap.</remarks>
    /// <exception cref="InvalidOperationException">Thrown when unable to allocate bitmap.</exception>
    public Roaring32Bitmap(uint capacity) => Pointer = CheckBitmapPointer(NativeMethods.roaring_bitmap_create_with_capacity(capacity));

    /// <summary>
    /// Initializes a new instance of the <see cref="Roaring32Bitmap"/> class with the given values.
    /// </summary>
    /// <param name="values">Values that will be added to the bitmap directly when it is created.</param>
    /// <exception cref="InvalidOperationException">Thrown when unable to allocate bitmap.</exception>
    public Roaring32Bitmap(uint[] values) => Pointer = CheckBitmapPointer(CreatePtrFromValues(values, 0, (uint)values.Length));

    internal Roaring32Bitmap(IntPtr pointer) => Pointer = CheckBitmapPointer(pointer);

    private static IntPtr CheckBitmapPointer(IntPtr pointer)
    {
        if (pointer == IntPtr.Zero)
        {
            throw new InvalidOperationException(ExceptionMessages.UnableToAllocateBitmap);
        }

        return pointer;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Roaring32Bitmap"/> class for the given range.
    /// </summary>
    /// <param name="start">Start of range (inclusive).</param>
    /// <param name="end">End of range (inclusive).</param>
    /// <param name="step">Step between values (i * stop from minimum value).</param>
    /// <returns>Instance of the <see cref="Roaring32Bitmap"/> class for the given range.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when arguments have invalid values.</exception>
    /// <exception cref="InvalidOperationException">Thrown when unable to allocate bitmap.</exception>
    public static Roaring32Bitmap FromRange(uint start, uint end, uint step = 1)
    {
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, ExceptionMessages.StartValueGreaterThenEndValue);
        }

        if (step == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(step), step, ExceptionMessages.StepEqualToZero);
        }

        return new(NativeMethods.roaring_bitmap_from_range(start, (ulong)end + 1, step));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Roaring32Bitmap"/> class with the given values.
    /// </summary>
    /// <param name="values">Values that will be added to the bitmap directly when it is created.</param>
    /// <returns>Instance of the <see cref="Roaring32Bitmap"/> class with the given values.</returns>
    /// <exception cref="InvalidOperationException">Thrown when unable to allocate bitmap.</exception>
    public static Roaring32Bitmap FromValues(uint[] values) => FromValues(values, 0U, (nuint)values.Length);

    /// <summary>
    /// Initializes a new instance of the <see cref="Roaring32Bitmap"/> class with the given values from subarray.
    /// </summary>
    /// <param name="values">An array containing the values to add.</param>
    /// <param name="offset">The position in the array from which to start adding data.</param>
    /// <param name="count">The number of values to add from the offset position.</param>
    /// <returns>Instance of the <see cref="Roaring32Bitmap"/> class with the given values from subarray.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when arguments have invalid values.</exception>
    /// <exception cref="InvalidOperationException">Thrown when unable to allocate bitmap.</exception>
    public static Roaring32Bitmap FromValues(uint[] values, nuint offset, nuint count)
        => new(CreatePtrFromValues(values, offset, count));

    private static IntPtr CreatePtrFromValues(uint[] values, nuint offset, nuint count)
    {
        if ((nuint)values.Length < offset + count)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), offset, ExceptionMessages.OffsetWithCountGreaterThanNumberOfValues);
        }

        fixed (uint* valuePtr = values)
        {
            return NativeMethods.roaring_bitmap_of_ptr(count, valuePtr + offset);
        }
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        NativeMethods.roaring_bitmap_free(Pointer);
        _isDisposed = true;
    }

    ~Roaring32Bitmap() => Dispose(false);

    /// <summary>
    /// Copies the bitmap.
    /// </summary>
    /// <returns>Instance of the <see cref="Roaring32Bitmap"/> class with the same values as the current bitmap.</returns>
    /// <exception cref="InvalidOperationException">Thrown when unable to allocate bitmap.</exception>
    public Roaring32Bitmap Clone() => new(NativeMethods.roaring_bitmap_copy(Pointer));

    /// <summary>
    /// Copies the bitmap from the given offset.
    /// </summary>
    /// <param name="offset">The position in the bitmap from which to start copying data.</param>
    /// <returns>Instance of the <see cref="Roaring32Bitmap"/> class with the same values as the current bitmap from the given offset.</returns>
    /// <exception cref="InvalidOperationException">Thrown when unable to allocate bitmap.</exception>
    public Roaring32Bitmap CloneWithOffset(long offset)
        => new(NativeMethods.roaring_bitmap_add_offset(Pointer, offset));

    /// <summary>
    /// Overwrites the current bitmap with the bitmap given in the <paramref name="source"/> parameter. <br/>
    /// The content of the current bitmap will be deleted.
    /// </summary>
    /// <param name="source">Bitmap that will be written in place of the current bitmap.</param>
    /// <returns><c>true</c> if successful; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// The <see cref="OverwriteWith"/> method can save on memory allocations compared to the <see cref="Clone"/> method. <br/>
    /// On failure, the current bitmap is left in a valid, empty state (all stored values are deleted).
    /// </remarks>
    public bool OverwriteWith(Roaring32BitmapBase source) => NativeMethods.roaring_bitmap_overwrite(Pointer, source.Pointer);

    /// <summary>
    /// Adds a value to the bitmap.
    /// </summary>
    /// <param name="value">A value that will be added to the bitmap.</param>
    public void Add(uint value) => NativeMethods.roaring_bitmap_add(Pointer, value);

    /// <summary>
    /// Adds values from the given array to the bitmap.
    /// </summary>
    /// <param name="values">An array containing the values to add.</param>
    public void AddMany(uint[] values) => AddMany(values, 0, (nuint)values.Length);

    /// <summary>
    /// Adds values from the given subarray to the bitmap.
    /// </summary>
    /// <param name="values">An array containing the values to add.</param>
    /// <param name="offset">The position in the array from which to start adding data.</param>
    /// <param name="count">The number of values to add from the offset position.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when arguments have invalid values.</exception>
    public void AddMany(uint[] values, nuint offset, nuint count)
    {
        if ((nuint)values.Length < offset + count)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), offset, ExceptionMessages.OffsetWithCountGreaterThanNumberOfValues);
        }

        fixed (uint* valuePtr = values)
        {
            NativeMethods.roaring_bitmap_add_many(Pointer, count, valuePtr + offset);
        }
    }

    /// <summary>
    /// Tries to add a value to the bitmap.
    /// </summary>
    /// <param name="value">A value that will be added to the bitmap.</param>
    /// <returns><c>true</c> if a new value does not exist in the bitmap and has been added; otherwise, <c>false</c>.</returns>
    public bool TryAdd(uint value) => NativeMethods.roaring_bitmap_add_checked(Pointer, value);

    /// <summary>
    /// Adds values from the given range.
    /// </summary>
    /// <param name="start">Start of range (inclusive).</param>
    /// <param name="end">End of range (inclusive).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when arguments have invalid values.</exception>
    public void AddRange(uint start, uint end)
    {
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, ExceptionMessages.StartValueGreaterThenEndValue);
        }

        NativeMethods.roaring_bitmap_add_range_closed(Pointer, start, end);
    }

    /// <summary>
    /// Adds an offset to all values stored in the bitmap.
    /// </summary>
    /// <param name="offset">The offset that will be added to all values.</param>
    /// <remarks>This method allocates a new bitmap.</remarks>
    /// <exception cref="InvalidOperationException">Thrown when unable to allocate bitmap.</exception>
    public void AddOffset(long offset)
    {
        IntPtr previousPtr = Pointer;
        Pointer = CheckBitmapPointer(NativeMethods.roaring_bitmap_add_offset(Pointer, offset));
        NativeMethods.roaring_bitmap_free(previousPtr);
    }

    /// <summary>
    /// Adds value to the bitmap using context from a previous bulk operation to optimize the addition process.
    /// </summary>
    /// <param name="context">A context that stores information between `*Bulk` method calls.</param>
    /// <param name="value">A value that will be added to the bitmap.</param>
    /// <exception cref="ArgumentException">Thrown when context belongs to another bitmap.</exception>
    /// <remarks>
    /// To take advantage of this optimization, the caller should call this method sequentially with values
    /// with the same "key" (high 16 bits of the value).
    /// </remarks>
    public void AddBulk(BulkContext context, uint value)
    {
        if (context.Bitmap != this)
        {
            throw new ArgumentException(ExceptionMessages.BulkContextBelongsToOtherBitmap, nameof(context));
        }

        NativeMethods.roaring_bitmap_add_bulk(Pointer, context.Pointer, value);
    }

    /// <summary>
    /// Removes a value from the bitmap.
    /// </summary>
    /// <param name="value">A value that will be removed from the bitmap.</param>
    public void Remove(uint value) => NativeMethods.roaring_bitmap_remove(Pointer, value);

    /// <summary>
    /// Removes the values contained in the given array from the bitmap.
    /// </summary>
    /// <param name="values">An array containing the values to remove.</param>
    public void RemoveMany(uint[] values) => RemoveMany(values, 0, (nuint)values.Length);

    /// <summary>
    /// Removes the values contained in the given subarray from the bitmap.
    /// </summary>
    /// <param name="values">An array containing the values to remove.</param>
    /// <param name="offset">The position in the array from which to start removing data.</param>
    /// <param name="count">The number of values to remove from the offset position.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when arguments have invalid values.</exception>
    public void RemoveMany(uint[] values, nuint offset, nuint count)
    {
        if ((nuint)values.Length < offset + count)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), offset, ExceptionMessages.OffsetWithCountGreaterThanNumberOfValues);
        }

        fixed (uint* valuePtr = values)
        {
            NativeMethods.roaring_bitmap_remove_many(Pointer, count, valuePtr + offset);
        }
    }

    /// <summary>
    /// Tries to remove a value from the bitmap.
    /// </summary>
    /// <param name="value">A value that will be removed from the bitmap.</param>
    /// <returns><c>true</c> if a value exists in the bitmap and has been removed; otherwise, <c>false</c>.</returns>
    public bool TryRemove(uint value) => NativeMethods.roaring_bitmap_remove_checked(Pointer, value);

    /// <summary>
    /// Removes values from the given range.
    /// </summary>
    /// <param name="start">Start of range (inclusive).</param>
    /// <param name="end">End of range (inclusive).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when arguments have invalid values.</exception>
    public void RemoveRange(uint start, uint end)
    {
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, ExceptionMessages.StartValueGreaterThenEndValue);
        }

        NativeMethods.roaring_bitmap_remove_range_closed(Pointer, start, end);
    }

    /// <summary>
    /// Removes all values from the bitmap.
    /// </summary>
    public void Clear() => NativeMethods.roaring_bitmap_clear(Pointer);

    /// <summary>
    /// Checks if a value is present in the bitmap.
    /// </summary>
    /// <param name="value">A value for which the check will be performed.</param>
    /// <returns><c>true</c> if a value exists in the bitmap; otherwise, <c>false</c>.</returns>
    public bool Contains(uint value) => NativeMethods.roaring_bitmap_contains(Pointer, value);

    /// <summary>
    /// Checks if the values for the given range are present in the bitmap.
    /// </summary>
    /// <param name="start">Start of range (inclusive).</param>
    /// <param name="end">End of range (inclusive).</param>
    /// <returns><c>true</c> if all values from the given range exist in the bitmap; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when arguments have invalid values.</exception>
    public bool ContainsRange(uint start, uint end)
    {
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, ExceptionMessages.StartValueGreaterThenEndValue);
        }

        return NativeMethods.roaring_bitmap_contains_range(Pointer, start, (ulong)end + 1);
    }

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
    public bool ValueEquals(Roaring32BitmapBase? bitmap)
    {
        if (bitmap == null)
        {
            return false;
        }

        return NativeMethods.roaring_bitmap_equals(Pointer, bitmap.Pointer);
    }

    /// <summary>
    /// Checks if the current bitmap is a subset of the <paramref name="bitmap"/>. 
    /// </summary>
    /// <param name="bitmap"></param>
    /// <returns><c>true</c> if current bitmaps is a subset of the <paramref name="bitmap"/>; otherwise, <c>false</c>.</returns>
    public bool IsSubsetOf(Roaring32BitmapBase? bitmap)
    {
        if (bitmap == null)
        {
            return false;
        }

        return NativeMethods.roaring_bitmap_is_subset(Pointer, bitmap.Pointer);
    }

    /// <summary>
    /// Checks if the current bitmap is a proper subset of the <paramref name="bitmap"/>. 
    /// </summary>
    /// <param name="bitmap"></param>
    /// <returns><c>true</c> if current bitmaps is a proper subset of the <paramref name="bitmap"/>; otherwise, <c>false</c>.</returns>
    public bool IsProperSubsetOf(Roaring32BitmapBase? bitmap)
    {
        if (bitmap == null)
        {
            return false;
        }

        return NativeMethods.roaring_bitmap_is_strict_subset(Pointer, bitmap.Pointer);
    }

    /// <summary>
    /// Checks if the current bitmap is a superset of the <paramref name="bitmap"/>. 
    /// </summary>
    /// <param name="bitmap"></param>
    /// <returns><c>true</c> if current bitmaps is a superset of the <paramref name="bitmap"/>; otherwise, <c>false</c>.</returns>
    public bool IsSupersetOf(Roaring32BitmapBase? bitmap)
    {
        if (bitmap == null)
        {
            return false;
        }

        if (NativeMethods.roaring_bitmap_get_cardinality(bitmap.Pointer) >
            NativeMethods.roaring_bitmap_get_cardinality(Pointer))
        {
            return false;
        }

        return NativeMethods.roaring_bitmap_is_subset(bitmap.Pointer, Pointer);
    }

    /// <summary>
    /// Checks if the current bitmap is a proper superset of the <paramref name="bitmap"/>. 
    /// </summary>
    /// <param name="bitmap"></param>
    /// <returns><c>true</c> if current bitmaps is a proper superset of the <paramref name="bitmap"/>; otherwise, <c>false</c>.</returns>
    public bool IsProperSupersetOf(Roaring32BitmapBase? bitmap)
    {
        if (bitmap == null)
        {
            return false;
        }

        if (NativeMethods.roaring_bitmap_get_cardinality(bitmap.Pointer) >=
            NativeMethods.roaring_bitmap_get_cardinality(Pointer))
        {
            return false;
        }

        return NativeMethods.roaring_bitmap_is_subset(bitmap.Pointer, Pointer);
    }

    /// <summary>
    /// Tries to get a value located at the given <paramref name="index"/> (rank).
    /// </summary>
    /// <param name="index">The index for which the value will be retrieved. Index values start from 0.</param>
    /// <param name="value">Retrieved value. <c>0</c> if value does not exist in the bitmap.</param>
    /// <returns><c>true</c> if a value exists in the bitmap; otherwise, <c>false</c>.</returns>
    public bool TryGetValue(uint index, out uint value) => NativeMethods.roaring_bitmap_select(Pointer, index, out value);

    /// <summary>
    /// Gets the index (rank) for the given value.
    /// </summary>
    /// <param name="value">The value for which the index will be retrieved.</param>
    /// <returns><c>-1</c> if a <paramref name="value"/> does not exist in the bitmap; otherwise, index (rank) of the <paramref name="value"/>.</returns>
    public long GetIndex(uint value) => NativeMethods.roaring_bitmap_get_index(Pointer, value);

    /// <summary>
    /// Counts the number of values less than or equal to <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value for which data will be counted.</param>
    /// <returns>The number of values that are less than or equal to the <paramref name="value"/>.</returns>
    public ulong CountLessOrEqualTo(uint value) => NativeMethods.roaring_bitmap_rank(Pointer, value);

    /// <summary>
    /// Counts the number of values less than or equal to for each element of <paramref name="values"/>.
    /// </summary>
    /// <param name="values">An ascending sorted set of tested values.</param>
    /// <returns>The number of values that are less than or equal to the value from <paramref name="values"/> placed under the same index.</returns>
    public ulong[] CountManyLessOrEqualTo(uint[] values)
    {
        var items = new ulong[values.Length];
        fixed (uint* valuesPtr = values)
        {
            NativeMethods.roaring_bitmap_rank_many(Pointer, valuesPtr, valuesPtr + values.Length, items);
        }
        return items;
    }

    /// <summary>
    /// Counts the number of values in the given range of values.
    /// </summary>
    /// <param name="start">Start of range (inclusive).</param>
    /// <param name="end">End of range (inclusive).</param>
    /// <returns>The number of values in the given range of values.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when arguments have invalid values.</exception>
    public ulong CountRange(uint start, uint end)
    {
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, ExceptionMessages.StartValueGreaterThenEndValue);
        }

        return NativeMethods.roaring_bitmap_range_cardinality(Pointer, start, (ulong)end + 1);
    }

    /// <summary>
    /// Creates a new negated bitmap based on the values in the current bitmap. 
    /// </summary>
    /// <returns>Instance of the <see cref="Roaring32Bitmap"/> class with negated values of current bitmap.</returns>
    public Roaring32Bitmap Not()
        => new(NativeMethods.roaring_bitmap_flip(Pointer, uint.MinValue, uint.MaxValue + 1UL));

    /// <summary>
    /// Negates all values in the current bitmap. 
    /// </summary>
    public void INot() => NativeMethods.roaring_bitmap_flip_inplace(Pointer, uint.MinValue, uint.MaxValue + 1UL);

    /// <summary>
    /// Creates a new negated bitmap based on the values in the current bitmap for the given range of values. 
    /// </summary>
    /// <param name="start">Start of range (inclusive).</param>
    /// <param name="end">End of range (inclusive).</param>
    /// <returns>Instance of the <see cref="Roaring32Bitmap"/> class with negated values of current bitmap in the given range of values.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when arguments have invalid values.</exception>
    /// <remarks>Values outside the range are left unchanged.</remarks>
    public Roaring32Bitmap NotRange(uint start, uint end)
    {
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, ExceptionMessages.StartValueGreaterThenEndValue);
        }

        return new(NativeMethods.roaring_bitmap_flip(Pointer, start, (ulong)end + 1));
    }

    /// <summary>
    /// Negates values in the current bitmap for the given range of values. 
    /// </summary>
    /// <param name="start">Start of range (inclusive).</param>
    /// <param name="end">End of range (inclusive).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when arguments have invalid values.</exception>
    /// <remarks>Values outside the range are left unchanged.</remarks>
    public void INotRange(uint start, uint end)
    {
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, ExceptionMessages.StartValueGreaterThenEndValue);
        }

        NativeMethods.roaring_bitmap_flip_inplace(Pointer, start, (ulong)end + 1);
    }

    public Roaring32Bitmap And(Roaring32BitmapBase bitmap) =>
        new(NativeMethods.roaring_bitmap_and(Pointer, bitmap.Pointer));

    public void IAnd(Roaring32BitmapBase bitmap)
        => NativeMethods.roaring_bitmap_and_inplace(Pointer, bitmap.Pointer);

    public ulong AndCount(Roaring32BitmapBase bitmap)
        => NativeMethods.roaring_bitmap_and_cardinality(Pointer, bitmap.Pointer);

    public Roaring32Bitmap AndNot(Roaring32BitmapBase bitmap) =>
        new(NativeMethods.roaring_bitmap_andnot(Pointer, bitmap.Pointer));

    public void IAndNot(Roaring32BitmapBase bitmap)
        => NativeMethods.roaring_bitmap_andnot_inplace(Pointer, bitmap.Pointer);

    public ulong AndNotCount(Roaring32BitmapBase bitmap)
        => NativeMethods.roaring_bitmap_andnot_cardinality(Pointer, bitmap.Pointer);

    public Roaring32Bitmap Or(Roaring32BitmapBase bitmap) =>
        new(NativeMethods.roaring_bitmap_or(Pointer, bitmap.Pointer));

    public ulong OrCount(Roaring32BitmapBase bitmap)
        => NativeMethods.roaring_bitmap_or_cardinality(Pointer, bitmap.Pointer);

    public Roaring32Bitmap OrMany(Roaring32BitmapBase[] bitmaps)
    {
        int length = bitmaps.Length + 1;
        var pointers = new IntPtr[length];
        for (int i = 0; i < bitmaps.Length; i++)
        {
            pointers[i] = bitmaps[i].Pointer;
        }
        pointers[length - 1] = Pointer;

        return new Roaring32Bitmap(NativeMethods.roaring_bitmap_or_many((nuint)pointers.Length, pointers));
    }

    public Roaring32Bitmap OrManyHeap(Roaring32BitmapBase[] bitmaps)
    {
        int length = bitmaps.Length + 1;
        var pointers = new IntPtr[length];
        for (int i = 0; i < bitmaps.Length; i++)
        {
            pointers[i] = bitmaps[i].Pointer;
        }
        pointers[length - 1] = Pointer;

        return new Roaring32Bitmap(NativeMethods.roaring_bitmap_or_many_heap((uint)pointers.Length, pointers));
    }

    public void IOr(Roaring32BitmapBase bitmap)
        => NativeMethods.roaring_bitmap_or_inplace(Pointer, bitmap.Pointer);

    public Roaring32Bitmap LazyOr(Roaring32BitmapBase bitmap, bool bitsetConversion) =>
        new(NativeMethods.roaring_bitmap_lazy_or(Pointer, bitmap.Pointer, bitsetConversion));

    public void ILazyOr(Roaring32BitmapBase bitmap, bool bitsetConversion)
        => NativeMethods.roaring_bitmap_lazy_or_inplace(Pointer, bitmap.Pointer, bitsetConversion);

    public Roaring32Bitmap Xor(Roaring32BitmapBase bitmap) =>
        new(NativeMethods.roaring_bitmap_xor(Pointer, bitmap.Pointer));

    public void IXor(Roaring32BitmapBase bitmap)
        => NativeMethods.roaring_bitmap_xor_inplace(Pointer, bitmap.Pointer);

    public ulong XorCount(Roaring32BitmapBase bitmap)
        => NativeMethods.roaring_bitmap_xor_cardinality(Pointer, bitmap.Pointer);

    public Roaring32Bitmap XorMany(params Roaring32BitmapBase[] bitmaps)
    {
        int length = bitmaps.Length + 1;
        var pointers = new IntPtr[length];
        for (int i = 0; i < bitmaps.Length; i++)
        {
            pointers[i] = bitmaps[i].Pointer;
        }
        pointers[length - 1] = Pointer;

        return new Roaring32Bitmap(NativeMethods.roaring_bitmap_xor_many((nuint)pointers.Length, pointers));
    }

    public Roaring32Bitmap LazyXor(Roaring32BitmapBase bitmap)
        => new(NativeMethods.roaring_bitmap_lazy_xor(Pointer, bitmap.Pointer));

    public void ILazyXor(Roaring32BitmapBase bitmap)
        => NativeMethods.roaring_bitmap_lazy_xor_inplace(Pointer, bitmap.Pointer);

    public void RepairAfterLazy()
        => NativeMethods.roaring_bitmap_repair_after_lazy(Pointer);

    public bool Overlaps(Roaring32BitmapBase bitmap)
        => NativeMethods.roaring_bitmap_intersect(Pointer, bitmap.Pointer);

    public bool OverlapsRange(uint start, uint end)
    {
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, ExceptionMessages.StartValueGreaterThenEndValue);
        }

        return NativeMethods.roaring_bitmap_intersect_with_range(Pointer, start, (ulong)end + 1);
    }

    public double GetJaccardIndex(Roaring32BitmapBase bitmap)
        => NativeMethods.roaring_bitmap_jaccard_index(Pointer, bitmap.Pointer);

    public bool Optimize()
        => NativeMethods.roaring_bitmap_run_optimize(Pointer);

    public bool RemoveRunCompression()
        => NativeMethods.roaring_bitmap_remove_run_compression(Pointer);

    public nuint ShrinkToFit()
        => NativeMethods.roaring_bitmap_shrink_to_fit(Pointer);

    public void CopyTo(uint[] buffer)
        => NativeMethods.roaring_bitmap_to_uint32_array(Pointer, buffer);

    public nuint GetSerializationBytes(SerializationFormat format = SerializationFormat.Normal)
        => format switch
        {
            SerializationFormat.Normal => NativeMethods.roaring_bitmap_size_in_bytes(Pointer),
            SerializationFormat.Portable => NativeMethods.roaring_bitmap_portable_size_in_bytes(Pointer),
            SerializationFormat.Frozen => NativeMethods.roaring_bitmap_frozen_size_in_bytes(Pointer),
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, ExceptionMessages.UnsupportedSerializationFormat)
        };

    public byte[] Serialize(SerializationFormat format = SerializationFormat.Normal)
    {
        byte[] buffer;
        switch (format)
        {
            case SerializationFormat.Normal:
                buffer = new byte[NativeMethods.roaring_bitmap_size_in_bytes(Pointer)];
                NativeMethods.roaring_bitmap_serialize(Pointer, buffer);
                break;
            case SerializationFormat.Portable:
                buffer = new byte[NativeMethods.roaring_bitmap_portable_size_in_bytes(Pointer)];
                NativeMethods.roaring_bitmap_portable_serialize(Pointer, buffer);
                break;
            case SerializationFormat.Frozen:
                buffer = new byte[NativeMethods.roaring_bitmap_frozen_size_in_bytes(Pointer)];
                fixed (byte* bufferPtr = buffer)
                {
                    NativeMethods.roaring_bitmap_frozen_serialize(Pointer, bufferPtr);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(format), format, ExceptionMessages.UnsupportedSerializationFormat);
        }

        return buffer;
    }

    public static Roaring32Bitmap Deserialize(byte[] buffer, SerializationFormat format = SerializationFormat.Normal)
    {
        IntPtr ptr = format switch
        {
            SerializationFormat.Normal => NativeMethods.roaring_bitmap_deserialize_safe(buffer, (nuint)buffer.Length),
            SerializationFormat.Portable => NativeMethods.roaring_bitmap_portable_deserialize_safe(buffer, (nuint)buffer.Length),
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, ExceptionMessages.UnsupportedSerializationFormat)
        };

        if (ptr == IntPtr.Zero)
        {
            throw new InvalidOperationException(ExceptionMessages.DeserializationFailedUnknownReason);
        }

        return new Roaring32Bitmap(ptr);
    }

    public static Roaring32Bitmap DeserializeUnsafe(byte[] buffer, SerializationFormat format = SerializationFormat.Normal)
    {
        IntPtr ptr = format switch
        {
            SerializationFormat.Normal => NativeMethods.roaring_bitmap_deserialize(buffer),
            SerializationFormat.Portable => NativeMethods.roaring_bitmap_portable_deserialize(buffer),
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, ExceptionMessages.UnsupportedSerializationFormat)
        };

        if (ptr == IntPtr.Zero)
        {
            throw new InvalidOperationException(ExceptionMessages.DeserializationFailedUnknownReason);
        }

        return new Roaring32Bitmap(ptr);
    }

    public static nuint GetSerializedSize(byte[] buffer, nuint expectedSize, SerializationFormat format = SerializationFormat.Portable)
    {
        nuint size = format switch
        {
            SerializationFormat.Portable => NativeMethods.roaring_bitmap_portable_deserialize_size(buffer, expectedSize),
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, ExceptionMessages.UnsupportedSerializationFormat)
        };

        return size;
    }

    public IEnumerable<uint> Values => new Roaring32Enumerator(Pointer);

    public uint[] ToArray()
    {
        ulong count = NativeMethods.roaring_bitmap_get_cardinality(Pointer);
        uint[] values = new uint[count];
        NativeMethods.roaring_bitmap_to_uint32_array(Pointer, values);
        return values;
    }

    public FrozenRoaring32Bitmap ToFrozen() => new(this);

    internal Roaring32Bitmap GetFrozenView(nuint size, byte* memoryPtr)
    {
        NativeMethods.roaring_bitmap_frozen_serialize(Pointer, memoryPtr);
        IntPtr ptr = NativeMethods.roaring_bitmap_frozen_view(memoryPtr, size);
        return new Roaring32Bitmap(ptr);
    }

    public uint[] Take(ulong count)
    {
        ulong cardinality = NativeMethods.roaring_bitmap_get_cardinality(Pointer);
        if (cardinality < count)
        {
            count = cardinality;
        }

        uint[] values = new uint[count];
        NativeMethods.roaring_bitmap_to_uint32_array(Pointer, values);
        return values;
    }

    public Statistics GetStatistics()
    {
        NativeMethods.roaring_bitmap_statistics(Pointer, out Statistics stats);
        return stats;
    }

    public bool IsValid() => IsValid(out _);

    public bool IsValid(out string? reason)
    {
        var result = NativeMethods.roaring_bitmap_internal_validate(Pointer, out IntPtr reasonPtr);
        reason = Marshal.PtrToStringAnsi(reasonPtr);
        return result;
    }

    public void SetCopyOnWrite(bool enabled) => NativeMethods.roaring_bitmap_set_copy_on_write(Pointer, enabled);
}