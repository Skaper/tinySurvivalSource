using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawnersCreator : MonoBehaviour
{
    //public List<WeaponData> Weapons;

    private Dictionary<WeaponData.WeaponType, WeaponSpawner> _weaponSpawners = new Dictionary<WeaponData.WeaponType, WeaponSpawner>();

    public void Initialize(Player player, WeaponPool weaponPool, EnemySpawner enemySpawner)
    {
        foreach (var weapon in player.LevelData.PlayerWeapons)
        {
            var weaponData = weapon.weaponData;
            if (_weaponSpawners.ContainsKey(weaponData.GetWeaponType()) == false)
            {
                var weaponSpawner = Instantiate(weaponData.Spawner, transform);
                weaponSpawner.Initialize(weaponData, player, weaponPool, enemySpawner);
                _weaponSpawners.Add(weaponData.GetWeaponType(), weaponSpawner);
            }
            else
            {
                Debug.LogError("Dictionary already contains spawner with key: " +  weaponData.GetWeaponType() 
                                        + " Weapon name: " + weaponData.GetName());
            }
        }
    }

    public WeaponSpawner GetSpawner(WeaponData.WeaponType type)
    {
        return _weaponSpawners[type];
    }
}
