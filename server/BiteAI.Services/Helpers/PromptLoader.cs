using System.Collections.Concurrent;

namespace BiteAI.Services.Helpers;

public static class PromptLoader
{
    private static readonly ConcurrentDictionary<string, string> Cache = new();

    private static string ResolvePath(string fileName)
    {
        // Files are copied to output under Prompts/
        var baseDir = AppContext.BaseDirectory;
        return Path.Combine(baseDir, "Prompts", fileName);
    }

    public static string Load(string fileName)
    {
        var path = ResolvePath(fileName);
        return Cache.GetOrAdd(path, p => File.ReadAllText(p));
    }

    public static string Format(string template, IDictionary<string, object> values)
    {
        var result = template;
        foreach (var kv in values)
        {
            var token = "{{" + kv.Key + "}}";
            result = result.Replace(token, kv.Value?.ToString() ?? string.Empty, StringComparison.OrdinalIgnoreCase);
        }
        return result.Trim();
    }
}
