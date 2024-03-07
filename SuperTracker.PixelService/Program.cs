using Rebus.Config;
using SuperTracker.PixelService.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRebus(configure =>
{
    var rabbitMqConnectionString = builder.Configuration.GetConnectionString("RabbitMq");
    var configurer = configure
        .Logging(l => l.ColoredConsole())
        .Transport(t => t.UseRabbitMqAsOneWayClient(rabbitMqConnectionString));
    return configurer;
});
builder.Services.AddTransient<ITrackingClient, TrackingClient>();
builder.Services.AddTransient<IImageGenerator, ImageGenerator>();

var app = builder.Build();

app.MapGet("/track", TrackRequest);

app.Run();

static async Task<IResult> TrackRequest(
    HttpContext context,
    ITrackingClient trackingClient,
    IImageGenerator imageGenerator)
{
    var request = context.Request;
    
    // The task specified for the referrer header but the standard header name is referer so I use referer here
    // The referer is a misspelling: https://en.wikipedia.org/wiki/HTTP_referer
    var referrer = request.Headers.Referer;
    var userAgent = request.Headers.UserAgent;
    var ip = request.HttpContext.Connection.RemoteIpAddress;
    
    await trackingClient.TrackNewRequest(userAgent, referrer, ip?.ToString());
    var gifData = await imageGenerator.GenerateOnePixelGif();
    
    return TypedResults.File(gifData, "image/gif");
}

