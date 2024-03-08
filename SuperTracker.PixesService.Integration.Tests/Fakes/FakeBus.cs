using Rebus.Bus;
using Rebus.Bus.Advanced;
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

namespace SuperTracker.PixesService.Integration.Tests.Fakes;

public class FakeBus : IBus
{
    public void Dispose()
    {
        
    }

    public Task SendLocal(object commandMessage, IDictionary<string, string> optionalHeaders = null)
    {
        return Task.CompletedTask;
    }

    public Task Send(object commandMessage, IDictionary<string, string> optionalHeaders = null)
    {
        return Task.CompletedTask;
    }

    public Task DeferLocal(TimeSpan delay, object message, IDictionary<string, string> optionalHeaders = null)
    {
        return Task.CompletedTask;
    }

    public Task Defer(TimeSpan delay, object message, IDictionary<string, string> optionalHeaders = null)
    {
        return Task.CompletedTask;
    }

    public Task Reply(object replyMessage, IDictionary<string, string> optionalHeaders = null)
    {
        return Task.CompletedTask;
    }

    public Task Subscribe<TEvent>()
    {
        return Task.CompletedTask;
    }

    public Task Subscribe(Type eventType)
    {
        return Task.CompletedTask;
    }

    public Task Unsubscribe<TEvent>()
    {
        return Task.CompletedTask;
    }

    public Task Unsubscribe(Type eventType)
    {
        return Task.CompletedTask;
    }

    public Task Publish(object eventMessage, IDictionary<string, string> optionalHeaders = null)
    {
        return Task.CompletedTask;
    }

    public IAdvancedApi Advanced { get; } = null!;
}