using System.Linq;
using Roaring.Net.CRoaring;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring32BitmapTests;

public class MaintenanceTests
{
    public class RepairAfterLazy
    {
        [Fact]
        public void RepairAfterLazy_BitmapIsInconsistent_RepairsBitmap()
        {
            // Arrange
            using Roaring32BitmapTestObject testObject1 = Roaring32BitmapTestObjectFactory.Default.GetForRange(0, 10_000);
            using Roaring32BitmapTestObject testObject2 = Roaring32BitmapTestObjectFactory.Default.GetForRange(uint.MaxValue - 10_000, uint.MaxValue);
            using Roaring32BitmapTestObject testObject3 = Roaring32BitmapTestObjectFactory.Default.GetForRange(uint.MaxValue - 10_000, uint.MaxValue);
            using Roaring32Bitmap temp = testObject1.Bitmap.LazyOr(testObject2.Bitmap, false);

            // Act
            using Roaring32Bitmap actual = temp.LazyOr(testObject3.Bitmap, false);

            var countBeforeRepair = actual.Count;
            actual.RepairAfterLazy();
            var countAfterRepair = actual.Count;

            // Assert
            var expectedCount = (uint)testObject1.Values.Union(testObject2.Values).Union(testObject3.Values).Count();
            Assert.NotEqual(expectedCount, countBeforeRepair);
            Assert.Equal(expectedCount, countAfterRepair);
        }
    }

    public class Optimize
    {
        [Fact]
        public void Optimize_BitmapCanBeOptimized_OptimizesBitmap()
        {
            // Arrange
            uint[] values = [1, 2, 3, 4, 6, 7, 999991, 999992, 999993, 999994, 999996, 999997];
            using var bitmap = new Roaring32Bitmap(values);

            // Act
            Statistics statisticsBeforeOptimize = bitmap.GetStatistics();
            bitmap.Optimize();
            Statistics statisticsAfterOptimize = bitmap.GetStatistics();

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
            using var bitmap = new Roaring32Bitmap();
            bitmap.AddMany(Enumerable.Range(10, 1_000).Select(x => (uint)x).ToArray());

            // Act
            var result = bitmap.Optimize();
            Statistics statisticsAfterOptimize = bitmap.GetStatistics();

            // Assert
            Assert.True(result);
            Assert.Equal(1U, statisticsAfterOptimize.RunContainerCount);
        }

        [Fact]
        public void Optimize_BitmapNotContainRunCompression_ReturnsFalse()
        {
            // Arrange
            using var bitmap = new Roaring32Bitmap();
            bitmap.AddMany(Enumerable.Range(1, 2).Select(x => (uint)x).ToArray());

            // Act
            var result = bitmap.Optimize();
            Statistics statisticsAfterOptimize = bitmap.GetStatistics();

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
            using var bitmap = new Roaring32Bitmap();
            bitmap.AddMany(Enumerable.Range(10, 1_000).Select(x => (uint)x).ToArray());

            // Act
            bitmap.Optimize();
            Statistics statisticsBeforeRemoveRunCompression = bitmap.GetStatistics();
            var result = bitmap.RemoveRunCompression();
            Statistics statisticsBeforeAfterRunCompression = bitmap.GetStatistics();

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
            using var bitmap = new Roaring32Bitmap();
            bitmap.AddMany(Enumerable.Range(1, 2).Select(x => (uint)x).ToArray());

            // Act
            bitmap.Optimize();
            Statistics statisticsBeforeRemoveRunCompression = bitmap.GetStatistics();
            var result = bitmap.RemoveRunCompression();
            Statistics statisticsBeforeAfterRunCompression = bitmap.GetStatistics();

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
            using var bitmap = new Roaring32Bitmap();
            bitmap.AddRange(0, 1_000);
            bitmap.RemoveRange(0, 1_0000);

            // Act
            var actual = bitmap.ShrinkToFit();

            // Assert
            Assert.Equal(0x16U, actual);

            actual = bitmap.ShrinkToFit();
            Assert.Equal(0x0U, actual);
        }
    }

