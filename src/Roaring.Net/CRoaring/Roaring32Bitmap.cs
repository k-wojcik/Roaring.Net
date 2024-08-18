using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Roaring.Net.CRoaring;

public unsafe class Roaring32Bitmap : Roaring32BitmapBase, IReadOnlyRoaring32Bitmap
{
    private bool _isDisposed;

    public ulong Count => NativeMethods.roaring_bitmap_get_cardinality(Pointer);
    public bool IsEmpty => NativeMethods.roaring_bitmap_is_empty(Pointer);
    public bool IsCopyOnWrite => NativeMethods.roaring_bitmap_get_copy_on_write(Pointer);
    public uint? Min => IsEmpty ? null : NativeMethods.roaring_bitmap_minimum(Pointer);
    public uint? Max => IsEmpty ? null : NativeMethods.roaring_bitmap_maximum(Pointer);
    public Roaring32Bitmap() => Pointer = CheckBitmapPointer(NativeMethods.roaring_bitmap_create_with_capacity(0));

    public Roaring32Bitmap(uint capacity) => Pointer = CheckBitmapPointer(NativeMethods.roaring_bitmap_create_with_capacity(capacity));
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

    public static Roaring32Bitmap FromValues(uint[] values) => FromValues(values, 0U, (nuint)values.Length);

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

    public Roaring32Bitmap Clone() => new(NativeMethods.roaring_bitmap_copy(Pointer));
    
    public Roaring32Bitmap CloneWithOffset(long offset) 
        => new(NativeMethods.roaring_bitmap_add_offset(Pointer, offset));

    public bool OverwriteWith(Roaring32BitmapBase source) => NativeMethods.roaring_bitmap_overwrite(Pointer, source.Pointer);
    
    public void Add(uint value) => NativeMethods.roaring_bitmap_add(Pointer, value);

    public void AddMany(uint[] values) => AddMany(values, 0, (nuint)values.Length);

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
    
    public bool TryAdd(uint value) => NativeMethods.roaring_bitmap_add_checked(Pointer, value);

    public void AddRange(uint start, uint end)
    {
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, ExceptionMessages.StartValueGreaterThenEndValue);
        }
        
        NativeMethods.roaring_bitmap_add_range_closed(Pointer, start, end);
    }
    
    public void AddOffset(long offset)
    {
        var previousPtr = Pointer;
        Pointer = NativeMethods.roaring_bitmap_add_offset(Pointer, offset);
        NativeMethods.roaring_bitmap_free(previousPtr);
    }
    
    public void Remove(uint value) => NativeMethods.roaring_bitmap_remove(Pointer, value);

    public void RemoveMany(uint[] values) => RemoveMany(values, 0, (nuint)values.Length);

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
    
    public bool TryRemove(uint value) => NativeMethods.roaring_bitmap_remove_checked(Pointer, value);

    public void RemoveRange(uint start, uint end)
    {
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, ExceptionMessages.StartValueGreaterThenEndValue);
        }
        
        NativeMethods.roaring_bitmap_remove_range_closed(Pointer, start, end);
    }
    
    public void Clear() => NativeMethods.roaring_bitmap_clear(Pointer);
    
    public bool Contains(uint value) => NativeMethods.roaring_bitmap_contains(Pointer, value);

    public bool ContainsRange(uint start, uint end)
    {
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, ExceptionMessages.StartValueGreaterThenEndValue);
        }
     
        return NativeMethods.roaring_bitmap_contains_range(Pointer, start, (ulong)end + 1);
    }

    public bool ValueEquals(Roaring32BitmapBase? bitmap)
    {
        if (bitmap == null)
        {
            return false;
        }

        return NativeMethods.roaring_bitmap_equals(Pointer, bitmap.Pointer);
    }

    public bool IsSubsetOf(Roaring32BitmapBase? bitmap)
    {
        if (bitmap == null)
        {
            return false;
        }

        return NativeMethods.roaring_bitmap_is_subset(Pointer, bitmap.Pointer);
    }

    public bool IsProperSubsetOf(Roaring32BitmapBase? bitmap)
    {
        if (bitmap == null)
        {
            return false;
        }
        
        return NativeMethods.roaring_bitmap_is_strict_subset(Pointer, bitmap.Pointer);
    }
    
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
    
    public bool TryGetValue(uint index, out uint value) => NativeMethods.roaring_bitmap_select(Pointer, index, out value);
    public long GetIndex(uint value) => NativeMethods.roaring_bitmap_get_index(Pointer, value);
    
    public ulong CountLessOrEqualTo(uint value) => NativeMethods.roaring_bitmap_rank(Pointer, value);

    /// <summary>
    /// Counts values less than or equal to for each element from <paramref name="values"/>.
    /// </summary>
    /// <param name="values">An ascending sorted set of tested values.</param>
    /// <returns>The number values that are less than or equal to the value from <paramref name="values"/> placed under the same index.</returns>
    public ulong[] CountManyLessOrEqualTo(uint[] values)
    {
        var items = new ulong[values.Length];
        fixed (uint* valuesPtr = values)
        {
            NativeMethods.roaring_bitmap_rank_many(Pointer, valuesPtr, valuesPtr + values.Length, items);
        }
        return items;
    }

    public ulong CountRange(uint start, uint end)
    {  
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, ExceptionMessages.StartValueGreaterThenEndValue);
        }
        
        return NativeMethods.roaring_bitmap_range_cardinality(Pointer, start, (ulong)end + 1);
    }

    public Roaring32Bitmap Not() => new(NativeMethods.roaring_bitmap_flip(Pointer, uint.MinValue, uint.MaxValue + 1UL));
    public void INot() => NativeMethods.roaring_bitmap_flip_inplace(Pointer, uint.MinValue, uint.MaxValue + 1UL);

    public Roaring32Bitmap NotRange(uint start, uint end)
    {
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, ExceptionMessages.StartValueGreaterThenEndValue);
        }
     
        return new(NativeMethods.roaring_bitmap_flip(Pointer, start, (ulong)end + 1));
    }

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
        var ptr = format switch
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
        var ptr = format switch
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
        var size = format switch
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
        var ptr = NativeMethods.roaring_bitmap_frozen_view(memoryPtr, size);
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
        NativeMethods.roaring_bitmap_statistics(Pointer, out var stats);
        return stats;
    }

    public bool IsValid() => IsValid(out _);
    
    public bool IsValid(out string? reason)
    {
        var result = NativeMethods.roaring_bitmap_internal_validate(Pointer, out var reasonPtr);
        reason = Marshal.PtrToStringAnsi(reasonPtr);
        return result;
    }

    public void SetCopyOnWrite(bool enabled) => NativeMethods.roaring_bitmap_set_copy_on_write(Pointer, enabled);
}