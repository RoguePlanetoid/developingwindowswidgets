namespace Prompt.Widget.Library.Config.Internal;

/// <summary>
/// Azure OpenAI Service Config
/// </summary>
internal class AzureConfig : OpenAIConfig, IAzureConfig
{
    /// <summary>
    /// Endpoint
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;
}
