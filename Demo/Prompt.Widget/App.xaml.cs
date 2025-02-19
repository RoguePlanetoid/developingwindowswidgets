﻿namespace Prompt.Widget;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    private static FileProvider? _file;
    private static IPromptProvider? _prompt;

    /// <summary>
    /// Host
    /// </summary>
    public static IHost? Host { get; private set; }

    /// <summary>
    /// Start Service Host
    /// </summary>
    private static async Task StartServiceHost()
    {
        Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
        .ConfigureServices(services => services.AddServices())
        .Build();
        await Host!.StartAsync();
        _file = Host?.Services.GetRequiredService<FileProvider>();
        _prompt = Host?.Services.GetRequiredService<IPromptProvider>();
    }

    /// <summary>
    /// Register Widget Provider
    /// </summary>
    private static void RegisterWidgetProvider()
    {
        ComWrappersSupport.InitializeComWrappers();
        WidgetProvider.AddWidget(PromptWidget.DefinitionId, (widgetId, initialState) =>
            new PromptWidget(_file, _prompt, widgetId, initialState));
        WidgetRegistrationManager<WidgetProvider>.RegisterProvider();
    }

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App() => InitializeComponent();

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        await StartServiceHost();
        RegisterWidgetProvider();
    }
}
