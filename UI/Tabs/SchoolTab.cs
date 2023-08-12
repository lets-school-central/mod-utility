using ModUtility.Commands;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI.Models;

namespace ModUtility.UI.Tabs;

internal class SchoolTab : UIModel
{
    public override GameObject UIRoot => _uiRoot!;
    internal UIPanel Parent { get; private set; }
    private GameObject? _uiRoot;

    internal SchoolTab(UIPanel parent)
    {
        Parent = parent;
    }

    public override void ConstructUI(GameObject parent)
    {
        var (scrollBlock, containerBlock) = UIFactoryHelper.AddContainer("SchoolModel", parent);
        _uiRoot = scrollBlock;
        
        UIFactoryHelper.AddInputLine("Name", containerBlock,
            detailsFunc: delegate(Text t)
            {
                SchoolCommands.OnNameChanged += delegate(string name) { t.text = name; };
            },
            setFunc: delegate(string s)
            {
                SchoolCommands.EditName(s);
            });

        UIFactoryHelper.AddInputButtonLine("Score", containerBlock,
            detailsFunc: delegate(Text t)
            {
                SchoolCommands.OnScoreChanged += delegate(int score) { t.text = score.ToString(); };
            },
            addFunc: delegate(string s)
            {
                if (int.TryParse(s, out var score)) SchoolCommands.AddScore(score);
            },
            subFunc: delegate(string s)
            {
                if (int.TryParse(s, out var score)) SchoolCommands.AddScore(-score);
            },
            setFunc: delegate(string s)
            {
                if (int.TryParse(s, out var score)) SchoolCommands.SetScore(score);
            });
    }
}
