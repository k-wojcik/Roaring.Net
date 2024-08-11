using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CRoaring;

public unsafe class Roaring32Bitmap : IDisposable
{
    private IntPtr _pointer;
    private bool _isDisposed;

    public ulong Count => NativeMethods.roaring_bitmap_get_cardinality(_pointer);
    public bool IsEmpty => NativeMethods.roaring_bitmap_is_empty(_pointer);
    public bool IsCopyOnWrite => NativeMethods.roaring_bitmap_get_copy_on_write(_pointer);
    public uint? Min => IsEmpty ? null : NativeMethods.roaring_bitmap_minimum(_pointer);
    public uint? Max => IsEmpty ? null : NativeMethods.roaring_bitmap_maximum(_pointer);
    public nuint SerializedBytes => NativeMethods.roaring_bitmap_size_in_bytes(_pointer);
    public nuint PortableSerializedBytes => NativeMethods.roaring_bitmap_portable_size_in_bytes(_pointer);
    public Roaring32Bitmap() => _pointer = CheckBitmapPointer(NativeMethods.roaring_bitmap_create_with_capacity(0));

    public Roaring32Bitmap(uint capacity) => _pointer = CheckBitmapPointer(NativeMethods.roaring_bitmap_create_with_capacity(capacity));
    public Roaring32Bitmap(uint[] values) => _pointer = CreatePtrFromValues(values, 0, (uint)values.Length);

    private Roaring32Bitmap(IntPtr pointer) => _pointer = pointer;

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
       
