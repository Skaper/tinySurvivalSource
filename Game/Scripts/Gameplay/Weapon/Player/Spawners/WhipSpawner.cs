using System.Collections;
using FMODUnity;
using UnityEngine;

public class WhipSpawner : WeaponSpawner
{
    private WaitForSeconds wait01 = new WaitForSeconds(0.1f);
    protected override IEnumerator StartAttack()
    {
        while (true)
        {
            UpdateAttackPower();
            UpdateAttackSpeed();

            SpawnWeapon(Direction.Self);
            RuntimeManager.PlayOneShot(_weaponSoundRef);
            yield return wait01;

            if (GetLevel() >= 2)
                SpawnWeapon(Direction.Opposite);

            yield return waitAtackSpeed;
        }
    }

    public override void LevelUp()
    {
        switch (GetLevel())
        {
            case 3:
                IncreaseAttackPower(5);
                break;
            case 4:
                IncreaseAttackPower(5);
                IncreaseAdditionalScale(10f);
                break;
            case 5:
                IncreaseAttackPower(5);
                DecreaseAttackSpeed(10f);
                break;
            case 6:
                IncreaseAttackPower(5);
                IncreaseAdditionalScale(10f);
                break;
            case 7:
                IncreaseAttackPower(10);
                DecreaseAttackSpeed(10f);
                IncreaseAdditionalScale(10f);
                break;
        }
    }
}
