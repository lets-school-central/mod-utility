using Il2CppProjectSchoolNs.SchoolNs;
using Il2CppProjectSchoolNs.WorldNs;
using MelonLoader;
using UnityEngine;

namespace ModUtility.Commands;

public static class FurnitureCommands
{
    public struct BasicFurnitureInfo
    {
        public string Name;
        public bool IsUnlocked;
        public Sprite Sprite;
        internal long Id;
    }

    internal static void Init(bool isInGame)
    {
        if (!isInGame)
        {
            _furnitureList.Clear();
        }
    }

    public static void Load()
    {
        foreach (var furnitureConfigItem in
                 MapModule.Instance.mapFurnitureSubModule.furnitureSubModuleConfig.FurnitureConfigItems)
        {
            MelonLogger.Msg("Furniture: " + furnitureConfigItem.Id);

            if (!MapModule.Instance.mapFurnitureSubModule.loadedFurnitureTemplates.ContainsKey(furnitureConfigItem.Id))
                continue;
            
            var loadedFurniture =
                MapModule.Instance.mapFurnitureSubModule.loadedFurnitureTemplates[furnitureConfigItem.Id];
            var isUnlocked = SchoolModule.Instance.unlockFurniture.Contains(loadedFurniture);
            
            if (_furnitureList.ContainsKey(furnitureConfigItem.Id))
                _furnitureList.Remove(furnitureConfigItem.Id);
            
            _furnitureList.Add(furnitureConfigItem.Id, new BasicFurnitureInfo
            {
                Name = loadedFurniture.GetFurnitureName(),
                IsUnlocked = isUnlocked,
                Sprite = loadedFurniture.icon,
                Id = furnitureConfigItem.Id
            });
        }
        OnFurnitureListChanged?.Invoke();
    }

    public static event Action? OnFurnitureListChanged;

    private static Dictionary<long, BasicFurnitureInfo> _furnitureList = new();

    public static IReadOnlyDictionary<long, BasicFurnitureInfo> FurnitureList =>
        UtilityModCore.IsInGame ? _furnitureList : new Dictionary<long, BasicFurnitureInfo>();

    public static bool ToggleFurnitureLock(long id)
    {
        if (!UtilityModCore.IsInGame) return false;
        
        if (!MapModule.Instance.mapFurnitureSubModule.loadedFurnitureTemplates.ContainsKey(id)) return false;
        
        var f = _furnitureList[id];
        var loadedFurniture = MapModule.Instance.mapFurnitureSubModule.loadedFurnitureTemplates[id];
        
        if (f.IsUnlocked) SchoolModule.Instance.LockFurniture(loadedFurniture);
        else SchoolModule.Instance.UnlockFurniture(loadedFurniture, false);
        
        _furnitureList[id] = f with { IsUnlocked = !f.IsUnlocked };
        OnFurnitureListChanged?.Invoke();
        
        return true;
    }
}