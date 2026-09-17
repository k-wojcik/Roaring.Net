using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Roaring.Net.CRoaring;

internal static unsafe partial class NativeMethods
{
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_create_with_capacity")]
    public static partial IntPtr roaring_bitmap_create_with_capacity(uint capacity);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_create")]
    public static partial IntPtr roaring64_bitmap_create();

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_from_range")]
    public static partial IntPtr roaring_bitmap_from_range(ulong min, ulong max, uint step);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_from_range")]
    public static partial IntPtr roaring64_bitmap_from_range(ulong min, ulong max, ulong step);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_of_ptr")]
    public static partial IntPtr roaring_bitmap_of_ptr(nuint count, uint* values);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_of_ptr")]
    public static partial IntPtr roaring64_bitmap_of_ptr(nuint count, ulong* values);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_copy")]
    public static partial IntPtr roaring_bitmap_copy(IntPtr bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_copy")]
    public static partial IntPtr roaring64_bitmap_copy(IntPtr bitmap);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_overwrite")]
    public static partial bool roaring_bitmap_overwrite(IntPtr destination, IntPtr source);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_overwrite")]
    public static partial void roaring64_bitmap_overwrite(IntPtr destination, IntPtr source);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_free")]
    public static partial void roaring_bitmap_free(IntPtr bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_free")]
    public static partial void roaring64_bitmap_free(IntPtr bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_move_from_roaring32")]
    public static partial IntPtr roaring64_bitmap_move_from_roaring32(IntPtr bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_get_cardinality")]
    public static partial ulong roaring_bitmap_get_cardinality(IntPtr bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_get_cardinality")]
    public static partial ulong roaring64_bitmap_get_cardinality(IntPtr bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_range_cardinality_closed")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static partial ulong roaring_bitmap_range_cardinality_closed(IntPtr bitmap, uint range_start, uint range_end);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_range_closed_cardinality")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static partial ulong roaring64_bitmap_range_closed_cardinality(IntPtr bitmap, ulong range_start, ulong range_end);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_is_empty")]
    public static partial bool roaring_bitmap_is_empty(IntPtr bitmap);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_is_empty")]
    public static partial bool roaring64_bitmap_is_empty(IntPtr bitmap);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_get_copy_on_write")]
    public static partial bool roaring_bitmap_get_copy_on_write(IntPtr bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_set_copy_on_write")]
    public static partial void roaring_bitmap_set_copy_on_write(IntPtr bitmap, [MarshalAs(UnmanagedType.I1)] bool cow);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_add")]
    public static partial void roaring_bitmap_add(IntPtr bitmap, uint value);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_add")]
    public static partial void roaring64_bitmap_add(IntPtr bitmap, ulong value);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_add_many")]
    public static partial void roaring_bitmap_add_many(IntPtr bitmap, nuint count, uint* values);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_add_many")]
    public static partial void roaring64_bitmap_add_many(IntPtr bitmap, nuint count, ulong* values);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_add_checked")]
    public static partial bool roaring_bitmap_add_checked(IntPtr bitmap, uint value);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_add_checked")]
    public static partial bool roaring64_bitmap_add_checked(IntPtr bitmap, ulong value);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_add_range_closed")]
    public static partial void roaring_bitmap_add_range_closed(IntPtr bitmap, uint min, uint max);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_add_range_closed")]
    public static partial void roaring64_bitmap_add_range_closed(IntPtr bitmap, ulong min, ulong max);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_add_offset")]
    public static partial IntPtr roaring_bitmap_add_offset(IntPtr bitmap, long offset);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_add_offset_signed")]
    public static partial IntPtr roaring64_bitmap_add_offset_signed(IntPtr bitmap, [MarshalAs(UnmanagedType.I1)] bool positive, ulong offset);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_remove")]
    public static partial void roaring_bitmap_remove(IntPtr bitmap, uint value);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_remove")]
    public static partial void roaring64_bitmap_remove(IntPtr bitmap, ulong value);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_remove_range_closed")]
    public static partial void roaring_bitmap_remove_range_closed(IntPtr bitmap, uint min, uint max);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_remove_range_closed")]
    public static partial void roaring64_bitmap_remove_range_closed(IntPtr bitmap, ulong min, ulong max);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_clear")]
    public static partial void roaring_bitmap_clear(IntPtr bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_clear")]
    public static partial void roaring64_bitmap_clear(IntPtr bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_remove_many")]
    public static partial void roaring_bitmap_remove_many(IntPtr bitmap, nuint count, uint* values);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_remove_many")]
    public static partial void roaring64_bitmap_remove_many(IntPtr bitmap, nuint count, ulong* values);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_remove_checked")]
    public static partial bool roaring_bitmap_remove_checked(IntPtr bitmap, uint value);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_remove_checked")]
    public static partial bool roaring64_bitmap_remove_checked(IntPtr bitmap, ulong value);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_contains")]
    public static partial bool roaring_bitmap_contains(IntPtr bitmap, uint value);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_contains")]
    public static partial bool roaring64_bitmap_contains(IntPtr bitmap, ulong value);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_contains_range")]
    public static partial bool roaring_bitmap_contains_range(IntPtr bitmap, ulong min, ulong max);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_contains_range")]
    public static partial bool roaring64_bitmap_contains_range(IntPtr bitmap, ulong min, ulong max);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_select")]
    public static partial bool roaring_bitmap_select(IntPtr bitmap, uint rank, out uint element);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_select")]
    public static partial bool roaring64_bitmap_select(IntPtr bitmap, ulong rank, out ulong element);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_rank")]
    public static partial ulong roaring_bitmap_rank(IntPtr bitmap, uint x);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_rank")]
    public static partial ulong roaring64_bitmap_rank(IntPtr bitmap, ulong val);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_rank_many")]
    public static partial void roaring_bitmap_rank_many(IntPtr bitmap, uint* begin, uint* end, [Out] ulong[] ans);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_get_index")]
    public static partial long roaring_bitmap_get_index(IntPtr bitmap, uint x);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_get_index")]
    public static partial bool roaring64_bitmap_get_index(IntPtr bitmap, ulong value, out ulong out_index);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_equals")]
    public static partial bool roaring_bitmap_equals(IntPtr bitmap1, IntPtr bitmap2);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_equals")]
    public static partial bool roaring64_bitmap_equals(IntPtr bitmap1, IntPtr bitmap2);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_is_subset")]
    public static partial bool roaring_bitmap_is_subset(IntPtr bitmap1, IntPtr bitmap2);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_is_subset")]
    public static partial bool roaring64_bitmap_is_subset(IntPtr bitmap1, IntPtr bitmap2);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_is_strict_subset")]
    public static partial bool roaring_bitmap_is_strict_subset(IntPtr bitmap1, IntPtr bitmap2);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_is_strict_subset")]
    public static partial bool roaring64_bitmap_is_strict_subset(IntPtr bitmap1, IntPtr bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_minimum")]
    public static partial uint roaring_bitmap_minimum(IntPtr bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_minimum")]
    public static partial ulong roaring64_bitmap_minimum(IntPtr bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_maximum")]
    public static partial uint roaring_bitmap_maximum(IntPtr bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_maximum")]
    public static partial ulong roaring64_bitmap_maximum(IntPtr bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_flip_closed")]
    public static partial IntPtr roaring_bitmap_flip_closed(IntPtr bitmap, uint start, uint end);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_flip_closed")]
    public static partial IntPtr roaring64_bitmap_flip_closed(IntPtr bitmap, ulong start, ulong end);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_flip_inplace_closed")]
    public static partial IntPtr roaring_bitmap_flip_inplace_closed(IntPtr bitmap, uint start, uint end);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_flip_closed_inplace")]
    public static partial void roaring64_bitmap_flip_closed_inplace(IntPtr bitmap, ulong start, ulong end);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_and")]
    public static partial IntPtr roaring_bitmap_and(IntPtr bitmap1, IntPtr bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_and")]
    public static partial IntPtr roaring64_bitmap_and(IntPtr bitmap1, IntPtr bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_and_inplace")]
    public static partial void roaring_bitmap_and_inplace(IntPtr bitmap1, IntPtr bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_and_inplace")]
    public static partial void roaring64_bitmap_and_inplace(IntPtr bitmap1, IntPtr bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_and_cardinality")]
    public static partial ulong roaring_bitmap_and_cardinality(IntPtr bitmap1, IntPtr bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_and_cardinality")]
    public static partial ulong roaring64_bitmap_and_cardinality(IntPtr bitmap1, IntPtr bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_andnot")]
    public static partial IntPtr roaring_bitmap_andnot(IntPtr bitmap1, IntPtr bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_andnot")]
    public static partial IntPtr roaring64_bitmap_andnot(IntPtr bitmap1, IntPtr bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_andnot_inplace")]
    public static partial void roaring_bitmap_andnot_inplace(IntPtr bitmap1, IntPtr bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_andnot_inplace")]
    public static partial void roaring64_bitmap_andnot_inplace(IntPtr bitmap1, IntPtr bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_andnot_cardinality")]
    public static partial ulong roaring_bitmap_andnot_cardinality(IntPtr bitmap1, IntPtr bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_andnot_cardinality")]
    public static partial ulong roaring64_bitmap_andnot_cardinality(IntPtr bitmap1, IntPtr bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_or")]
    public static partial IntPtr roaring_bitmap_or(IntPtr bitmap1, IntPtr bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_or")]
    public static partial IntPtr roaring64_bitmap_or(IntPtr bitmap1, IntPtr bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_or_inplace")]
    public static partial void roaring_bitmap_or_inplace(IntPtr bitmap1, IntPtr bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_or_inplace")]
    public static partial void roaring64_bitmap_or_inplace(IntPtr bitmap1, IntPtr bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_or_cardinality")]
    public static partial ulong roaring_bitmap_or_cardinality(IntPtr bitmap1, IntPtr bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_or_cardinality")]
    public static partial ulong roaring64_bitmap_or_cardinality(IntPtr bitmap1, IntPtr bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_or_many")]
    public static partial IntPtr roaring_bitmap_or_many(nuint count, IntPtr[] bitmaps);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_or_many_heap")]
    public static partial IntPtr roaring_bitmap_or_many_heap(uint count, IntPtr[] bitmaps);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_xor")]
    public static partial IntPtr roaring_bitmap_xor(IntPtr bitmap1, IntPtr bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_xor")]
    public static partial IntPtr roaring64_bitmap_xor(IntPtr bitmap1, IntPtr bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_xor_inplace")]
    public static partial void roaring_bitmap_xor_inplace(IntPtr bitmap1, IntPtr bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_xor_inplace")]
    public static partial void roaring64_bitmap_xor_inplace(IntPtr bitmap1, IntPtr bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_xor_cardinality")]
    public static partial ulong roaring_bitmap_xor_cardinality(IntPtr bitmap1, IntPtr bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_xor_cardinality")]
    public static partial ulong roaring64_bitmap_xor_cardinality(IntPtr bitmap1, IntPtr bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_xor_many")]
    public static partial IntPtr roaring_bitmap_xor_many(nuint count, IntPtr[] bitmaps);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_lazy_or")]
    public static partial IntPtr roaring_bitmap_lazy_or(IntPtr bitmap1, IntPtr bitmap2, [MarshalAs(UnmanagedType.I1)] bool bitsetConversion);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_lazy_or_inplace")]
    public static partial void roaring_bitmap_lazy_or_inplace(IntPtr bitmap1, IntPtr bitmap2, [MarshalAs(UnmanagedType.I1)] bool bitsetConversion);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_lazy_xor")]
    public static partial IntPtr roaring_bitmap_lazy_xor(IntPtr bitmap1, IntPtr bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_lazy_xor_inplace")]
    public static partial void roaring_bitmap_lazy_xor_inplace(IntPtr bitmap1, IntPtr bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_repair_after_lazy")]
    public static partial IntPtr roaring_bitmap_repair_after_lazy(IntPtr bitmap);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_intersect")]
    public static partial bool roaring_bitmap_intersect(IntPtr bitmap1, IntPtr bitmap2);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_intersect")]
    public static partial bool roaring64_bitmap_intersect(IntPtr bitmap1, IntPtr bitmap2);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_intersect_with_range")]
    public static partial bool roaring_bitmap_intersect_with_range(IntPtr bitmap1, ulong x, ulong y);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_intersect_with_range")]
    public static partial bool roaring64_bitmap_intersect_with_range(IntPtr bitmap1, ulong x, ulong y);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_jaccard_index")]
    public static partial double roaring_bitmap_jaccard_index(IntPtr bitmap1, IntPtr bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_jaccard_index")]
    public static partial double roaring64_bitmap_jaccard_index(IntPtr bitmap1, IntPtr bitmap2);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_run_optimize")]
    public static partial bool roaring_bitmap_run_optimize(IntPtr bitmap);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_run_optimize")]
    public static partial bool roaring64_bitmap_run_optimize(IntPtr bitmap);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_remove_run_compression")]
    public static partial bool roaring_bitmap_remove_run_compression(IntPtr bitmap);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_remove_run_compression")]
    public static partial bool roaring64_bitmap_remove_run_compression(IntPtr bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_shrink_to_fit")]
    public static partial nuint roaring_bitmap_shrink_to_fit(IntPtr bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_shrink_to_fit")]
    public static partial nuint roaring64_bitmap_shrink_to_fit(IntPtr bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_size_in_bytes")]
    public static partial nuint roaring_bitmap_size_in_bytes(IntPtr bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_portable_size_in_bytes")]
    public static partial nuint roaring_bitmap_portable_size_in_bytes(IntPtr bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_portable_size_in_bytes")]
    public static partial nuint roaring64_bitmap_portable_size_in_bytes(IntPtr bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_frozen_size_in_bytes")]
    public static partial nuint roaring_bitmap_frozen_size_in_bytes(IntPtr bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_frozen_size_in_bytes")]
    public static partial nuint roaring64_bitmap_frozen_size_in_bytes(IntPtr bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_to_uint32_array")]
    public static partial void roaring_bitmap_to_uint32_array(IntPtr bitmap, [Out] uint[] values);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_to_uint64_array")]
    public static partial void roaring64_bitmap_to_uint64_array(IntPtr bitmap, [Out] ulong[] values);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_to_uint32_array")]
    public static partial void roaring_bitmap_to_uint32_array(IntPtr bitmap, uint* values);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_to_uint64_array")]
    public static partial void roaring64_bitmap_to_uint64_array(IntPtr bitmap, ulong* values);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_serialize")]
    public static partial nuint roaring_bitmap_serialize(IntPtr bitmap, [Out] byte[] buffer);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_deserialize")]
    public static partial IntPtr roaring_bitmap_deserialize(byte[] buffer);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_deserialize_safe")]
    public static partial IntPtr roaring_bitmap_deserialize_safe(byte[] buffer, nuint maxbytes);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_portable_serialize")]
    public static partial nuint roaring_bitmap_portable_serialize(IntPtr bitmap, [Out] byte[] buffer);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_portable_serialize")]
    public static partial nuint roaring64_bitmap_portable_serialize(IntPtr bitmap, byte* buffer);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_portable_deserialize")]
    public static partial IntPtr roaring_bitmap_portable_deserialize(byte[] buffer);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_portable_deserialize_safe")]
    public static partial IntPtr roaring_bitmap_portable_deserialize_safe(byte[] buffer, nuint maxbytes);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_portable_deserialize_safe")]
    public static partial IntPtr roaring64_bitmap_portable_deserialize_safe(byte[] buffer, nuint maxbytes);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_portable_deserialize_size")]
    public static partial nuint roaring_bitmap_portable_deserialize_size(byte[] buffer, nuint maxbytes);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_portable_deserialize_size")]
    public static partial nuint roaring64_bitmap_portable_deserialize_size(byte[] buffer, nuint maxbytes);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_frozen_serialize")]
    public static partial void roaring_bitmap_frozen_serialize(IntPtr bitmap, byte* buffer);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_frozen_serialize")]
    public static partial void roaring64_bitmap_frozen_serialize(IntPtr bitmap, byte* buffer);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_portable_deserialize_frozen")]
    public static partial IntPtr roaring_bitmap_portable_deserialize_frozen(byte* buffer);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_frozen_view")]
    public static partial IntPtr roaring_bitmap_frozen_view(byte* buffer, nuint length);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_frozen_view")]
    public static partial IntPtr roaring64_bitmap_frozen_view(byte* buffer, nuint length);

    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct Iterator
    {
        public readonly IntPtr parent;
        public readonly IntPtr container;
        public readonly byte typecode;
        public readonly int container_index;
        public readonly uint highbits;

        public readonly ContainerIterator container_it;

        public readonly uint current_value;
        public readonly bool has_value;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct ContainerIterator
    {
        public readonly int index;
    }

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring")]
    public static partial bool roaring_iterate(IntPtr bitmap, IteratorDelegate iterator, IntPtr tag);

    public static bool roaring_iterate(IntPtr bitmap, Func<uint, bool> iterator)
    {
        return roaring_iterate(bitmap, (v, _) => iterator(v), IntPtr.Zero);
    }

    public delegate bool IteratorDelegate(uint value, IntPtr tag);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_iterate64")]
    // ReSharper disable once InconsistentNaming
    public static partial bool roaring_iterate64(IntPtr bitmap, IteratorDelegate iterator, ulong high_bits, IntPtr tag);

    // ReSharper disable once InconsistentNaming
    public static bool roaring_iterate64(IntPtr bitmap, Func<uint, bool> iterator, ulong high_bits)
    {
        return roaring_iterate64(bitmap, (v, _) => iterator(v), high_bits, IntPtr.Zero);
    }

    public delegate bool IteratorDelegate64(uint value, IntPtr tag);

    [LibraryImport("roaring", EntryPoint = "roaring_iterator_create")]
    public static partial IntPtr roaring_iterator_create(IntPtr bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring64_iterator_create")]
    public static partial IntPtr roaring64_iterator_create(IntPtr bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring_iterator_init")]
    public static partial void roaring_iterator_init(IntPtr bitmap, IntPtr iterator);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_iterator_has_value")]
    public static partial bool roaring64_iterator_has_value(IntPtr iterator);

    [LibraryImport("roaring", EntryPoint = "roaring64_iterator_value")]
    public static partial ulong roaring64_iterator_value(IntPtr iterator);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_uint32_iterator_advance")]
    public static partial bool roaring_uint32_iterator_advance(IntPtr iterator);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_iterator_advance")]
    public static partial bool roaring64_iterator_advance(IntPtr iterator);

    [LibraryImport("roaring", EntryPoint = "roaring_uint32_iterator_copy")]
    public static partial IntPtr roaring_uint32_iterator_copy(IntPtr iterator);

    [LibraryImport("roaring", EntryPoint = "roaring_uint32_iterator_free")]
    public static partial void roaring_uint32_iterator_free(IntPtr iterator);

    [LibraryImport("roaring", EntryPoint = "roaring64_iterator_free")]
    public static partial void roaring64_iterator_free(IntPtr iterator);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_statistics")]
    public static partial void roaring_bitmap_statistics(IntPtr bitmap, out Statistics stats);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_statistics")]
    public static partial void roaring64_bitmap_statistics(IntPtr bitmap, out Statistics64 stats);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_internal_validate")]
    public static partial bool roaring_bitmap_internal_validate(IntPtr bitmap, out IntPtr reasonPtr);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_internal_validate")]
    public static partial bool roaring64_bitmap_internal_validate(IntPtr bitmap, out IntPtr reasonPtr);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_add_bulk")]
    public static partial void roaring_bitmap_add_bulk(IntPtr bitmap, IntPtr context, uint value);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_add_bulk")]
    public static partial void roaring64_bitmap_add_bulk(IntPtr bitmap, IntPtr context, ulong value);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_remove_bulk")]
    public static partial void roaring64_bitmap_remove_bulk(IntPtr bitmap, IntPtr context, ulong value);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_contains_bulk")]
    public static partial bool roaring_bitmap_contains_bulk(IntPtr bitmap, IntPtr context, uint value);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_contains_bulk")]
    public static partial bool roaring64_bitmap_contains_bulk(IntPtr bitmap, IntPtr context, ulong value);
}
