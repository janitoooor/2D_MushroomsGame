using Assets.Scripts;

public class GemBank
{
    public delegate void GemBankSetNewBalance(long newBalance, long oldBalance);
    public event GemBankSetNewBalance GemBankSetsNewBalance;

    private static readonly GemBank s_gemBank = new();

    private long _gemsBalance;
    public long GemsBalance { get => _gemsBalance; }

    public void AddGems(long amount)
    {
        long oldBalance = _gemsBalance;
        _gemsBalance += amount;
        GemBankSetsNewBalance?.Invoke(_gemsBalance, oldBalance);
        JsonBalanceSaveSystem.Instance.Save();
    }

    public void WithdrawGems(long amount)
    {
        long oldBalance = _gemsBalance;
        _gemsBalance -= amount;
        GemBankSetsNewBalance?.Invoke(_gemsBalance, oldBalance);
        JsonBalanceSaveSystem.Instance.Save();
    }

    public void LoadGemBalance(long balance)
    {
        _gemsBalance = balance;
    }

    public static GemBank GetInstance()
    {
        return s_gemBank;
    }
}
