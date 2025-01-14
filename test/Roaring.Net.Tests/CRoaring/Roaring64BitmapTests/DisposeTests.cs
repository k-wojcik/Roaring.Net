using Roaring.Net.CRoaring;
using Roaring.Net.Tests.CRoaring.TestData;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring64BitmapTests;

public class DisposeTests
{
    public class Dispose
    {
        [Fact]
        public void Dispose_InvokedMoreThanOnce_BlocksRedundantCalls()
        {
            // Arrange
            Roaring64Bitmap bitmap = SerializationTestBitmap.GetTestBitmap64();

            // Act && Assert
            bitmap.Dispose();
            bitmap.Dispose();
        }
    }
}