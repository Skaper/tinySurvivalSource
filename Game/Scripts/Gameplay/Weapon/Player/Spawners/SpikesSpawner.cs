using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class SpikesSpawner : WeaponSpawner
{
    public float SpawnDelay = 1f;
    public List<float> SpawnDelaysPerLevel = new List<float>();

    private WaitForSeconds waitForSpawnDelay;
    private void Start()
    {
        waitForSpawnDelay = new(SpawnDelay);
    }

    protected void SpawnWeapon()
    {
        var weapon= _weaponPool.GetPlayerWeapon(weaponData);
        
        var weaponGo= weapon.gameObject;
        
        weapon.SetParameters(weaponData, GetAttackPower(), Direction.Self, level);
        weaponGo.transform.position = _player.transform.position;
        weaponGo.SetActive(true);
    }

    protected override IEnumerator StartAttack()
    {
        while (true)
        {
            UpdateAttackPower();
            UpdateAttackSpeed();
            
            SpawnWeapon();
            RuntimeManager.PlayOneShot(_weaponSoundRef);
            yield return waitForSpawnDelay;
        }
    }

    public override void LevelUp()
    {
        var lvl = GetLevel();
        if (SpawnDelaysPerLevel.Count > level)
            SpawnDelay = SpawnDelaysPerLevel[lvl];
        switch (lvl)
        {
            case 2:
                IncreaseAttackPower(5);
                break;
            case 3:
                IncreaseAttackPower(5);
                IncreaseAdditionalScale(10f);
                break;
            case 4:
                IncreaseAttackPower(5);
                break;
            case 5:
                IncreaseAttackPower(5);
                IncreaseAdditionalScale(10f);
                break;
            case 6:
                IncreaseAdditionalScale(10f);
                break;
        }
    }
}
