using JetBrains.Annotations;
using Rebus.Handlers;
using SuperTracker.Domain.Events;
using SuperTracker.StorageService.Services;

namespace SuperTracker.StorageService.Handlers;

[UsedImplicitly]
public class TrackRequestReceivedEventHandler(IRequestStorageService storageService)
    : IHandleMessages<TrackRequestReceivedEvent>
{
    public Task Handle(TrackRequestReceivedEvent message)
    {
        return storageService.StoreRequest(message.UserAgent, message.Referrer, message.IpAddress);
    }
}