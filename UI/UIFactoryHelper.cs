using UnityEngine;
using UnityEngine.UI;
using UniverseLib;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace ModUtility.UI;

internal static class UIFactoryHelper
{
    internal static (GameObject, GameObject) AddContainer(string name, GameObject parent)
    {
        var scrollBlock =
            UIFactory.CreateScrollView(parent, "scrollBlock", out var scrollBlockContent, out _);

        var container = UIFactory.CreateVerticalGroup(scrollBlockContent, "parentBlock",
            true, false, true, true,
            10, Vector4.zero,
            Color.clear,
            TextAnchor.UpperCenter);

        return (scrollBlock, container);
    }

    internal struct TabContainer
    {
        internal struct Option
        {
            internal readonly string Name;
            internal readonly Func<UIModel> BuildFunc;
            internal readonly Action? OnClickFunc;

            public Option(string name, Func<UIModel> buildFunc, Action? onClickFunc = null)
            {
                Name = name;
                BuildFunc = buildFunc;
                OnClickFunc = onClickFunc;
            }
        }
        
        internal GameObject TabGroup;
        internal List<(string, ButtonRef, UIModel, Action?)> Tabs;
        
        public bool Active => TabGroup.active;

        public void SetActiveTab(string name)
        {
            foreach (var (tabName, buttonRef, uiModel, onClick) in Tabs)
            {
                if (tabName == name)
                {
                    uiModel.SetActive(true);
                    RuntimeHelper.SetColorBlock(buttonRef.Component, UniversalUI.EnabledButtonColor,
                        UniversalUI.EnabledButtonColor * 1.2f);
                    onClick?.Invoke();
                }
                else
                {
                    uiModel.SetActive(false);
                    RuntimeHelper.SetColorBlock(buttonRef.Component, UniversalUI.DisabledButtonColor,
                        UniversalUI.DisabledButtonColor * 1.2f);
                }
            }
        }

        public void SetActive(bool active)
        {
            TabGroup.SetActive(active);

            if (active)
            {
                SetActiveTab(Tabs[0].Item1);
                Tabs[0].Item4?.Invoke();
            }
            else
            {
                foreach (var (_, _, uiModel, _) in Tabs)
                {
                    uiModel.SetActive(false);
                }
            }
        }
    }

    internal static TabContainer AddTabContainer(string name, GameObject parent, TabContainer.Option[] tabOptions)
    {
        var tabGroup = UIFactory.CreateHorizontalGroup(parent, "parentBlock",
            true, true, true, true,
            2, new Vector4(2, 2, 2, 2),
            Color.clear,
            TextAnchor.MiddleLeft);
        UIFactory.SetLayoutElement(tabGroup, minHeight: 30, flexibleHeight: 0);

        var self = new TabContainer()
        {
            TabGroup = tabGroup,
            Tabs = new List<(string, ButtonRef, UIModel, Action?)>(),
        };

        foreach (var tabOption in tabOptions)
        {
            var button = UIFactory.CreateButton(tabGroup, $"{tabOption.Name}TabButton", tabOption.Name);
            RuntimeHelper.SetColorBlock(button.Component, UniversalUI.DisabledButtonColor,
                UniversalUI.DisabledButtonColor * 1.2f);
            var model = tabOption.BuildFunc();
            model.ConstructUI(parent);
            model.SetActive(false);

            button.OnClick += () => { self.SetActiveTab(tabOption.Name); };

            self.Tabs.Add((tabOption.Name, button, model, tabOption.OnClickFunc));
        }

        self.SetActiveTab(tabOptions[0].Name);

        return self;
    }