        return new(CheckBitmapPointer(NativeMethods.roaring_bitmap_from_range(start, (ulong)end + 1, step)));
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
            return CheckBitmapPointer(NativeMethods.roaring_bitmap_of_ptr(count, valuePtr + offset));
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            NativeMethods.roaring_bitmap_free(_pointer);
            _isDisposed = true;
        }
    }

    ~Roaring32Bitmap() => Dispose(false);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public Roaring32Bitmap Clone() => new(CheckBitmapPointer(NativeMethods.roaring_bitmap_copy(_pointer)));
    
    public Roaring32Bitmap CloneWithOffset(long offset) 
        => new(CheckBitmapPointer(NativeMethods.roaring_bitmap_add_offset(_pointer, offset)));

    public bool OverwriteWith(Roaring32Bitmap source) => NativeMethods.roaring_bitmap_overwrite(_pointer, source._pointer);
    
    public void Add(uint value) => NativeMethods.roaring_bitmap_add(_pointer, value);

    public void AddMany(uint[] values) => AddMany(values, 0, (nuint)values.Length);

    public void AddMany(uint[] values, nuint offset, nuint count)
    {
        if ((nuint)values.Length < offset + count)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), offset, ExceptionMessages.OffsetWithCountGreaterThanNumberOfValues);
        }
        
        fixed (uint* valuePtr = values)
        {
            NativeMethods.roaring_bitmap_add_many(_pointer, count, valuePtr + offset);
        }
    }
    
    public bool TryAdd(uint value) => NativeMethods.roaring_bitmap_add_checked(_pointer, value);

    public void AddRange(uint start, uint end)
    {
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, ExceptionMessages.StartValueGreaterThenEndValue);
        }
        
        NativeMethods.roaring_bitmap_add_range_closed(_pointer, start, end);
    }
    
    public void AddOffset(long offset)
    {
        var previousPtr = _pointer;
        _pointer = NativeMethods.roaring_bitmap_add_offset(_pointer, offset);
        NativeMethods.roaring_bitmap_free(previousPtr);
    }
    
    public void Remove(uint value) => NativeMethods.roaring_bitmap_remove(_pointer, value);

    public void RemoveMany(uint[] values) => RemoveMany(values, 0, (nuint)values.Length);

    public void RemoveMany(uint[] values, nuint offset, nuint count)
    {
        if ((nuint)values.Length < offset + count)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), offset, ExceptionMessages.OffsetWithCountGreaterThanNumberOfValues);
        }
      
        fixed (uint* valuePtr = values)
        {
            NativeMethods.roaring_bitmap_remove_many(_pointer, count, valuePtr + offset);
        }
    }
    
    public bool TryRemove(uint value) => NativeMethods.roaring_bitmap_remove_checked(_pointer, value);

    public void RemoveRange(uint start, uint end)
    {
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, ExceptionMessages.StartValueGreaterThenEndValue);
        }
        
        NativeMethods.roaring_bitmap_remove_range_closed(_pointer, start, end);
    }
    
    public void Clear() => NativeMethods.roaring_bitmap_clear(_pointer);
    
    public bool Contains(uint value) => NativeMethods.roaring_bitmap_contains(_pointer, value);

    public bool ContainsRange(uint start, uint end)
    {
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, ExceptionMessages.StartValueGreaterThenEndValue);
        }
     
        return NativeMethods.roaring_bitmap_contains_range(_pointer, start, (ulong)end + 1);
    }

    public bool ValueEquals(Roaring32Bitmap? bitmap)
    {
        if (bitmap == null)
        {
            return false;
        }

        return NativeMethods.roaring_bitmap_equals(_pointer, bitmap._pointer);
    }

    public bool IsSubsetOf(Roaring32Bitmap? bitmap)
    {
        if (bitmap == null)
        {
            return false;
        }

        return NativeMethods.roaring_bitmap_is_subset(_pointer, bitmap._pointer);
    }

    public bool IsProperSubsetOf(Roaring32Bitmap? bitmap)
    {
        if (bitmap == null)
        {
            return false;
        }
        
        return NativeMethods.roaring_bitmap_is_strict_subset(_pointer, bitmap._pointer);
    }
    
    public bool IsSupersetOf(Roaring32Bitmap? bitmap)
    {
        if (bitmap == null)
        {
            return false;
        }

        if (NativeMethods.roaring_bitmap_get_cardinality(bitmap._pointer) >
            NativeMethods.roaring_bitmap_get_cardinality(_pointer))
        {
            return false;
        }
        
        return NativeMethods.roaring_bitmap_is_subset(bitmap._pointer, _pointer);
    }
    
    public bool IsProperSupersetOf(Roaring32Bitmap? bitmap)
    {
        if (bitmap == null)
        {
            return false;
        }
        
        if (NativeMethods.roaring_bitmap_get_cardinality(bitmap._pointer) >=
            NativeMethods.roaring_bitmap_get_cardinality(_pointer))
        {
            return false;
        }
        
        return NativeMethods.roaring_bitmap_is_subset(bitmap._pointer, _pointer);
    }
    
    public bool TryGetValue(uint index, out uint value) => NativeMethods.roaring_bitmap_select(_pointer, index, out value);
    public long GetIndex(uint value) => NativeMethods.roaring_bitmap_get_index(_pointer, value);
    
    public ulong CountLessOrEqualTo(uint value) => NativeMethods.roaring_bitmap_rank(_pointer, value);

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
            NativeMethods.roaring_bitmap_rank_many(_pointer, valuesPtr, valuesPtr + values.Length, items);
        }
        return items;
    }

    public ulong CountRange(uint start, uint end)
    {  
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, ExceptionMessages.StartValueGreaterThenEndValue);
        }
        
        return NativeMethods.roaring_bitmap_range_cardinality(_pointer, start, (ulong)end + 1);
    }

    public Roaring32Bitmap Not() => new(CheckBitmapPointer(NativeMethods.roaring_bitmap_flip(_pointer, uint.MinValue, uint.MaxValue + 1UL)));
    public void INot() => NativeMethods.roaring_bitmap_flip_inplace(_pointer, uint.MinValue, uint.MaxValue + 1UL);

    public Roaring32Bitmap NotRange(uint start, uint end)
    {
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, ExceptionMessages.StartValueGreaterThenEndValue);
        }
     
        return new(CheckBitmapPointer(NativeMethods.roaring_bitmap_flip(_pointer, start, (ulong)end + 1)));
    }

    public void INotRange(uint start, uint end)
    {
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, ExceptionMessages.StartValueGreaterThenEndValue);
        }

        NativeMethods.roaring_bitmap_flip_inplace(_pointer, start, (ulong)end + 1);
    }

    public Roaring32Bitmap And(Roaring32Bitmap bitmap) =>
        new(NativeMethods.roaring_bitmap_and(_pointer, bitmap._pointer));

    public void IAnd(Roaring32Bitmap bitmap)
        => NativeMethods.roaring_bitmap_and_inplace(_pointer, bitmap._pointer);

    public ulong AndCount(Roaring32Bitmap bitmap)
        => NativeMethods.roaring_bitmap_and_cardinality(_pointer, bitmap._pointer);

    public Roaring32Bitmap AndNot(Roaring32Bitmap bitmap) =>
        new(NativeMethods.roaring_bitmap_andnot(_pointer, bitmap._pointer));

    public void IAndNot(Roaring32Bitmap bitmap)
        => NativeMethods.roaring_bitmap_andnot_inplace(_pointer, bitmap._pointer);

    public ulong AndNotCount(Roaring32Bitmap bitmap)
        => NativeMethods.roaring_bitmap_andnot_cardinality(_pointer, bitmap._pointer);

    public Roaring32Bitmap Or(Roaring32Bitmap bitmap) =>
        new(NativeMethods.roaring_bitmap_or(_pointer, bitmap._pointer));
    
    public ulong OrCount(Roaring32Bitmap bitmap)
        => NativeMethods.roaring_bitmap_or_cardinality(_pointer, bitmap._pointer);
    
    public Roaring32Bitmap OrMany(Roaring32Bitmap[] bitmaps)
    {    
        int length = bitmaps.Length + 1;
        var pointers = new IntPtr[length];
        for (int i = 0; i < bitmaps.Length; i++)
        {
            pointers[i] = bitmaps[i]._pointer;
        }
        pointers[length - 1] = _pointer;
        
        return new Roaring32Bitmap(NativeMethods.roaring_bitmap_or_many((nuint)pointers.Length, pointers));
    }

    public Roaring32Bitmap OrManyHeap(Roaring32Bitmap[] bitmaps)
    {
        int length = bitmaps.Length + 1;
        var pointers = new IntPtr[length];
        for (int i = 0; i < bitmaps.Length; i++)
        {
            pointers[i] = bitmaps[i]._pointer;
        }
        pointers[length - 1] = _pointer;

        return new Roaring32Bitmap(NativeMethods.roaring_bitmap_or_many_heap((uint)pointers.Length, pointers));
    }

    public void IOr(Roaring32Bitmap bitmap)
        => NativeMethods.roaring_bitmap_or_inplace(_pointer, bitmap._pointer);
    
    public Roaring32Bitmap LazyOr(Roaring32Bitmap bitmap, bool bitsetConversion) =>
        new(NativeMethods.roaring_bitmap_lazy_or(_pointer, bitmap._pointer, bitsetConversion));

    public void ILazyOr(Roaring32Bitmap bitmap, bool bitsetConversion)
        => NativeMethods.roaring_bitmap_lazy_or_inplace(_pointer, bitmap._pointer, bitsetConversion);
    
    public Roaring32Bitmap Xor(Roaring32Bitmap bitmap) =>
        new(NativeMethods.roaring_bitmap_xor(_pointer, bitmap._pointer));

    public void IXor(Roaring32Bitmap bitmap)
        => NativeMethods.roaring_bitmap_xor_inplace(_pointer, bitmap._pointer);

    public ulong XorCount(Roaring32Bitmap bitmap)
        => NativeMethods.roaring_bitmap_xor_cardinality(_pointer, bitmap._pointer);
    
    public Roaring32Bitmap XorMany(params Roaring32Bitmap[] bitmaps)
    {
        int length = bitmaps.Length + 1;
        var pointers = new IntPtr[length];
        for (int i = 0; i < bitmaps.Length; i++)
        {
            pointers[i] = bitmaps[i]._pointer;
        }
        pointers[length - 1] = _pointer;
        
        return new Roaring32Bitmap(NativeMethods.roaring_bitmap_xor_many((nuint)pointers.Length, pointers));
    }
    
    public Roaring32Bitmap LazyXor(Roaring32Bitmap bitmap)
        => new(NativeMethods.roaring_bitmap_lazy_xor(_pointer, bitmap._pointer));

    public void ILazyXor(Roaring32Bitmap bitmap)
        => NativeMethods.roaring_bitmap_lazy_xor_inplace(_pointer, bitmap._pointer);
    
    public void RepairAfterLazy()
        => NativeMethods.roaring_bitmap_repair_after_lazy(_pointer);

    public bool Overlaps(Roaring32Bitmap bitmap)
        => NativeMethods.roaring_bitmap_intersect(_pointer, bitmap._pointer);
    
    public bool OverlapsRange(uint start, uint end)
    {
        if (start > end)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, ExceptionMessages.StartValueGreaterThenEndValue);
        }
        
        return NativeMethods.roaring_bitmap_intersect_with_range(_pointer, start, (ulong)end + 1);
    }
    
    public double GetJaccardIndex(Roaring32Bitmap bitmap)
        => NativeMethods.roaring_bitmap_jaccard_index(_pointer, bitmap._pointer);

    public bool Optimize()
        => NativeMethods.roaring_bitmap_run_optimize(_pointer);

    public bool RemoveRunCompression()
        => NativeMethods.roaring_bitmap_remove_run_compression(_pointer);

    public nuint ShrinkToFit()
        => NativeMethods.roaring_bitmap_shrink_to_fit(_pointer);

    public void CopyTo(uint[] buffer)
        => NativeMethods.roaring_bitmap_to_uint32_array(_pointer, buffer);

    public byte[] Serialize(SerializationFormat format = SerializationFormat.Normal)
    {
        byte[] buffer;
        switch (format)
        {
            case SerializationFormat.Normal:
                buffer = new byte[NativeMethods.roaring_bitmap_size_in_bytes(_pointer)];
                NativeMethods.roaring_bitmap_serialize(_pointer, buffer);
                break;
            case SerializationFormat.Portable:
                buffer = new byte[NativeMethods.roaring_bitmap_portable_size_in_bytes(_pointer)];
                NativeMethods.roaring_bitmap_portable_serialize(_pointer, buffer);
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

    public IEnumerable<uint> Values => new Roaring32Enumerator(_pointer);

    public uint[] ToArray()
    {
        ulong count = NativeMethods.roaring_bitmap_get_cardinality(_pointer);
        uint[] values = new uint[count];
        NativeMethods.roaring_bitmap_to_uint32_array(_pointer, values);
        return values;
    }

    public uint[] Take(ulong count)
    {
        ulong cardinality = NativeMethods.roaring_bitmap_get_cardinality(_pointer);
        if (cardinality < count)
        {
            count = cardinality;
        }

        uint[] values = new uint[count];
        NativeMethods.roaring_bitmap_to_uint32_array(_pointer, values);
        return values;
    }

    public Statistics GetStatistics()
    {
        NativeMethods.roaring_bitmap_statistics(_pointer, out var stats);
        return stats;
    }

    public bool IsValid() => IsValid(out _);
    
    public bool IsValid(out string? reason)
    {
        var result = NativeMethods.roaring_bitmap_internal_validate(_pointer, out var reasonPtr);
        reason = Marshal.PtrToStringAnsi(reasonPtr);
        return result;
    }

    public void SetCopyOnWrite(bool enabled) => NativeMethods.roaring_bitmap_set_copy_on_write(_pointer, enabled);
}