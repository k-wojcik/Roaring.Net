using System;
using System.Collections.Generic;
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

    /// <inheritdoc />
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
    /// Counts number of values less than or equal to <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value for which data will be counted.</param>
    /// <returns>The number of values that are less than or equal to the <paramref name="value"/>.</returns>
    public ulong CountLessOrEqualTo(uint value) => NativeMethods.roaring_bitmap_rank(Pointer, value);

    /// <summary>
    /// Counts number of values less than or equal to for each element of <paramref name="values"/>.
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
    /// Counts number of values in the given range of values.
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

    /// <summary>
    /// Creates a intersection between the current bitmap and the <paramref name="bitmap"/> given in the parameter.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the intersection will be performed.</param>
    /// <returns><see cref="Roaring32Bitmap"/> with the result of intersection of two bitmaps.</returns>
    /// <remarks>
    /// Performance hints:
    /// <list type="bullet">
    /// <item>if you are computing the intersection between several bitmaps, two-by-two, it is best to start with the smallest bitmap,</item>
    /// <item>you may also rely on <see cref="IAnd"/> to avoid creating many temporary bitmaps.</item>
    /// </list>
    /// </remarks>
    public Roaring32Bitmap And(Roaring32BitmapBase bitmap) =>
        new(NativeMethods.roaring_bitmap_and(Pointer, bitmap.Pointer));

    /// <summary>
    /// Intersects (in place) the current bitmap with the <paramref name="bitmap"/> given in the parameter.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the intersection will be performed.</param>
    /// <remarks>
    /// Performance hints:
    /// <list type="bullet">
    /// <item>if you are computing the intersection between several bitmaps, two-by-two, it is best to start with the smallest bitmap,</item>
    /// </list>
    /// </remarks>
    public void IAnd(Roaring32BitmapBase bitmap)
        => NativeMethods.roaring_bitmap_and_inplace(Pointer, bitmap.Pointer);

    /// <summary>
    /// Intersects the current bitmap with the <paramref name="bitmap"/> given in the parameter and returns the number of values contained in the resulting bitmap.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the intersection will be performed.</param>
    /// <returns>Number of values contained in the resulting bitmap after intersection.</returns>
    public ulong AndCount(Roaring32BitmapBase bitmap)
        => NativeMethods.roaring_bitmap_and_cardinality(Pointer, bitmap.Pointer);

    /// <summary>
    /// Creates a difference between the current bitmap and the <paramref name="bitmap"/> given in the parameter.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the difference will be performed.</param>
    /// <returns><see cref="Roaring32Bitmap"/> with the result of the difference of two bitmaps.</returns>
    public Roaring32Bitmap AndNot(Roaring32BitmapBase bitmap) =>
        new(NativeMethods.roaring_bitmap_andnot(Pointer, bitmap.Pointer));

    /// <summary>
    /// Creates (in place) a difference between the current bitmap with the <paramref name="bitmap"/> given in the parameter.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the difference will be performed.</param>
    public void IAndNot(Roaring32BitmapBase bitmap)
        => NativeMethods.roaring_bitmap_andnot_inplace(Pointer, bitmap.Pointer);

    /// <summary>
    /// Creates a difference between the current bitmap and the <paramref name="bitmap"/> given in the parameter and returns the number of values contained in the resulting bitmap.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the difference will be performed.</param>
    /// <returns>Number of values contained in the resulting bitmap after difference.</returns>
    public ulong AndNotCount(Roaring32BitmapBase bitmap)
        => NativeMethods.roaring_bitmap_andnot_cardinality(Pointer, bitmap.Pointer);

    /// <summary>
    /// Creates a union between the current bitmap and the <paramref name="bitmap"/> given in the parameter.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the union will be performed.</param>
    /// <returns><see cref="Roaring32Bitmap"/> with the result of union two bitmaps.</returns>
    public Roaring32Bitmap Or(Roaring32BitmapBase bitmap) =>
        new(NativeMethods.roaring_bitmap_or(Pointer, bitmap.Pointer));

    /// <summary>
    /// Creates a union between the current bitmap and the <paramref name="bitmap"/> given in the parameter and returns the number of values contained in the resulting bitmap.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the union will be performed.</param>
    /// <returns>Number of values contained in the resulting bitmap after union.</returns>
    public ulong OrCount(Roaring32BitmapBase bitmap)
        => NativeMethods.roaring_bitmap_or_cardinality(Pointer, bitmap.Pointer);

    /// <summary>
    /// Creates a union between the current bitmap and the <paramref name="bitmaps"/> given in the parameter.
    /// </summary>
    /// <param name="bitmaps">Bitmaps with which the union will be performed.</param>
    /// <returns><see cref="Roaring32Bitmap"/> with the result of union of many bitmaps.</returns>
    /// <remarks>This method may be slower than <see cref="OrManyHeap"/> in some cases.</remarks>
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

    /// <summary>
    /// Creates a union between the current bitmap and the <paramref name="bitmaps"/> given in the parameter using a heap.
    /// </summary>
    /// <param name="bitmaps">Bitmaps with which the union will be performed.</param>
    /// <returns><see cref="Roaring32Bitmap"/> with the result of union of many bitmaps.</returns>
    /// <remarks>This method may be faster than <see cref="OrMany"/> in some cases.</remarks>
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

    /// <summary>
    /// Creates (in place) a union between the current bitmap and the <paramref name="bitmap"/> given in the parameter.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the union will be performed.</param>
    public void IOr(Roaring32BitmapBase bitmap)
        => NativeMethods.roaring_bitmap_or_inplace(Pointer, bitmap.Pointer);

    /// <summary>
    /// Creates a union between the current bitmap and the <paramref name="bitmap"/> given in the parameter using lazy algorithm.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the union will be performed.</param>
    /// <param name="bitsetConversion">Flag which determines whether container-container operations force a bitset conversion.</param>
    /// <returns><see cref="Roaring32Bitmap"/> with the result of union two bitmaps.</returns>
    /// <remarks>
    /// You must call <see cref="RepairAfterLazy"/> on the resulting bitmap after executing "lazy" computations. <br/>
    /// Lazy operations can be called multiple times in sequence.
    /// </remarks>
    public Roaring32Bitmap LazyOr(Roaring32BitmapBase bitmap, bool bitsetConversion) =>
        new(NativeMethods.roaring_bitmap_lazy_or(Pointer, bitmap.Pointer, bitsetConversion));

    /// <summary>
    /// Creates (in place) a union between the current bitmap and the <paramref name="bitmap"/> given in the parameter using lazy algorithm.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the union will be performed.</param>
    /// <param name="bitsetConversion">Flag which determines whether container-container operations force a bitset conversion.</param>
    /// <remarks>
    /// You must call <see cref="RepairAfterLazy"/> on the resulting bitmap after executing "lazy" computations. <br/>
    /// Lazy operations can be called multiple times in sequence.
    /// </remarks>
    public void ILazyOr(Roaring32BitmapBase bitmap, bool bitsetConversion)
        => NativeMethods.roaring_bitmap_lazy_or_inplace(Pointer, bitmap.Pointer, bitsetConversion);

    /// <summary>
    /// Creates a symmetric difference between the current bitmap and the <paramref name="bitmap"/> given in the parameter.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the symmetric difference will be performed.</param>
    /// <returns><see cref="Roaring32Bitmap"/> with the result of the symmetric difference of two bitmaps.</returns>
    public Roaring32Bitmap Xor(Roaring32BitmapBase bitmap) =>
        new(NativeMethods.roaring_bitmap_xor(Pointer, bitmap.Pointer));

    /// <summary>
    /// Creates (in place) a symmetric difference between the current bitmap and the <paramref name="bitmap"/> given in the parameter.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the symmetric difference will be performed.</param>
    public void IXor(Roaring32BitmapBase bitmap)
        => NativeMethods.roaring_bitmap_xor_inplace(Pointer, bitmap.Pointer);

    /// <summary>
    /// Creates a symmetric difference between the current bitmap and the <paramref name="bitmap"/> given in the parameter and returns the number of values contained in the resulting bitmap. 
    /// </summary>
    /// <param name="bitmap">Bitmap with which the symmetric difference will be performed.</param>
    /// <returns>Number of values contained in the resulting bitmap after symmetric difference.</returns>
    public ulong XorCount(Roaring32BitmapBase bitmap)
        => NativeMethods.roaring_bitmap_xor_cardinality(Pointer, bitmap.Pointer);

    /// <summary>
    /// Creates a symmetric difference between the current bitmap and the <paramref name="bitmaps"/> given in the parameter.
    /// </summary>
    /// <param name="bitmaps">Bitmaps with which the symmetric difference will be performed.</param>
    /// <returns><see cref="Roaring32Bitmap"/> with the result of the symmetric difference of many bitmaps.</returns>
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

    /// <summary>
    /// Creates a symmetric difference between the current bitmap and the <paramref name="bitmap"/> given in the parameter using lazy algorithm.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the symmetric difference will be performed.</param>
    /// <returns><see cref="Roaring32Bitmap"/> with the result of the symmetric difference of two bitmaps.</returns>
    /// <remarks>
    /// You must call <see cref="RepairAfterLazy"/> on the resulting bitmap after executing "lazy" computations. <br/>
    /// Lazy operations can be called multiple times in sequence.
    /// </remarks>
    public Roaring32Bitmap LazyXor(Roaring32BitmapBase bitmap)
        => new(NativeMethods.roaring_bitmap_lazy_xor(Pointer, bitmap.Pointer));

    /// <summary>
    /// Creates (in place) a symmetric difference between the current bitmap and the <paramref name="bitmap"/> given in the parameter using lazy algorithm.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the symmetric difference will be performed.</param>
    /// <remarks>
    /// You must call <see cref="RepairAfterLazy"/> on the resulting bitmap after executing "lazy" computations. <br/>
    /// Lazy operations can be called multiple times in sequence.
    /// </remarks>
    public void ILazyXor(Roaring32BitmapBase bitmap)
        => NativeMethods.roaring_bitmap_lazy_xor_inplace(Pointer, bitmap.Pointer);

    /// <summary>
    /// Checks whether the current bitmaps overlaps (at least one element exists in both bitmaps) the <paramref name="bitmap"/> given in the parameter.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the calculation will be performed.</param>
    /// <returns><c>true</c> if current bitmap overlaps the given bitmap and the bitmaps are not empty; otherwise, <c>false</c>.</returns>
    public bool Overlaps(Roaring32BitmapBase bitmap)
        => NativeMethods.roaring_bitmap_intersect(Pointer, bitmap.Pointer);

    /// <summary>
    /// Checks whether the current bitmaps overlaps (at least one element exists in range) the range given in the parameters.
    /// </summary>
    /// <param name="start">Start of range (inclusive).</param>
    /// <param name="end">End of range (inclusive).</param>
    /// <returns><c>true</c> if current bitmap overlaps the given range; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when arguments have invalid values.</exception>
    public bool OverlapsRange(uint start, uint end)
    {
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, ExceptionMessages.StartValueGreaterThenEndValue);
        }

        return NativeMethods.roaring_bitmap_intersect_with_range(Pointer, start, (ulong)end + 1);
    }

    /// <summary>
    /// Computes the Jaccard index (Tanimoto distance, Jaccard similarity coefficient)
    /// between current bitmap and the <paramref name="bitmap"/> given in the parameter. 
    /// </summary>
    /// <param name="bitmap">Bitmap with which the Jaccard index computation will be performed.</param>
    /// <returns>Value of the Jaccard index.</returns>
    /// <remarks>The Jaccard index is undefined if both bitmaps are empty.</remarks>
    public double GetJaccardIndex(Roaring32BitmapBase bitmap)
        => NativeMethods.roaring_bitmap_jaccard_index(Pointer, bitmap.Pointer);

    /// <summary>
    /// Executes maintenance on the current bitmap after using "*Lazy" methods.
    /// </summary>
    public void RepairAfterLazy()
        => NativeMethods.roaring_bitmap_repair_after_lazy(Pointer);

    /// <summary>
    /// Performs optimizations on the current bitmap.
    /// </summary>
    /// <returns><c>true</c> if the result has at least one run container; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// Converts array and bitmap containers to run containers when it is more
    /// efficient, also converts from run containers when more space efficient. <br/>
    /// Additional savings might be possible by calling <see cref="ShrinkToFit"/>.
    /// </remarks>
    public bool Optimize()
        => NativeMethods.roaring_bitmap_run_optimize(Pointer);

    /// <summary>
    /// Removes run-length encoding even when it is more space efficient.
    /// </summary>
    /// <returns><c>true</c> if remove operation has been performed; otherwise, <c>false</c>.</returns>
    public bool RemoveRunCompression()
        => NativeMethods.roaring_bitmap_remove_run_compression(Pointer);

    /// <summary>
    /// Tries to reallocate memory to reduce the memory usage.
    /// </summary>
    /// <returns>The number of bytes saved after performing the shrink operation.</returns>
    public nuint ShrinkToFit()
        => NativeMethods.roaring_bitmap_shrink_to_fit(Pointer);

    /// <summary>
    /// Writes current bitmap to the <paramref name="buffer"/> given in the parameter.
    /// </summary>
    /// <param name="buffer">The array in which the bitmap will be written.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="buffer"/> size is too small to write the bitmap.</exception>
    public void CopyTo(uint[] buffer)
    {
        if ((ulong)buffer.Length < Count)
        {
            throw new ArgumentOutOfRangeException(nameof(buffer), buffer.Length, ExceptionMessages.BufferSizeIsTooSmall);
        }

        NativeMethods.roaring_bitmap_to_uint32_array(Pointer, buffer);
    }

    /// <summary>
    /// Writes current bitmap to the <paramref name="buffer"/> given in the parameter.
    /// </summary>
    /// <param name="buffer">The <see cref="Memory{T}"/> in which the bitmap will be written.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="buffer"/> size is too small to write the bitmap.</exception>
    public void CopyTo(Memory<uint> buffer) => CopyTo(buffer.Span);

    /// <summary>
    /// Writes current bitmap to the <paramref name="buffer"/> given in the parameter.
    /// </summary>
    /// <param name="buffer">The <see cref="Span{T}"/> in which the bitmap will be written.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="buffer"/> size is too small to write the bitmap.</exception>
    public void CopyTo(Span<uint> buffer)
    {
        if ((ulong)buffer.Length < Count)
        {
            throw new ArgumentOutOfRangeException(nameof(buffer), buffer.Length, ExceptionMessages.BufferSizeIsTooSmall);
        }

        fixed (uint* ptr = &MemoryMarshal.GetReference(buffer))
        {
            NativeMethods.roaring_bitmap_to_uint32_array(Pointer, ptr);
        }
    }

    /// <summary>
    /// Gets enumerator that returns the values contained in the bitmap.
    /// </summary>
    /// <remarks>The values are ordered from smallest to largest.</remarks>
    public IEnumerable<uint> Values => new Roaring32Enumerator(Pointer);

    /// <summary>
    /// Writes current bitmap to the array.
    /// </summary>
    /// <returns>The array containing the values of the bitmap.</returns>
    public uint[] ToArray()
    {
        ulong count = NativeMethods.roaring_bitmap_get_cardinality(Pointer);
        uint[] values = new uint[count];
        NativeMethods.roaring_bitmap_to_uint32_array(Pointer, values);
        return values;
    }

    /// <summary>
    /// Converts current bitmap to the <see cref="FrozenRoaring32Bitmap"/>.
    /// </summary>
    /// <returns>Instance of the <see cref="FrozenRoaring32Bitmap"/> class with the same values as the current bitmap.</returns>
    /// <exception cref="InvalidOperationException">Thrown when unable to allocate bitmap.</exception>
    public FrozenRoaring32Bitmap ToFrozen() => new(this);

    internal Roaring32Bitmap GetFrozenView(nuint size, byte* memoryPtr)
    {
        NativeMethods.roaring_bitmap_frozen_serialize(Pointer, memoryPtr);
        IntPtr ptr = NativeMethods.roaring_bitmap_frozen_view(memoryPtr, size);
        return new Roaring32Bitmap(ptr);
    }

    /// <summary>
    /// Takes the given number of values from the current bitmap and puts them into an array.
    /// </summary>
    /// <param name="count">Number of values to take from the bitmap.</param>
    /// <returns>An array containing the given number of values from the bitmap.</returns>
    /// <remarks>If the bitmap contains fewer values than the given number then the array will be adjusted to the number of values.</remarks>
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

    /// <summary>
    /// Gets statistics about the bitmap.
    /// </summary>
    /// <returns>Structure containing statistics about the bitmap.</returns>
    public Statistics GetStatistics()
    {
        NativeMethods.roaring_bitmap_statistics(Pointer, out Statistics stats);
        return stats;
    }

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
    public bool IsValid() => IsValid(out _);

    /// <summary>
    /// Performs internal consistency checks and returns the cause of inconsistencies.
    /// </summary>
    /// <param name="reason">Reason of inconsistency.</param>
    /// <returns><c>true</c> if the bitmap is consistent; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// It may be useful to call this after deserializing bitmaps from
    /// untrusted sources. If <see cref="IsValid()"/> returns <c>true</c>, then the
    /// bitmap should be consistent and can be trusted not to cause crashes or memory corruption.
    /// </remarks>
    public bool IsValid(out string? reason)
    {
        var result = NativeMethods.roaring_bitmap_internal_validate(Pointer, out IntPtr reasonPtr);
        reason = Marshal.PtrToStringAnsi(reasonPtr);
        return result;
    }

    /// <summary>
    /// Allows you to enable copy-on-write (COW) mode. <br/>
    /// This mode saves memory and avoids copying, but requires more care in a threaded context.
    /// </summary>
    /// <param name="enabled"><c>true</c> if the copy-on-write should be enabled; otherwise, <c>false</c></param>
    /// <remarks>
    /// If you enable this mode, make sure you do it for all your bitmaps,
    /// because interactions between bitmaps with and without COW is unsafe.
    /// </remarks>
    public void SetCopyOnWrite(bool enabled) => NativeMethods.roaring_bitmap_set_copy_on_write(Pointer, enabled);

    /// <summary>
    /// Gets the number of bytes required for a given serialization format.
    /// </summary>
    /// <param name="format">Serialization format for which we get the number of bytes.</param>
    /// <returns>Number of bytes required for the given serialization format.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when serialization format is not supported.</exception>
    public nuint GetSerializationBytes(SerializationFormat format = SerializationFormat.Normal)
        => format switch
        {
            SerializationFormat.Normal => NativeMethods.roaring_bitmap_size_in_bytes(Pointer),
            SerializationFormat.Portable => NativeMethods.roaring_bitmap_portable_size_in_bytes(Pointer),
            SerializationFormat.Frozen => NativeMethods.roaring_bitmap_frozen_size_in_bytes(Pointer),
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, ExceptionMessages.UnsupportedSerializationFormat)
        };

    /// <summary>
    /// Serializes the current bitmap to the given serialization format.
    /// </summary>
    /// <param name="format">The serialization format to which we serialize the bitmap.</param>
    /// <returns>An array that contains a bitmap in a serialized form.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when serialization format is not supported.</exception>
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

    /// <summary>
    /// Deserializes the bitmap from the given serialization format.
    /// </summary>
    /// <param name="buffer">An array that contains a bitmap in a serialized form.</param>
    /// <param name="format">The serialization format from which we deserialize the bitmap.</param>
    /// <returns><see cref="Roaring32Bitmap"/> deserialized from the provided array.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when serialization format is not supported.</exception>
    /// <exception cref="InvalidOperationException">Thrown when unable to allocate bitmap.</exception>
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

    /// <summary>
    /// Deserializes the bitmap from the given serialization format in unsafe mode.
    /// </summary>
    /// <param name="buffer">An array that contains a bitmap in a serialized form.</param>
    /// <param name="format">The serialization format from which we deserialize the bitmap.</param>
    /// <returns><see cref="Roaring32Bitmap"/> deserialized from the provided array.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when serialization format is not supported.</exception>
    /// <exception cref="InvalidOperationException">Thrown when unable to allocate bitmap.</exception>
    /// <remarks>
    /// This method does not check that the input buffer is a valid bitmap and may cause crashes or memory corruption.
    /// </remarks>
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

    /// <summary>
    /// Checks how many bytes will be read from the array to deserialize the bitmap.
    /// </summary>
    /// <param name="buffer">An array that contains a bitmap in a serialized form.</param>
    /// <param name="expectedSize">The expected number of bytes after which the check will be aborted.</param>
    /// <param name="format">The serialization format for which we check the number of bytes.</param>
    /// <returns><c>0</c> if the bitmap is invalid; otherwise, the number of bytes required to deserialize the bitmap.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when serialization format is not supported.</exception>
    public static nuint GetSerializedSize(byte[] buffer, nuint expectedSize, SerializationFormat format = SerializationFormat.Portable)
    {
        nuint size = format switch
        {
            SerializationFormat.Portable => NativeMethods.roaring_bitmap_portable_deserialize_size(buffer, expectedSize),
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, ExceptionMessages.UnsupportedSerializationFormat)
        };

        return size;
    }
}