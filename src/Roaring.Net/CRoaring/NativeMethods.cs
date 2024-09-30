using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Roaring.Net.CRoaring;

internal static unsafe partial class NativeMethods
{
#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_create_with_capacity")]
    public static partial IntPtr roaring_bitmap_create_with_capacity(uint capacity);
#else
    [DllImport("roaring")]
    public static extern IntPtr roaring_bitmap_create_with_capacity(uint capacity);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_from_range")]
    public static partial IntPtr roaring_bitmap_from_range(ulong min, ulong max, uint step);
#else
    [DllImport("roaring")]
    public static extern IntPtr roaring_bitmap_from_range(ulong min, ulong max, uint step);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_of_ptr")]
    public static partial IntPtr roaring_bitmap_of_ptr(nuint count, uint* values);
#else
    [DllImport("roaring")]
    public static extern IntPtr roaring_bitmap_of_ptr(nuint count, uint* values);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_copy")]
    public static partial IntPtr roaring_bitmap_copy(IntPtr bitmap);
#else
    [DllImport("roaring")]
    public static extern IntPtr roaring_bitmap_copy(IntPtr bitmap);
#endif

    [return: MarshalAs(UnmanagedType.I1)]
#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_overwrite")]
    public static partial bool roaring_bitmap_overwrite(IntPtr destination, IntPtr source);
#else
    [DllImport("roaring")]
    public static extern bool roaring_bitmap_overwrite(IntPtr destination, IntPtr source);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_free")]
    public static partial void roaring_bitmap_free(IntPtr bitmap);
#else
    [DllImport("roaring")]
    public static extern void roaring_bitmap_free(IntPtr bitmap);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_get_cardinality")]
    public static partial ulong roaring_bitmap_get_cardinality(IntPtr bitmap);
#else
    [DllImport("roaring")]
    public static extern ulong roaring_bitmap_get_cardinality(IntPtr bitmap);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_range_cardinality")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static partial ulong roaring_bitmap_range_cardinality(IntPtr bitmap, ulong range_start, ulong range_end);
#else
    [DllImport("roaring")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static extern ulong roaring_bitmap_range_cardinality(IntPtr bitmap, ulong range_start, ulong range_end);
#endif

    [return: MarshalAs(UnmanagedType.I1)]
#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_is_empty")]
    public static partial bool roaring_bitmap_is_empty(IntPtr bitmap);
#else
    [DllImport("roaring")]
    public static extern bool roaring_bitmap_is_empty(IntPtr bitmap);
#endif

    [return: MarshalAs(UnmanagedType.I1)]
#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_get_copy_on_write")]
    public static partial bool roaring_bitmap_get_copy_on_write(IntPtr bitmap);
#else
    [DllImport("roaring")]
    public static extern bool roaring_bitmap_get_copy_on_write(IntPtr bitmap);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_set_copy_on_write")]
    public static partial void roaring_bitmap_set_copy_on_write(IntPtr bitmap, [MarshalAs(UnmanagedType.I1)] bool cow);
#else
    [DllImport("roaring")]
    public static extern void roaring_bitmap_set_copy_on_write(IntPtr bitmap, [MarshalAs(UnmanagedType.I1)] bool cow);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_add")]
    public static partial void roaring_bitmap_add(IntPtr bitmap, uint value);
#else
    [DllImport("roaring")]
    public static extern void roaring_bitmap_add(IntPtr bitmap, uint value);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_add_many")]
    public static partial void roaring_bitmap_add_many(IntPtr bitmap, nuint count, uint* values);
#else
    [DllImport("roaring")]
    public static extern void roaring_bitmap_add_many(IntPtr bitmap, nuint count, uint* values);
#endif

    [return: MarshalAs(UnmanagedType.I1)]
#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_add_checked")]
    public static partial bool roaring_bitmap_add_checked(IntPtr bitmap, uint value);
#else
    [DllImport("roaring")]
    public static extern bool roaring_bitmap_add_checked(IntPtr bitmap, uint value);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_add_range_closed")]
    public static partial void roaring_bitmap_add_range_closed(IntPtr bitmap, uint min, uint max);
#else
    [DllImport("roaring")]
    public static extern void roaring_bitmap_add_range_closed(IntPtr bitmap, uint min, uint max);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_add_offset")]
    public static partial IntPtr roaring_bitmap_add_offset(IntPtr bitmap, long offset);
#else
    [DllImport("roaring")]
    public static extern IntPtr roaring_bitmap_add_offset(IntPtr bitmap, long offset);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_remove")]
    public static partial void roaring_bitmap_remove(IntPtr bitmap, uint value);
#else
    [DllImport("roaring")]
    public static extern void roaring_bitmap_remove(IntPtr bitmap, uint value);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_remove_range_closed")]
    public static partial void roaring_bitmap_remove_range_closed(IntPtr bitmap, uint min, uint max);
#else
    [DllImport("roaring")]
    public static extern void roaring_bitmap_remove_range_closed(IntPtr bitmap, uint min, uint max);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_clear")]
    public static partial void roaring_bitmap_clear(IntPtr bitmap);
#else
    [DllImport("roaring")]
    public static extern void roaring_bitmap_clear(IntPtr bitmap);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_remove_many")]
    public static partial void roaring_bitmap_remove_many(IntPtr bitmap, nuint count, uint* values);
#else
    [DllImport("roaring")]
    public static extern void roaring_bitmap_remove_many(IntPtr bitmap, nuint count, uint* values);
#endif

    [return: MarshalAs(UnmanagedType.I1)]
#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_remove_checked")]
    public static partial bool roaring_bitmap_remove_checked(IntPtr bitmap, uint value);
#else
    [DllImport("roaring")]
    public static extern bool roaring_bitmap_remove_checked(IntPtr bitmap, uint value);
#endif

    [return: MarshalAs(UnmanagedType.I1)]
#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_contains")]
    public static partial bool roaring_bitmap_contains(IntPtr bitmap, uint value);
#else
    [DllImport("roaring")]
    public static extern bool roaring_bitmap_contains(IntPtr bitmap, uint value);
#endif

    [return: MarshalAs(UnmanagedType.I1)]
#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_contains_range")]
    public static partial bool roaring_bitmap_contains_range(IntPtr bitmap, ulong min, ulong max);
#else
    [DllImport("roaring")]
    public static extern bool roaring_bitmap_contains_range(IntPtr bitmap, ulong min, ulong max);
#endif

    [return: MarshalAs(UnmanagedType.I1)]
#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_select")]
    public static partial bool roaring_bitmap_select(IntPtr bitmap, uint rank, out uint element);
#else
    [DllImport("roaring")]
    public static extern bool roaring_bitmap_select(IntPtr bitmap, uint rank, out uint element);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_rank")]
    public static partial ulong roaring_bitmap_rank(IntPtr bitmap, uint x);
#else
    [DllImport("roaring")]
    public static extern ulong roaring_bitmap_rank(IntPtr bitmap, uint x);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_rank_many")]
    public static partial void roaring_bitmap_rank_many(IntPtr bitmap, uint* begin, uint* end, [Out] ulong[] ans);
#else
    [DllImport("roaring")]
    public static extern void roaring_bitmap_rank_many(IntPtr bitmap, uint* begin, uint* end, [Out] ulong[] ans);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_get_index")]
    public static partial long roaring_bitmap_get_index(IntPtr bitmap, uint x);
#else
    [DllImport("roaring")]
    public static extern long roaring_bitmap_get_index(IntPtr bitmap, uint x);
#endif

    [return: MarshalAs(UnmanagedType.I1)]
#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_equals")]
    public static partial bool roaring_bitmap_equals(IntPtr bitmap1, IntPtr bitmap2);
#else
    [DllImport("roaring")]
    public static extern bool roaring_bitmap_equals(IntPtr bitmap1, IntPtr bitmap2);
#endif

    [return: MarshalAs(UnmanagedType.I1)]
#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_is_subset")]
    public static partial bool roaring_bitmap_is_subset(IntPtr bitmap1, IntPtr bitmap2);
#else
    [DllImport("roaring")]
    public static extern bool roaring_bitmap_is_subset(IntPtr bitmap1, IntPtr bitmap2);
#endif

    [return: MarshalAs(UnmanagedType.I1)]
#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_is_strict_subset")]
    public static partial bool roaring_bitmap_is_strict_subset(IntPtr bitmap1, IntPtr bitmap2);
#else
    [DllImport("roaring")]
    public static extern bool roaring_bitmap_is_strict_subset(IntPtr bitmap1, IntPtr bitmap2);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_minimum")]
    public static partial uint roaring_bitmap_minimum(IntPtr bitmap);
#else
    [DllImport("roaring")]
    public static extern uint roaring_bitmap_minimum(IntPtr bitmap);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_maximum")]
    public static partial uint roaring_bitmap_maximum(IntPtr bitmap);
#else
    [DllImport("roaring")]
    public static extern uint roaring_bitmap_maximum(IntPtr bitmap);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_flip")]
    public static partial IntPtr roaring_bitmap_flip(IntPtr bitmap, ulong start, ulong end);
#else
    [DllImport("roaring")]
    public static extern IntPtr roaring_bitmap_flip(IntPtr bitmap, ulong start, ulong end);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_flip_inplace")]
    public static partial void roaring_bitmap_flip_inplace(IntPtr bitmap, ulong start, ulong end);
#else
    [DllImport("roaring")]
    public static extern void roaring_bitmap_flip_inplace(IntPtr bitmap, ulong start, ulong end);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_and")]
    public static partial IntPtr roaring_bitmap_and(IntPtr bitmap1, IntPtr bitmap2);
#else
    [DllImport("roaring")]
    public static extern IntPtr roaring_bitmap_and(IntPtr bitmap1, IntPtr bitmap2);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_and_inplace")]
    public static partial void roaring_bitmap_and_inplace(IntPtr bitmap1, IntPtr bitmap2);
#else
    [DllImport("roaring")]
    public static extern void roaring_bitmap_and_inplace(IntPtr bitmap1, IntPtr bitmap2);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_and_cardinality")]
    public static partial ulong roaring_bitmap_and_cardinality(IntPtr bitmap1, IntPtr bitmap2);
#else
    [DllImport("roaring")]
    public static extern ulong roaring_bitmap_and_cardinality(IntPtr bitmap1, IntPtr bitmap2);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_andnot")]
    public static partial IntPtr roaring_bitmap_andnot(IntPtr bitmap1, IntPtr bitmap2);
#else
    [DllImport("roaring")]
    public static extern IntPtr roaring_bitmap_andnot(IntPtr bitmap1, IntPtr bitmap2);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_andnot_inplace")]
    public static partial void roaring_bitmap_andnot_inplace(IntPtr bitmap1, IntPtr bitmap2);
#else
    [DllImport("roaring")]
    public static extern void roaring_bitmap_andnot_inplace(IntPtr bitmap1, IntPtr bitmap2);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_andnot_cardinality")]
    public static partial ulong roaring_bitmap_andnot_cardinality(IntPtr bitmap1, IntPtr bitmap2);
#else
    [DllImport("roaring")]
    public static extern ulong roaring_bitmap_andnot_cardinality(IntPtr bitmap1, IntPtr bitmap2);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_or")]
    public static partial IntPtr roaring_bitmap_or(IntPtr bitmap1, IntPtr bitmap2);
#else
    [DllImport("roaring")]
    public static extern IntPtr roaring_bitmap_or(IntPtr bitmap1, IntPtr bitmap2);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_or_inplace")]
    public static partial void roaring_bitmap_or_inplace(IntPtr bitmap1, IntPtr bitmap2);
#else
    [DllImport("roaring")]
    public static extern void roaring_bitmap_or_inplace(IntPtr bitmap1, IntPtr bitmap2);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_or_cardinality")]
    public static partial ulong roaring_bitmap_or_cardinality(IntPtr bitmap1, IntPtr bitmap2);
#else
    [DllImport("roaring")]
    public static extern ulong roaring_bitmap_or_cardinality(IntPtr bitmap1, IntPtr bitmap2);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_or_many")]
    public static partial IntPtr roaring_bitmap_or_many(nuint count, IntPtr[] bitmaps);
#else
    [DllImport("roaring")]
    public static extern IntPtr roaring_bitmap_or_many(nuint count, IntPtr[] bitmaps);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_or_many_heap")]
    public static partial IntPtr roaring_bitmap_or_many_heap(uint count, IntPtr[] bitmaps);
#else
    [DllImport("roaring")]
    public static extern IntPtr roaring_bitmap_or_many_heap(uint count, IntPtr[] bitmaps);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_xor")]
    public static partial IntPtr roaring_bitmap_xor(IntPtr bitmap1, IntPtr bitmap2);
#else
    [DllImport("roaring")]
    public static extern IntPtr roaring_bitmap_xor(IntPtr bitmap1, IntPtr bitmap2);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_xor_inplace")]
    public static partial void roaring_bitmap_xor_inplace(IntPtr bitmap1, IntPtr bitmap2);
#else
    [DllImport("roaring")]
    public static extern void roaring_bitmap_xor_inplace(IntPtr bitmap1, IntPtr bitmap2);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_xor_cardinality")]
    public static partial ulong roaring_bitmap_xor_cardinality(IntPtr bitmap1, IntPtr bitmap2);
#else
    [DllImport("roaring")]
    public static extern ulong roaring_bitmap_xor_cardinality(IntPtr bitmap1, IntPtr bitmap2);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_xor_many")]
    public static partial IntPtr roaring_bitmap_xor_many(nuint count, IntPtr[] bitmaps);
#else
    [DllImport("roaring")]
    public static extern IntPtr roaring_bitmap_xor_many(nuint count, IntPtr[] bitmaps);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_lazy_or")]
    public static partial IntPtr roaring_bitmap_lazy_or(IntPtr bitmap1, IntPtr bitmap2, [MarshalAs(UnmanagedType.I1)] bool bitsetConversion);
#else
    [DllImport("roaring")]
    public static extern IntPtr roaring_bitmap_lazy_or(IntPtr bitmap1, IntPtr bitmap2, bool bitsetConversion);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_lazy_or_inplace")]
    public static partial void roaring_bitmap_lazy_or_inplace(IntPtr bitmap1, IntPtr bitmap2, [MarshalAs(UnmanagedType.I1)] bool bitsetConversion);
#else
    [DllImport("roaring")]
    public static extern void roaring_bitmap_lazy_or_inplace(IntPtr bitmap1, IntPtr bitmap2, bool bitsetConversion);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_lazy_xor")]
    public static partial IntPtr roaring_bitmap_lazy_xor(IntPtr bitmap1, IntPtr bitmap2);
#else
    [DllImport("roaring")]
    public static extern IntPtr roaring_bitmap_lazy_xor(IntPtr bitmap1, IntPtr bitmap2);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_lazy_xor_inplace")]
    public static partial void roaring_bitmap_lazy_xor_inplace(IntPtr bitmap1, IntPtr bitmap2);
#else
    [DllImport("roaring")]
    public static extern void roaring_bitmap_lazy_xor_inplace(IntPtr bitmap1, IntPtr bitmap2);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_repair_after_lazy")]
    public static partial IntPtr roaring_bitmap_repair_after_lazy(IntPtr bitmap);
#else
    [DllImport("roaring")]
    public static extern IntPtr roaring_bitmap_repair_after_lazy(IntPtr bitmap);
#endif

    [return: MarshalAs(UnmanagedType.I1)]
#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_intersect")]
    public static partial bool roaring_bitmap_intersect(IntPtr bitmap1, IntPtr bitmap2);
#else
    [DllImport("roaring")]
    public static extern bool roaring_bitmap_intersect(IntPtr bitmap1, IntPtr bitmap2);
#endif

    [return: MarshalAs(UnmanagedType.I1)]
#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_intersect_with_range")]
    public static partial bool roaring_bitmap_intersect_with_range(IntPtr bitmap1, ulong x, ulong y);
#else
    [DllImport("roaring")]
    public static extern bool roaring_bitmap_intersect_with_range(IntPtr bitmap1, ulong x, ulong y);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_jaccard_index")]
    public static partial double roaring_bitmap_jaccard_index(IntPtr bitmap1, IntPtr bitmap2);
#else
    [DllImport("roaring")]
    public static extern double roaring_bitmap_jaccard_index(IntPtr bitmap1, IntPtr bitmap2);
#endif

    [return: MarshalAs(UnmanagedType.I1)]
#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_run_optimize")]
    public static partial bool roaring_bitmap_run_optimize(IntPtr bitmap);
#else
    [DllImport("roaring")]
    public static extern bool roaring_bitmap_run_optimize(IntPtr bitmap);
#endif

    [return: MarshalAs(UnmanagedType.I1)]
#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_remove_run_compression")]
    public static partial bool roaring_bitmap_remove_run_compression(IntPtr bitmap);
#else
    [DllImport("roaring")]
    public static extern bool roaring_bitmap_remove_run_compression(IntPtr bitmap);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_shrink_to_fit")]
    public static partial nuint roaring_bitmap_shrink_to_fit(IntPtr bitmap);
#else
    [DllImport("roaring")]
    public static extern nuint roaring_bitmap_shrink_to_fit(IntPtr bitmap);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_size_in_bytes")]
    public static partial nuint roaring_bitmap_size_in_bytes(IntPtr bitmap);
#else
    [DllImport("roaring")]
    public static extern nuint roaring_bitmap_size_in_bytes(IntPtr bitmap);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_portable_size_in_bytes")]
    public static partial nuint roaring_bitmap_portable_size_in_bytes(IntPtr bitmap);
#else
    [DllImport("roaring")]
    public static extern nuint roaring_bitmap_portable_size_in_bytes(IntPtr bitmap);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_frozen_size_in_bytes")]
    public static partial nuint roaring_bitmap_frozen_size_in_bytes(IntPtr bitmap);
#else
    [DllImport("roaring")]
    public static extern nuint roaring_bitmap_frozen_size_in_bytes(IntPtr bitmap);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_to_uint32_array")]
    public static partial void roaring_bitmap_to_uint32_array(IntPtr bitmap, [Out] uint[] values);
#else
    [DllImport("roaring")]
    public static extern void roaring_bitmap_to_uint32_array(IntPtr bitmap, [Out] uint[] values);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_to_uint32_array")]
    public static partial void roaring_bitmap_to_uint32_array(IntPtr bitmap, uint* values);
#else
    [DllImport("roaring")]
    public static extern void roaring_bitmap_to_uint32_array(IntPtr bitmap, uint* values);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_serialize")]
    public static partial nuint roaring_bitmap_serialize(IntPtr bitmap, [Out] byte[] buffer);
#else
    [DllImport("roaring")]
    public static extern nuint roaring_bitmap_serialize(IntPtr bitmap, [Out] byte[] buffer);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_deserialize")]
    public static partial IntPtr roaring_bitmap_deserialize(byte[] buffer);
#else
    [DllImport("roaring")]
    public static extern IntPtr roaring_bitmap_deserialize(byte[] buffer);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_deserialize_safe")]
    public static partial IntPtr roaring_bitmap_deserialize_safe(byte[] buffer, nuint maxbytes);
#else
    [DllImport("roaring")]
    public static extern IntPtr roaring_bitmap_deserialize_safe(byte[] buffer, nuint maxbytes);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_portable_serialize")]
    public static partial nuint roaring_bitmap_portable_serialize(IntPtr bitmap, [Out] byte[] buffer);
#else
    [DllImport("roaring")]
    public static extern nuint roaring_bitmap_portable_serialize(IntPtr bitmap, [Out] byte[] buffer);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_portable_deserialize")]
    public static partial IntPtr roaring_bitmap_portable_deserialize(byte[] buffer);
#else
    [DllImport("roaring")]
    public static extern IntPtr roaring_bitmap_portable_deserialize(byte[] buffer);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_portable_deserialize_safe")]
    public static partial IntPtr roaring_bitmap_portable_deserialize_safe(byte[] buffer, nuint maxbytes);
#else
    [DllImport("roaring")]
    public static extern IntPtr roaring_bitmap_portable_deserialize_safe(byte[] buffer, nuint maxbytes);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_portable_deserialize_size")]
    public static partial nuint roaring_bitmap_portable_deserialize_size(byte[] buffer, nuint maxbytes);
#else
    [DllImport("roaring")]
    public static extern nuint roaring_bitmap_portable_deserialize_size(byte[] buffer, nuint maxbytes);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_frozen_serialize")]
    public static partial void roaring_bitmap_frozen_serialize(IntPtr bitmap, byte* buffer);
#else
    [DllImport("roaring")]
    public static extern void roaring_bitmap_frozen_serialize(IntPtr bitmap, byte* buffer);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_portable_deserialize_frozen")]
    public static partial IntPtr roaring_bitmap_portable_deserialize_frozen(byte* buffer);
#else
    [DllImport("roaring")]
    public static extern IntPtr roaring_bitmap_portable_deserialize_frozen(byte* buffer);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_frozen_view")]
    public static partial IntPtr roaring_bitmap_frozen_view(byte* buffer, nuint length);
#else
    [DllImport("roaring")]
    public static extern IntPtr roaring_bitmap_frozen_view(byte* buffer, nuint length);
#endif


    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Iterator
    {
        public readonly IntPtr parent;
        public readonly IntPtr container;
        public readonly byte typecode;
        public readonly int container_index;
        public readonly uint highbits;

        public readonly ContainerIt container_it;

        public readonly uint current_value;
        public readonly bool has_value;
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct ContainerIt
    {
        public readonly int index;
    }

    [return: MarshalAs(UnmanagedType.I1)]
#if NET7_0_OR_GREATER
    [LibraryImport("roaring")]
    public static partial bool roaring_iterate(IntPtr bitmap, IteratorDelegate iterator, IntPtr tag);
#else
    [DllImport("roaring")]
    public static extern bool roaring_iterate(IntPtr bitmap, IteratorDelegate iterator, IntPtr tag);
#endif

    public static bool roaring_iterate(IntPtr bitmap, Func<uint, bool> iterator)
    {
        return roaring_iterate(bitmap, (v, _) => iterator(v), IntPtr.Zero);
    }

    public delegate bool IteratorDelegate(uint value, IntPtr tag);

    [return: MarshalAs(UnmanagedType.I1)]
#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_iterate64")]
    // ReSharper disable once InconsistentNaming
    public static partial bool roaring_iterate64(IntPtr bitmap, IteratorDelegate iterator, ulong high_bits, IntPtr tag);
#else
    [DllImport("roaring")]
    // ReSharper disable once InconsistentNaming
    public static extern bool roaring_iterate64(IntPtr bitmap, IteratorDelegate iterator, ulong high_bits, IntPtr tag);
#endif

    // ReSharper disable once InconsistentNaming
    public static bool roaring_iterate64(IntPtr bitmap, Func<uint, bool> iterator, ulong high_bits)
    {
        return roaring_iterate64(bitmap, (v, _) => iterator(v), high_bits, IntPtr.Zero);
    }

    public delegate bool IteratorDelegate64(uint value, IntPtr tag);


#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_iterator_create")]
    public static partial IntPtr roaring_iterator_create(IntPtr bitmap);
#else
    [DllImport("roaring")]
    public static extern IntPtr roaring_iterator_create(IntPtr bitmap);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_iterator_init")]
    public static partial void roaring_iterator_init(IntPtr bitmap, IntPtr iterator);
#else
    [DllImport("roaring")]
    public static extern void roaring_iterator_init(IntPtr bitmap, IntPtr iterator);
#endif

    [return: MarshalAs(UnmanagedType.I1)]
#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_uint32_iterator_advance")]
    public static partial bool roaring_uint32_iterator_advance(IntPtr iterator);
#else
    [DllImport("roaring")]
    public static extern bool roaring_uint32_iterator_advance(IntPtr iterator);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_uint32_iterator_copy")]
    public static partial IntPtr roaring_uint32_iterator_copy(IntPtr iterator);
#else
    [DllImport("roaring")]
    public static extern IntPtr roaring_uint32_iterator_copy(IntPtr iterator);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_uint32_iterator_free")]
    public static partial void roaring_uint32_iterator_free(IntPtr iterator);
#else
    [DllImport("roaring")]
    public static extern void roaring_uint32_iterator_free(IntPtr iterator);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_statistics")]
    public static partial void roaring_bitmap_statistics(IntPtr bitmap, out Statistics stats);
#else
    [DllImport("roaring")]
    public static extern void roaring_bitmap_statistics(IntPtr bitmap, out Statistics stats);
#endif    

    [return: MarshalAs(UnmanagedType.I1)]
#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_internal_validate")]
    public static partial bool roaring_bitmap_internal_validate(IntPtr bitmap, out IntPtr reasonPtr);
#else
    [DllImport("roaring")]
    public static extern bool roaring_bitmap_internal_validate(IntPtr bitmap, out IntPtr reasonPtr);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_add_bulk")]
    public static partial void roaring_bitmap_add_bulk(IntPtr bitmap, IntPtr context, uint value);
#else
    [DllImport("roaring")]
    public static extern void roaring_bitmap_add_bulk(IntPtr bitmap, IntPtr context, uint value);
#endif

    [return: MarshalAs(UnmanagedType.I1)]
#if NET7_0_OR_GREATER
    [LibraryImport("roaring", EntryPoint = "roaring_bitmap_contains_bulk")]
    public static partial bool roaring_bitmap_contains_bulk(IntPtr bitmap, IntPtr context, uint value);
#else
    [DllImport("roaring")]
    public static extern bool roaring_bitmap_contains_bulk(IntPtr bitmap, IntPtr context, uint value);
#endif
}