using Xunit;

namespace Roaring.Test.Roaring32;

public class ConstructorTests
{
    [Fact]
    public void Ctor_Default_CreatesBitmapWithZeroCapacity()
    {
        // Arrange
        using var uut = new Roaring32Bitmap();

        // Act
        var actual = uut.SerializedBytes;
        
        // Assert
        Assert.True(actual > 0);
    }
    
    [Fact]
    public void Ctor_WithCapacity_CreatesBitmapWithPriviedCapacity()
    {
        // Arrange
        using var uut = new Roaring32Bitmap(1000);

        // Act
        var actual = uut.SerializedBytes;
        
        // Assert
        Assert.True(actual > 0);
    }
}