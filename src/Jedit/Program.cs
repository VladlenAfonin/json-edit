using Mono.Options;
using Jedit.Core;

string? path = null;
string? key = null;
string? value = null;

var options = new OptionSet
{
    { "p|path=", v => path = v },
    { "k|key=", v => key = v },
    { "v|value=", v => value = v },
};

options.Parse(args);

if (string.IsNullOrWhiteSpace(path))
{
    Console.Error.WriteLine("No input file.");
    Environment.Exit(1);
}

if (!File.Exists(path))
{
    Console.Error.WriteLine("File doesn't exist.");
    Environment.Exit(1);
}

if (string.IsNullOrWhiteSpace(key))
{
    Console.Error.WriteLine("No input key.");
    Environment.Exit(1);
}

if (string.IsNullOrWhiteSpace(value))
{
    Console.Error.WriteLine("No input value.");
    Environment.Exit(1);
}

using (var editor = new JeditProcessor(path))
{
    editor.Set(key, value);
}