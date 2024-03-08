using System.Text;
using SuperTracker.StorageService.Services;

namespace SuperTracker.StorageService.Tests.Fakes;

public class FakeFileWrapper : IFileWrapper
{
    public string Content { get; private set; } = string.Empty;
    
    public void AppendAllText(string path, string contents)
    {
        var sb = new StringBuilder(Content);
        sb.Append(contents);
        Content = sb.ToString();
    }
}