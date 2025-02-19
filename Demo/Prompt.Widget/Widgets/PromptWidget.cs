namespace Prompt.Widget.Widgets;

/// <summary>
/// Prompt Widget
/// </summary>
internal class PromptWidget : WidgetBase
{
    private const string save = "save";
    private const string reset = "reset";
    private const string close = "close";
    private const string name = "prompt.json";
    private const string required = "Endpoint or Prompt Required";
    private const string template = "ms-appx:///Assets/Template.json";
    private const string configure = "ms-appx:///Assets/Configure.json";
    private static readonly string default_template = WidgetHelper.ReadJsonFromPackage(template);
    private static readonly string configure_template = WidgetHelper.ReadJsonFromPackage(configure);
    private static readonly string default_data = new JsonObject().ToJsonString();

    private readonly IPromptProvider? _prompt;
    private readonly FileProvider? _file;

    private ConfigureModel _configure = new();
    private CardModel _model = new();

    /// <summary>
    /// Get Default Widget
    /// </summary>
    private static CardModel GetDefaultModel() => new()
    {
        Template = default_template,
        Data = default_data         
    };

    /// <summary>
    /// Load
    /// </summary>
    /// <returns>Widget Model</returns>
    private CardModel Load() =>
        _model = _file!.Load<CardModel>(name) ?? 
            GetDefaultModel();

    /// <summary>
    /// Save
    /// </summary>
    private void Save(CardModel model)
    {
        if (_file!.Save(name, model))
            _model = model;
    }

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        SetState(_file!.ToJson(_configure));
        var updateOptions = new WidgetUpdateRequestOptions(Id)
        {
            Data = GetDataForWidget(),
            Template = GetTemplateForWidget(),
            CustomState = State
        };
        WidgetManager.GetDefault().UpdateWidget(updateOptions);
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="file">File Provider</param>
    /// <param name="prompt">Prompt Provider</param>
    /// <param name="widgetId">Widget Id</param>
    /// <param name="initialState">Initial State</param>
    public PromptWidget(FileProvider? file, IPromptProvider? prompt,
        string widgetId, string initialState) : base(widgetId, initialState)
    {
        _file = file;
        _prompt = prompt;
        _configure = _file?.FromJson<ConfigureModel>(initialState) ?? new();
    }

    /// <summary>
    /// Definition Id
    /// </summary>
    public static string DefinitionId { get; } = nameof(PromptWidget);

    /// <summary>
    /// Configure
    /// </summary>
    protected bool Configure { get; set; } = false;

    /// <summary>
    /// On Action Invoked
    /// </summary>
    /// <param name="actionInvokedArgs">Action Invoked Args</param>
    public override void OnActionInvoked(
        WidgetActionInvokedArgs actionInvokedArgs)
    {
        switch(actionInvokedArgs.Verb)
        {
            case save:
                CardModel? model = null;
                _configure = _file!.FromJson<ConfigureModel>(actionInvokedArgs.Data) ?? new();
                if(_configure.Prompt == null && _configure.Endpoint == null)
                {
                    _configure.Error = required;
                }
                else
                {
                    model = _prompt?.GetCardModel(
                        _configure.Endpoint, _configure.Prompt, _configure.Type).Result;
                }
                if (model?.Error != null)
                {
                    _configure.Error = model.Error;
                    Update();
                }
                else
                {
                    _model = new CardModel
                    {
                        Template = model?.Template ?? default_template,
                        Data = model?.Data ?? default_data
                    };
                    Save(_model);
                    Configure = false;
                    Update();
                }
                break;
            case reset:
                _configure = new();
                Save(GetDefaultModel());
                Configure = false;
                Update();
                break;
            case close:
                Configure = false;
                Update();
                break;
            default:
                if(_configure?.Endpoint != null)
                {
                    var data = _prompt?.FetchJsonData(_configure.Endpoint).Result;
                    _model.Data = data ?? default_data;
                    Save(_model);
                    Update();
                }
                break;
        }
    }

    /// <summary>
    /// On Customization Requested
    /// </summary>
    /// <param name="customizationRequestedArgs">Customization Requested Args</param>
    public override void OnCustomizationRequested(
        WidgetCustomizationRequestedArgs customizationRequestedArgs)
    {
        Configure = true;
        Update();
    }

    /// <summary>
    /// Get Data for Widget
    /// </summary>
    /// <returns>Widget Data</returns>
    public override string GetDataForWidget() => Configure ?
        _file!.ToJson(_configure) : (Load().Data ?? default_data);

    /// <summary>
    /// Get Template for Widget
    /// </summary>
    /// <returns>Widget Template</returns>
    public override string GetTemplateForWidget() => Configure ?
        configure_template : (Load().Template ?? default_template);
}
