using System.Reflection;
using Roaring.Net.CRoaring;
using Roaring.Net.Tests.CRoaring.Roaring64BitmapTests;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.BulkContext64Tests;

public class DisposeTests
{
    public class Dispose
    {
        [Fact]
        public void Dispose_InvokedMoreThanOnce_BlocksRedundantCalls()
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetEmpty();
            var context = BulkContext64.For(testObject.Bitmap);

            // Act && Assert
            context.Dispose();
            context.Dispose();
        }
    }

    public class Finalizer
    {
        [Fact]
        public void Finalizer_InvokedMoreThanOnce_BlocksRedundantCalls()
        {
            // Arrange
            using Roaring64BitmapTestObject testObject = Roaring64BitmapTestObjectFactory.Default.GetEmpty();
            using var context = BulkContext64.For(testObject.Bitmap);
            MethodInfo? finalizer = context.GetType().GetMethod("Finalize", BindingFlags.Instance | BindingFlags.NonPublic);

            // Act && Assert
            Assert.NotNull(finalizer);
            finalizer.Invoke(context, null);
            finalizer.Invoke(context, null);
        }
    }
}