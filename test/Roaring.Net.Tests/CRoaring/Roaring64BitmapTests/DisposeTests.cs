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

    public class Lifetime
    {
        [Fact]
        public void FromBitmap_SourceBitmapDisposed_ResultStillContainsValues()
        {
            // Arrange
            var source = new Roaring32Bitmap(new uint[] { 1, 2, 3 });
            using var bitmap = Roaring64Bitmap.FromBitmap(source);

            // Act
            source.Dispose();

            // Assert
            Assert.Equal(new ulong[] { 1, 2, 3 }, bitmap.ToArray());
        }
    }
}