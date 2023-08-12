using Il2CppInterop.Runtime;
using Il2CppProjectSchoolNs.SchoolNs;

namespace ModUtility.Commands;

public static class CurrenciesCommands
{
    private static OnCurrencyChanged? _onCurrencyChangedDelegate;
    private static OnMoneyChanged? _onMoneyChangedDelegate;

    internal static void Init(bool isInGame)
    {
        if (!isInGame)
        {
            UnsubscribeToEvents();

            OnMoneyChanged?.Invoke(0);
            OnHumanityPointsChanged?.Invoke(0);
            OnSciencePointsChanged?.Invoke(0);
            OnArtPointsChanged?.Invoke(0);
            OnSportPointsChanged?.Invoke(0);

            return;
        }

        OnMoneyChanged?.Invoke(Money);
        OnHumanityPointsChanged?.Invoke(HumanityPoints);
        OnSciencePointsChanged?.Invoke(SciencePoints);
        OnArtPointsChanged?.Invoke(ArtPoints);
        OnSportPointsChanged?.Invoke(SportPoints);

        SubscribeToEvents();
    }

    private static void SubscribeToEvents()
    {
        if (_onMoneyChangedDelegate == null)
            _onMoneyChangedDelegate = DelegateSupport.ConvertDelegate<OnMoneyChanged>(
                delegate(int _, MoneyUseType _) { OnMoneyChanged?.Invoke(Money); });

        if (_onCurrencyChangedDelegate == null)
            _onCurrencyChangedDelegate = DelegateSupport.ConvertDelegate<OnCurrencyChanged>(
                delegate(CurrencyType type, int _, MoneyUseType _)
                {
                    switch (type)
                    {
                        case CurrencyType.CultureScore:
                            OnHumanityPointsChanged?.Invoke(HumanityPoints);
                            break;
                        case CurrencyType.ScienceScore:
                            OnSciencePointsChanged?.Invoke(SciencePoints);
                            break;
                        case CurrencyType.ArtsScore:
                            OnArtPointsChanged?.Invoke(ArtPoints);
                            break;
                        case CurrencyType.SportsScore:
                            OnSportPointsChanged?.Invoke(SportPoints);
                            break;
                    }
                });

        SchoolModule.Instance.add_onMoneyChange(_onMoneyChangedDelegate);
        SchoolModule.Instance.add_OnCurrencyChanged(_onCurrencyChangedDelegate);
    }

    private static void UnsubscribeToEvents()
    {
        SchoolModule.Instance.remove_onMoneyChange(_onMoneyChangedDelegate);
        SchoolModule.Instance.remove_OnCurrencyChanged(_onCurrencyChangedDelegate);
    }

    public static event Action<int>? OnHumanityPointsChanged;
    public static event Action<int>? OnSciencePointsChanged;
    public static event Action<int>? OnArtPointsChanged;
    public static event Action<int>? OnSportPointsChanged;
    public static event Action<int>? OnMoneyChanged;

    public static int HumanityPoints =>
        UtilityModCore.IsInGame ? SchoolModule.Instance.GetCurrency(CurrencyType.CultureScore) : 0;

    public static int SciencePoints =>
        UtilityModCore.IsInGame ? SchoolModule.Instance.GetCurrency(CurrencyType.ScienceScore) : 0;

    public static int ArtPoints =>
        UtilityModCore.IsInGame ? SchoolModule.Instance.GetCurrency(CurrencyType.ArtsScore) : 0;

    public static int SportPoints =>
        UtilityModCore.IsInGame ? SchoolModule.Instance.GetCurrency(CurrencyType.SportsScore) : 0;

    public static int Money =>
        UtilityModCore.IsInGame ? SchoolModule.Instance.money : 0;

    public static bool AddMoney(int count = 1000)
    {
        if (!UtilityModCore.IsInGame) return false;

        SchoolModule.Instance.AddMoney(count, MoneyUseType.other);
        return true;
    }

    public static bool SetMoney(int count = 1000)
    {
        return AddMoney(count - Money);
    }

    public static bool AddHumanityPoints(int count = 1000)
    {
        if (!UtilityModCore.IsInGame) return false;

        SchoolModule.Instance.ModifyCurrency(CurrencyType.CultureScore, count, MoneyUseType.other);
        return true;
    }

    public static bool SetHumanityPoints(int count = 1000)
    {
        return AddHumanityPoints(count - HumanityPoints);
    }

    public static bool AddSciencePoints(int count = 1000)
    {
        if (!UtilityModCore.IsInGame) return false;

        SchoolModule.Instance.ModifyCurrency(CurrencyType.ScienceScore, count, MoneyUseType.other);
        return true;
    }

    public static bool SetSciencePoints(int count = 1000)
    {
        return AddSciencePoints(count - SciencePoints);
    }

    public static bool AddArtPoints(int count = 1000)
    {
        if (!UtilityModCore.IsInGame) return false;

        SchoolModule.Instance.ModifyCurrency(CurrencyType.ArtsScore, count, MoneyUseType.other);
        return true;
    }

    public static bool SetArtPoints(int count = 1000)
    {
        return AddArtPoints(count - ArtPoints);
    }

    public static bool AddSportPoints(int count = 1000)
    {
        if (!UtilityModCore.IsInGame) return false;

        SchoolModule.Instance.ModifyCurrency(CurrencyType.SportsScore, count, MoneyUseType.other);
        return true;
    }

    public static bool SetSportPoints(int count = 1000)
    {
        return AddSportPoints(count - SportPoints);
    }
}