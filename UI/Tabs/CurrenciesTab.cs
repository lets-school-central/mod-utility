using ModUtility.Commands;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI.Models;

namespace ModUtility.UI.Tabs;

public class CurrenciesTab : UIModel
{
    public override GameObject UIRoot => _uiRoot!;
    internal UIPanel Parent { get; private set; }
    private GameObject? _uiRoot;

    internal CurrenciesTab(UIPanel parent)
    {
        Parent = parent;
    }
    
    public override void ConstructUI(GameObject parent)
    {
        var (scrollBlock, containerBlock) = UIFactoryHelper.AddContainer("SchoolModel", parent);
        _uiRoot = scrollBlock;
        
        UIFactoryHelper.AddInputButtonLine("Money", containerBlock,
            detailsFunc: delegate(Text t)
            {
                CurrenciesCommands.OnMoneyChanged += delegate(int money) { t.text = money.ToString(); };
            },
            addFunc: delegate(string s)
            {
                if (int.TryParse(s, out var money)) CurrenciesCommands.AddMoney(money);
            },
            subFunc: delegate(string s)
            {
                if (int.TryParse(s, out var money)) CurrenciesCommands.AddMoney(-money);
            },
            setFunc: delegate(string s)
            {
                if (int.TryParse(s, out var money)) CurrenciesCommands.SetMoney(money);
            });
        
        UIFactoryHelper.AddInputButtonLine("Humanity", containerBlock,
            detailsFunc: delegate(Text t)
            {
                CurrenciesCommands.OnHumanityPointsChanged += delegate(int points) { t.text = points.ToString(); };
            },
            addFunc: delegate(string s)
            {
                if (int.TryParse(s, out var points)) CurrenciesCommands.AddHumanityPoints(points);
            },
            subFunc: delegate(string s)
            {
                if (int.TryParse(s, out var points)) CurrenciesCommands.AddHumanityPoints(-points);
            },
            setFunc: delegate(string s)
            {
                if (int.TryParse(s, out var points)) CurrenciesCommands.SetHumanityPoints(points);
            });
        
        UIFactoryHelper.AddInputButtonLine("Science", containerBlock,
            detailsFunc: delegate(Text t)
            {
                CurrenciesCommands.OnSciencePointsChanged += delegate(int points) { t.text = points.ToString(); };
            },
            addFunc: delegate(string s)
            {
                if (int.TryParse(s, out var points)) CurrenciesCommands.AddSciencePoints(points);
            },
            subFunc: delegate(string s)
            {
                if (int.TryParse(s, out var points)) CurrenciesCommands.AddSciencePoints(-points);
            },
            setFunc: delegate(string s)
            {
                if (int.TryParse(s, out var points)) CurrenciesCommands.SetSciencePoints(points);
            });
        
        UIFactoryHelper.AddInputButtonLine("Art", containerBlock,
            detailsFunc: delegate(Text t)
            {
                CurrenciesCommands.OnArtPointsChanged += delegate(int points) { t.text = points.ToString(); };
            },
            addFunc: delegate(string s)
            {
                if (int.TryParse(s, out var points)) CurrenciesCommands.AddArtPoints(points);
            },
            subFunc: delegate(string s)
            {
                if (int.TryParse(s, out var points)) CurrenciesCommands.AddArtPoints(-points);
            },
            setFunc: delegate(string s)
            {
                if (int.TryParse(s, out var points)) CurrenciesCommands.SetArtPoints(points);
            });
        
        UIFactoryHelper.AddInputButtonLine("Sport", containerBlock,
            detailsFunc: delegate(Text t)
            {
                CurrenciesCommands.OnSportPointsChanged += delegate(int points) { t.text = points.ToString(); };
            },
            addFunc: delegate(string s)
            {
                if (int.TryParse(s, out var points)) CurrenciesCommands.AddSportPoints(points);
            },
            subFunc: delegate(string s)
            {
                if (int.TryParse(s, out var points)) CurrenciesCommands.AddSportPoints(-points);
            },
            setFunc: delegate(string s)
            {
                if (int.TryParse(s, out var points)) CurrenciesCommands.SetSportPoints(points);
            });
    }
}