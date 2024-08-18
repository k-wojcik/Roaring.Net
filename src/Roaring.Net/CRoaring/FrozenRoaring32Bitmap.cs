using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Roaring.Net.CRoaring;

public unsafe class FrozenRoaring32Bitmap : Roaring32BitmapBase, IReadOnlyRoaring32Bitmap
{
    internal Roaring32BitmapMemory Memory { get; }
    
    private readonly Roaring32Bitmap _bitmap;
    private bool _isDisposed;

    public ulong Count => _bitmap.Count;
    public bool IsEmpty => _bitmap.IsEmpty;
    public uint? Min => _bitmap.Min;
    public uint? Max => _bitmap.Max;

    internal FrozenRoaring32Bitmap(Roaring32Bitmap bitmap)
    {
        var size = bitmap.GetSerializationBytes(SerializationFormat.Frozen);
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
    
    public bool Contains(uint value) => _bitmap.Contains(value);

    public bool ContainsRange(uint start, uint end) => _bitmap.ContainsRange(start, end);
    
    public bool ValueEquals(Roaring32BitmapBase? bitmap) => _bitmap.ValueEquals(bitmap);
    
    public bool IsSubsetOf(Roaring32BitmapBase? bitmap) => _bitmap.IsSubsetOf(bitmap);

    public bool IsProperSubsetOf(Roaring32BitmapBase? bitmap) => _bitmap.IsProperSubsetOf(bitmap);
    
    public bool IsSupersetOf(Roaring32BitmapBase? bitmap) => _bitmap.IsSupersetOf(bitmap);
    
    public bool IsProperSupersetOf(Roaring32BitmapBase? bitmap)  => _bitmap.IsProperSupersetOf(bitmap);
    
    public bool TryGetValue(uint index, out uint value) => _bitmap.TryGetValue(index, out value);
    public long GetIndex(uint value) => _bitmap.GetIndex(value);
    
    public ulong CountLessOrEqualTo(uint value) => _bitmap.CountLessOrEqualTo(value);
    
    public ulong[] CountManyLessOrEqualTo(uint[] values) => _bitmap.CountManyLessOrEqualTo(values);

    public ulong CountRange(uint start, uint end) => _bitmap.CountRange(start, end);

    public Roaring32Bitmap Not() => _bitmap.Not();
    
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
    
    public Roaring32Bitmap ToBitmap() => _bitmap.Clone();
    
    public Roaring32Bitmap ToBitmapWithOffset(long offset) => _bitmap.CloneWithOffset(offset);
    
    public nuint GetSerializationBytes(SerializationFormat format = SerializationFormat.Normal) => _bitmap.GetSerializationBytes(format);

    public byte[] Serialize(SerializationFormat format = SerializationFormat.Normal) => _bitmap.Serialize(format);

    public uint[] Take(ulong count) => _bitmap.Take(count);

    public Statistics GetStatistics() => _bitmap.GetStatistics();
    
    public bool IsValid() => _bitmap.IsValid();
    
    public bool IsValid(out string? reason) => _bitmap.IsValid(out reason);
}