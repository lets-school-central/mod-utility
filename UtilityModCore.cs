using MelonLoader;
using MelonLoader.Utils;
using ModUtility;
using ModUtility.Commands;
using ModUtility.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniverseLib;
using UniverseLib.Config;

[assembly: MelonInfo(typeof(UtilityModCore), UtilityModCore.Name, UtilityModCore.Version, UtilityModCore.Author)]
[assembly: MelonGame("Pathea Games", "LetsSchool")]
[assembly: MelonPlatformDomain(MelonPlatformDomainAttribute.CompatibleDomains.IL2CPP)]
#pragma warning disable CS0618
[assembly: MelonColor(ConsoleColor.Yellow)]
#pragma warning restore CS0618

namespace ModUtility;

public class UtilityModCore : MelonMod
{
    public const string Name = "UtilityMod";
    public const string Description = "A tool to edit the game, useful for modders.";
    public const string Version = "0.0.1";
    public const string Author = "Scorsi";
    public const string Guid = "app.lets-school-central.mod-utility";
    
    private static bool _isInGame;
    public static bool IsInGame
    {
        get => _isInGame;
        private set
        {
            if (value == _isInGame) return;
            
            _isInGame = value;
            
            UIManager.IsInGame = value;
            
            FurnitureCommands.Init(value);
            CurrenciesCommands.Init(value);
            SchoolCommands.Init(value);
            OtherForcesCommands.Init(value);
        }
    }

    public override void OnInitializeMelon()
    {
        LoggerInstance.Msg($"{Name} (version: {Version}) has been loaded!");
        LoggerInstance.Msg(Description);
    }

    public override void OnLateInitializeMelon()
    {
        Universe.Init(
            1f,
            UIManager.Init,
            (_, _) => { },
            new UniverseLibConfig
            {
                Unhollowed_Modules_Folder = Path.Combine(
                    Path.GetDirectoryName(MelonEnvironment.ModsDirectory)!,
                    Path.Combine("MelonLoader", "Il2CppAssemblies")
                )
            }
        );
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F9)) UIManager.ShowUI = !UIManager.ShowUI;

        IsInGame = SceneManager.GetActiveScene().name == "NewWorld";
    }
}
