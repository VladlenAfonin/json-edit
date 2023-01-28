using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonEditor;

/// <summary>Base JSON processor.</summary>
public class JsonEditorBase
{
    private JObject? _json;
    private readonly char _keyDelimiter = '.';

    /// <summary>Initialize new <see cref="JsonEditorBase"/>.</summary>
    /// <param name="json">JSON content.</param>
    /// <exception cref="InvalidOperationException">
    /// Unable to open JSON.
    /// </exception>
    protected void Initialize(string json)
    {
        _json = JsonConvert.DeserializeObject(json) as JObject;

        if (_json is null)
        {
            throw new InvalidOperationException("Unable to parse JSON.");
        }
    }

    /// <summary>
    /// Create object at <paramref name="key"/> if it does not exist.
    /// </summary>
    /// <typeparam name="T">Object type.</typeparam>
    /// <param name="key">Object key.</param>
    /// <param name="value">Value to create if object does not exist.</param>
    public void EnsureExists<T>(string key, T? value = default)
    {
        var token = EnsureCreatedAndGet(key);

        if (token.ToString() == @"{}")
        {
            token.Replace(JToken.FromObject(value));
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
    public void Set(string key, object value)
    {
        var token = EnsureCreatedAndGet(key);

        token.Replace(JToken.FromObject(value));
    }

    /// <summary>Get current JSON as string.</summary>
    /// <returns>Current JSON as string.</returns>
    public string Get()
    {
        return _json.ToString();
    }

    /// <summary>Get object by key.</summary>
    /// <typeparam name="T">Object type.</typeparam>
    /// <param name="key">Object key.</param>
    /// <returns>Object.</returns>
    public T? GetOrDefault<T>(string key)
    {
        var token = _json.SelectToken(key);

        if (token is null)
        {
            return default;
        }

        return token.ToObject<T>();
    }


    private JToken EnsureCreatedAndGet(string key)
    {
        var sections = key.Trim().Split(_keyDelimiter);

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

        return token;
    }
}