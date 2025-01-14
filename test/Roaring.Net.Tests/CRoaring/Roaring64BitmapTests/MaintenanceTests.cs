using System.Linq;
using Roaring.Net.CRoaring;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring64BitmapTests;

public class MaintenanceTests
{
    public class Optimize
    {
        [Fact]
        public void Optimize_BitmapCanBeOptimized_OptimizesBitmap()
        {
            // Arrange
            ulong[] values = [1, 2, 3, 4, 6, 7, 999991, 999992, 999993, 999994, 999996, 999997];
            using var bitmap = new Roaring64Bitmap(values);

            // Act
            Statistics64 statisticsBeforeOptimize = bitmap.GetStatistics();
            bitmap.Optimize();
            Statistics64 statisticsAfterOptimize = bitmap.GetStatistics();

            // Assert
            Assert.Equal(2U, statisticsBeforeOptimize.ArrayContainerCount);
            Assert.Equal(0U, statisticsAfterOptimize.ArrayContainerCount);

            Assert.Equal(0U, statisticsBeforeOptimize.RunContainerCount);
            Assert.Equal(2U, statisticsAfterOptimize.RunContainerCount);
        }

        [Fact]
        public void Optimize_BitmapContainsRunCompression_ReturnsTrue()
        {
            // Arrange
            using var bitmap = new Roaring64Bitmap();
            bitmap.AddMany(Enumerable.Range(10, 1_000).Select(x => (ulong)x).ToArray());

            // Act
            var result = bitmap.Optimize();
            Statistics64 statisticsAfterOptimize = bitmap.GetStatistics();

            // Assert
            Assert.True(result);
            Assert.Equal(1U, statisticsAfterOptimize.RunContainerCount);
        }

        [Fact]
        public void Optimize_BitmapNotContainRunCompression_ReturnsFalse()
        {
            // Arrange
            using var bitmap = new Roaring64Bitmap();
            bitmap.AddMany(Enumerable.Range(1, 2).Select(x => (ulong)x).ToArray());

            // Act
            var result = bitmap.Optimize();
            Statistics64 statisticsAfterOptimize = bitmap.GetStatistics();

            // Assert
            Assert.False(result);
            Assert.Equal(0U, statisticsAfterOptimize.RunContainerCount);
        }
    }

    public class IsValid
    {
        [Theory]
        [InlineTestObject]
        public void IsValid_BitmapIsValid_ReturnsTrue(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetDefault();

            // Act
            var actual = testObject.ReadOnlyBitmap.IsValid();

            // Assert
            Assert.True(actual);
        }
    }
}