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

    public class RemoveRunCompression
    {
        [Fact]
        public void RemoveRunCompression_BitmapContainsRunCompression_RemovesRunCompressionFromBitmapAndReturnsTrue()
        {
            // Arrange
            using var bitmap = new Roaring64Bitmap();
            bitmap.AddMany([.. Enumerable.Range(10, 1_000).Select(x => (ulong)x)]);

            // Act
            bitmap.Optimize();
            Statistics64 statisticsBeforeRemoveRunCompression = bitmap.GetStatistics();
            var result = bitmap.RemoveRunCompression();
            Statistics64 statisticsBeforeAfterRunCompression = bitmap.GetStatistics();

            // Assert
            Assert.True(result);
            Assert.Equal(1U, statisticsBeforeRemoveRunCompression.ContainerCount);
            Assert.Equal(0U, statisticsBeforeRemoveRunCompression.ArrayContainerCount);
            Assert.Equal(1U, statisticsBeforeRemoveRunCompression.RunContainerCount);
            Assert.Equal(0U, statisticsBeforeRemoveRunCompression.BitsetContainerCount);

            Assert.Equal(1U, statisticsBeforeAfterRunCompression.ContainerCount);
            Assert.Equal(1U, statisticsBeforeAfterRunCompression.ArrayContainerCount);
            Assert.Equal(0U, statisticsBeforeAfterRunCompression.RunContainerCount);
            Assert.Equal(0U, statisticsBeforeAfterRunCompression.BitsetContainerCount);
        }

        [Fact]
        public void RemoveRunCompression_BitmapNotContainRunCompression_ReturnsFalse()
        {
            // Arrange
            using var bitmap = new Roaring64Bitmap();
            bitmap.AddMany([.. Enumerable.Range(1, 2).Select(x => (ulong)x)]);

            // Act
            bitmap.Optimize();
            Statistics64 statisticsBeforeRemoveRunCompression = bitmap.GetStatistics();
            var result = bitmap.RemoveRunCompression();
            Statistics64 statisticsBeforeAfterRunCompression = bitmap.GetStatistics();

            // Assert
            Assert.False(result);
            Assert.Equal(0U, statisticsBeforeAfterRunCompression.RunContainerCount);

            Assert.Equal(statisticsBeforeAfterRunCompression.ContainerCount, statisticsBeforeRemoveRunCompression.ContainerCount);
            Assert.Equal(statisticsBeforeAfterRunCompression.ArrayContainerCount, statisticsBeforeRemoveRunCompression.ArrayContainerCount);
            Assert.Equal(statisticsBeforeAfterRunCompression.RunContainerCount, statisticsBeforeRemoveRunCompression.RunContainerCount);
            Assert.Equal(statisticsBeforeAfterRunCompression.BitsetContainerCount, statisticsBeforeRemoveRunCompression.BitsetContainerCount);
        }
    }

    public class ShrinkToFit
    {
        [Fact]
        public void ShrinkToFit_BitmapCanBeShrink_ReturnsSavedBytes()
        {
            // Arrange
            using var bitmap = new Roaring64Bitmap();
            bitmap.AddRange(0, 1_000);
            bitmap.RemoveRange(0, 1_0000);

            // Act
            var actual = bitmap.ShrinkToFit();

            // Assert
            Assert.Equal(0x30U, actual);

            actual = bitmap.ShrinkToFit();
            Assert.Equal(0x0U, actual);
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

        [Theory]
        [InlineTestObject]
        public void IsValid_WithReason_BitmapIsValid_ReturnsTrueAndNullReason(IRoaring64BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring64BitmapTestObject testObject = factory.GetDefault();

            // Act
            var actual = testObject.ReadOnlyBitmap.IsValid(out var reason);

            // Assert
            Assert.True(actual);
            Assert.Null(reason);
        }
    }
}