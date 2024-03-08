namespace SuperTracker.StorageService.Services;

/// <summary>
/// The interface for providing time-related functionality. Used to make testing easier.
/// </summary>
public interface ITimeProvider
{
    DateTime GetUtcNow();
}

internal class TimeProvider : ITimeProvider
{
    public DateTime GetUtcNow()
    {
        return DateTime.UtcNow;
    }
}
