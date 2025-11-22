using System;
using System.Collections.Generic;

namespace Roaring.Net.CRoaring;

internal interface IReadOnlyRoaring64Bitmap : IDisposable
{
    ulong Count { get; }

    bool IsEmpty { get; }

    ulong? Min { get; }

    ulong? Max { get; }

    Roaring64Bitmap NotRange(ulong start, ulong end);

    Roaring64Bitmap And(Roaring64BitmapBase bitmap);

    ulong AndCount(Roaring64BitmapBase bitmap);

    Roaring64Bitmap AndNot(Roaring64BitmapBase bitmap);

    ulong AndNotCount(Roaring64BitmapBase bitmap);

    Roaring64Bitmap Or(Roaring64BitmapBase bitmap);

    ulong OrCount(Roaring64BitmapBase bitmap);

    Roaring64Bitmap OrMany(Roaring64BitmapBase[] bitmaps);

    Roaring64Bitmap Xor(Roaring64BitmapBase bitmap);

    ulong XorCount(Roaring64BitmapBase bitmap);

    Roaring64Bitmap XorMany(params Roaring64BitmapBase[] bitmaps);

    bool Overlaps(Roaring64BitmapBase bitmap);

    bool OverlapsRange(ulong start, ulong end);

    double GetJaccardIndex(Roaring64BitmapBase bitmap);

    bool Contains(ulong value);

    bool ContainsBulk(BulkContext64 context, ulong value);

    bool ContainsRange(ulong start, ulong end);

    bool ValueEquals(Roaring64BitmapBase? bitmap);

    bool IsSubsetOf(Roaring64BitmapBase? bitmap);

    bool IsProperSubsetOf(Roaring64BitmapBase? bitmap);

    bool IsSupersetOf(Roaring64BitmapBase? bitmap);

    bool IsProperSupersetOf(Roaring64BitmapBase? bitmap);

    bool TryGetValue(ulong index, out ulong value);

    bool TryGetIndex(ulong value, out ulong index);

    ulong CountLessOrEqualTo(ulong value);

    ulong[] CountManyLessOrEqualTo(ulong[] values);

    ulong CountRange(ulong start, ulong end);

    void CopyTo(ulong[] buffer);

    void CopyTo(Memory<ulong> buffer);

    void CopyTo(Span<ulong> buffer);

    IEnumerable<ulong> Values { get; }

    ulong[] ToArray();

    ulong[] Take(ulong count);

    nuint GetSerializationBytes(SerializationFormat format = SerializationFormat.Normal);

    byte[] Serialize(SerializationFormat format = SerializationFormat.Normal);

    Statistics64 GetStatistics();

    bool IsValid();

    bool IsValid(out string? reason);
}