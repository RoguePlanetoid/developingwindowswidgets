namespace Prompt.Widget.Tests;

/// <summary>
/// Prompt Tests
/// </summary>
public class PromptTests
{
    private IHost? _host;
    private IPromptProvider? _provider = null;

    [SetUp]
    public void Setup()
    {
        _host = Host.CreateDefaultBuilder()
        .ConfigureServices(services => services.AddServices())
        .Build();
        _provider = _host.Services.GetRequiredService<IPromptProvider>();
    }

    [TestCase(PromptType.Azure)]
    [TestCase(PromptType.GitHub)]
    [TestCase(PromptType.OpenAI)]
    public async Task Get_Test(PromptType type)
    {
        // Arrange
        var prompt = "Tell me a joke";
        // Act
        var message = await _provider!.Get(prompt, type);
        // Assert
        Assert.That(message, Is.Not.Null, "The message should not be null.");
    }

    [TestCase(PromptType.Azure)]
    [TestCase(PromptType.GitHub)]
    [TestCase(PromptType.OpenAI)]
    public async Task Card_Endpoint_Test(PromptType type)
    {
        // Arrange
        var endpoint = "https://catfact.ninja/fact";
        // Act
        var card = await _provider!.GetCardModel(endpoint: endpoint, type: type);
        // Assert
        Assert.That(card, Is.Not.Null, "The card should not be null.");
    }

    [TestCase(PromptType.Azure)]
    [TestCase(PromptType.GitHub)]
    [TestCase(PromptType.OpenAI)]
    public async Task Card_Prompt_Test(PromptType type)
    {
        // Arrange
        var prompt = "Celebrate Peter Bull becoming a Microsoft MVP";
        // Act
        var card = await _provider!.GetCardModel(prompt: prompt, type: type);
        // Assert
        Assert.That(card, Is.Not.Null, "The card should not be null.");
    }

    [TearDown]
    public void Teardown() => _host?.Dispose();
}
