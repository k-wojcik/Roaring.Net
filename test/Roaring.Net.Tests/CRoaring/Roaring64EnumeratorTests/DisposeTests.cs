using System.Reflection;
using Roaring.Net.CRoaring;
using Roaring.Net.Tests.CRoaring.TestData;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring64EnumeratorTests;

public class DisposeTests
{
    public class Finalizer
    {
        [Fact]
        public void Finalizer_InvokedMoreThanOnce_BlocksRedundantCalls()
        {
            // Arrange
            using Roaring64Bitmap bitmap = SerializationTestBitmap.GetTestBitmap64();
            var enumerator = new Roaring64Enumerator(bitmap.Pointer);

            MethodInfo? finalizer = enumerator.GetType().GetMethod("Finalize", BindingFlags.Instance | BindingFlags.NonPublic);

            // Act && Assert
            Assert.NotNull(finalizer);
            finalizer.Invoke(enumerator, null);
            finalizer.Invoke(enumerator, null);
        }
    }
}