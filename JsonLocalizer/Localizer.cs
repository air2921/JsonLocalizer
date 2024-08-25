namespace JsonLocalizer;

public static class Localizer
{
    private static ILocalizer? _localizer;

    public static void SetLocalizer(ILocalizer localizer)
    {
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
    }

    public static string Translate(string key)
    {
        if (_localizer is null)
            throw new InvalidOperationException("Localizer has not been initialized");

        return _localizer.Translate(key);
    }
}
