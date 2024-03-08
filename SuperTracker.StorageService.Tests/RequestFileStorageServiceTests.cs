using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Moq;
using SuperTracker.StorageService.Services;
using SuperTracker.StorageService.Tests.Fakes;

namespace SuperTracker.StorageService.Tests;

public class RequestFileStorageServiceTests
{
    private readonly FakeFileWrapper _fakeFileWrapper = new();
    private readonly TestTimeProvider _testTimeProvider = new();

    [Fact]
    public async Task StoreRequest_CorrectFormatStored()
    {
        // Arrange
        var storageService = GetSut();
        var time = new DateTime(2024, 3, 8, 12, 1, 2);
        _testTimeProvider.SetTime(time);
        
        // Act
        await storageService.StoreRequest("userAgent1", "referrer1", "10.0.0.1");
        
        // Assert
        var expected = $"{_testTimeProvider.GetUtcNow():o} | referrer1 | userAgent1 | 10.0.0.1{Environment.NewLine}";
        Assert.Equal(expected, _fakeFileWrapper.Content);
    }

    [Theory]
    [InlineData(null, "referrer1")]
    [InlineData("userAgent", null)]
    public async Task StoreRequest_WhenNoUserAgentOrReferrer_ThenSubstituted(string? userAgent, string? referrer)
    {
        // Arrange
        var storageService = GetSut();
        var time = new DateTime(2024, 3, 8, 12, 1, 2);
        _testTimeProvider.SetTime(time);
        const string ip = "10.0.0.1";
        
        // Act
        await storageService.StoreRequest(userAgent, referrer, ip);
        
        // Assert
        var expected = $"{time:o} | {referrer ?? "null"} | {userAgent ?? "null"} | {ip}{Environment.NewLine}";
        Assert.Equal(expected, _fakeFileWrapper.Content);
    }

    [Fact]
    public async Task StoreRequest_WhenIpAddress_ThenLogAdded()
    {
        // Arrange
        var storageService = GetSut();
        
        // Act
        await storageService.StoreRequest("userAgent1", "referrer1", "10.0.0.1");
        await storageService.StoreRequest("userAgent1", "referrer1", "10.0.0.2");
        await storageService.StoreRequest("userAgent1", "referrer1", "10.0.0.3");
        
        // Assert
        var linesCount = Regex.Matches(_fakeFileWrapper.Content, Environment.NewLine).Count;
        Assert.Equal(3, linesCount);
    }

    [Fact]
    public async Task StoreRequest_WhenNoIpAddress_ThenNoLogAdded()
    {
        // Arrange
        var storageService = GetSut();
        
        // Act
        await storageService.StoreRequest("userAgent1", "referrer1", "10.0.0.1");
        await storageService.StoreRequest("userAgent2", "referrer2", null);

        // Assert
        var linesCount = Regex.Matches(_fakeFileWrapper.Content, Environment.NewLine).Count;
        Assert.Equal(1, linesCount);
    }

    private RequestFileStorageService GetSut()
    {
        var config = new EnvConfig
        {
            RabbitMq = new RabbitMqConfig
            {
                ConnectionString = "cs",
                QueueName = "queue"
            }
        };

        return new RequestFileStorageService(config,
            _fakeFileWrapper,
            _testTimeProvider,
            Mock.Of<ILogger<RequestFileStorageService>>());
    }
}