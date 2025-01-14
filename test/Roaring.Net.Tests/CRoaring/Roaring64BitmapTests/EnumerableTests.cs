using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring64BitmapTests;

public class EnumerableTests
{
    public class Values
    {
        [Theory]
        [InlineTestObject]
        public void Values_BitmapContainsValues_EnumeratesBitmap(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetDefault();

            // Act
            IEnumerable<ulong> actual = testObject.ReadOnlyBitmap.Values;

            // Assert
            Assert.Equal(testObject.Values, actual);
        }

        [Theory]
        [InlineTestObject]
        public void Values_BitmapIsEmpty_BreaksEnumeration(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetEmpty();

            // Act && Assert
            Assert.Empty(testObject.ReadOnlyBitmap.Values);
        }

        [Theory]
        [InlineTestObject]
        public void Values_Reset_ThrowsNotSupportedException(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetDefault();
            using IEnumerator<ulong> enumerator = testObject.ReadOnlyBitmap.Values.GetEnumerator();

            // Act && Assert
            Assert.Throws<NotSupportedException>(() => enumerator.Reset());
        }

        [Theory]
        [InlineTestObject]
        public void Values_Current_ReturnsFirstElement(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetDefault();
            using IEnumerator<ulong> enumerator = testObject.ReadOnlyBitmap.Values.GetEnumerator();

            // Act && Assert
            Assert.Equal(testObject.Values[0], enumerator.Current);
        }

        [Theory]
        [InlineTestObject]
        public void Values_MoveNext_ReturnsNextElement(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetDefault();
            using IEnumerator<ulong> enumerator = testObject.ReadOnlyBitmap.Values.GetEnumerator();

            // Act
            enumerator.MoveNext();
            enumerator.MoveNext();

            // Assert
            Assert.Equal(testObject.Values[1], enumerator.Current);
        }
    }

    public class Enumerator
    {
        [Theory]
        [InlineTestObject]
        public void Values_ForNonGenericEnumerator_Current_ReturnsFirstElement(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetDefault();
            using IEnumerator<ulong> enumerator = testObject.ReadOnlyBitmap.Values.GetEnumerator();
            IEnumerator enumeratorNonGeneric = enumerator;

            // Act && Assert
            Assert.Equal(testObject.Values[0], enumeratorNonGeneric.Current);
        }

        [Theory]
        [InlineTestObject]
        public void Values_ForNonGenericEnumerator_MoveNext_ReturnsNextElement(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetDefault();
            using IEnumerator<ulong> enumerator = testObject.ReadOnlyBitmap.Values.GetEnumerator();
            IEnumerator enumeratorNonGeneric = enumerator;

            // Act
            enumeratorNonGeneric.MoveNext();
            enumeratorNonGeneric.MoveNext();

            // Assert
            Assert.Equal(testObject.Values[1], enumeratorNonGeneric.Current);
        }

        [Theory]
        [InlineTestObject]
        public void Values_ForNonGenericEnumeratorReset_ThrowsNotSupportedException(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetDefault();
            using IEnumerator<ulong> enumerator = testObject.ReadOnlyBitmap.Values.GetEnumerator();
            IEnumerator enumeratorNonGeneric = enumerator;

            // Act && Assert
            Assert.Throws<NotSupportedException>(() => enumeratorNonGeneric.Reset());
        }
    }

    public class Enumerable
    {
        [Theory]
        [InlineTestObject]
        public void Values_ForNonGenericEnumerable_GetEnumerator_ReturnsEnumerator(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetDefault();
            var enumerable = (IEnumerable)testObject.ReadOnlyBitmap.Values;

            // Act
            IEnumerator enumerator = enumerable.GetEnumerator();
            using var enumeratorDisposable = enumerator as IDisposable;

            // Assert
            Assert.Equal(testObject.Values[0], enumerator.Current);
        }
    }

    public class Dispose
    {
        [Theory]
        [InlineTestObject]
        public void Values_Dispose_CurrentThrowsObjectDisposedException(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetDefault();
            IEnumerator<ulong> enumerator = testObject.ReadOnlyBitmap.Values.GetEnumerator();

            // Act
            enumerator.Dispose();

            // Assert
            Assert.Throws<ObjectDisposedException>(() => enumerator.Current);
        }

        [Theory]
        [InlineTestObject]
        public void Values_Dispose_MoveNextThrowsObjectDisposedException(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetDefault();
            IEnumerator<ulong> enumerator = testObject.ReadOnlyBitmap.Values.GetEnumerator();

            // Act
            enumerator.Dispose();

            // Assert
            Assert.Throws<ObjectDisposedException>(() => enumerator.MoveNext());
        }

        [Theory]
        [InlineTestObject]
        public void Values_DisposeTwice_IgnoresSecondDispose(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetDefault();
            IEnumerator<ulong> enumerator = testObject.ReadOnlyBitmap.Values.GetEnumerator();

            // Act
            enumerator.Dispose();
            enumerator.Dispose();

            // Assert
            Assert.Throws<ObjectDisposedException>(() => enumerator.MoveNext());
        }
    }
}