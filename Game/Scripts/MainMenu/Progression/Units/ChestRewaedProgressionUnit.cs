public class ChestRewaedProgressionUnit : ProgressionUnit
{
    public ChestReward Chest;
    public override void Activate()
    {
        Chest.Unlock();
    }

    public override void ActivateWithAnimation()
    {
        //TODO PLAY OPEN ANIMATION
        Activate();
    }

    public override void Deactivate()
    {
        Chest.Lock();
    }
}
