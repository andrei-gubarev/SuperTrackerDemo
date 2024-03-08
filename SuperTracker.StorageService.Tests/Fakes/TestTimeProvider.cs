using SuperTracker.StorageService.Services;

namespace SuperTracker.StorageService.Tests.Fakes;

public class TestTimeProvider : ITimeProvider
{
    private DateTime? _time;

    public DateTime GetUtcNow()
    {
        return _time ?? new DateTime(2024, 3, 8, 12, 1, 1);
    }
    
    public void SetTime(DateTime time)
    {
        _time = time;
    }
}