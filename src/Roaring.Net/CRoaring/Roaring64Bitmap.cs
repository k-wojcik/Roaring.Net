using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Roaring.Net.CRoaring;

/// <summary>
/// Represents a 64-bit CRoaring bitmap.
/// </summary>
public unsafe class Roaring64Bitmap : Roaring64BitmapBase, IReadOnlyRoaring64Bitmap
{
    private bool _isDisposed;

    /// <summary>
    /// Gets the number of elements (cardinality) contained in the <see cref="Roaring64Bitmap"/>.
    /// </summary>
    /// <returns>The number of elements contained in the <see cref="Roaring64Bitmap"/>.</returns>
    public ulong Count => NativeMethods.roaring64_bitmap_get_cardinality(Pointer);

    /// <summary>
    /// Gets a value indicating that <see cref="Roaring64Bitmap"/> is empty (cardinality is zero).
    /// </summary>
    /// <returns><c>true</c> if <see cref="Roaring64Bitmap"/> is empty (cardinality is zero); otherwise, <c>false</c>.</returns>
    public bool IsEmpty => NativeMethods.roaring64_bitmap_is_empty(Pointer);

    /// <summary>
    /// Gets the minimum value in the <see cref="Roaring64Bitmap"/>.
    /// </summary>
    /// <returns>The minimum value in the <see cref="Roaring64Bitmap"/> or <c>null</c> when the bitmap is empty.</returns>
    public ulong? Min => IsEmpty ? null : NativeMethods.roaring64_bitmap_minimum(Pointer);

    /// <summary>
    /// Gets the maximum value in the <see cref="Roaring64Bitmap"/>.
    /// </summary>
    /// <returns>The maximum value in the <see cref="Roaring64Bitmap"/> or <see langword="null"/> when the bitmap is empty.</returns>
    public ulong? Max => IsEmpty ? null : NativeMethods.roaring64_bitmap_maximum(Pointer);

    /// <summary>
    /// Initializes a new instance of the <see cref="Roaring64Bitmap"/> class.
    /// </summary>
    /// <remarks>Initializes an empty bitmap.</remarks>
    /// <exception cref="InvalidOperationException">Thrown when unable to allocate bitmap.</exception>
    public Roaring64Bitmap() => Pointer = CheckBitmapPointer(NativeMethods.roaring64_bitmap_create());

    /// <summary>
    /// Initializes a new instance of the <see cref="Roaring64Bitmap"/> class with the given values.
    /// </summary>
    /// <param name="values">Values that will be added to the bitmap directly when it is created.</param>
    /// <exception cref="InvalidOperationException">Thrown when unable to allocate bitmap.</exception>
    public Roaring64Bitmap(ulong[] values) => Pointer = CheckBitmapPointer(CreatePtrFromValues(values, 0, (uint)values.Length));

    internal Roaring64Bitmap(IntPtr pointer) => Pointer = CheckBitmapPointer(pointer);

    private static IntPtr CheckBitmapPointer(IntPtr pointer)
    {
        if (pointer == IntPtr.Zero)
        {
            throw new InvalidOperationException(ExceptionMessages.UnableToAllocateBitmap);
        }

        return pointer;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Roaring64Bitmap"/> class for the given range.
    /// </summary>
    /// <param name="start">Start of range (inclusive).</param>
    /// <param name="end">End of range (inclusive).</param>
    /// <param name="step">Step between values (i * stop from minimum value).</param>
    /// <returns>Instance of the <see cref="Roaring64Bitmap"/> class for the given range.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when arguments have invalid values.</exception>
    /// <exception cref="InvalidOperationException">Thrown when unable to allocate bitmap.</exception>
    public static Roaring64Bitmap FromRange(ulong start, ulong end, ulong step = 1)
    {
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, ExceptionMessages.StartValueGreaterThenEndValue);
        }

