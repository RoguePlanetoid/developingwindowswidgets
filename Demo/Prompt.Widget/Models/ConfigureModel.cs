namespace Prompt.Widget.Models;

/// <summary>
/// Configure Model
/// </summary>
internal class ConfigureModel
{
    /// <summary>
    /// Endpoint
    /// </summary>
    public string? Endpoint { get; set; }

    /// <summary>
    /// Prompt
    /// </summary>
    public string? Prompt { get; set; }

    /// <summary>
    /// Type
    /// </summary>
    public PromptType? Type { get; set; } = PromptType.GitHub;

    /// <summary>
    /// Error
    /// </summary>
    public string? Error { get; set; }
}