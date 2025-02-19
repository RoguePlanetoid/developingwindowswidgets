namespace Prompt.Widget.Library;

/// <summary>
/// Extensions
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Add Prompts
    /// </summary>
    /// <param name="services">Service Collection</param>
    /// <returns>Service Collection</returns>
    internal static IServiceCollection AddPrompts(this IServiceCollection services) =>
        services.AddSingleton<AzurePrompt>()
        .AddSingleton<GitHubPrompt>()
        .AddSingleton<OpenAIPrompt>();

    /// <summary>
    /// Add Providers
    /// </summary>
    /// <param name="services">Service Collection</param>
    /// <returns>Service Collection</returns>
    internal static IServiceCollection AddProviders(this IServiceCollection services) =>
        services.AddSingleton<IPromptProvider, PromptProvider>();

    /// <summary>
    /// Add Config
    /// </summary>
    /// <param name="services">Service Collection</param>
    /// <param name="root">Configuration Root</param>
    /// <returns>Service Collection</returns>
    private static IServiceCollection AddConfig(this IServiceCollection services, IConfigurationRoot root) =>
        services.AddSingleton<IPromptConfig>(root.GetSection(nameof(PromptConfig)).Get<PromptConfig>() ?? new())
        .AddSingleton<IAzureConfig>(root.GetSection(nameof(AzureConfig)).Get<AzureConfig>() ?? new())
        .AddSingleton<IGitHubConfig>(root.GetSection(nameof(GitHubConfig)).Get<GitHubConfig>() ?? new())
        .AddSingleton<IOpenAIConfig>(root.GetSection(nameof(OpenAIConfig)).Get<OpenAIConfig>() ?? new());

    /// <summary>
    /// Add Library
    /// </summary>
    /// <param name="services">Service Collection</param>
    /// <param name="root">Configuration Root</param> 
    /// <returns>Service Collection</returns>
    public static IServiceCollection AddLibrary(this IServiceCollection services, IConfigurationRoot root) =>
        services
        .AddHttpClient()
        .AddProviders()
        .AddPrompts()
        .AddConfig(root);
}
