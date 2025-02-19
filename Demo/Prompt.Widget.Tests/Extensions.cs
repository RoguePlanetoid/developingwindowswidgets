namespace Prompt.Widget.Tests;

/// <summary>
/// Extensions
/// </summary>
internal static class Extensions
{
    private const string secret_settings = "appsettings.secret.json";
    private const string app_settings = "appsettings.json";

    /// <summary>
    /// Add Services
    /// </summary>
    /// <param name="services">Service Collection</param>
    /// <returns>Service Collection</returns>
    public static IServiceCollection AddServices(this IServiceCollection services) =>
        services.AddLibrary(new ConfigurationBuilder()
        .AddJsonFile(secret_settings, true, true)
        .AddJsonFile(app_settings, true, true)
        .Build());
}
