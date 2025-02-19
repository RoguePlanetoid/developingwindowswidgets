namespace Prompt.Widget.Library.Config;

/// <summary>
/// Azure OpenAI Service Config
/// </summary>
public interface IAzureConfig : IOpenAIConfig
{
    /// <summary>
    /// Endpoint
    /// </summary>
    public string Endpoint { get; }
}
