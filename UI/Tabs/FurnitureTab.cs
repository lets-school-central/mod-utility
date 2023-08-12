using ModUtility.Commands;
using UnityEngine;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace ModUtility.UI.Tabs;

public class FurnitureTab : UIModel
{
    public override GameObject UIRoot => _uiRoot!;

    internal UIPanel Parent { get; private set; }
    private GameObject? _uiRoot;

    private List<GameObject> _furnitureBlocks = new();

    internal FurnitureTab(UIPanel parent)
    {
        Parent = parent;
    }

    public override void ConstructUI(GameObject parent)
    {
        var (scrollBlock, containerBlock) = UIFactoryHelper.AddContainer("SchoolModel", parent);
        _uiRoot = scrollBlock;

        FurnitureCommands.OnFurnitureListChanged += delegate
        {
            foreach (var block in _furnitureBlocks)
            {
                block.SetActive(false);
                GameObject.Destroy(block);
            }

            _furnitureBlocks.Clear();

            foreach (var fs in FurnitureCommands.FurnitureList.Values)
            {
                _furnitureBlocks.Add(CreateFurnitureBlock(fs, containerBlock,
                    delegate { FurnitureCommands.ToggleFurnitureLock(fs.Id); }));
            }
        };
    }

    private static GameObject CreateFurnitureBlock(FurnitureCommands.BasicFurnitureInfo bfi, GameObject parent,
        Action toggleLockFunc)
    {
        var block = UIFactory.CreateHorizontalGroup(
            parent, $"Furniture{bfi.Name}Block",
            true, true, true, true,
            25, new Vector4(4, 4, 4, 4),
            Color.grey, TextAnchor.MiddleLeft);
        UIFactory.SetLayoutElement(block, minHeight: 90, flexibleHeight: 0);

        UIFactoryHelper.AddImage(block, bfi.Sprite, width: 90, height: 90);

        var secondBlock = UIFactory.CreateVerticalGroup(
            block, "SecondBlock",
            true, true, true, true,
            5, Vector4.zero,
            Color.grey, TextAnchor.MiddleLeft);
        UIFactory.SetLayoutElement(block, minWidth: 250, minHeight: 90, flexibleHeight: 0, flexibleWidth: 9999999);

        var nameLabel =
            UIFactory.CreateLabel(secondBlock, "NameLabel", bfi.Name, alignment: TextAnchor.MiddleLeft, fontSize: 25);
        UIFactory.SetLayoutElement(nameLabel.gameObject, minHeight: 30, flexibleHeight: 0, flexibleWidth: 9999999);

        var isUnlocked =
            UIFactory.CreateLabel(secondBlock, "IsDestroyedLabel", bfi.IsUnlocked ? "Unlocked" : "Locked",
                alignment: TextAnchor.MiddleLeft, color: bfi.IsUnlocked ? Color.green : Color.red);
        UIFactory.SetLayoutElement(isUnlocked.gameObject, minHeight: 15, flexibleHeight: 0);

        var buttonBlock = UIFactory.CreateHorizontalGroup(
            secondBlock, "ButtonBlock",
            true, true, true, true,
            5, Vector4.zero,
            Color.grey, TextAnchor.MiddleLeft);
        UIFactory.SetLayoutElement(buttonBlock, minHeight: 30, flexibleHeight: 0);

        var unlockButton = UIFactory.CreateButton(buttonBlock, "UnlockButton", bfi.IsUnlocked ? "Lock" : "Unlock");
        UIFactory.SetLayoutElement(unlockButton.GameObject, minWidth: 60, flexibleHeight: 0);
        unlockButton.OnClick += toggleLockFunc;

        return block;
    }
}