using System.Collections;
using FMODUnity;
using UnityEngine;

public class AxeSpawner : WeaponSpawner
{
    private WaitForSeconds wait01 = new(0.1f);
    protected override IEnumerator StartAttack()
    {
        while (true)
        {
            UpdateAttackPower();
            UpdateAttackSpeed();
            
            SpawnWeapon(Direction.Left);

            yield return wait01;

            if (GetLevel() >= 2)
                SpawnWeapon(Direction.Right);

            yield return wait01;

            if (GetLevel() >= 5)
                SpawnWeapon(Direction.Left);

            yield return wait01;

            if (GetLevel() >= 7)
                SpawnWeapon(Direction.Right);

            yield return waitAtackSpeed;
        }
    }

    protected override void SpawnWeapon(Direction direction)
    {
        base.SpawnWeapon(direction);
        RuntimeManager.PlayOneShot(_weaponSoundRef);
    }
}
