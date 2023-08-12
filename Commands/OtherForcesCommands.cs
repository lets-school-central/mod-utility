using Il2CppProjectSchoolNs.OtherForcesNs;
using UnityEngine;

namespace ModUtility.Commands;

public static class OtherForcesCommands
{
    public struct BasicFriendlySchoolInfo
    {
        public string Name;
        public int Score;
        public bool IsDestroyed;
        public Sprite Sprite;
        internal long Id;
    }

    internal static void Init(bool isInGame)
    {
        if (!isInGame)
        {
            _friendlySchoolList.Clear();
            OnFriendlySchoolListChanged?.Invoke();

            return;
        }

        _friendlySchoolList.Clear();
        for (var i = 0; i < OtherForcesModule.Instance.friendlySchoolList.Count; i++)
        {
            var friendlySchool = (FriendlySchoolInstance)OtherForcesModule.Instance.friendlySchoolList[(Index)i];

            if (friendlySchool.config.GetforceName == "My School") continue;
            
            _friendlySchoolList.Add(friendlySchool.config.GetforceName, new BasicFriendlySchoolInfo
            {
                Name = friendlySchool.config.GetforceName,
                Score = friendlySchool.Score,
                IsDestroyed = friendlySchool.isDestory,
                Sprite = friendlySchool.config.Icon,
                Id = i
            });
        }

        OnFriendlySchoolListChanged?.Invoke();
    }

    public static event Action? OnFriendlySchoolListChanged;

    private static Dictionary<string, BasicFriendlySchoolInfo> _friendlySchoolList = new();

    public static IReadOnlyDictionary<string, BasicFriendlySchoolInfo> FriendlySchoolList =>
        UtilityModCore.IsInGame ? _friendlySchoolList : new Dictionary<string, BasicFriendlySchoolInfo>();

    public static bool SetFriendlySchoolIsDestroyed(string name, bool isDestroyed = false)
    {
        if (!UtilityModCore.IsInGame) return false;

        var fs = _friendlySchoolList[name];

        ((FriendlySchoolInstance)OtherForcesModule.Instance.friendlySchoolList[(Index)fs.Id]).isDestory = isDestroyed;
        _friendlySchoolList[name] = fs with { IsDestroyed = isDestroyed };

        OnFriendlySchoolListChanged?.Invoke();
        return true;
    }
}