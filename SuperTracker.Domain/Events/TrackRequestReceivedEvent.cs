namespace SuperTracker.Domain.Events;

public record TrackRequestReceivedEvent(string? UserAgent, string? Referrer, string? IpAddress);