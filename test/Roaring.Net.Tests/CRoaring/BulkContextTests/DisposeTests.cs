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
}