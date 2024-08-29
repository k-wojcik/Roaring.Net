using System.Reflection;
using Roaring.Net.CRoaring;
using Roaring.Net.Tests.CRoaring.TestData;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring32EnumeratorTests;

public class DisposeTests
{
    public class Finalizer
    {
        [Fact]
        public void Finalizer_InvokedMoreThanOnce_BlocksRedundantCalls()
        {   
            // Arrange
            using var bitmap = SerializationTestBitmap.GetTestBitmap();
            var enumerator = new Roaring32Enumerator(bitmap.Pointer);
            
            var finalizer = enumerator.GetType().GetMethod("Finalize", BindingFlags.Instance | BindingFlags.NonPublic);
            
            // Act && Assert
            Assert.NotNull(finalizer);
            finalizer.Invoke(enumerator, null);
            finalizer.Invoke(enumerator, null);
        }
    }
}