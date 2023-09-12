using System.Collections;
using FMODUnity;
using UnityEngine;

public class BowSpawner : WeaponSpawner
{
    private WaitForSeconds wait01 = new (0.1f);
    protected override IEnumerator StartAttack()
    {
        while (true)
        {
            UpdateAttackPower();
            UpdateAttackSpeed();
            
            if(EnemySpawner.GetListCount() > 0)
                SpawnWeapon(Direction.Right);

            yield return wait01;

            if (GetLevel() >= 2 && EnemySpawner.GetListCount() > 0)
                SpawnWeapon(Direction.Right);

            yield return wait01;

            if (GetLevel() >= 3 && EnemySpawner.GetListCount() > 0)
                SpawnWeapon(Direction.Right);

            yield return wait01;

            if (GetLevel() >= 5 && EnemySpawner.GetListCount() > 0)
                SpawnWeapon(Direction.Right);

            yield return wait01;

            if (GetLevel() >= 7 && EnemySpawner.GetListCount() > 0)
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