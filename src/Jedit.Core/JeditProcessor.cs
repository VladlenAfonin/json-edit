using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Jedit.Core;

public class JeditProcessor : IDisposable
{
    private readonly JObject _json;
    private readonly string _path;

    /// <summary>Initialize new <see cref="JeditProcessor"/>.</summary>
    /// <param name="path">JSON file path.</param>
    /// <exception cref="InvalidOperationException">
    /// Unable to open JSON.
    /// </exception>
    public JeditProcessor(string path)
    {
        var jsonText = File.ReadAllText(path);

        _path = path;
        _json = JsonConvert.DeserializeObject(jsonText) as JObject;

        if (_json is null)
        {
            throw new InvalidOperationException("Unable to open JSON.");
        }
    }

    /// <summary>Set value to a given key.</summary>
    /// <param name="key">
    /// Json key in form "Section1.Section2.SomeKey". Delimiter can be chosen.
    /// </param>
    /// <param name="value">New value.</param>
    /// <exception cref="InvalidOperationException">
    /// Value already exists for some intermediate section key.
    /// </exception>
    public void Set(string key, object value, char delimiter = '.')
    {
        var sections = key.Trim().Split(delimiter);

        var token = _json as JToken;
        foreach (var section in sections)
        {
            if (token is JValue)
            {
                throw new InvalidOperationException(
                    "Intermediate value already exists.");
            }

            if (token[section] is null)
            {
                ((JObject)token).Add(section, JObject.Parse(@"{}"));
            }

            token = token[section];
        }

        token.Replace(JToken.FromObject(value));
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        File.WriteAllText(_path, _json.ToString());
    }
}