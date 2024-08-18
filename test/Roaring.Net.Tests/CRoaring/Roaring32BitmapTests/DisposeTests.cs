using System;
using Roaring.Net.CRoaring;
using Roaring.Net.Tests.CRoaring.TestData;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring32BitmapTests;

public class DisposeTests
{
    public class Dispose
    {
        [Fact]
        public void Dispose_InvokedMoreThanOnce_BlocksRedundantCalls()
        {   
            // Arrange
            var bitmap = SerializationTestBitmap.GetTestBitmap();
            
            // Act && Assert
            bitmap.Dispose();
            bitmap.Dispose();
        }
    }
}