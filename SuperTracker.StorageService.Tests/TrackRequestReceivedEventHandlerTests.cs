using Moq;
using SuperTracker.Domain.Events;
using SuperTracker.StorageService.Handlers;
using SuperTracker.StorageService.Services;

namespace SuperTracker.StorageService.Tests;

public class TrackRequestReceivedEventHandlerTests
{
    [Theory]
    [InlineData("userAgent", "referer", "10.0.0.1")]
    [InlineData(null, "referer", "10.0.0.1")]
    [InlineData("userAgent", null, "10.0.0.1")]
    [InlineData("userAgent", "referer", null)]
    [InlineData(null, null, "10.0.0.1")]
    [InlineData(null, "referer", null)]
    [InlineData("userAgent", null, null)]
    [InlineData(null, null, null)]
    public async Task WhenHandleIsCalled_ThenStoreRequestIsCalled(string userAgent, string referer, string ipAddress)
    {
        // Arrange
        var storageService = new Mock<IRequestStorageService>();
        var handler = new TrackRequestReceivedEventHandler(storageService.Object);
        var message = new TrackRequestReceivedEvent(userAgent, referer, ipAddress);
        
        // Act
        await handler.Handle(message);
        
        // Assert
        storageService.Verify(x => x.StoreRequest(
            It.Is<string>(s => s == userAgent),
            It.Is<string>(s => s == referer),
            It.Is<string>(s => s == ipAddress)), Times.Once);
    }
}