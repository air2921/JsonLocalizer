namespace JsonLocalizer;

public class Current(LocalizerOptions options) : ICurrent
{
    /// <summary>
    /// Set language for current request
    /// </summary>
    /// <value>Language for translation</value>
    /// <example>en ru, fr</example>
    public string CurrentLanguage { get; set; } = options.DefaultLanguage;
}

public interface ICurrent
{
    /// <summary>
    /// Set language for current request
    /// </summary>
    /// <value>Language for translation</value>
    /// <example>en ru, fr</example>
    public string CurrentLanguage { get; set; }
}
