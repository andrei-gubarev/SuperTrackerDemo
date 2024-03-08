using Moq;
using Rebus.Bus;
using SuperTracker.Domain.Events;
using SuperTracker.PixelService.Services;

namespace SuperTracker.PixelService.Tests;

public class TrackingClientTests
{
    [Theory]
    [InlineData("userAgent", "referer", "ipAddress")]
    [InlineData(null, "referer", "ipAddress")]
    [InlineData("userAgent", null, "ipAddress")]
    [InlineData("userAgent", "referer", null)]
    [InlineData(null, null, "ipAddress")]
    [InlineData(null, "referer", null)]
    [InlineData("userAgent", null, null)]
    [InlineData(null, null, null)]
    public async Task WhenTrackNewRequestIsCalled_ThenPublishEvent(string userAgent, string referer, string ipAddress)
    {
        // Arrange
        var bus = new Mock<IBus>();
        var trackingClient = new TrackingClient(bus.Object);
        
        // Act
        await trackingClient.TrackNewRequest(userAgent, referer, ipAddress);
        
        // Assert
        bus.Verify(b => b.Publish(
                It.Is<TrackRequestReceivedEvent>(e => e.UserAgent == userAgent && e.Referrer == referer && e.IpAddress == ipAddress),
                It.IsAny<IDictionary<string, string>>()), 
            Times.Once);
    }
}