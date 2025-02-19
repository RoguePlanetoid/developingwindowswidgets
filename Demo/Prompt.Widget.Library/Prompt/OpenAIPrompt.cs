namespace Prompt.Widget.Library.Prompt;

/// <summary>
/// OpenAI Prompt
/// </summary>
internal class OpenAIPrompt(IOpenAIConfig config) :
    BasePrompt(new OpenAIClient(config.ApiKey)
    .AsChatClient(config.Model));