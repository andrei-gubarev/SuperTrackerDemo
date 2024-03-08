namespace SuperTracker.StorageService.Services;

/// <summary>
/// A wrapper around operations on file system to make testing easier.
/// </summary>
public interface IFileWrapper
{
    void AppendAllText(string path, string contents);
}

internal class FileWrapper : IFileWrapper
{
    public void AppendAllText(string path, string contents)
    {
        File.AppendAllText(path, contents);
    }
}