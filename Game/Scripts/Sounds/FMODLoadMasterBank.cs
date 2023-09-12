using FMODUnity;

public class FMODLoadMasterBank
{
    public const string MASTER_BANK = "Master";

    public void StartLoading()
    {
        try
        {
            RuntimeManager.LoadBank(MASTER_BANK, true);
        }
        catch (BankLoadException e)
        {
            RuntimeUtils.DebugLogException(e);
        }
        RuntimeManager.WaitForAllSampleLoading();
    }

    public bool IsLoaded()
    {
        return RuntimeManager.HasBankLoaded(MASTER_BANK);
    }
}
