namespace JsonEditor;

/// <summary>JSON file processor.</summary>
public class JsonFileEditor : JsonEditorBase, IDisposable
{
    private readonly string _path;

    /// <summary>
    /// Initialize new <see cref="JsonFileEditor"/> processor.
    /// </summary>
    /// <param name="path">JSON file path.</param>
    public JsonFileEditor(string path)
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