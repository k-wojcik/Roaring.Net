using System.Reflection;
using Roaring.Net.CRoaring;
using Roaring.Net.Tests.CRoaring.Roaring32BitmapTests;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.BulkContextTests;

public class DisposeTests
{
    public class Dispose
    {
        [Fact]
        public void Dispose_InvokedMoreThanOnce_BlocksRedundantCalls()
        {   
            // Arrange
            using var testObject = Roaring32BitmapTestObjectFactory.Default.GetEmpty();
            var context = BulkContext.For(testObject.Bitmap);

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
            using var testObject = Roaring32BitmapTestObjectFactory.Default.GetEmpty();
            using var context = BulkContext.For(testObject.Bitmap);
            var finalizer = context.GetType().GetMethod("Finalize", BindingFlags.Instance | BindingFlags.NonPublic);
            
            // Act && Assert
            Assert.NotNull(finalizer);
            finalizer.Invoke(context, null);
            finalizer.Invoke(context, null);
        }
    }
}