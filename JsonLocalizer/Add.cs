using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JsonLocalizer;

public static class Add
{
    /// <summary>
    /// The order in which the service is configured matters !
    /// </summary>
    /// <param name="services">Service collection for add in DI</param>
    /// <param name="environment">IWebHostEnvironment for search directory with translations</param>
    /// <param name="configureOptions">Options</param>
    /// <remarks>The following order is required: LocalizationDirectory, SupportedLanguages, DefaultLanguage</remarks>
    public static void AddJsonLocalizer(this IServiceCollection services, IHostEnvironment environment,
        Action<LocalizerOptions> configureOptions)
    {
        var options = new LocalizerOptions(environment);
        configureOptions(options);

        services.AddSingleton(options);
        services.AddSingleton<ICurrent, Current>();
        services.AddSingleton<ILocalizer, Localizer>();

        Console.WriteLine($"Default Language {options.DefaultLanguage}");
        Console.WriteLine($"File with translations {options.LocalizationDirectory}");
        foreach (var lng in options.SupportedLanguages)
            Console.WriteLine($"Supported language: {lng}");
    }
}