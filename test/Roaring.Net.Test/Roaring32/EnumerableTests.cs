using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace Roaring.Test.Roaring32;

public class EnumerableTests
{
    [Fact]
    public void Values_BitmapContainsValues_EnumeratesBitmap()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetDefault();

        // Act
        var actual = testObject.Bitmap.Values;

        // Assert
        Assert.Equal(testObject.Values, actual);
    }

    [Fact]
    public void Values_BitmapIsEmpty_BreaksEnumeration()
    { 
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetEmpty();

        // Act && Assert
        Assert.Empty(testObject.Bitmap.Values);
    }

    [Fact]
    public void Values_Reset_ThrowsNotSupportedException()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetDefault();
        using var enumerator = testObject.Bitmap.Values.GetEnumerator();
        
        // Act && Assert
        Assert.Throws<NotSupportedException>(() => enumerator.Reset());
    }

    [Fact]
    public void Values_ForNonGenericEnumeratorReset_ThrowsNotSupportedException()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetDefault();
        using var enumerator = testObject.Bitmap.Values.GetEnumerator();
        IEnumerator enumeratorNonGeneric = enumerator;

        // Act && Assert
        Assert.Throws<NotSupportedException>(() => enumeratorNonGeneric.Reset());
    }

    [Fact]
    public void Values_Current_ReturnsFirstElement()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetDefault();
        using var enumerator = testObject.Bitmap.Values.GetEnumerator();

        // Act && Assert
        Assert.Equal(testObject.Values[0], enumerator.Current);
    }

    [Fact]
    public void Values_ForNonGenericEnumerator_Current_ReturnsFirstElement()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetDefault();
        using var enumerator = testObject.Bitmap.Values.GetEnumerator();
        IEnumerator enumeratorNonGeneric = enumerator;

        // Act && Assert
        Assert.Equal(testObject.Values[0], enumeratorNonGeneric.Current);
    }

    [Fact]
    public void Values_MoveNext_ReturnsNextElement()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetDefault();
        using var enumerator = testObject.Bitmap.Values.GetEnumerator();

        // Act
        enumerator.MoveNext();
        enumerator.MoveNext();

        // Assert
        Assert.Equal(testObject.Values[1], enumerator.Current);
    }

    [Fact]
    public void Values_ForNonGenericEnumerator_MoveNext_ReturnsNextElement()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetDefault();
        using var enumerator = testObject.Bitmap.Values.GetEnumerator();
        IEnumerator enumeratorNonGeneric = enumerator;

        // Act
        enumeratorNonGeneric.MoveNext();
        enumeratorNonGeneric.MoveNext();

        // Assert
        Assert.Equal(testObject.Values[1], enumeratorNonGeneric.Current);
    }

    [Fact]
    public void Values_ForNonGenericEnumerable_GetEnumerator_ReturnsEnumerator()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetDefault();
        var enumerable = (IEnumerable)testObject.Bitmap.Values;
        
        // Act
        var enumerator = enumerable.GetEnumerator();
        using var enumeratorDisposable = enumerator as IDisposable;

        // Assert
        Assert.Equal(testObject.Values[0], enumerator.Current);
    }

    [Fact]
    public void Values_Dispose_CurrentThrowsObjectDisposedException()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetDefault();
        var enumerator = testObject.Bitmap.Values.GetEnumerator();
        
        // Act
        enumerator.Dispose();
        
        // Assert
        Assert.Throws<ObjectDisposedException>(() => enumerator.Current);
    }
    
    [Fact]
    public void Values_Dispose_MoveNextThrowsObjectDisposedException()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetDefault();
        var enumerator = testObject.Bitmap.Values.GetEnumerator();
        
        // Act
        enumerator.Dispose();
        
        // Assert
        Assert.Throws<ObjectDisposedException>(() => enumerator.MoveNext());
    }
    
    [Fact]
    public void Values_DisposeTwice_IgnoresSecondDipose()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetDefault();
        var enumerator = testObject.Bitmap.Values.GetEnumerator();
        
        // Act
        enumerator.Dispose();
        enumerator.Dispose();
        
        // Assert
        Assert.Throws<ObjectDisposedException>(() => enumerator.MoveNext());
    }
    
    [Fact]
    public void Values_Destructor_InvokesDispose()
    {
        // Arrange
        WeakReference<IEnumerable<uint>> weakReference = null;
        var dispose = () => 
        {
            using var testObject = Roaring32BitmapTestObject.GetDefault();
            var enumerable = testObject.Bitmap.Values;
            weakReference = new WeakReference<IEnumerable<uint>>(enumerable, true);
        };
        
        // Act
        dispose();
        GC.Collect(0, GCCollectionMode.Forced);
        GC.WaitForPendingFinalizers();
    
        // Assert
        weakReference.TryGetTarget(out var target);
        Assert.Throws<ObjectDisposedException>(() => target?.GetEnumerator().Current);
    }
}