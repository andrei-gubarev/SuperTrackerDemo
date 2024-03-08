using BoDi;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Bus;
using SuperTracker.PixesService.Integration.Tests.Fakes;

namespace SuperTracker.PixesService.Integration.Tests.Hooks
{
    [Binding]
    public class Hooks
    {
        private readonly IObjectContainer _objectContainer;

        public Hooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeScenario]
        public async Task RegisterServices()
        {
            await Task.CompletedTask;
            var factory = GetWebApplicationFactory();
            _objectContainer.RegisterInstanceAs(factory);
        }

        private WebApplicationFactory<Program> GetWebApplicationFactory() =>
            new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddSingleton<IBus, FakeBus>();
                    });
                });
    }
}