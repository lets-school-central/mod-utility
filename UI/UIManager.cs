using ModUtility.Commands;
using ModUtility.UI.Tabs;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Panels;

namespace ModUtility.UI;

public static class UIManager
{
    private static UIBase? _uiBase;

    private static UIPanel? _panel;

    public static bool ShowUI
    {
        get => _uiBase is { Enabled: true };
        set
        {
            if (_uiBase == null || _panel == null || _uiBase.Enabled == value)
                return;

            UniversalUI.SetUIActive(UtilityModCore.Guid, value);
            _panel.ShowUI = value;
        }
    }

    internal static bool IsInGame
    {
        set
        {
            if (_panel == null)
                return;

            _panel.IsInGame = value;
        }
    }

    internal static void Init()
    {
        _uiBase = UniversalUI.RegisterUI(UtilityModCore.Guid, Update);
        _panel = new UIPanel(_uiBase);
    }

    private static void Update()
    {
    }
}

internal class UIPanel : PanelBase
{
    public UIPanel(UIBase owner) : base(owner)
    {
    }

    public override string Name => "Utilities";
    public override int MinWidth => 600;
    public override int MinHeight => 500;
    public override Vector2 DefaultAnchorMin => new(0.25f, 0.25f);
    public override Vector2 DefaultAnchorMax => new(0.75f, 0.75f);
    public override bool CanDragAndResize => true;

    private UIFactoryHelper.TabContainer? _tabs;
    private Text? _waitingPanel;

    internal bool ShowUI
    {
        set
        {
            if (ContentRoot == null || _tabs == null || _waitingPanel == null || ContentRoot.active == value)
                return;
            
            ContentRoot.SetActive(value);
        }
    }

    internal bool IsInGame
    {
        set
        {
            if (_tabs == null || _waitingPanel == null)
                return;

            _tabs.Value.SetActive(value);
            _waitingPanel.gameObject.SetActive(!value);
        }
    }

    protected override void ConstructPanelContent()
    {
        _tabs = UIFactoryHelper.AddTabContainer("Utilities", ContentRoot, new UIFactoryHelper.TabContainer.Option[]
        {
            new("School", () => new SchoolTab(this)),
            new("Currencies", () => new CurrenciesTab(this)),
            new("Other Forces", () => new OtherForcesTab(this)),
            new("Furniture", () => new FurnitureTab(this), delegate { FurnitureCommands.Load(); }),
        });
        _tabs.Value.SetActive(false);

        _waitingPanel =
            UIFactory.CreateLabel(ContentRoot, "WaitingLabel", "Waiting for game to load...", TextAnchor.MiddleCenter, Color.red, fontSize: 24);
    }
}
