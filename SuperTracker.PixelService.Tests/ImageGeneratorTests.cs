using SuperTracker.PixelService.Services;

namespace SuperTracker.PixelService.Tests;

public class ImageGeneratorTests
{
    [Fact]
    public async Task WhenGenerateOnePixelGifIsCalled_ThenReturnOnePixelGif()
    {
        // Arrange
        var imageGenerator = new ImageGenerator();
        
        // Act
        var result = await imageGenerator.GenerateOnePixelGif();
        
        // Assert
        Assert.Equal("R0lGODlhAQABAIAAAP///wAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==", Convert.ToBase64String(result));
    }
}