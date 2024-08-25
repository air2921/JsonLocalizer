using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Text;

namespace JsonLocalizer;

public class NoneStaticLocalizer(ICurrent current, LocalizerOptions options) : ILocalizer
{
    private static readonly ConcurrentDictionary<string, Dictionary<string, string>> _translations = [];

    public string Translate(string key)
    {
        if (!options.SupportedLanguages.Contains(current.CurrentLanguage))
            return key;

        if (!_translations.ContainsKey(current.CurrentLanguage))
            Initialize(current.CurrentLanguage);

        if (!_translations.TryGetValue(current.CurrentLanguage, out Dictionary<string, string> translations))
            return key;

        if (!translations.TryGetValue(key, out string value))
            return key;

        return value;
    }

    public void Initialize(string language)
    {
        var translations = MergeJson(language);
        _translations.TryAdd(language, translations);
    }

    private Dictionary<string, string> MergeJson(string language)
    {
        var files = Directory.GetFiles(options.LocalizationDirectory, $"*.{language}.json");
        var translations = new Dictionary<string, string>();

        foreach (var file in files)
        {
            var json = File.ReadAllText(file, Encoding.UTF8);
            var jsonObjects = JsonConvert.DeserializeObject<IEnumerable<JsonObject>>(json);

            if (jsonObjects is not null)
                foreach (var obj in jsonObjects)
                    translations[obj.Key] = obj.Value;
        }

        return translations;
    }
}

public interface ILocalizer
{
    void Initialize(string language);
    string Translate(string key);
}
