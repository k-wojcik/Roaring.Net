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
        uint[] values = [1, 2, 3, 4, 5, 100, 1000];

        // Act
        using var result = Roaring32Bitmap.FromValues(values);

        // Assert
        Assert.Equal(result.Values, values);
    }

    [Fact]
    public void Values_BitmapIsEmpty_BreaksEnumeration()
    { 
        // Arrange
        uint[] values = [];
        using var result = Roaring32Bitmap.FromValues(values);

        // Act && Assert
        Assert.Empty(result.Values);
    }

    [Fact]
    public void Values_Reset_ThrowsNotSupportedException()
    {
        // Arrange
        uint[] values = [1, 2, 3, 4, 5, 100, 1000];
        using var result = Roaring32Bitmap.FromValues(values);
        using var enumerator = result.Values.GetEnumerator();
        
        // Act && Assert
        Assert.Throws<NotSupportedException>(() => enumerator.Reset());
    }

    [Fact]
    public void Values_ForNonGenericEnumeratorReset_ThrowsNotSupportedException()
    {
        // Arrange
        uint[] values = [1, 2, 3, 4, 5, 100, 1000];
        using var result = Roaring32Bitmap.FromValues(values);
        using var enumerator = result.Values.GetEnumerator();
        IEnumerator enumeratorNonGeneric = enumerator;

        // Act && Assert
        Assert.Throws<NotSupportedException>(() => enumeratorNonGeneric.Reset());
    }

    [Fact]
    public void Values_Current_ReturnsFirstElement()
    {
        // Arrange
        uint[] values = [1, 2, 3, 4, 5, 100, 1000];
        using var result = Roaring32Bitmap.FromValues(values);
        using var enumerator = result.Values.GetEnumerator();

        // Act && Assert
        Assert.Equal(values[0], enumerator.Current);
    }

    [Fact]
    public void Values_ForNonGenericEnumerator_Current_ReturnsFirstElement()
    {
        uint[] values = [1, 2, 3, 4, 5, 100, 1000];
        using var result = Roaring32Bitmap.FromValues(values);
        using var enumerator = result.Values.GetEnumerator();
        IEnumerator enumeratorNonGeneric = enumerator;

        // Act && Assert
        Assert.Equal(values[0], enumeratorNonGeneric.Current);
    }

    [Fact]
    public void Values_MoveNext_ReturnsNextElement()
    {
        // Arrange
        uint[] values = [1, 2, 3, 4, 5, 100, 1000];
        using var result = Roaring32Bitmap.FromValues(values);
        using var enumerator = result.Values.GetEnumerator();

        // Act
        enumerator.MoveNext();
        enumerator.MoveNext();

        // Assert
        Assert.Equal(values[1], enumerator.Current);
    }

    [Fact]
    public void Values_ForNonGenericEnumerator_MoveNext_ReturnsNextElement()
    {
        // Arrange
        uint[] values = [1, 2, 3, 4, 5, 100, 1000];
        using var result = Roaring32Bitmap.FromValues(values);
        using var enumerator = result.Values.GetEnumerator();
        IEnumerator enumeratorNonGeneric = enumerator;

        // Act
        enumeratorNonGeneric.MoveNext();
        enumeratorNonGeneric.MoveNext();

        // Assert
        Assert.Equal(values[1], enumeratorNonGeneric.Current);
    }

    [Fact]
    public void Values_ForNonGenericEnumerable_GetEnumerator_ReturnsEnumerator()
    {
        // Arrange
        uint[] values = [1, 2, 3, 4, 5, 100, 1000];
        using var result = Roaring32Bitmap.FromValues(values);
        var enumerable = (IEnumerable)result.Values;
        
        // Act
        var enumerator = enumerable.GetEnumerator();
        using var enumeratorDisposable = enumerator as IDisposable;

        // Assert
        Assert.Equal(values[0], enumerator.Current);
    }

    [Fact]
    public void Values_Dispose_CurrentThrowsObjectDisposedException()
    {
        // Arrange
        uint[] values = [1, 2, 3, 4, 5, 100, 1000];
        using var result = Roaring32Bitmap.FromValues(values);
        var enumerator = result.Values.GetEnumerator();
        
        // Act
        enumerator.Dispose();
        
        // Assert
        Assert.Throws<ObjectDisposedException>(() => enumerator.Current);
    }
    
    [Fact]
    public void Values_Dispose_MoveNextThrowsObjectDisposedException()
    {
        // Arrange
        uint[] values = [1, 2, 3, 4, 5, 100, 1000];
        using var result = Roaring32Bitmap.FromValues(values);
        var enumerator = result.Values.GetEnumerator();
        
        // Act
        enumerator.Dispose();
        
        // Assert
        Assert.Throws<ObjectDisposedException>(() => enumerator.MoveNext());
    }
    
    [Fact]
    public void Values_DisposeTwice_IgnoresSecondDipose()
    {
        // Arrange
        uint[] values = [1, 2, 3, 4, 5, 100, 1000];
        using var result = Roaring32Bitmap.FromValues(values);
        var enumerator = result.Values.GetEnumerator();
        
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
            uint[] values = [1, 2, 3, 4, 5, 100, 1000];
            using var result = Roaring32Bitmap.FromValues(values);
            var enumerable = result.Values;
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