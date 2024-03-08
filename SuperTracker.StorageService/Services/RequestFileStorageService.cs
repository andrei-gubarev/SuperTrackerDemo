using Microsoft.Extensions.Logging;

namespace SuperTracker.StorageService.Services;

public interface IRequestStorageService
{
    Task StoreRequest(string? userAgent, string? referer, string? ipAddress);
}

internal class RequestFileStorageService(
    EnvConfig config,
    IFileWrapper fileWrapper,
    ITimeProvider timeProvider,
    ILogger<RequestFileStorageService> logger) : IRequestStorageService
{
    private static readonly object LockObject = new ();

    public Task StoreRequest(string? userAgent, string? referer, string? ipAddress)
    {
        if (string.IsNullOrEmpty(ipAddress))
        {
            logger.LogWarning("Cannot log request as IP address is missing. User Agent: {UserAgent}. Referrer: {Referrer}",
                userAgent, referer);
            return Task.CompletedTask;
        }
        
        var log = BuildLog(userAgent, referer, ipAddress);
        
        // As the storage service is deployed as a single instance this approach is acceptable
        // Otherwise we would need to use distributed lock as one of the options.
        lock (LockObject)
        {
            // In high load application this operation can be batched.
            // For example, tracking request can be collected somewhere and then written to the file in a batch.
            // Another option is scheduling a job to write the logs to the file.
            // Otherwise, it may be resource consuming as there maybe thousands of requests per second.
            fileWrapper.AppendAllText(config.GetRequestLogFilePath(), log);
        }
        return Task.CompletedTask;
    }
    
    private string BuildLog(string? userAgent, string? referer, string ipAddress)
    {
        return $"{timeProvider.GetUtcNow():o}|{GetValueOrNull(referer)}|{GetValueOrNull(userAgent)}|{ipAddress}{Environment.NewLine}";
    }

    private static string GetValueOrNull(string? str)
    {
        return !string.IsNullOrEmpty(str) ? str : "null";
    }
}