using System.Linq;
using Xunit;

namespace Roaring.Test.Roaring32;

public class MaintenanceTests
{
    public class RepairAfterLazy
    {
        [Fact]
        public void RepairAfterLazy_BitmapIsInconsistent_RepairsBitmap()
        {
            // Arrange
            using var testObject1 = Roaring32BitmapTestObject.GetForRange(0, 10_000);
            using var testObject2 = Roaring32BitmapTestObject.GetForRange(uint.MaxValue - 10_000, uint.MaxValue);
            using var testObject3 = Roaring32BitmapTestObject.GetForRange(uint.MaxValue - 10_000, uint.MaxValue);
       
            // Act
            using var temp = testObject1.Bitmap.LazyOr(testObject2.Bitmap, false);
            using var actual = temp.LazyOr(testObject3.Bitmap, false);

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
            var statisticsBeforeOptimize = bitmap.GetStatistics();
            bitmap.Optimize();
            var statisticsAfterOptimize = bitmap.GetStatistics();

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
            var statisticsAfterOptimize = bitmap.GetStatistics();

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
            var statisticsAfterOptimize = bitmap.GetStatistics();

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
            bitmap.AddMany(Enumerable.Range(10, 1_000).Select(x=> (uint)x).ToArray());
        
            // Act
            bitmap.Optimize();
            var statisticsBeforeRemoveRunCompression = bitmap.GetStatistics();
            var result = bitmap.RemoveRunCompression();
            var statisticsBeforeAfterRunCompression = bitmap.GetStatistics();
        
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
            bitmap.AddMany(Enumerable.Range(1, 2).Select(x=> (uint)x).ToArray());
        
            // Act
            bitmap.Optimize();
            var statisticsBeforeRemoveRunCompression = bitmap.GetStatistics();
            var result = bitmap.RemoveRunCompression();
            var statisticsBeforeAfterRunCompression = bitmap.GetStatistics();
        
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
        }
    }
}