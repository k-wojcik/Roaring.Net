using System.Linq;
using Roaring.Net.CRoaring;
using Xunit;

namespace Roaring.Net.Tests.CRoaring.Roaring32BitmapTests;

public class StatisticsTests
{
    [Theory]
    [InlineTestObject]
    public void GetStatistics_ForTestData_ReturnsCount(IRoaring32BitmapTestObjectFactory factory)
    {
        // Arrange
        uint[] values = [1, 2, 3, 4, 6, 7, 999991, 999992, 999993, 999994, 999996, 999997];
        using var testObject = factory.GetFromValues(values);
        
        // Act
        var actual = testObject.ReadOnlyBitmap.GetStatistics();

        // Assert
        Assert.Equal((ulong)values.Length, actual.Count);
    }
    
    [Theory]
    [InlineTestObject]
    public void GetStatistics_ForTestData_ReturnsMinMax(IRoaring32BitmapTestObjectFactory factory)
    {
        // Arrange
        uint[] values = [1, 2, 3, 4, 6, 7, 999991, 999992, 999993, 999994, 999996, 999997];
        using var testObject = factory.GetFromValues(values);
        
        // Act
        var actual = testObject.ReadOnlyBitmap.GetStatistics();

        // Assert
        Assert.Equal(1U, actual.MinValue);
        Assert.Equal(999997U, actual.MaxValue);
    }
    
    [Theory]
    [InlineTestObject]
    public void GetStatistics_ForEmpty_ReturnsZeroForContainerCount(IRoaring32BitmapTestObjectFactory factory)
    {
        // Arrange
        using var testObject = factory.GetEmpty();
        
        // Act
        var actual = testObject.ReadOnlyBitmap.GetStatistics();

        // Assert
        Assert.Equal(0U, actual.ContainerCount);
        Assert.Equal(0U, actual.ArrayContainerCount);
        Assert.Equal(0U, actual.RunContainerCount);
        Assert.Equal(0U, actual.BitsetContainerCount);
    }
    
    [Fact]
    public void GetStatistics_ForArrayContainerTestData_ReturnsContainerCount()
    {
        // Arrange
        uint[] values = [1, 3, 4, 7, 999991, 999992, 999994, 999997];
        using var bitmap = new Roaring32Bitmap(values);
        
        // Act
        bitmap.Optimize();
        var actual = bitmap.GetStatistics();

        // Assert
        Assert.Equal(2U, actual.ContainerCount);
        Assert.Equal(2U, actual.ArrayContainerCount);
        Assert.Equal(0U, actual.RunContainerCount);
        Assert.Equal(0U, actual.BitsetContainerCount);
    }
    
    [Fact]
    public void GetStatistics_ForBitsetContainerTestData_ReturnsContainerCount()
    {
        // Arrange
        using var bitmap = new Roaring32Bitmap();
        bitmap.AddMany(Enumerable.Range(0, 65536).Select(x=> (uint)x * 2).ToArray());
        
        // Act
        bitmap.Optimize();
        var actual = bitmap.GetStatistics();
        
        // Assert
        Assert.Equal(2U, actual.ContainerCount);
        Assert.Equal(0U, actual.ArrayContainerCount);
        Assert.Equal(0U, actual.RunContainerCount);
        Assert.Equal(2U, actual.BitsetContainerCount);
    }
    
    [Fact]
    public void GetStatistics_ForRunContainerTestData_ReturnsContainerCount()
    {
        // Arrange
        using var bitmap = new Roaring32Bitmap();
        bitmap.AddMany(Enumerable.Range(10, 1_000).Select(x=> (uint)x).ToArray());
        
        // Act
        bitmap.Optimize();
        var actual = bitmap.GetStatistics();
        
        // Assert
        Assert.Equal(1U, actual.ContainerCount);
        Assert.Equal(0U, actual.ArrayContainerCount);
        Assert.Equal(1U, actual.RunContainerCount);
        Assert.Equal(0U, actual.BitsetContainerCount);
    }
    
