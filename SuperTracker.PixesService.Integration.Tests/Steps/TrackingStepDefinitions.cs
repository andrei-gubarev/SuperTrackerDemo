using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace SuperTracker.PixesService.Integration.Tests.Steps;

[Binding]
public class TrackingStepDefinitions
{
    private const string BaseAddress = "http://localhost";
    private readonly WebApplicationFactory<Program> _factory;
    private HttpClient _client = null!;
    private readonly ScenarioContext _scenarioContext;
    
    public TrackingStepDefinitions(WebApplicationFactory<Program> factory,
        ScenarioContext scenarioContext)
    {
        _factory = factory;
        _scenarioContext = scenarioContext;
    }
    
    [Given(@"A client's application is going to get the tracking pixel")]
    public void GivenAClientApplicationIsGoingToGetTheTrackingPixel()
    {
        _client = _factory.CreateDefaultClient(new Uri(BaseAddress));
    }

    [When(@"The client application requests the tracking pixel")]
    public async Task WhenTheClientApplicationRequestsTheTrackingPixel()
    {
        var response = await _client.GetAsync("/track");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsByteArrayAsync();
        Assert.Equal("image/gif", response.Content.Headers.ContentType?.MediaType);
        _scenarioContext.Set(Convert.ToBase64String(content), "imageBase64");
    }

    [Then(@"A transparent gif is returned")]
    public void ThenATransparentGifIsReturned()
    {
        var base64 = _scenarioContext.Get<string>("imageBase64");
        Assert.Equal("R0lGODlhAQABAIAAAP///wAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==", base64);
    }
}