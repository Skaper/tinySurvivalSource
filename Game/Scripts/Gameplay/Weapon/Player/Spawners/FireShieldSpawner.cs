using System.Collections;

public class FireShieldSpawner : WeaponSpawner
{
    private FMODInstancePlay _instancePlayer;
    protected override void AfterInitialization()
    {
        base.AfterInitialization();
        _instancePlayer = gameObject.AddComponent<FMODInstancePlay>();
        _instancePlayer.Initialize(_weaponSoundRef);
    }

    protected override IEnumerator StartAttack()
    {
        while (true)
        {
            UpdateAttackPower();
            UpdateAttackSpeed();

            SpawnWeapon(Direction.Self);

            yield return waitAtackSpeed;
        }
    }

    protected override void SpawnWeapon(Direction direction)
    {
        base.SpawnWeapon(direction);
        _instancePlayer.Play();
    }

    protected override void OnWeaponDeactivated(Weapon weapon)
    {
        base.OnWeaponDeactivated(weapon);
        _instancePlayer.Stop();
    }

    public override void LevelUp()
    {
        if(level == 1)
            return;

        IncreaseAdditionalScale(10f);
        IncreaseAttackPower(2);
        
        if(level > 7)
            return;
        DecreaseInactiveDelay(0.1f);
        
        switch (GetLevel())
        {
            case 4:
                IncreaseAttackPower(10);
                break;
            case 7:
                IncreaseAdditionalScale(10f);
                break;
        }
    }
}
