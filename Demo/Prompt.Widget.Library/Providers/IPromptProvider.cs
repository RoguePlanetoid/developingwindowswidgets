namespace Prompt.Widget.Library.Providers;

/// <summary>
/// Prompt Provider
/// </summary>
public interface IPromptProvider
{
    /// <summary>
    /// Get
    /// </summary>
    /// <param name="prompt">Prompt</param>
    /// <param name="type">Type</param>
    /// <returns>Message</returns>
    Task<string?> Get(string prompt,
        PromptType? type = PromptType.Azure);

    /// <summary>
    /// Get Json Data
    /// </summary>
    /// <param name="endpoint">Endpoint</param>
    /// <returns>Json Data</returns>
    Task<string?> GetJsonData(string? endpoint);

    /// <summary>
    /// Fetch Json Data
    /// </summary>
    /// <param name="endpoint">Endpoint</param>
    /// <returns>Json Data</returns>
    Task<string?> FetchJsonData(string? endpoint);

    /// <summary>
    /// Get Card Model
    /// </summary>
    /// <param name="endpoint">Endpoint</param> 
    /// <param name="prompt">Prompt</param>
    /// <param name="type">Type</param>
    /// <returns>Card Model</returns>
    Task<CardModel> GetCardModel(
        string? endpoint = null,
        string? prompt = null,
        PromptType? type = PromptType.Azure);
}