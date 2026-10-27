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
    public static partial IntPtr roaring_bitmap_copy(SafeRoaring32BitmapHandle bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_copy")]
    public static partial IntPtr roaring64_bitmap_copy(SafeRoaring64BitmapHandle bitmap);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_overwrite")]
    public static partial bool roaring_bitmap_overwrite(SafeRoaring32BitmapHandle destination, SafeRoaring32BitmapHandle source);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_overwrite")]
    public static partial void roaring64_bitmap_overwrite(SafeRoaring64BitmapHandle destination, SafeRoaring64BitmapHandle source);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_free")]
    public static partial void roaring_bitmap_free(IntPtr bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_free")]
    public static partial void roaring64_bitmap_free(IntPtr bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_move_from_roaring32")]
    public static partial IntPtr roaring64_bitmap_move_from_roaring32(SafeRoaring32BitmapHandle bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_get_cardinality")]
    public static partial ulong roaring_bitmap_get_cardinality(SafeRoaring32BitmapHandle bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_get_cardinality")]
    public static partial ulong roaring64_bitmap_get_cardinality(SafeRoaring64BitmapHandle bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_range_cardinality_closed")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static partial ulong roaring_bitmap_range_cardinality_closed(SafeRoaring32BitmapHandle bitmap, uint range_start, uint range_end);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_range_closed_cardinality")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static partial ulong roaring64_bitmap_range_closed_cardinality(SafeRoaring64BitmapHandle bitmap, ulong range_start, ulong range_end);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_range_uint32_array")]
    public static partial bool roaring_bitmap_range_uint32_array(SafeRoaring32BitmapHandle bitmap, nuint offset, nuint limit, uint[] values);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_is_empty")]
    public static partial bool roaring_bitmap_is_empty(SafeRoaring32BitmapHandle bitmap);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_is_empty")]
    public static partial bool roaring64_bitmap_is_empty(SafeRoaring64BitmapHandle bitmap);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_get_copy_on_write")]
    public static partial bool roaring_bitmap_get_copy_on_write(SafeRoaring32BitmapHandle bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_set_copy_on_write")]
    public static partial void roaring_bitmap_set_copy_on_write(SafeRoaring32BitmapHandle bitmap, [MarshalAs(UnmanagedType.I1)] bool cow);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_add")]
    public static partial void roaring_bitmap_add(SafeRoaring32BitmapHandle bitmap, uint value);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_add")]
    public static partial void roaring64_bitmap_add(SafeRoaring64BitmapHandle bitmap, ulong value);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_add_many")]
    public static partial void roaring_bitmap_add_many(SafeRoaring32BitmapHandle bitmap, nuint count, uint* values);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_add_many")]
    public static partial void roaring64_bitmap_add_many(SafeRoaring64BitmapHandle bitmap, nuint count, ulong* values);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_add_checked")]
    public static partial bool roaring_bitmap_add_checked(SafeRoaring32BitmapHandle bitmap, uint value);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_add_checked")]
    public static partial bool roaring64_bitmap_add_checked(SafeRoaring64BitmapHandle bitmap, ulong value);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_add_range_closed")]
    public static partial void roaring_bitmap_add_range_closed(SafeRoaring32BitmapHandle bitmap, uint min, uint max);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_add_range_closed")]
    public static partial void roaring64_bitmap_add_range_closed(SafeRoaring64BitmapHandle bitmap, ulong min, ulong max);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_add_offset")]
    public static partial IntPtr roaring_bitmap_add_offset(SafeRoaring32BitmapHandle bitmap, long offset);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_add_offset_signed")]
    public static partial IntPtr roaring64_bitmap_add_offset_signed(SafeRoaring64BitmapHandle bitmap, [MarshalAs(UnmanagedType.I1)] bool positive, ulong offset);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_remove")]
    public static partial void roaring_bitmap_remove(SafeRoaring32BitmapHandle bitmap, uint value);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_remove")]
    public static partial void roaring64_bitmap_remove(SafeRoaring64BitmapHandle bitmap, ulong value);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_remove_range_closed")]
    public static partial void roaring_bitmap_remove_range_closed(SafeRoaring32BitmapHandle bitmap, uint min, uint max);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_remove_range_closed")]
    public static partial void roaring64_bitmap_remove_range_closed(SafeRoaring64BitmapHandle bitmap, ulong min, ulong max);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_clear")]
    public static partial void roaring_bitmap_clear(SafeRoaring32BitmapHandle bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_clear")]
    public static partial void roaring64_bitmap_clear(SafeRoaring64BitmapHandle bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_remove_many")]
    public static partial void roaring_bitmap_remove_many(SafeRoaring32BitmapHandle bitmap, nuint count, uint* values);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_remove_many")]
    public static partial void roaring64_bitmap_remove_many(SafeRoaring64BitmapHandle bitmap, nuint count, ulong* values);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_remove_checked")]
    public static partial bool roaring_bitmap_remove_checked(SafeRoaring32BitmapHandle bitmap, uint value);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_remove_checked")]
    public static partial bool roaring64_bitmap_remove_checked(SafeRoaring64BitmapHandle bitmap, ulong value);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_contains")]
    public static partial bool roaring_bitmap_contains(SafeRoaring32BitmapHandle bitmap, uint value);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_contains")]
    public static partial bool roaring64_bitmap_contains(SafeRoaring64BitmapHandle bitmap, ulong value);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_contains_range_closed")]
    public static partial bool roaring_bitmap_contains_range_closed(SafeRoaring32BitmapHandle bitmap, uint min, uint max);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_contains_range_closed")]
    public static partial bool roaring64_bitmap_contains_range_closed(SafeRoaring64BitmapHandle bitmap, ulong min, ulong max);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_select")]
    public static partial bool roaring_bitmap_select(SafeRoaring32BitmapHandle bitmap, uint rank, out uint element);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_select")]
    public static partial bool roaring64_bitmap_select(SafeRoaring64BitmapHandle bitmap, ulong rank, out ulong element);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_rank")]
    public static partial ulong roaring_bitmap_rank(SafeRoaring32BitmapHandle bitmap, uint x);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_rank")]
    public static partial ulong roaring64_bitmap_rank(SafeRoaring64BitmapHandle bitmap, ulong val);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_rank_many")]
    public static partial void roaring_bitmap_rank_many(SafeRoaring32BitmapHandle bitmap, uint* begin, uint* end, [Out] ulong[] ans);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_rank_many")]
    public static partial void roaring_bitmap_rank_many(SafeRoaring32BitmapHandle bitmap, uint* begin, uint* end, ulong* ans);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_get_index")]
    public static partial long roaring_bitmap_get_index(SafeRoaring32BitmapHandle bitmap, uint x);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_get_index")]
    public static partial bool roaring64_bitmap_get_index(SafeRoaring64BitmapHandle bitmap, ulong value, out ulong out_index);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_equals")]
    public static partial bool roaring_bitmap_equals(SafeRoaring32BitmapHandle bitmap1, SafeRoaring32BitmapHandle bitmap2);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_equals")]
    public static partial bool roaring64_bitmap_equals(SafeRoaring64BitmapHandle bitmap1, SafeRoaring64BitmapHandle bitmap2);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_is_subset")]
    public static partial bool roaring_bitmap_is_subset(SafeRoaring32BitmapHandle bitmap1, SafeRoaring32BitmapHandle bitmap2);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_is_subset")]
    public static partial bool roaring64_bitmap_is_subset(SafeRoaring64BitmapHandle bitmap1, SafeRoaring64BitmapHandle bitmap2);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_is_strict_subset")]
    public static partial bool roaring_bitmap_is_strict_subset(SafeRoaring32BitmapHandle bitmap1, SafeRoaring32BitmapHandle bitmap2);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_is_strict_subset")]
    public static partial bool roaring64_bitmap_is_strict_subset(SafeRoaring64BitmapHandle bitmap1, SafeRoaring64BitmapHandle bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_minimum")]
    public static partial uint roaring_bitmap_minimum(SafeRoaring32BitmapHandle bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_minimum")]
    public static partial ulong roaring64_bitmap_minimum(SafeRoaring64BitmapHandle bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_maximum")]
    public static partial uint roaring_bitmap_maximum(SafeRoaring32BitmapHandle bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_maximum")]
    public static partial ulong roaring64_bitmap_maximum(SafeRoaring64BitmapHandle bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_flip_closed")]
    public static partial IntPtr roaring_bitmap_flip_closed(SafeRoaring32BitmapHandle bitmap, uint start, uint end);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_flip_closed")]
    public static partial IntPtr roaring64_bitmap_flip_closed(SafeRoaring64BitmapHandle bitmap, ulong start, ulong end);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_flip_inplace_closed")]
    public static partial IntPtr roaring_bitmap_flip_inplace_closed(SafeRoaring32BitmapHandle bitmap, uint start, uint end);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_flip_closed_inplace")]
    public static partial void roaring64_bitmap_flip_closed_inplace(SafeRoaring64BitmapHandle bitmap, ulong start, ulong end);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_and")]
    public static partial IntPtr roaring_bitmap_and(SafeRoaring32BitmapHandle bitmap1, SafeRoaring32BitmapHandle bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_and")]
    public static partial IntPtr roaring64_bitmap_and(SafeRoaring64BitmapHandle bitmap1, SafeRoaring64BitmapHandle bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_and_inplace")]
    public static partial void roaring_bitmap_and_inplace(SafeRoaring32BitmapHandle bitmap1, SafeRoaring32BitmapHandle bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_and_inplace")]
    public static partial void roaring64_bitmap_and_inplace(SafeRoaring64BitmapHandle bitmap1, SafeRoaring64BitmapHandle bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_and_cardinality")]
    public static partial ulong roaring_bitmap_and_cardinality(SafeRoaring32BitmapHandle bitmap1, SafeRoaring32BitmapHandle bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_and_cardinality")]
    public static partial ulong roaring64_bitmap_and_cardinality(SafeRoaring64BitmapHandle bitmap1, SafeRoaring64BitmapHandle bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_andnot")]
    public static partial IntPtr roaring_bitmap_andnot(SafeRoaring32BitmapHandle bitmap1, SafeRoaring32BitmapHandle bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_andnot")]
    public static partial IntPtr roaring64_bitmap_andnot(SafeRoaring64BitmapHandle bitmap1, SafeRoaring64BitmapHandle bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_andnot_inplace")]
    public static partial void roaring_bitmap_andnot_inplace(SafeRoaring32BitmapHandle bitmap1, SafeRoaring32BitmapHandle bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_andnot_inplace")]
    public static partial void roaring64_bitmap_andnot_inplace(SafeRoaring64BitmapHandle bitmap1, SafeRoaring64BitmapHandle bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_andnot_cardinality")]
    public static partial ulong roaring_bitmap_andnot_cardinality(SafeRoaring32BitmapHandle bitmap1, SafeRoaring32BitmapHandle bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_andnot_cardinality")]
    public static partial ulong roaring64_bitmap_andnot_cardinality(SafeRoaring64BitmapHandle bitmap1, SafeRoaring64BitmapHandle bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_or")]
    public static partial IntPtr roaring_bitmap_or(SafeRoaring32BitmapHandle bitmap1, SafeRoaring32BitmapHandle bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_or")]
    public static partial IntPtr roaring64_bitmap_or(SafeRoaring64BitmapHandle bitmap1, SafeRoaring64BitmapHandle bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_or_inplace")]
    public static partial void roaring_bitmap_or_inplace(SafeRoaring32BitmapHandle bitmap1, SafeRoaring32BitmapHandle bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_or_inplace")]
    public static partial void roaring64_bitmap_or_inplace(SafeRoaring64BitmapHandle bitmap1, SafeRoaring64BitmapHandle bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_or_cardinality")]
    public static partial ulong roaring_bitmap_or_cardinality(SafeRoaring32BitmapHandle bitmap1, SafeRoaring32BitmapHandle bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_or_cardinality")]
    public static partial ulong roaring64_bitmap_or_cardinality(SafeRoaring64BitmapHandle bitmap1, SafeRoaring64BitmapHandle bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_or_many")]
    public static partial IntPtr roaring_bitmap_or_many(nuint count, IntPtr[] bitmaps);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_or_many_heap")]
    public static partial IntPtr roaring_bitmap_or_many_heap(uint count, IntPtr[] bitmaps);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_xor")]
    public static partial IntPtr roaring_bitmap_xor(SafeRoaring32BitmapHandle bitmap1, SafeRoaring32BitmapHandle bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_xor")]
    public static partial IntPtr roaring64_bitmap_xor(SafeRoaring64BitmapHandle bitmap1, SafeRoaring64BitmapHandle bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_xor_inplace")]
    public static partial void roaring_bitmap_xor_inplace(SafeRoaring32BitmapHandle bitmap1, SafeRoaring32BitmapHandle bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_xor_inplace")]
    public static partial void roaring64_bitmap_xor_inplace(SafeRoaring64BitmapHandle bitmap1, SafeRoaring64BitmapHandle bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_xor_cardinality")]
    public static partial ulong roaring_bitmap_xor_cardinality(SafeRoaring32BitmapHandle bitmap1, SafeRoaring32BitmapHandle bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_xor_cardinality")]
    public static partial ulong roaring64_bitmap_xor_cardinality(SafeRoaring64BitmapHandle bitmap1, SafeRoaring64BitmapHandle bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_xor_many")]
    public static partial IntPtr roaring_bitmap_xor_many(nuint count, IntPtr[] bitmaps);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_lazy_or")]
    public static partial IntPtr roaring_bitmap_lazy_or(SafeRoaring32BitmapHandle bitmap1, SafeRoaring32BitmapHandle bitmap2, [MarshalAs(UnmanagedType.I1)] bool bitsetConversion);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_lazy_or_inplace")]
    public static partial void roaring_bitmap_lazy_or_inplace(SafeRoaring32BitmapHandle bitmap1, SafeRoaring32BitmapHandle bitmap2, [MarshalAs(UnmanagedType.I1)] bool bitsetConversion);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_lazy_xor")]
    public static partial IntPtr roaring_bitmap_lazy_xor(SafeRoaring32BitmapHandle bitmap1, SafeRoaring32BitmapHandle bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_lazy_xor_inplace")]
    public static partial void roaring_bitmap_lazy_xor_inplace(SafeRoaring32BitmapHandle bitmap1, SafeRoaring32BitmapHandle bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_repair_after_lazy")]
    public static partial IntPtr roaring_bitmap_repair_after_lazy(SafeRoaring32BitmapHandle bitmap);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_intersect")]
    public static partial bool roaring_bitmap_intersect(SafeRoaring32BitmapHandle bitmap1, SafeRoaring32BitmapHandle bitmap2);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_intersect")]
    public static partial bool roaring64_bitmap_intersect(SafeRoaring64BitmapHandle bitmap1, SafeRoaring64BitmapHandle bitmap2);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_intersect_with_range")]
    public static partial bool roaring_bitmap_intersect_with_range(SafeRoaring32BitmapHandle bitmap1, ulong x, ulong y);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_intersect_with_range")]
    public static partial bool roaring64_bitmap_intersect_with_range(SafeRoaring64BitmapHandle bitmap1, ulong x, ulong y);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_jaccard_index")]
    public static partial double roaring_bitmap_jaccard_index(SafeRoaring32BitmapHandle bitmap1, SafeRoaring32BitmapHandle bitmap2);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_jaccard_index")]
    public static partial double roaring64_bitmap_jaccard_index(SafeRoaring64BitmapHandle bitmap1, SafeRoaring64BitmapHandle bitmap2);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_run_optimize")]
    public static partial bool roaring_bitmap_run_optimize(SafeRoaring32BitmapHandle bitmap);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_run_optimize")]
    public static partial bool roaring64_bitmap_run_optimize(SafeRoaring64BitmapHandle bitmap);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_remove_run_compression")]
    public static partial bool roaring_bitmap_remove_run_compression(SafeRoaring32BitmapHandle bitmap);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_remove_run_compression")]
    public static partial bool roaring64_bitmap_remove_run_compression(SafeRoaring64BitmapHandle bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_shrink_to_fit")]
    public static partial nuint roaring_bitmap_shrink_to_fit(SafeRoaring32BitmapHandle bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_shrink_to_fit")]
    public static partial nuint roaring64_bitmap_shrink_to_fit(SafeRoaring64BitmapHandle bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_size_in_bytes")]
    public static partial nuint roaring_bitmap_size_in_bytes(SafeRoaring32BitmapHandle bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_portable_size_in_bytes")]
    public static partial nuint roaring_bitmap_portable_size_in_bytes(SafeRoaring32BitmapHandle bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_portable_size_in_bytes")]
    public static partial nuint roaring64_bitmap_portable_size_in_bytes(SafeRoaring64BitmapHandle bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_frozen_size_in_bytes")]
    public static partial nuint roaring_bitmap_frozen_size_in_bytes(SafeRoaring32BitmapHandle bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_frozen_size_in_bytes")]
    public static partial nuint roaring64_bitmap_frozen_size_in_bytes(SafeRoaring64BitmapHandle bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_to_uint32_array")]
    public static partial void roaring_bitmap_to_uint32_array(SafeRoaring32BitmapHandle bitmap, [Out] uint[] values);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_to_uint64_array")]
    public static partial void roaring64_bitmap_to_uint64_array(SafeRoaring64BitmapHandle bitmap, [Out] ulong[] values);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_to_uint32_array")]
    public static partial void roaring_bitmap_to_uint32_array(SafeRoaring32BitmapHandle bitmap, uint* values);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_to_uint64_array")]
    public static partial void roaring64_bitmap_to_uint64_array(SafeRoaring64BitmapHandle bitmap, ulong* values);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_serialize")]
    public static partial nuint roaring_bitmap_serialize(SafeRoaring32BitmapHandle bitmap, byte* buffer);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_deserialize")]
    public static partial IntPtr roaring_bitmap_deserialize(byte* buffer);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_deserialize_safe")]
    public static partial IntPtr roaring_bitmap_deserialize_safe(byte* buffer, nuint maxbytes);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_portable_serialize")]
    public static partial nuint roaring_bitmap_portable_serialize(SafeRoaring32BitmapHandle bitmap, byte* buffer);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_portable_serialize")]
    public static partial nuint roaring64_bitmap_portable_serialize(SafeRoaring64BitmapHandle bitmap, byte* buffer);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_portable_deserialize")]
    public static partial IntPtr roaring_bitmap_portable_deserialize(byte* buffer);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_portable_deserialize_safe")]
    public static partial IntPtr roaring_bitmap_portable_deserialize_safe(byte* buffer, nuint maxbytes);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_portable_deserialize_safe")]
    public static partial IntPtr roaring64_bitmap_portable_deserialize_safe(byte* buffer, nuint maxbytes);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_portable_deserialize_size")]
    public static partial nuint roaring_bitmap_portable_deserialize_size(byte* buffer, nuint maxbytes);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_portable_deserialize_size")]
    public static partial nuint roaring64_bitmap_portable_deserialize_size(byte* buffer, nuint maxbytes);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_frozen_serialize")]
    public static partial void roaring_bitmap_frozen_serialize(SafeRoaring32BitmapHandle bitmap, byte* buffer);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_frozen_serialize")]
    public static partial void roaring64_bitmap_frozen_serialize(SafeRoaring64BitmapHandle bitmap, byte* buffer);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_frozen_serialize")]
    public static partial void roaring_bitmap_frozen_serialize_to_memory(SafeRoaring32BitmapHandle bitmap, SafeRoaring32MemoryHandle buffer);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_frozen_serialize")]
    public static partial void roaring64_bitmap_frozen_serialize_to_memory(SafeRoaring64BitmapHandle bitmap, SafeRoaring64MemoryHandle buffer);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_portable_deserialize_frozen")]
    public static partial IntPtr roaring_bitmap_portable_deserialize_frozen(byte* buffer);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_frozen_view")]
    public static partial IntPtr roaring_bitmap_frozen_view(byte* buffer, nuint length);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_frozen_view")]
    public static partial IntPtr roaring64_bitmap_frozen_view(byte* buffer, nuint length);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_frozen_view")]
    public static partial IntPtr roaring_bitmap_frozen_view_from_memory(SafeRoaring32MemoryHandle buffer, nuint length);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_frozen_view")]
    public static partial IntPtr roaring64_bitmap_frozen_view_from_memory(SafeRoaring64MemoryHandle buffer, nuint length);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_portable_deserialize_frozen")]
    public static partial IntPtr roaring_bitmap_portable_deserialize_frozen_from_memory(SafeRoaring32MemoryHandle buffer);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_portable_deserialize_frozen")]
    public static partial IntPtr roaring64_bitmap_portable_deserialize_frozen(byte* buffer, nuint maxbytes);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_portable_deserialize_frozen")]
    public static partial IntPtr roaring64_bitmap_portable_deserialize_frozen_from_memory(SafeRoaring64MemoryHandle buffer, nuint maxbytes);

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
    public static partial bool roaring_iterate(SafeRoaring32BitmapHandle bitmap, IteratorDelegate iterator, IntPtr tag);

    public static bool roaring_iterate(SafeRoaring32BitmapHandle bitmap, Func<uint, bool> iterator)
    {
        return roaring_iterate(bitmap, (v, _) => iterator(v), IntPtr.Zero);
    }

    public delegate bool IteratorDelegate(uint value, IntPtr tag);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_iterate64")]
    // ReSharper disable once InconsistentNaming
    public static partial bool roaring_iterate64(SafeRoaring32BitmapHandle bitmap, IteratorDelegate iterator, ulong high_bits, IntPtr tag);

    // ReSharper disable once InconsistentNaming
    public static bool roaring_iterate64(SafeRoaring32BitmapHandle bitmap, Func<uint, bool> iterator, ulong high_bits)
    {
        return roaring_iterate64(bitmap, (v, _) => iterator(v), high_bits, IntPtr.Zero);
    }

    public delegate bool IteratorDelegate64(uint value, IntPtr tag);

    [LibraryImport("roaring", EntryPoint = "roaring_iterator_create")]
    public static partial IntPtr roaring_iterator_create(SafeRoaring32BitmapHandle bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring64_iterator_create")]
    public static partial IntPtr roaring64_iterator_create(SafeRoaring64BitmapHandle bitmap);

    [LibraryImport("roaring", EntryPoint = "roaring_iterator_init")]
    public static partial void roaring_iterator_init(SafeRoaring32BitmapHandle bitmap, IntPtr iterator);

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

    [LibraryImport("roaring", EntryPoint = "roaring64_iterator_read")]
    public static partial nuint roaring64_iterator_read(IntPtr iterator, ulong* buffer, nuint count);

    [LibraryImport("roaring", EntryPoint = "roaring_uint32_iterator_copy")]
    public static partial IntPtr roaring_uint32_iterator_copy(IntPtr iterator);

    [LibraryImport("roaring", EntryPoint = "roaring_uint32_iterator_free")]
    public static partial void roaring_uint32_iterator_free(IntPtr iterator);

    [LibraryImport("roaring", EntryPoint = "roaring64_iterator_free")]
    public static partial void roaring64_iterator_free(IntPtr iterator);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_statistics")]
    public static partial void roaring_bitmap_statistics(SafeRoaring32BitmapHandle bitmap, out Statistics stats);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_statistics")]
    public static partial void roaring64_bitmap_statistics(SafeRoaring64BitmapHandle bitmap, out Statistics64 stats);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_internal_validate")]
    public static partial bool roaring_bitmap_internal_validate(SafeRoaring32BitmapHandle bitmap, out IntPtr reasonPtr);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_internal_validate")]
    public static partial bool roaring64_bitmap_internal_validate(SafeRoaring64BitmapHandle bitmap, out IntPtr reasonPtr);

    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_add_bulk")]
    public static partial void roaring_bitmap_add_bulk(SafeRoaring32BitmapHandle bitmap, SafeBulkContextHandle context, uint value);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_add_bulk")]
    public static partial void roaring64_bitmap_add_bulk(SafeRoaring64BitmapHandle bitmap, SafeBulkContextHandle context, ulong value);

    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_remove_bulk")]
    public static partial void roaring64_bitmap_remove_bulk(SafeRoaring64BitmapHandle bitmap, SafeBulkContextHandle context, ulong value);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_contains_bulk")]
    public static partial bool roaring_bitmap_contains_bulk(SafeRoaring32BitmapHandle bitmap, SafeBulkContextHandle context, uint value);

    [return: MarshalAs(UnmanagedType.I1)]
    [LibraryImport("roaring", EntryPoint = "roaring64_bitmap_contains_bulk")]
    public static partial bool roaring64_bitmap_contains_bulk(SafeRoaring64BitmapHandle bitmap, SafeBulkContextHandle context, ulong value);
}
