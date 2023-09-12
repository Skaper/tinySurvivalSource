using System.Collections;
using FMODUnity;
using UnityEngine;

public class LightningSpawner : WeaponSpawner
{
    private WaitForSeconds wait01 = new WaitForSeconds(0.1f);
    protected override IEnumerator StartAttack()
    {
        while (true)
        {
            UpdateAttackPower();
            UpdateAttackSpeed();
            RuntimeManager.PlayOneShot(_weaponSoundRef);
            if(EnemySpawner.GetListCount() > 0)
                SpawnWeapon(Direction.Right);

            yield return wait01;

            if (EnemySpawner.GetListCount() > 0)
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
}