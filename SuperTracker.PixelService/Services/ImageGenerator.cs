namespace SuperTracker.PixelService.Services;

public interface IImageGenerator
{
    public Task<byte[]> GenerateOnePixelGif();
}

internal class ImageGenerator : IImageGenerator
{
    public Task<byte[]> GenerateOnePixelGif()
    {
        const string gifBase64 = "R0lGODlhAQABAIAAAP///wAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==";

        return Task.FromResult(Convert.FromBase64String(gifBase64));
    }
}