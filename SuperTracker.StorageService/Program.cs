using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rebus.Config;
using SuperTracker.Domain.Events;
using SuperTracker.StorageService;
using SuperTracker.StorageService.Handlers;
using SuperTracker.StorageService.Services;
using TimeProvider = SuperTracker.StorageService.Services.TimeProvider;

[assembly: InternalsVisibleTo("SuperTracker.StorageService.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2, PublicKey=0024000004800000940000000602000000240000525341310004000001000100c547cac37abd99c8db225ef2f6c8a3602f3b3606cc9891605d02baa56104f4cfc0734aa39b93bf7852f7d9266654753cc297e7d2edfe0bac1cdcf9f717241550e0a7b191195b7667bb4f64bcb8e2121380fd1d9d46ad2d92d2d15605093924cceaf74c4861eff62abf69b9291ed0a340e113be11e6a7d3113e92484cf7045cc7")]

var builder = Host.CreateApplicationBuilder(args);

var config = builder.Configuration.Get<EnvConfig>()!;
builder.Services.AddSingleton(config);

builder.Services.AddTransient<IRequestStorageService, RequestFileStorageService>();
builder.Services.AddTransient<IFileWrapper, FileWrapper>();
builder.Services.AddTransient<ITimeProvider, TimeProvider>();

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