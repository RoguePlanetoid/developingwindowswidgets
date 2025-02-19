namespace Prompt.Widget.Library.Config;

/// <summary>
/// Prompt Config
/// </summary>
public interface IPromptConfig
{
    /// <summary>
    /// Data Prompt
    /// </summary>
    public string DataPrompt { get; }

    /// <summary>
    /// Card Prompt
    /// </summary>
    public string CardPrompt { get; }

    /// <summary>
    /// Retries
    /// </summary>
    public int Retries { get; }
}
