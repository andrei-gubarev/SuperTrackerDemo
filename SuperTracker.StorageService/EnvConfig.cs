namespace SuperTracker.StorageService;

public class EnvConfig
{
    public string? RequestLogFilePath { get; init; }
    
    public required RabbitMqConfig RabbitMq { get; init; }
    
    public string GetRequestLogFilePath()
    {
        var relativePath =  RequestLogFilePath ?? "tmp/visits.log";
        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
    }
}

public class RabbitMqConfig
{
    public required string ConnectionString { get; init; }
    public required string QueueName { get; init; }
}