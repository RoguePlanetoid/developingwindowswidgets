namespace Prompt.Widget.Library.Config;

/// <summary>
/// Open AI Config
/// </summary>
public interface IOpenAIConfig
{
    /// <summary>
    /// API Key
    /// </summary>
    public string ApiKey { get; }

    /// <summary>
    /// Model
    /// </summary>
    public string Model { get; }
}
