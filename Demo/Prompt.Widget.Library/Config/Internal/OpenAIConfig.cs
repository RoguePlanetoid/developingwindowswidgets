namespace Prompt.Widget.Library.Config.Internal;

/// <summary>
/// Open AI Config
/// </summary>
internal class OpenAIConfig : IOpenAIConfig
{
    /// <summary>
    /// Api Key
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Model
    /// </summary>
    public string Model { get; set; } = string.Empty;
}