    internal static void AddInputButtonLine(string name, GameObject parent, string? placeholderText = null,
        Action<Text>? detailsFunc = null, Action<string>? addFunc = null,
        Action<string>? subFunc = null, Action<string>? setFunc = null)
    {
        var block = UIFactory.CreateVerticalGroup(
            parent, $"{name}Block",
            true, true, true, true,
            5, new Vector4(4, 4, 4, 4),
            Color.grey, TextAnchor.MiddleCenter);
        UIFactory.SetLayoutElement(block, minHeight: 60, flexibleHeight: 0);

        var labelLineBlock = UIFactory.CreateHorizontalGroup(
            block, $"{name}LabelLineBlock",
            false, true, true, true,
            10, Vector4.zero,
            Color.grey,
            TextAnchor.MiddleLeft);
        UIFactory.SetLayoutElement(labelLineBlock, minHeight: 30, flexibleHeight: 0);

        var label = UIFactory.CreateLabel(labelLineBlock, $"{name}Label", $"{name}:", alignment: TextAnchor.MiddleLeft);
        UIFactory.SetLayoutElement(label.gameObject, minWidth: 150, flexibleWidth: 0);

        if (detailsFunc != null)
        {
            var details = UIFactory.CreateLabel(labelLineBlock, $"{name}DetailsLabel", "",
                alignment: TextAnchor.MiddleLeft);
            UIFactory.SetLayoutElement(details.gameObject, minWidth: 150, flexibleWidth: 0);
            detailsFunc(details);
        }

        var buttonLineBlock = UIFactory.CreateHorizontalGroup(
            block, $"{name}ButtonLineBlock",
            false, true, true, true,
            10, Vector4.zero,
            Color.grey,
            TextAnchor.MiddleLeft);
        UIFactory.SetLayoutElement(buttonLineBlock, minHeight: 30, flexibleHeight: 0);

        var input = UIFactory.CreateInputField(buttonLineBlock, $"{name}Input", placeholderText ?? name);
        UIFactory.SetLayoutElement(input.GameObject, minWidth: 200, flexibleWidth: 0);

        if (addFunc != null)
        {
            var addButton = UIFactory.CreateButton(buttonLineBlock, $"{name}AddButton", "Add");
            UIFactory.SetLayoutElement(addButton.GameObject, minWidth: 50, flexibleWidth: 0);
            addButton.OnClick += () => { addFunc(input.Text); };
        }

        if (subFunc != null)
        {
            var subButton = UIFactory.CreateButton(buttonLineBlock, $"{name}SubButton", "Sub");
            UIFactory.SetLayoutElement(subButton.GameObject, minWidth: 50, flexibleWidth: 0);
            subButton.OnClick += () => { subFunc(input.Text); };
        }

        if (setFunc != null)
        {
            var setButton = UIFactory.CreateButton(buttonLineBlock, $"{name}SetButton", "Set");
            UIFactory.SetLayoutElement(setButton.GameObject, minWidth: 50, flexibleWidth: 0);
            setButton.OnClick += () => { setFunc(input.Text); };
        }
    }

    internal static void AddInputLine(string name, GameObject parent, string? placeholderText = null,
        Action<Text>? detailsFunc = null, Action<string>? setFunc = null)
    {
        var block = UIFactory.CreateVerticalGroup(
            parent, $"{name}Block",
            true, true, true, true,
            5, new Vector4(4, 4, 4, 4),
            Color.grey, TextAnchor.MiddleCenter);
        UIFactory.SetLayoutElement(block, minHeight: 60, flexibleHeight: 0);

        var labelLineBlock = UIFactory.CreateHorizontalGroup(
            block, $"{name}LabelLineBlock",
            false, true, true, true,
            10, Vector4.zero,
            Color.grey,
            TextAnchor.MiddleLeft);
        UIFactory.SetLayoutElement(labelLineBlock, minHeight: 30, flexibleHeight: 0);

        var label = UIFactory.CreateLabel(labelLineBlock, $"{name}Label", $"{name}:", alignment: TextAnchor.MiddleLeft);
        UIFactory.SetLayoutElement(label.gameObject, minWidth: 150, flexibleWidth: 0);

        if (detailsFunc != null)
        {
            var details = UIFactory.CreateLabel(labelLineBlock, $"{name}DetailsLabel", "",
                alignment: TextAnchor.MiddleLeft);
            UIFactory.SetLayoutElement(details.gameObject, minWidth: 150, flexibleWidth: 0);
            detailsFunc(details);
        }

        var buttonLineBlock = UIFactory.CreateHorizontalGroup(
            block, $"{name}ButtonLineBlock",
            false, true, true, true,
            10, Vector4.zero,
            Color.grey,
            TextAnchor.MiddleLeft);
        UIFactory.SetLayoutElement(buttonLineBlock, minHeight: 30, flexibleHeight: 0);

        var input = UIFactory.CreateInputField(buttonLineBlock, $"{name}Input", placeholderText ?? name);
        UIFactory.SetLayoutElement(input.GameObject, minWidth: 200, flexibleWidth: 0);

        var setButton = UIFactory.CreateButton(buttonLineBlock, $"{name}SetButton", "Set");
        UIFactory.SetLayoutElement(setButton.GameObject, minWidth: 50, flexibleWidth: 0);
        setButton.OnClick += () => { setFunc?.Invoke(input.Text); };
    }

    internal static GameObject AddImage(GameObject parent, Sprite sprite, int width = 60, int height = 60)
    {
        var imageViewport = UIFactory.CreateVerticalGroup(parent, "ImageViewport", false, false, true, true,
            bgColor: new Color(1, 1, 1, 0), childAlignment: TextAnchor.MiddleCenter);
        UIFactory.SetLayoutElement(imageViewport, minWidth: width, minHeight: height, flexibleWidth: 0, flexibleHeight: 0);

        var imageHolder = UIFactory.CreateUIObject("ImageHolder", imageViewport);
        UIFactory.SetLayoutElement(imageHolder, minWidth: width, minHeight: height, flexibleWidth: 0, flexibleHeight: 0);

        var actualImageObj = UIFactory.CreateUIObject("ActualImage", imageHolder);
        var actualRect = actualImageObj.GetComponent<RectTransform>();
        actualRect.anchorMin = new Vector2(0, 0);
        actualRect.anchorMax = new Vector2(1, 1);
        var image = actualImageObj.AddComponent<Image>();

        image.sprite = sprite;

        return imageViewport;
    }
}