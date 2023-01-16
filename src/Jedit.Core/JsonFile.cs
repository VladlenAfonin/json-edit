namespace Jedit.Core;

/// <summary>JSON file processor.</summary>
public class JsonFile : JeditProcessor, IDisposable
{
    private readonly string _path;

    /// <summary>Initialize new <see cref="JsonFile"/> processor.</summary>
    /// <param name="path">JSON file path.</param>
    public JsonFile(string path)
	{
        var json = File.ReadAllText(path);
        _path = path;

        Initialize(json);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        File.WriteAllText(_path, Get());
    }
}