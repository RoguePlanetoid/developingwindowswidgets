namespace Prompt.Widget.Library.Config.Internal;

/// <summary>
/// Prompt Config
/// </summary>
internal class PromptConfig : IPromptConfig
{
    private const int default_retries = 3;

    /// <summary>
    /// Data Prompt
    /// </summary>
    public string DataPrompt { get; set; } = string.Empty;

    /// <summary>
    /// Card Prompt
    /// </summary>
    public string CardPrompt { get; set; } = string.Empty;

    /// <summary>
    /// Retries
    /// </summary>
    public int Retries { get; set; } = default_retries;
}