        if (step == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(step), step, ExceptionMessages.StepEqualToZero);
        }

        Roaring64Bitmap bitmap;
        if (start != ulong.MaxValue)
        {
            ulong inclusiveEnd = end;
            if (end != ulong.MaxValue)
            {
                inclusiveEnd += 1;
            }

            bitmap = new Roaring64Bitmap(NativeMethods.roaring64_bitmap_from_range(start, inclusiveEnd, step));

            if (end == ulong.MaxValue && (end - start) % step == 0)
            {
                bitmap.Add(ulong.MaxValue);
            }
        }
        else
        {
            bitmap = FromValues(new[] { ulong.MaxValue });
        }

        return bitmap;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Roaring64Bitmap"/> class with the given values.
    /// </summary>
    /// <param name="values">Values that will be added to the bitmap directly when it is created.</param>
    /// <returns>Instance of the <see cref="Roaring64Bitmap"/> class with the given values.</returns>
    /// <exception cref="InvalidOperationException">Thrown when unable to allocate bitmap.</exception>
    public static Roaring64Bitmap FromValues(ulong[] values) => FromValues(values, 0U, (nuint)values.Length);

    /// <summary>
    /// Initializes a new instance of the <see cref="Roaring64Bitmap"/> class with the given values from subarray.
    /// </summary>
    /// <param name="values">An array containing the values to add.</param>
    /// <param name="offset">The position in the array from which to start adding data.</param>
    /// <param name="count">The number of values to add from the offset position.</param>
    /// <returns>Instance of the <see cref="Roaring64Bitmap"/> class with the given values from subarray.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when arguments have invalid values.</exception>
    /// <exception cref="InvalidOperationException">Thrown when unable to allocate bitmap.</exception>
    public static Roaring64Bitmap FromValues(ulong[] values, nuint offset, nuint count)
        => new(CreatePtrFromValues(values, offset, count));

    private static IntPtr CreatePtrFromValues(ulong[] values, nuint offset, nuint count)
    {
        if ((nuint)values.Length < offset + count)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), offset, ExceptionMessages.OffsetWithCountGreaterThanNumberOfValues);
        }

        fixed (ulong* valuePtr = values)
        {
            return NativeMethods.roaring64_bitmap_of_ptr(count, valuePtr + offset);
        }
    }

    /// <summary>
    ///  Initializes a new instance of the <see cref="Roaring64Bitmap"/> class with the values from CRoaring 32-bit bitmap.
    /// </summary>
    /// <param name="bitmap">CRoaring 32-bit bitmap.</param>
    /// <returns>Instance of the <see cref="Roaring64Bitmap"/> class with the values from given <paramref name="bitmap"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown when unable to allocate bitmap.</exception>
    public static Roaring64Bitmap FromBitmap(Roaring32BitmapBase bitmap)
        => new(NativeMethods.roaring64_bitmap_move_from_roaring32(bitmap.Pointer));

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        NativeMethods.roaring64_bitmap_free(Pointer);
        _isDisposed = true;
    }

    /// <inheritdoc />
    ~Roaring64Bitmap() => Dispose(false);

    /// <summary>
    /// Copies the bitmap.
    /// </summary>
    /// <returns>Instance of the <see cref="Roaring64Bitmap"/> class with the same values as the current bitmap.</returns>
    /// <exception cref="InvalidOperationException">Thrown when unable to allocate bitmap.</exception>
    public Roaring64Bitmap Clone() => new(NativeMethods.roaring64_bitmap_copy(Pointer));

    /// <summary>
    /// Adds a value to the bitmap.
    /// </summary>
    /// <param name="value">A value that will be added to the bitmap.</param>
    public void Add(ulong value) => NativeMethods.roaring64_bitmap_add(Pointer, value);

    /// <summary>
    /// Adds values from the given array to the bitmap.
    /// </summary>
    /// <param name="values">An array containing the values to add.</param>
    public void AddMany(ulong[] values) => AddMany(values, 0, (nuint)values.Length);

    /// <summary>
    /// Adds values from the given subarray to the bitmap.
    /// </summary>
    /// <param name="values">An array containing the values to add.</param>
    /// <param name="offset">The position in the array from which to start adding data.</param>
    /// <param name="count">The number of values to add from the offset position.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when arguments have invalid values.</exception>
    public void AddMany(ulong[] values, nuint offset, nuint count)
    {
        if ((nuint)values.Length < offset + count)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), offset, ExceptionMessages.OffsetWithCountGreaterThanNumberOfValues);
        }

        fixed (ulong* valuePtr = values)
        {
            NativeMethods.roaring64_bitmap_add_many(Pointer, count, valuePtr + offset);
        }
    }

    /// <summary>
    /// Tries to add a value to the bitmap.
    /// </summary>
    /// <param name="value">A value that will be added to the bitmap.</param>
    /// <returns><c>true</c> if a new value does not exist in the bitmap and has been added; otherwise, <c>false</c>.</returns>
    public bool TryAdd(ulong value) => NativeMethods.roaring64_bitmap_add_checked(Pointer, value);

    /// <summary>
    /// Adds values from the given range.
    /// </summary>
    /// <param name="start">Start of range (inclusive).</param>
    /// <param name="end">End of range (inclusive).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when arguments have invalid values.</exception>
    public void AddRange(ulong start, ulong end)
    {
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, ExceptionMessages.StartValueGreaterThenEndValue);
        }

        NativeMethods.roaring64_bitmap_add_range_closed(Pointer, start, end);
    }

    /// <summary>
    /// Adds value to the bitmap using context from a previous bulk operation to optimize the addition process.
    /// </summary>
    /// <param name="context">A context that stores information between `*Bulk` method calls.</param>
    /// <param name="value">A value that will be added to the bitmap.</param>
    /// <exception cref="ArgumentException">Thrown when context belongs to another bitmap.</exception>
    public void AddBulk(BulkContext64 context, ulong value)
    {
        if (context.Bitmap != this)
        {
            throw new ArgumentException(ExceptionMessages.BulkContextBelongsToOtherBitmap, nameof(context));
        }

        NativeMethods.roaring64_bitmap_add_bulk(Pointer, context.Pointer, value);
    }

    /// <summary>
    /// Removes a value from the bitmap.
    /// </summary>
    /// <param name="value">A value that will be removed from the bitmap.</param>
    public void Remove(ulong value) => NativeMethods.roaring64_bitmap_remove(Pointer, value);

    /// <summary>
    /// Removes the values contained in the given array from the bitmap.
    /// </summary>
    /// <param name="values">An array containing the values to remove.</param>
    public void RemoveMany(ulong[] values) => RemoveMany(values, 0, (nuint)values.Length);

    /// <summary>
    /// Removes the values contained in the given subarray from the bitmap.
    /// </summary>
    /// <param name="values">An array containing the values to remove.</param>
    /// <param name="offset">The position in the array from which to start removing data.</param>
    /// <param name="count">The number of values to remove from the offset position.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when arguments have invalid values.</exception>
    public void RemoveMany(ulong[] values, nuint offset, nuint count)
    {
        if ((nuint)values.Length < offset + count)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), offset, ExceptionMessages.OffsetWithCountGreaterThanNumberOfValues);
        }

        fixed (ulong* valuePtr = values)
        {
            NativeMethods.roaring64_bitmap_remove_many(Pointer, count, valuePtr + offset);
        }
    }

    /// <summary>
    /// Tries to remove a value from the bitmap.
    /// </summary>
    /// <param name="value">A value that will be removed from the bitmap.</param>
    /// <returns><c>true</c> if a value exists in the bitmap and has been removed; otherwise, <c>false</c>.</returns>
    public bool TryRemove(ulong value) => NativeMethods.roaring64_bitmap_remove_checked(Pointer, value);

    /// <summary>
    /// Removes values from the given range.
    /// </summary>
    /// <param name="start">Start of range (inclusive).</param>
    /// <param name="end">End of range (inclusive).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when arguments have invalid values.</exception>
    public void RemoveRange(ulong start, ulong end)
    {
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, ExceptionMessages.StartValueGreaterThenEndValue);
        }

        NativeMethods.roaring64_bitmap_remove_range_closed(Pointer, start, end);
    }

    /// <summary>
    /// Removes value from the bitmap using context from a previous bulk operation to optimize the addition process.
    /// </summary>
    /// <param name="context">A context that stores information between `*Bulk` method calls.</param>
    /// <param name="value">A value that will be removed from the bitmap.</param>
    /// <exception cref="ArgumentException">Thrown when context belongs to another bitmap.</exception>
    public void RemoveBulk(BulkContext64 context, ulong value)
    {
        if (context.Bitmap != this)
        {
            throw new ArgumentException(ExceptionMessages.BulkContextBelongsToOtherBitmap, nameof(context));
        }

        NativeMethods.roaring64_bitmap_remove_bulk(Pointer, context.Pointer, value);
    }


    /// <summary>
    /// Removes all values from the bitmap.
    /// </summary>
    public void Clear() => NativeMethods.roaring64_bitmap_clear(Pointer);

    /// <summary>
    /// Checks if a value is present in the bitmap.
    /// </summary>
    /// <param name="value">A value for which the check will be performed.</param>
    /// <returns><c>true</c> if a value exists in the bitmap; otherwise, <c>false</c>.</returns>
    public bool Contains(ulong value) => NativeMethods.roaring64_bitmap_contains(Pointer, value);

    /// <summary>
    /// Checks if the values for the given range are present in the bitmap.
    /// </summary>
    /// <param name="start">Start of range (inclusive).</param>
    /// <param name="end">End of range (inclusive).</param>
    /// <returns><c>true</c> if all values from the given range exist in the bitmap; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when arguments have invalid values.</exception>
    public bool ContainsRange(ulong start, ulong end)
    {
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, ExceptionMessages.StartValueGreaterThenEndValue);
        }

        return NativeMethods.roaring64_bitmap_contains_range(Pointer, start, end)
               && (end != ulong.MaxValue || NativeMethods.roaring64_bitmap_contains(Pointer, end));
    }

    /// <summary>
    /// Checks if a value is present in the bitmap using context from a previous bulk operation to optimize the checking process.
    /// </summary>
    /// <param name="context">A context that stores information between `*Bulk` method calls.</param>
    /// <param name="value">A value for which the check will be performed.</param>
    /// <exception cref="ArgumentException">Thrown when context belongs to another bitmap.</exception>
    public bool ContainsBulk(BulkContext64 context, ulong value)
    {
        if (context.Bitmap != this)
        {
            throw new ArgumentException(ExceptionMessages.BulkContextBelongsToOtherBitmap, nameof(context));
        }

        return NativeMethods.roaring64_bitmap_contains_bulk(Pointer, context.Pointer, value);
    }

    /// <summary>
    /// Compares the equality of bitmaps based on the values they contain.
    /// </summary>
    /// <param name="bitmap">Bitmap with which equality will be compared.</param>
    /// <returns><c>true</c> if both bitmaps have the same values; otherwise, <c>false</c>.</returns>
    public bool ValueEquals(Roaring64BitmapBase? bitmap)
    {
        if (bitmap == null)
        {
            return false;
        }

        return NativeMethods.roaring64_bitmap_equals(Pointer, bitmap.Pointer);
    }

    /// <summary>
    /// Checks if the current bitmap is a subset of the <paramref name="bitmap"/>. 
    /// </summary>
    /// <param name="bitmap"></param>
    /// <returns><c>true</c> if current bitmaps is a subset of the <paramref name="bitmap"/>; otherwise, <c>false</c>.</returns>
    public bool IsSubsetOf(Roaring64BitmapBase? bitmap)
    {
        if (bitmap == null)
        {
            return false;
        }

        return NativeMethods.roaring64_bitmap_is_subset(Pointer, bitmap.Pointer);
    }

    /// <summary>
    /// Checks if the current bitmap is a proper subset of the <paramref name="bitmap"/>. 
    /// </summary>
    /// <param name="bitmap"></param>
    /// <returns><c>true</c> if current bitmaps is a proper subset of the <paramref name="bitmap"/>; otherwise, <c>false</c>.</returns>
    public bool IsProperSubsetOf(Roaring64BitmapBase? bitmap)
    {
        if (bitmap == null)
        {
            return false;
        }

        return NativeMethods.roaring64_bitmap_is_strict_subset(Pointer, bitmap.Pointer);
    }

    /// <summary>
    /// Checks if the current bitmap is a superset of the <paramref name="bitmap"/>. 
    /// </summary>
    /// <param name="bitmap"></param>
    /// <returns><c>true</c> if current bitmaps is a superset of the <paramref name="bitmap"/>; otherwise, <c>false</c>.</returns>
    public bool IsSupersetOf(Roaring64BitmapBase? bitmap)
    {
        if (bitmap == null)
        {
            return false;
        }

        if (NativeMethods.roaring64_bitmap_get_cardinality(bitmap.Pointer) >
            NativeMethods.roaring64_bitmap_get_cardinality(Pointer))
        {
            return false;
        }

        return NativeMethods.roaring64_bitmap_is_subset(bitmap.Pointer, Pointer);
    }

    /// <summary>
    /// Checks if the current bitmap is a proper superset of the <paramref name="bitmap"/>. 
    /// </summary>
    /// <param name="bitmap"></param>
    /// <returns><c>true</c> if current bitmaps is a proper superset of the <paramref name="bitmap"/>; otherwise, <c>false</c>.</returns>
    public bool IsProperSupersetOf(Roaring64BitmapBase? bitmap)
    {
        if (bitmap == null)
        {
            return false;
        }

        if (NativeMethods.roaring64_bitmap_get_cardinality(bitmap.Pointer) >=
            NativeMethods.roaring64_bitmap_get_cardinality(Pointer))
        {
            return false;
        }

        return NativeMethods.roaring64_bitmap_is_subset(bitmap.Pointer, Pointer);
    }

    /// <summary>
    /// Tries to get a value located at the given <paramref name="index"/> (rank).
    /// </summary>
    /// <param name="index">The index for which the value will be retrieved. Index values start from 0.</param>
    /// <param name="value">Retrieved value. <c>0</c> if value does not exist in the bitmap.</param>
    /// <returns><c>true</c> if a value exists in the bitmap; otherwise, <c>false</c>.</returns>
    public bool TryGetValue(ulong index, out ulong value) => NativeMethods.roaring64_bitmap_select(Pointer, index, out value);

    /// <summary>
    /// Gets the index (rank) for the given value.
    /// </summary>
    /// <param name="value">The value for which the index will be retrieved.</param>
    /// <param name="index">Index (rank) of the <paramref name="value"/>. <c>0</c> if value does not exist in the bitmap.</param>
    /// <returns><c>true</c> if index for <paramref name="value"/> exists in the bitmap; otherwise, <c>false</c>.</returns>
    public bool TryGetIndex(ulong value, out ulong index) => NativeMethods.roaring64_bitmap_get_index(Pointer, value, out index);

    /// <summary>
    /// Counts number of values less than or equal to <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value for which data will be counted.</param>
    /// <returns>The number of values that are less than or equal to the <paramref name="value"/>.</returns>
    public ulong CountLessOrEqualTo(ulong value) => NativeMethods.roaring64_bitmap_rank(Pointer, value);

    /// <summary>
    /// Counts number of values in the given range of values.
    /// </summary>
    /// <param name="start">Start of range (inclusive).</param>
    /// <param name="end">End of range (inclusive).</param>
    /// <returns>The number of values in the given range of values.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when arguments have invalid values.</exception>
    public ulong CountRange(ulong start, ulong end)
    {
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, ExceptionMessages.StartValueGreaterThenEndValue);
        }

        return NativeMethods.roaring64_bitmap_range_closed_cardinality(Pointer, start, end);
    }

    /* unavailable due to CRoaring performance issues
    
    /// <summary>
    /// Creates a new negated bitmap based on the values in the current bitmap. 
    /// </summary>
    /// <returns>Instance of the <see cref="Roaring64Bitmap"/> class with negated values of current bitmap.</returns>
    public Roaring64Bitmap Not()
        => new(NativeMethods.roaring64_bitmap_flip_closed(Pointer, ulong.MinValue, ulong.MaxValue));

    /// <summary>
    /// Negates all values in the current bitmap. 
    /// </summary>
    public void INot() => NativeMethods.roaring64_bitmap_flip_closed_inplace(Pointer, ulong.MinValue, ulong.MaxValue);

    */

    /// <summary>
    /// Creates a new negated bitmap based on the values in the current bitmap for the given range of values. 
    /// </summary>
    /// <param name="start">Start of range (inclusive).</param>
    /// <param name="end">End of range (inclusive).</param>
    /// <returns>Instance of the <see cref="Roaring64Bitmap"/> class with negated values of current bitmap in the given range of values.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when arguments have invalid values.</exception>
    /// <remarks>Values outside the range are left unchanged.</remarks>
    public Roaring64Bitmap NotRange(ulong start, ulong end)
    {
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, ExceptionMessages.StartValueGreaterThenEndValue);
        }

        return new(NativeMethods.roaring64_bitmap_flip_closed(Pointer, start, end));
    }

    /// <summary>
    /// Negates values in the current bitmap for the given range of values. 
    /// </summary>
    /// <param name="start">Start of range (inclusive).</param>
    /// <param name="end">End of range (inclusive).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when arguments have invalid values.</exception>
    /// <remarks>Values outside the range are left unchanged.</remarks>
    public void INotRange(ulong start, ulong end)
    {
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, ExceptionMessages.StartValueGreaterThenEndValue);
        }

        NativeMethods.roaring64_bitmap_flip_closed_inplace(Pointer, start, end);
    }

    /// <summary>
    /// Creates an intersection between the current bitmap and the <paramref name="bitmap"/> given in the parameter.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the intersection will be performed.</param>
    /// <returns><see cref="Roaring64Bitmap"/> with the result of intersection of two bitmaps.</returns>
    /// <remarks>
    /// Performance hints:
    /// <list type="bullet">
    /// <item>if you are computing the intersection between several bitmaps, two-by-two, it is best to start with the smallest bitmap,</item>
    /// <item>you may also rely on <see cref="IAnd"/> to avoid creating many temporary bitmaps.</item>
    /// </list>
    /// </remarks>
    public Roaring64Bitmap And(Roaring64BitmapBase bitmap) =>
        new(NativeMethods.roaring64_bitmap_and(Pointer, bitmap.Pointer));

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
    public void IAnd(Roaring64BitmapBase bitmap)
        => NativeMethods.roaring64_bitmap_and_inplace(Pointer, bitmap.Pointer);

    /// <summary>
    /// Intersects the current bitmap with the <paramref name="bitmap"/> given in the parameter and returns the number of values contained in the resulting bitmap.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the intersection will be performed.</param>
    /// <returns>Number of values contained in the resulting bitmap after intersection.</returns>
    public ulong AndCount(Roaring64BitmapBase bitmap)
        => NativeMethods.roaring64_bitmap_and_cardinality(Pointer, bitmap.Pointer);

    /// <summary>
    /// Creates a difference between the current bitmap and the <paramref name="bitmap"/> given in the parameter.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the difference will be performed.</param>
    /// <returns><see cref="Roaring64Bitmap"/> with the result of the difference of two bitmaps.</returns>
    public Roaring64Bitmap AndNot(Roaring64BitmapBase bitmap) =>
        new(NativeMethods.roaring64_bitmap_andnot(Pointer, bitmap.Pointer));

    /// <summary>
    /// Creates (in place) a difference between the current bitmap with the <paramref name="bitmap"/> given in the parameter.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the difference will be performed.</param>
    public void IAndNot(Roaring64BitmapBase bitmap)
        => NativeMethods.roaring64_bitmap_andnot_inplace(Pointer, bitmap.Pointer);

    /// <summary>
    /// Creates a difference between the current bitmap and the <paramref name="bitmap"/> given in the parameter and returns the number of values contained in the resulting bitmap.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the difference will be performed.</param>
    /// <returns>Number of values contained in the resulting bitmap after difference.</returns>
    public ulong AndNotCount(Roaring64BitmapBase bitmap)
        => NativeMethods.roaring64_bitmap_andnot_cardinality(Pointer, bitmap.Pointer);

    /// <summary>
    /// Creates a union between the current bitmap and the <paramref name="bitmap"/> given in the parameter.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the union will be performed.</param>
    /// <returns><see cref="Roaring64Bitmap"/> with the result of union two bitmaps.</returns>
    public Roaring64Bitmap Or(Roaring64BitmapBase bitmap) =>
        new(NativeMethods.roaring64_bitmap_or(Pointer, bitmap.Pointer));

    /// <summary>
    /// Creates a union between the current bitmap and the <paramref name="bitmap"/> given in the parameter and returns the number of values contained in the resulting bitmap.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the union will be performed.</param>
    /// <returns>Number of values contained in the resulting bitmap after union.</returns>
    public ulong OrCount(Roaring64BitmapBase bitmap)
        => NativeMethods.roaring64_bitmap_or_cardinality(Pointer, bitmap.Pointer);

    /// <summary>
    /// Creates (in place) a union between the current bitmap and the <paramref name="bitmap"/> given in the parameter.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the union will be performed.</param>
    public void IOr(Roaring64BitmapBase bitmap)
        => NativeMethods.roaring64_bitmap_or_inplace(Pointer, bitmap.Pointer);

    /// <summary>
    /// Creates a symmetric difference between the current bitmap and the <paramref name="bitmap"/> given in the parameter.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the symmetric difference will be performed.</param>
    /// <returns><see cref="Roaring64Bitmap"/> with the result of the symmetric difference of two bitmaps.</returns>
    public Roaring64Bitmap Xor(Roaring64BitmapBase bitmap) =>
        new(NativeMethods.roaring64_bitmap_xor(Pointer, bitmap.Pointer));

    /// <summary>
    /// Creates (in place) a symmetric difference between the current bitmap and the <paramref name="bitmap"/> given in the parameter.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the symmetric difference will be performed.</param>
    public void IXor(Roaring64BitmapBase bitmap)
        => NativeMethods.roaring64_bitmap_xor_inplace(Pointer, bitmap.Pointer);

    /// <summary>
    /// Creates a symmetric difference between the current bitmap and the <paramref name="bitmap"/> given in the parameter and returns the number of values contained in the resulting bitmap. 
    /// </summary>
    /// <param name="bitmap">Bitmap with which the symmetric difference will be performed.</param>
    /// <returns>Number of values contained in the resulting bitmap after symmetric difference.</returns>
    public ulong XorCount(Roaring64BitmapBase bitmap)
        => NativeMethods.roaring64_bitmap_xor_cardinality(Pointer, bitmap.Pointer);

    /// <summary>
    /// Checks whether the current bitmaps overlaps (at least one element exists in both bitmaps) the <paramref name="bitmap"/> given in the parameter.
    /// </summary>
    /// <param name="bitmap">Bitmap with which the calculation will be performed.</param>
    /// <returns><c>true</c> if current bitmap overlaps the given bitmap and the bitmaps are not empty; otherwise, <c>false</c>.</returns>
    public bool Overlaps(Roaring64BitmapBase bitmap)
        => NativeMethods.roaring64_bitmap_intersect(Pointer, bitmap.Pointer);

    /// <summary>
    /// Checks whether the current bitmaps overlaps (at least one element exists in range) the range given in the parameters.
    /// </summary>
    /// <param name="start">Start of range (inclusive).</param>
    /// <param name="end">End of range (inclusive).</param>
    /// <returns><c>true</c> if current bitmap overlaps the given range; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when arguments have invalid values.</exception>
    public bool OverlapsRange(ulong start, ulong end)
    {
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, ExceptionMessages.StartValueGreaterThenEndValue);
        }

        bool overlaps;
        if (start != ulong.MaxValue)
        {
            ulong endInclusive = end;
            if (end != ulong.MaxValue)
            {
                endInclusive += 1;
            }

            overlaps = NativeMethods.roaring64_bitmap_intersect_with_range(Pointer, start, endInclusive)
                       || (end == ulong.MaxValue && NativeMethods.roaring64_bitmap_contains(Pointer, end));
        }
        else
        {
            overlaps = NativeMethods.roaring64_bitmap_contains(Pointer, start);
        }

        return overlaps;
    }

    /// <summary>
    /// Computes the Jaccard index (Tanimoto distance, Jaccard similarity coefficient)
    /// between current bitmap and the <paramref name="bitmap"/> given in the parameter. 
    /// </summary>
    /// <param name="bitmap">Bitmap with which the Jaccard index computation will be performed.</param>
    /// <returns>Value of the Jaccard index.</returns>
    /// <remarks>The Jaccard index is undefined if both bitmaps are empty.</remarks>
    public double GetJaccardIndex(Roaring64BitmapBase bitmap)
        => NativeMethods.roaring64_bitmap_jaccard_index(Pointer, bitmap.Pointer);

    /// <summary>
    /// Performs optimizations on the current bitmap.
    /// </summary>
    /// <returns><c>true</c> if the result has at least one run container; otherwise, <c>false</c>.</returns>
    public bool Optimize()
        => NativeMethods.roaring64_bitmap_run_optimize(Pointer);

    /// <summary>
    /// Tries to reallocate memory to reduce the memory usage.
    /// </summary>
    /// <returns>The number of bytes saved after performing the shrink operation.</returns>
    public nuint ShrinkToFit()
        => NativeMethods.roaring64_bitmap_shrink_to_fit(Pointer);

    /// <summary>
    /// Writes current bitmap to the <paramref name="buffer"/> given in the parameter.
    /// </summary>
    /// <param name="buffer">The array in which the bitmap will be written.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="buffer"/> size is too small to write the bitmap.</exception>
    public void CopyTo(ulong[] buffer)
    {
        if ((ulong)buffer.LongLength < Count)
        {
            throw new ArgumentOutOfRangeException(nameof(buffer), buffer.Length, ExceptionMessages.BufferSizeIsTooSmall);
        }

        NativeMethods.roaring64_bitmap_to_uint64_array(Pointer, buffer);
    }

    /// <summary>
    /// Writes current bitmap to the <paramref name="buffer"/> given in the parameter.
    /// </summary>
    /// <param name="buffer">The <see cref="Memory{T}"/> in which the bitmap will be written.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="buffer"/> size is too small to write the bitmap.</exception>
    public void CopyTo(Memory<ulong> buffer) => CopyTo(buffer.Span);

    /// <summary>
    /// Writes current bitmap to the <paramref name="buffer"/> given in the parameter.
    /// </summary>
    /// <param name="buffer">The <see cref="Span{T}"/> in which the bitmap will be written.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="buffer"/> size is too small to write the bitmap.</exception>
    public void CopyTo(Span<ulong> buffer)
    {
        if ((ulong)buffer.Length < Count)
        {
            throw new ArgumentOutOfRangeException(nameof(buffer), buffer.Length, ExceptionMessages.BufferSizeIsTooSmall);
        }

        fixed (ulong* ptr = &MemoryMarshal.GetReference(buffer))
        {
            NativeMethods.roaring64_bitmap_to_uint64_array(Pointer, ptr);
        }
    }

    /// <summary>
    /// Gets enumerator that returns the values contained in the bitmap.
    /// </summary>
    /// <remarks>The values are ordered from smallest to largest.</remarks>
    public IEnumerable<ulong> Values => new Roaring64Enumerator(Pointer);

    /// <summary>
    /// Writes current bitmap to the array.
    /// </summary>
    /// <returns>The array containing the values of the bitmap.</returns>
    public ulong[] ToArray()
    {
        ulong count = NativeMethods.roaring64_bitmap_get_cardinality(Pointer);
        ulong[] values = new ulong[count];
        NativeMethods.roaring64_bitmap_to_uint64_array(Pointer, values);
        return values;
    }

    /// <summary>
    /// Takes the given number of values from the current bitmap and puts them into an array.
    /// </summary>
    /// <param name="count">Number of values to take from the bitmap.</param>
    /// <returns>An array containing the given number of values from the bitmap.</returns>
    /// <remarks>If the bitmap contains fewer values than the given number then the array will be adjusted to the number of values.</remarks>
    public ulong[] Take(ulong count)
    {
        ulong cardinality = NativeMethods.roaring64_bitmap_get_cardinality(Pointer);
        if (cardinality < count)
        {
            count = cardinality;
        }

        ulong[] values = new ulong[count];
        NativeMethods.roaring64_bitmap_to_uint64_array(Pointer, values);
        return values;
    }

    /// <summary>
    /// Gets statistics about the bitmap.
    /// </summary>
    /// <returns>Structure containing statistics about the bitmap.</returns>
    public Statistics64 GetStatistics()
    {
        NativeMethods.roaring64_bitmap_statistics(Pointer, out Statistics64 stats);
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
        var result = NativeMethods.roaring64_bitmap_internal_validate(Pointer, out IntPtr reasonPtr);
        reason = Marshal.PtrToStringAnsi(reasonPtr);
        return result;
    }

    /// <summary>
    /// Gets the number of bytes required for a given serialization format.
    /// </summary>
    /// <param name="format">Serialization format for which we get the number of bytes.</param>
    /// <returns>Number of bytes required for the given serialization format.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when serialization format is not supported.</exception>
    public nuint GetSerializationBytes(SerializationFormat format = SerializationFormat.Portable)
    {
        switch (format)
        {
            case SerializationFormat.Portable:
                return NativeMethods.roaring64_bitmap_portable_size_in_bytes(Pointer);
            case SerializationFormat.Frozen:
                ShrinkToFit(); // CRoaring requires shrink_to_fit before frozen operations
                return NativeMethods.roaring64_bitmap_frozen_size_in_bytes(Pointer);
            default:
                throw new ArgumentOutOfRangeException(nameof(format), format,
                    ExceptionMessages.UnsupportedSerializationFormat);
        }
    }

    /// <summary>
    /// Serializes the current bitmap to the given serialization format.
    /// </summary>
    /// <param name="format">The serialization format to which we serialize the bitmap.</param>
    /// <returns>An array that contains a bitmap in a serialized form.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when serialization format is not supported.</exception>
    public byte[] Serialize(SerializationFormat format = SerializationFormat.Portable)
    {
        byte[] buffer;

        switch (format)
        {
            case SerializationFormat.Portable:
                buffer = new byte[NativeMethods.roaring64_bitmap_portable_size_in_bytes(Pointer)];
                fixed (byte* bufferPtr = buffer)
                {
                    NativeMethods.roaring64_bitmap_portable_serialize(Pointer, bufferPtr);
                }
                break;
            case SerializationFormat.Frozen:
                ShrinkToFit(); // CRoaring requires shrink_to_fit before frozen operations
                buffer = new byte[NativeMethods.roaring64_bitmap_frozen_size_in_bytes(Pointer)];
                fixed (byte* bufferPtr = buffer)
                {
                    NativeMethods.roaring64_bitmap_frozen_serialize(Pointer, bufferPtr);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(format), format,
                    ExceptionMessages.UnsupportedSerializationFormat);
        }

        return buffer;
    }

    /// <summary>
    /// Deserializes the bitmap from the given serialization format.
    /// </summary>
    /// <param name="buffer">An array that contains a bitmap in a serialized form.</param>
    /// <param name="format">The serialization format from which we deserialize the bitmap.</param>
    /// <returns><see cref="Roaring64Bitmap"/> deserialized from the provided array.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when serialization format is not supported.</exception>
    /// <exception cref="InvalidOperationException">Thrown when unable to allocate bitmap.</exception>
    public static Roaring64Bitmap Deserialize(byte[] buffer, SerializationFormat format = SerializationFormat.Portable)
    {
        IntPtr ptr = format switch
        {
            SerializationFormat.Portable => NativeMethods.roaring64_bitmap_portable_deserialize_safe(buffer, (nuint)buffer.Length),
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, ExceptionMessages.UnsupportedSerializationFormat)
        };

        if (ptr == IntPtr.Zero)
        {
            throw new InvalidOperationException(ExceptionMessages.DeserializationFailedUnknownReason);
        }

        return new Roaring64Bitmap(ptr);
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
            SerializationFormat.Portable => NativeMethods.roaring64_bitmap_portable_deserialize_size(buffer, expectedSize),
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, ExceptionMessages.UnsupportedSerializationFormat)
        };

        return size;
    }

    /// <summary>
    /// Converts current bitmap to the <see cref="FrozenRoaring32Bitmap"/>.
    /// </summary>
    /// <returns>Instance of the <see cref="FrozenRoaring32Bitmap"/> class with the same values as the current bitmap.</returns>
    /// <exception cref="InvalidOperationException">Thrown when unable to allocate bitmap.</exception>
    public FrozenRoaring64Bitmap ToFrozen() => new(this);

    internal Roaring64Bitmap GetFrozenView(nuint size, byte* memoryPtr)
    {
        ShrinkToFit(); // CRoaring requires shrink_to_fit before frozen operations
        NativeMethods.roaring64_bitmap_frozen_serialize(Pointer, memoryPtr);
        IntPtr ptr = NativeMethods.roaring64_bitmap_frozen_view(memoryPtr, size);
        return new Roaring64Bitmap(ptr);
    }
}