    public class IsValid
    {
        [Theory]
        [InlineTestObject]
        public void IsValid_BitmapIsValid_ReturnsTrue(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetDefault();

            // Act
            var actual = testObject.ReadOnlyBitmap.IsValid();

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineMatrixTestObject]
        public void IsValid_BitmapIsNotValid_ReturnsFalse(TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject1 = matrix.X.GetForRange(0, 10_000);
            using IRoaring32BitmapTestObject testObject2 = matrix.Y.GetForRange(uint.MaxValue - 10_000, uint.MaxValue);
            using IRoaring32BitmapTestObject testObject3 = matrix.Z.GetForRange(uint.MaxValue - 10_000, uint.MaxValue);
            using Roaring32Bitmap temp = testObject1.ReadOnlyBitmap.LazyOr(testObject2.Bitmap, false);
            using Roaring32Bitmap bitmap = temp.LazyOr(testObject3.Bitmap, false);

            // Act
            var actual = bitmap.IsValid();

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineTestObject]
        public void IsValid_WithReason_BitmapIsValid_ReturnsTrueAndNullReason(IRoaring32BitmapTestObjectFactory factory)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject = factory.GetDefault();

            // Act
            var actual = testObject.ReadOnlyBitmap.IsValid(out var reason);

            // Assert
            Assert.True(actual);
            Assert.Null(reason);
        }

        [Theory]
        [InlineMatrixTestObject]
        public void IsValid_WithReason_BitmapIsNotValid_ReturnsFalseAndReason(TestObjectMatrix<IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory, IRoaring32BitmapTestObjectFactory> matrix)
        {
            // Arrange
            using IRoaring32BitmapTestObject testObject1 = matrix.X.GetForRange(0, 10_000);
            using IRoaring32BitmapTestObject testObject2 = matrix.Y.GetForRange(uint.MaxValue - 10_000, uint.MaxValue);
            using IRoaring32BitmapTestObject testObject3 = matrix.Z.GetForRange(uint.MaxValue - 10_000, uint.MaxValue);
            using Roaring32Bitmap temp = testObject1.ReadOnlyBitmap.LazyOr(testObject2.Bitmap, false);
            using Roaring32Bitmap bitmap = temp.LazyOr(testObject3.Bitmap, false);

            // Act
            var actual = bitmap.IsValid(out var reason);

            // Assert
            Assert.False(actual);
            Assert.Equal("cardinality is incorrect", reason);
        }
    }

    public class IsCopyOnWrite
    {
        [Fact]
        public void IsCopyOnWrite_Default_ReturnsFalse()
        {
            // Arrange
            using Roaring32BitmapTestObject testObject = Roaring32BitmapTestObjectFactory.Default.GetDefault();

            // Act
            var actual = testObject.Bitmap.IsCopyOnWrite;

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void IsCopyOnWrite_CopyOnWriteEnabled_ReturnsTrue()
        {
            // Arrange
            using Roaring32BitmapTestObject testObject = Roaring32BitmapTestObjectFactory.Default.GetDefault();

            // Act
            testObject.Bitmap.SetCopyOnWrite(true);
            var actual = testObject.Bitmap.IsCopyOnWrite;

            // Assert
            Assert.True(actual);
        }
    }

    public class SetCopyOnWrite
    {
        [Fact]
        public void SetCopyOnWrite_WithTrue_EnablesCopyOnWrite()
        {
            // Arrange
            using Roaring32BitmapTestObject testObject = Roaring32BitmapTestObjectFactory.Default.GetDefault();

            // Act
            testObject.Bitmap.SetCopyOnWrite(true);

            // Assert
            Assert.True(testObject.Bitmap.IsCopyOnWrite);
        }

        [Fact]
        public void SetCopyOnWrite_WithFalse_DisablesCopyOnWrite()
        {
            // Arrange
            using Roaring32BitmapTestObject testObject = Roaring32BitmapTestObjectFactory.Default.GetDefault();

            // Act
            testObject.Bitmap.SetCopyOnWrite(false);

            // Assert
            Assert.False(testObject.Bitmap.IsCopyOnWrite);
        }
    }
}