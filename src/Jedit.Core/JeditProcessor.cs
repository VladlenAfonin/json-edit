using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Jedit.Core;

/// <summary>Base JSON processor.</summary>
public class JeditProcessor
{
    private JObject _json;

    /// <summary>Initialize new <see cref="JeditProcessor"/>.</summary>
    /// <param name="json">JSON content.</param>
    /// <exception cref="InvalidOperationException">
    /// Unable to open JSON.
    /// </exception>
    protected void Initialize(string json)
    {
        _json = JsonConvert.DeserializeObject(json) as JObject;

        if (_json is null)
        {
            throw new InvalidOperationException("Unable to open JSON.");
        }
    }

    /// <summary>Set key to a given value.</summary>
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

    /// <summary>Get current JSON as string.</summary>
    /// <returns>Current JSON as string.</returns>
    public string Get()
    {
        return _json.ToString();
    }
}