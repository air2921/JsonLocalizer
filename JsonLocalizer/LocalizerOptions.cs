using Microsoft.Extensions.Hosting;

namespace JsonLocalizer;

public class LocalizerOptions(IHostEnvironment env)
{
    private string _directory = string.Empty;
    private string _defaultLanguage = string.Empty;
    private HashSet<string> _supportedLanguages = [];

    public string LocalizationDirectory
    {
        get => _directory;
        set
        {
            var directory = Path.Combine(env.ContentRootPath, value);
            if (!Directory.Exists(directory) && Directory.GetFiles(directory, "*.json").Length <= 0)
                throw new ArgumentException($"{nameof(LocalizationDirectory)} is not found, or {nameof(LocalizationDirectory)} has no '.json' files");

            _directory = directory;
        }
    }

    public HashSet<string> SupportedLanguages
    {
        get => _supportedLanguages;
        set
        {
            var files = Directory.GetFiles(LocalizationDirectory)
                .Select(Path.GetFileName)
                .ToArray();

            if (!AreAllLanguagesPresent(files, value))
                throw new ArgumentException("Relevant localization files are missing");

            _supportedLanguages = value;
        }
    }

    public string DefaultLanguage
    {
        get => _defaultLanguage;
        set
        {
            if (!SupportedLanguages.Contains(value))
                throw new ArgumentException("Default language isn't supported");

            _defaultLanguage = value;
        }
    }

    private bool AreAllLanguagesPresent(IEnumerable<string> files, HashSet<string> supportedLanguages)
    {
        var fileNameLanguageMap = new Dictionary<string, HashSet<string>>();

        foreach (var file in files)
        {
            var (baseName, language) = ParseFileName(file);

            if (!string.IsNullOrEmpty(language))
            {
                if (!fileNameLanguageMap.ContainsKey(baseName))
                    fileNameLanguageMap[baseName] = [];

                fileNameLanguageMap[baseName].Add(language);
            }
        }

        foreach (var kvp in fileNameLanguageMap)
        {
            var languagesInFile = kvp.Value;

            if (supportedLanguages.Except(languagesInFile).Any())
            {
                return false;
            }
        }

        return true;
    }

    private (string baseName, string language) ParseFileName(string fileName)
    {
        var parts = fileName.Split('.');
        if (parts.Length >= 3)
        {
            var baseName = string.Join(".", parts.Take(parts.Length - 2));
            var language = parts[parts.Length - 2];
            return (baseName, language);
        }
        return (string.Empty, string.Empty);
    }
}
