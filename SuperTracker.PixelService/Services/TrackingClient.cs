using Rebus.Bus;
using SuperTracker.Domain.Events;

namespace SuperTracker.PixelService.Services;

public interface ITrackingClient
{
    Task TrackNewRequest(string? userAgent, string? referer, string? ipAddress);
}

internal class TrackingClient(IBus bus) : ITrackingClient
{
    public Task TrackNewRequest(string? userAgent, string? referer, string? ipAddress)
    {
        var evt = new TrackRequestReceivedEvent(userAgent, referer, ipAddress);
        return bus.Publish(evt);
    }
}