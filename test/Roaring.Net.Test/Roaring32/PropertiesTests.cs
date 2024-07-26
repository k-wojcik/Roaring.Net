using Xunit;

namespace Roaring.Test.Roaring32;

public class PropertiesTests
{
    [Fact]
    public void Count_BitmapContainsValues_ReturnsCardinalityOfValues()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetDefault();

        // Act
        var actual = testObject.Bitmap.Count;

        // Assert
        Assert.Equal((ulong)testObject.Values.Length, actual);
    }

    [Fact]
    public void Count_EmptyBitmap_ReturnsZero()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetEmpty();

        // Act
        var actual = testObject.Bitmap.Count;

        // Assert
        Assert.Equal(0U, actual);
    }

    [Fact]
    public void IsEmpty_BitmapContainsValues_ReturnsFalse()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetDefault();

        // Act
        var actual = testObject.Bitmap.IsEmpty;

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void IsEmpty_EmptyBitmap_ReturnsTrue()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetEmpty();

        // Act
        var actual = testObject.Bitmap.IsEmpty;

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void Min_BitmapContainsValues_ReturnsMinValue()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetForRange(start: 10, end: uint.MaxValue, count: 1000);

        // Act
        var actual = testObject.Bitmap.Min;

        // Assert
        Assert.Equal(10U, actual);
    }

    [Fact]
    public void Min_EmptyBitmap_ReturnsNull()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetEmpty();

        // Act
        uint? actual = testObject.Bitmap.Min;

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public void Max_BitmapContainsValues_ReturnsMaxValue()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetForRange(start: 0, end: 1001, count: 1001);

        // Act
        var actual = testObject.Bitmap.Max;

        // Assert
        Assert.Equal(1000U, actual);
    }

    [Fact]
    public void Max_EmptyBitmap_ReturnsNull()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetEmpty();

        // Act
        uint? actual = testObject.Bitmap.Max;

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public void SerializedBytes_EmptyBitmap_ReturnsValueGreaterThanZero()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetEmpty();

        // Act
        var actual = testObject.Bitmap.SerializedBytes;

        // Assert
        Assert.True(actual > 0);
    }

    [Fact]
    public void SerializedBytes_BitmapContainsValues_ReturnsValueGreaterThanZero()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetDefault();

        // Act
        var actual = testObject.Bitmap.SerializedBytes;

        // Assert
        Assert.True(actual > 0);
    }

    [Fact]
    public void PortableSerializedBytes_EmptyBitmap_ReturnsValueGreaterThanZero()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetEmpty();

        // Act
        var actual = testObject.Bitmap.PortableSerializedBytes;

        // Assert
        Assert.True(actual > 0);
    }

    [Fact]
    public void PortableSerializedBytes_BitmapContainsValues_ReturnsValueGreaterThanZero()
    {
        // Arrange
        using var testObject = Roaring32BitmapTestObject.GetDefault();

        // Act
        var actual = testObject.Bitmap.PortableSerializedBytes;

        // Assert
        Assert.True(actual > 0);
    }
}