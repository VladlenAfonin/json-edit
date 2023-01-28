# JSON editor library

Edit values in JSON files arbitrarily.

## Usage

```cs
using JsonEditor;

using (var editor = new JsonFileEditor(@"path/to/file.json"))
{
    editor.Set("SomeSection.SomeKey", someObject);
}
```