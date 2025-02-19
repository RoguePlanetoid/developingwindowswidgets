namespace Prompt.Widget.Library.Prompt;

/// <summary>
/// Azure OpenAI Service Prompt
/// </summary>
internal class AzurePrompt(IAzureConfig config) :
    BasePrompt(new ChatCompletionsClient(new Uri(config.Endpoint),
    new AzureKeyCredential(config.ApiKey))
    .AsChatClient(config.Model));