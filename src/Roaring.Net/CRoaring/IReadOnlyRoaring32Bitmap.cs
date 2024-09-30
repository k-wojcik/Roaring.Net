using System;
using System.Collections.Generic;

namespace Roaring.Net.CRoaring;

internal interface IReadOnlyRoaring32Bitmap : IDisposable
{
    ulong Count { get; }

    bool IsEmpty { get; }

    uint? Min { get; }

    uint? Max { get; }

    Roaring32Bitmap Not();

    Roaring32Bitmap NotRange(uint start, uint end);

    Roaring32Bitmap And(Roaring32BitmapBase bitmap);

    ulong AndCount(Roaring32BitmapBase bitmap);

    Roaring32Bitmap AndNot(Roaring32BitmapBase bitmap);

    ulong AndNotCount(Roaring32BitmapBase bitmap);

    Roaring32Bitmap Or(Roaring32BitmapBase bitmap);

    ulong OrCount(Roaring32BitmapBase bitmap);

    Roaring32Bitmap OrMany(Roaring32BitmapBase[] bitmaps);

    Roaring32Bitmap OrManyHeap(Roaring32BitmapBase[] bitmaps);

    Roaring32Bitmap LazyOr(Roaring32BitmapBase bitmap, bool bitsetConversion);

    Roaring32Bitmap Xor(Roaring32BitmapBase bitmap);

    ulong XorCount(Roaring32BitmapBase bitmap);

    Roaring32Bitmap XorMany(params Roaring32BitmapBase[] bitmaps);

    Roaring32Bitmap LazyXor(Roaring32BitmapBase bitmap);

    bool Overlaps(Roaring32BitmapBase bitmap);

    bool OverlapsRange(uint start, uint end);

    double GetJaccardIndex(Roaring32BitmapBase bitmap);

    bool Contains(uint value);

    bool ContainsBulk(BulkContext context, uint value);

    bool ContainsRange(uint start, uint end);

    bool ValueEquals(Roaring32BitmapBase? bitmap);

    bool IsSubsetOf(Roaring32BitmapBase? bitmap);

    bool IsProperSubsetOf(Roaring32BitmapBase? bitmap);

    bool IsSupersetOf(Roaring32BitmapBase? bitmap);

    bool IsProperSupersetOf(Roaring32BitmapBase? bitmap);

    bool TryGetValue(uint index, out uint value);

    long GetIndex(uint value);

    ulong CountLessOrEqualTo(uint value);

    ulong[] CountManyLessOrEqualTo(uint[] values);

    ulong CountRange(uint start, uint end);

    void CopyTo(uint[] buffer);

    void CopyTo(Memory<uint> buffer);

    void CopyTo(Span<uint> buffer);

    IEnumerable<uint> Values { get; }

    uint[] ToArray();

    uint[] Take(ulong count);

    nuint GetSerializationBytes(SerializationFormat format = SerializationFormat.Normal);

    byte[] Serialize(SerializationFormat format = SerializationFormat.Normal);

    Statistics GetStatistics();

    bool IsValid();

    bool IsValid(out string? reason);
}