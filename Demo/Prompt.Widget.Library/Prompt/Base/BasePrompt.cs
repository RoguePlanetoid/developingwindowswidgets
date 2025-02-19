namespace Prompt.Widget.Library.Prompt.Base;

/// <summary>
/// Base Prompt
/// </summary>
/// <param name="client">Chat Client</param>
internal abstract class BasePrompt(IChatClient client)
{
    /// <summary>
    /// Get
    /// </summary>
    /// <param name="prompt">Prompt</param>
    /// <returns>Message</returns>
    public async Task<string?> Get(string prompt)
    {
        try
        {
            return (await client.CompleteAsync(prompt)).Message.Text;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
