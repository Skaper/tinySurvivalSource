using System.Collections;

public class BeholderSpawner : WeaponSpawner
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

            SpawnWeapon(Direction.Right);

            if (GetLevel() >= 2)
                SpawnWeapon(Direction.Left);

            if (GetLevel() >= 5)
                SpawnWeapon(Direction.Up);

            if (GetLevel() >= 7)
                SpawnWeapon(Direction.Down);

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
}