    [Fact]
    public void GetStatistics_ForArrayContainerTestData_ReturnsContainerValuesCount()
    {
        // Arrange
        uint[] values = [1, 3, 4, 7, 999991, 999992, 999994, 999997];
        using var bitmap = new Roaring32Bitmap(values);
        
        // Act
        bitmap.Optimize();
        var actual = bitmap.GetStatistics();

        // Assert
        Assert.Equal(8U, actual.ArrayContainerValuesCount);
        Assert.Equal(0U, actual.RunContainerValuesCount);
        Assert.Equal(0U, actual.BitsetContainerValuesCount);
    }
    
    [Fact]
    public void GetStatistics_ForBitsetContainerTestData_ReturnsContainerValuesCount()
    {
        // Arrange
        using var bitmap = new Roaring32Bitmap();
        bitmap.AddMany(Enumerable.Range(0, 65536).Select(x=> (uint)x * 2).ToArray());
        
        // Act
        bitmap.Optimize();
        var actual = bitmap.GetStatistics();
        
        // Assert
        Assert.Equal(0U, actual.ArrayContainerValuesCount);
        Assert.Equal(0U, actual.RunContainerValuesCount);
        Assert.Equal(65536U, actual.BitsetContainerValuesCount);
    }
    
    [Fact]
    public void GetStatistics_ForRunContainerTestData_ReturnsContainerValuesCount()
    {
        // Arrange
        using var bitmap = new Roaring32Bitmap();
        bitmap.AddMany(Enumerable.Range(10, 1_000).Select(x=> (uint)x).ToArray());
        
        // Act
        bitmap.Optimize();
        var actual = bitmap.GetStatistics();
        
        // Assert
        Assert.Equal(0U, actual.ArrayContainerValuesCount);
        Assert.Equal(1000U, actual.RunContainerValuesCount);
        Assert.Equal(0U, actual.BitsetContainerValuesCount);
    }
    
    [Fact]
    public void GetStatistics_ForArrayContainerTestData_ReturnsContainerBytes()
    {
        // Arrange
        uint[] values = [1, 3, 4, 7, 999991, 999992, 999994, 999997];
        using var bitmap = new Roaring32Bitmap(values);
        
        // Act
        bitmap.Optimize();
        var actual = bitmap.GetStatistics();

        // Assert
        Assert.Equal(16U, actual.ArrayContainerBytes);
        Assert.Equal(0U, actual.RunContainerBytes);
        Assert.Equal(0U, actual.BitsetContainerBytes);
    }
    
    [Fact]
    public void GetStatistics_ForBitsetContainerTestData_ReturnsContainerBytes()
    {
        // Arrange
        using var bitmap = new Roaring32Bitmap();
        bitmap.AddMany(Enumerable.Range(0, 65536).Select(x=> (uint)x * 2).ToArray());
        
        // Act
        bitmap.Optimize();
        var actual = bitmap.GetStatistics();
        
        // Assert
        Assert.Equal(0U, actual.ArrayContainerBytes);
        Assert.Equal(0U, actual.RunContainerBytes);
        Assert.Equal(16384U, actual.BitsetContainerBytes);
    }
    
    [Fact]
    public void GetStatistics_ForRunContainerTestData_ReturnsContainerBytes()
    {
        // Arrange
        using var bitmap = new Roaring32Bitmap();
        bitmap.AddMany(Enumerable.Range(10, 1_000).Select(x=> (uint)x).ToArray());
        
        // Act
        bitmap.Optimize();
        var actual = bitmap.GetStatistics();
        
        // Assert
        Assert.Equal(0U, actual.ArrayContainerBytes);
        Assert.Equal(6U, actual.RunContainerBytes);
        Assert.Equal(0U, actual.BitsetContainerBytes);
    }
}