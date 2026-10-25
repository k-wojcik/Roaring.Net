using System;
using System.Reflection;
using Roaring.Net.CRoaring;
using Xunit;

#pragma warning disable CS0618

namespace Roaring.Net.Tests.CRoaring.Roaring64BitmapTests;

public class LegacyPointerTests
{
    [Fact]
    public void Pointer_IsMarkedObsoleteProtectedInternalField()
    {
        FieldInfo? field = typeof(Roaring64BitmapBase).GetField("Pointer", BindingFlags.Instance | BindingFlags.NonPublic);

        Assert.NotNull(field);
        Assert.Equal(typeof(IntPtr), field.FieldType);
        Assert.True(field.IsFamilyOrAssembly);
        Assert.NotNull(field.GetCustomAttribute<ObsoleteAttribute>());
    }

    [Fact]
    public void Pointer_AfterConstruction_MatchesCurrentHandle()
    {
        using var bitmap = new Roaring64Bitmap(new ulong[] { 1, 2, 3 });

        Assert.Equal(bitmap.Handle.DangerousGetHandle(), bitmap.Pointer);
    }

    [Fact]
    public void Pointer_AfterHandleReplacement_MatchesCurrentHandle()
    {
        using var bitmap = new Roaring64Bitmap(new ulong[] { 1, 2, 3 });

        bitmap.AddOffset(1);

        Assert.Equal(bitmap.Handle.DangerousGetHandle(), bitmap.Pointer);
    }

    [Fact]
    public void Pointer_AfterDispose_IsZero()
    {
        var bitmap = new Roaring64Bitmap(new ulong[] { 1, 2, 3 });

        bitmap.Dispose();

        Assert.Equal(IntPtr.Zero, bitmap.Pointer);
    }
}
