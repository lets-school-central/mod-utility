using Il2CppProjectSchoolNs.SchoolNs;
using Il2CppProjectSchoolNs.SchoolScoreNs;

namespace ModUtility.Commands;

public class SchoolCommands
{
    internal static void Init(bool isInGame)
    {
        if (!isInGame)
        {
            OnNameChanged?.Invoke("");
            OnScoreChanged?.Invoke(0);

            return;
        }

        OnNameChanged?.Invoke(Name);
        OnScoreChanged?.Invoke(Score);
    }

    public static event Action<string>? OnNameChanged;
    public static event Action<int>? OnScoreChanged;

    public static string Name =>
        UtilityModCore.IsInGame ? SchoolModule.Instance.schoolName : "";

    public static int Score => 
        UtilityModCore.IsInGame ? SchoolScoreModule.Instance.GetTotalScore() : 0;

    public static bool EditName(string name)
    {
        if (!UtilityModCore.IsInGame) return false;

        SchoolModule.Instance.SetSchoolName(name);
        OnNameChanged?.Invoke(name);
        return true;
    }

    public static bool AddScore(int score = 1000)
    {
        if (!UtilityModCore.IsInGame) return false;

        SchoolScoreModule.Instance.ModifyScore("挑战", score);
        OnScoreChanged?.Invoke(Score);
        return true;
    }

    public static bool SetScore(int score = 1000)
    {
        return AddScore(score - Score);
    }
}