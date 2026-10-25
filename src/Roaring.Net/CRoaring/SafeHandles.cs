using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Roaring.Net.CRoaring;

internal sealed class SafeRoaring32BitmapHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    internal SafeRoaring32BitmapHandle(IntPtr handle) : base(true)
    {
        SetHandle(handle);
    }

    protected override bool ReleaseHandle()
    {
        NativeMethods.roaring_bitmap_free(handle);
        return true;
    }
}

internal sealed class SafeRoaring64BitmapHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    internal SafeRoaring64BitmapHandle(IntPtr handle) : base(true)
    {
        SetHandle(handle);
    }

    protected override bool ReleaseHandle()
    {
        NativeMethods.roaring64_bitmap_free(handle);
        return true;
    }
}

internal sealed class SafeRoaring32IteratorHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    private SafeRoaring32BitmapHandle? _bitmap;
    private SafeRoaring32MemoryLease? _memoryLease;
    private object? _owner;
    private bool _bitmapReferenceAdded;

    internal SafeRoaring32IteratorHandle(IntPtr handle, SafeRoaring32BitmapHandle bitmap, SafeRoaring32MemoryLease? memoryLease, object? owner) : base(true)
    {
        try
        {
            bitmap.DangerousAddRef(ref _bitmapReferenceAdded);
        }
        catch
        {
            NativeMethods.roaring_uint32_iterator_free(handle);
            memoryLease?.Dispose();
            throw;
        }

        _bitmap = bitmap;
        _memoryLease = memoryLease;
        _owner = owner;
        SetHandle(handle);
    }

    protected override bool ReleaseHandle()
    {
        NativeMethods.roaring_uint32_iterator_free(handle);

        if (_bitmapReferenceAdded)
        {
            _bitmapReferenceAdded = false;
            _bitmap!.DangerousRelease();
        }

        _bitmap = null;
        _memoryLease?.Dispose();
        _memoryLease = null;
        GC.KeepAlive(_owner);
        _owner = null;
        return true;
    }
}

internal sealed class SafeRoaring64IteratorHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    private SafeRoaring64BitmapHandle? _bitmap;
    private SafeRoaring64MemoryLease? _memoryLease;
    private object? _owner;
    private bool _bitmapReferenceAdded;

    internal SafeRoaring64IteratorHandle(IntPtr handle, SafeRoaring64BitmapHandle bitmap, SafeRoaring64MemoryLease? memoryLease, object? owner) : base(true)
    {
        try
        {
            bitmap.DangerousAddRef(ref _bitmapReferenceAdded);
        }
        catch
        {
            NativeMethods.roaring64_iterator_free(handle);
            memoryLease?.Dispose();
            throw;
        }

        _bitmap = bitmap;
        _memoryLease = memoryLease;
        _owner = owner;
        SetHandle(handle);
    }

    protected override bool ReleaseHandle()
    {
        NativeMethods.roaring64_iterator_free(handle);

        if (_bitmapReferenceAdded)
        {
            _bitmapReferenceAdded = false;
            _bitmap!.DangerousRelease();
        }

        _bitmap = null;
        _memoryLease?.Dispose();
        _memoryLease = null;
        GC.KeepAlive(_owner);
        _owner = null;
        return true;
    }
}

internal sealed class SafeBulkContextHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    internal SafeBulkContextHandle(IntPtr handle) : base(true)
    {
        SetHandle(handle);
    }

    protected override bool ReleaseHandle()
    {
        Marshal.FreeHGlobal(handle);
        return true;
    }
}

internal sealed unsafe class SafeRoaring32MemoryHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    internal nuint Size { get; }

    internal SafeRoaring32MemoryHandle(IntPtr handle, nuint size) : base(true)
    {
        Size = size;
        SetHandle(handle);
    }

    protected override bool ReleaseHandle()
    {
        NativeMemory.AlignedFree((void*)handle);
        GC.RemoveMemoryPressure((long)Size);
        return true;
    }
}

internal sealed unsafe class SafeRoaring64MemoryHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    internal nuint Size { get; }

    internal SafeRoaring64MemoryHandle(IntPtr handle, nuint size) : base(true)
    {
        Size = size;
        SetHandle(handle);
    }

    protected override bool ReleaseHandle()
    {
        NativeMemory.AlignedFree((void*)handle);
        GC.RemoveMemoryPressure((long)Size);
        return true;
    }
}

internal sealed class SafeRoaring32MemoryLease : SafeHandle
{
    private Roaring32BitmapMemory? _memory;
    private SafeRoaring32BitmapHandle? _viewHandle;

    internal SafeRoaring32MemoryLease(Roaring32BitmapMemory memory, SafeRoaring32BitmapHandle? viewHandle) : base(IntPtr.Zero, true)
    {
        _memory = memory;
        _viewHandle = viewHandle;
    }

    public override bool IsInvalid => false;

    protected override bool ReleaseHandle()
    {
        _viewHandle?.Dispose();
        _viewHandle = null;
        _memory?.ReleaseLease();
        _memory = null;
        return true;
    }
}

internal sealed class SafeRoaring64MemoryLease : SafeHandle
{
    private Roaring64BitmapMemory? _memory;
    private SafeRoaring64BitmapHandle? _viewHandle;

    internal SafeRoaring64MemoryLease(Roaring64BitmapMemory memory, SafeRoaring64BitmapHandle? viewHandle) : base(IntPtr.Zero, true)
    {
        _memory = memory;
        _viewHandle = viewHandle;
    }

    public override bool IsInvalid => false;

    protected override bool ReleaseHandle()
    {
        _viewHandle?.Dispose();
        _viewHandle = null;
        _memory?.ReleaseLease();
        _memory = null;
        return true;
    }
}
