using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rebus.Config;
using SuperTracker.Domain.Events;
using SuperTracker.StorageService;
using SuperTracker.StorageService.Handlers;
using SuperTracker.StorageService.Services;

var builder = Host.CreateApplicationBuilder(args);

var config = builder.Configuration.Get<EnvConfig>()!;
builder.Services.AddSingleton(config);

builder.Services.AddTransient<IRequestStorageService, RequestFileStorageService>();

builder.Services.AddRebus(
    configure => {
        return configure
            .Transport(t => t.UseRabbitMq(
                config.RabbitMq.ConnectionString,
                config.RabbitMq.QueueName));
    },
    onCreated: async bus => {
        await bus.Subscribe<TrackRequestReceivedEvent>();
    }
);
builder.Services.AddRebusHandler<TrackRequestReceivedEventHandler>();

using var host = builder.Build();

EnsureFileExists(config.GetRequestLogFilePath());

await host.RunAsync();

void EnsureFileExists(string path)
{
    var dirName = Path.GetDirectoryName(path);
    if (dirName is not null)
    {
        Directory.CreateDirectory(dirName);
    }
    if (!File.Exists(path))
    {
        File.Create(path).Dispose();
    }
}