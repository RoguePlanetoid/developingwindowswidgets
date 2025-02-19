namespace Prompt.Widget.Library.Providers.Internal;

/// <summary>
/// Prompt Provider
/// </summary>
/// <param name="config">Config</param>
/// <param name="factory">Client Factory</param>
/// <param name="azurePrompt">Azure OpenAI Service Prompt</param>
/// <param name="gitHubPrompt">GitHub Models Prompt</param>
/// <param name="openAIPrompt">OpenAI Prompt</param>
internal class PromptProvider(
    IPromptConfig config,
    IHttpClientFactory factory,
    AzurePrompt azurePrompt,
    GitHubPrompt gitHubPrompt,
    OpenAIPrompt openAIPrompt) : IPromptProvider
{
    private const string comma = ",";
    private const string invalid = "Invalid Json";

    private readonly HttpClient _client = factory.CreateClient();
    private readonly AsyncPolicy<bool> _retry =
        Policy.HandleResult<bool>(r => !r)
        .WaitAndRetryAsync(config.Retries, attempt =>
        TimeSpan.FromSeconds(Math.Pow(config.Retries, attempt)));

    /// <summary>
    /// Is Valid Json
    /// </summary>
    /// <param name="json">Json String</param>
    /// <returns>True if if, False if Not</returns>
    private static bool IsValidJson(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return false;
        try
        {
            using var document = JsonDocument.Parse(json);
            return document != null;
        }
        catch (JsonException)
        {
            return false;
        }
    }

    /// <summary>
    /// Is Valid Adaptive Card
    /// </summary>
    /// <param name="card">Card</param>
    /// <param name="message">Warning or Error Message</param>
    /// <returns>True if Is, False if Not</returns>
    private static bool IsValidAdaptiveCard(string? card, out string message)
    {
        try
        {
            if (IsValidJson(card))
            {
                message = string.Join(comma,
                AdaptiveCard.FromJson(card)
                .Warnings.Select(s => s.Message));
                return true;
            }
            else
            {
                message = invalid;
                return false;
            }
        }
        catch (AdaptiveSerializationException ex)
        {
            message = ex.Message;
            return false;
        }
    }

    /// <summary>
    /// Is Valid Endpoint
    /// </summary>
    /// <param name="url">Url</param>
    /// <returns>True if is, False if Not</returns>
    private async Task<bool> IsValidEndpoint(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return false;
        try
        {
            var uri = new Uri(url);
            var response = await _client.SendAsync(
                new HttpRequestMessage(HttpMethod.Head, uri));
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Get Json
    /// </summary>
    /// <param name="url">Url</param>
    /// <returns>Json String</returns>
    private async Task<string?> GetJson(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return null;
        try
        {
            var response = await _client.GetStringAsync(url);
            return IsValidJson(response) ? response : null;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Get
    /// </summary>
    /// <param name="prompt">Prompt</param>
    /// <param name="type">Type</param>
    /// <returns>Message</returns>
    public async Task<string?> Get(string prompt,
        PromptType? type = PromptType.Azure) => type switch
        {
            PromptType.GitHub => await gitHubPrompt.Get(prompt),
            PromptType.OpenAI => await openAIPrompt.Get(prompt),
            _ => await azurePrompt.Get(prompt),
        };

    /// <summary>
    /// Get Json Data
    /// </summary>
    /// <param name="endpoint">Endpoint</param>
    /// <returns>Json Data</returns>
    public async Task<string?> GetJsonData(string? endpoint)
    {
        var data = await GetJson(endpoint);
        if (!IsValidJson(data))
            data = new JsonObject().ToJsonString();
        return data;
    }

    /// <summary>
    /// Fetch Json Data
    /// </summary>
    /// <param name="endpoint">Endpoint</param>
    /// <returns>Json Data</returns>
    public async Task<string?> FetchJsonData(string? endpoint) =>
        await IsValidEndpoint(endpoint) ? await GetJsonData(endpoint) : null;

    /// <summary>
    /// Get Card Model
    /// </summary>
    /// <param name="endpoint">Endpoint</param> 
    /// <param name="prompt">Prompt</param>
    /// <param name="type">Type</param>
    /// <returns>Card Model</returns>
    public async Task<CardModel> GetCardModel(
        string? endpoint = null,
        string? prompt = null,
        PromptType? type = PromptType.Azure)
    {
        CardModel model = new();
        if (!string.IsNullOrWhiteSpace(prompt))
        {
            prompt = string.Format(config.CardPrompt, prompt);
        }
        if (await IsValidEndpoint(endpoint))
        {
            model.Data = await GetJsonData(endpoint);            
            prompt = string.Format(config.DataPrompt, endpoint, model.Data);
        }
        if (!string.IsNullOrWhiteSpace(prompt))
        {
            string? card = null;
            string? message = null;
            var success = await _retry.ExecuteAsync(async () =>
            {
                card = await Get(prompt, type);
                var valid = IsValidAdaptiveCard(card, out message);
                return valid && message.Equals(string.Empty);
            });
            if (success)
            {
                model.Template = card;
            }
            else
            {
                model.Error = message;
            }
        }
        return model;
    }
}
