using ModUtility.Commands;
using UnityEngine;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace ModUtility.UI.Tabs;

public class OtherForcesTab : UIModel
{
    public override GameObject UIRoot => _uiRoot!;

    internal UIPanel Parent { get; private set; }
    private GameObject? _uiRoot;

    private List<GameObject> _friendlySchoolBlocks = new();

    internal OtherForcesTab(UIPanel parent)
    {
        Parent = parent;
    }

    public override void ConstructUI(GameObject parent)
    {
        var (scrollBlock, containerBlock) = UIFactoryHelper.AddContainer("SchoolModel", parent);
        _uiRoot = scrollBlock;

        OtherForcesCommands.OnFriendlySchoolListChanged += delegate
        {
            foreach (var block in _friendlySchoolBlocks)
            {
                block.SetActive(false);
                GameObject.Destroy(block);
            }

            _friendlySchoolBlocks.Clear();

            foreach (var fs in OtherForcesCommands.FriendlySchoolList.Values)
            {
                _friendlySchoolBlocks.Add(CreateFriendlySchoolBlock(fs, containerBlock,
                    delegate { OtherForcesCommands.SetFriendlySchoolIsDestroyed(fs.Name, !fs.IsDestroyed); }));
            }
        };
    }

    private static GameObject CreateFriendlySchoolBlock(OtherForcesCommands.BasicFriendlySchoolInfo bfsi, GameObject parent,
        Action toggleIsDestroyedFunc)
    {
        var block = UIFactory.CreateHorizontalGroup(
            parent, $"FriendlySchool{bfsi.Name}Block",
            true, true, true, true,
            25, new Vector4(4, 4, 4, 4),
            Color.grey, TextAnchor.MiddleCenter);
        UIFactory.SetLayoutElement(block, minHeight: 90, flexibleHeight: 0);

        UIFactoryHelper.AddImage(block, bfsi.Sprite, width: 90, height: 90);

        var secondBlock = UIFactory.CreateVerticalGroup(
            block, "SecondBlock",
            true, true, true, true,
            5, Vector4.zero,
            Color.grey, TextAnchor.MiddleLeft);
        UIFactory.SetLayoutElement(block, minWidth: 250, minHeight: 90, flexibleHeight: 0, flexibleWidth: 9999999);
        
        var nameLabel =
            UIFactory.CreateLabel(secondBlock, "NameLabel", $"{bfsi.Name}:", alignment: TextAnchor.MiddleLeft, fontSize: 30);
        UIFactory.SetLayoutElement(nameLabel.gameObject, minHeight: 30, flexibleHeight: 0, flexibleWidth: 9999999);

        var scoreLabel =
            UIFactory.CreateLabel(secondBlock, "ScoreLabel", $"Score: {bfsi.Score}", alignment: TextAnchor.MiddleLeft);
        UIFactory.SetLayoutElement(scoreLabel.gameObject, minWidth: 150, flexibleWidth: 0);

        var isDestroyedLabel =
            UIFactory.CreateLabel(secondBlock, "IsDestroyedLabel", bfsi.IsDestroyed ? "Destroyed" : "Standing",
                alignment: TextAnchor.MiddleLeft, color: bfsi.IsDestroyed ? Color.red : Color.green);
        UIFactory.SetLayoutElement(isDestroyedLabel.gameObject, minWidth: 150, flexibleWidth: 0);

        var buttonLineBlock = UIFactory.CreateHorizontalGroup(
            secondBlock, "ButtonLineBlock",
            false, true, true, true,
            10, Vector4.zero,
            Color.grey,
            TextAnchor.MiddleLeft);
        UIFactory.SetLayoutElement(buttonLineBlock, minHeight: 30, flexibleHeight: 0);

        var toggleIsDestroyedButton = UIFactory.CreateButton(buttonLineBlock, $"ToggleIsDestroyedButton",
            bfsi.IsDestroyed ? "Restore" : "Destroy");
        UIFactory.SetLayoutElement(toggleIsDestroyedButton.GameObject, minWidth: 60, flexibleWidth: 0);
        toggleIsDestroyedButton.OnClick += toggleIsDestroyedFunc;

        return block;
    }
}