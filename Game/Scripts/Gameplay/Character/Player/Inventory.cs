using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponSpawnersCreator))]
[RequireComponent(typeof(AccessoriesCreator))]
public class Inventory : MonoBehaviour
{
    public event Action InventoryChanged;
    [SerializeField]private WeaponSpawnersCreator _weaponSpawnersCreator;
    [SerializeField]private AccessoriesCreator _accessoriesCreator;
    
    private Dictionary<WeaponData.WeaponType, int> weaponInventory = new Dictionary<WeaponData.WeaponType, int>();
    private Dictionary<AccessoryData.AccessoryType, int> accesoInventory = new Dictionary<AccessoryData.AccessoryType, int>();
    
    public void Initialize(Player player, WeaponPool weaponPool, EnemySpawner enemySpawner)
    {
        _weaponSpawnersCreator.Initialize(player, weaponPool, enemySpawner);
        _accessoriesCreator.Initialize(player);
    }
    
    void Awake()
    {
        if (_weaponSpawnersCreator == null)
        {
            _weaponSpawnersCreator = gameObject.GetComponent<WeaponSpawnersCreator>();
        }

        if (_accessoriesCreator == null)
        {
            _accessoriesCreator = gameObject.GetComponent<AccessoriesCreator>();
        }
    }

    public WeaponData GetWeaponDataAsset(WeaponData.WeaponType type)
    {
        return _weaponSpawnersCreator.GetSpawner(type).weaponData;
    }
    
    public AccessoryData GetAccessoryDataAsset(AccessoryData.AccessoryType type)
    {
        return _accessoriesCreator.GetAccessory(type).accessoryData;
    }
    
    

    public Dictionary<WeaponData.WeaponType, int> GetWeaponInventory()
    {
        return weaponInventory;
    }

    public Dictionary<AccessoryData.AccessoryType, int> GetAccInventory()
    {
        return accesoInventory;
    }

    public void AddWeapon(WeaponData.WeaponType weaponType)
    {
        var spawner = _weaponSpawnersCreator.GetSpawner(weaponType);

        if (weaponInventory.ContainsKey(weaponType))
        {
            spawner.IncreaseLevel();
            weaponInventory.Remove(spawner.GetWeaponType());
            weaponInventory.Add(spawner.GetWeaponType(), spawner.GetLevel());
            Debug.Log("Adding upgraded weapon to player: " + weaponType);
        }
        else
        {
            weaponInventory.Add(spawner.GetWeaponType(), 1);
            spawner.StartWeapon();
        }
        InventoryChanged?.Invoke();
    }

    public void AddAccessory(AccessoryData.AccessoryType accessoryType)
    {
        var accessory = _accessoriesCreator.GetAccessory(accessoryType);

        if (accesoInventory.ContainsKey(accessoryType))
        {
            accessory.IncreaseLevel();
            accesoInventory.Remove(accessory.GetAccessoryType());
            accesoInventory.Add(accessory.GetAccessoryType(), accessory.GetLevel());
            accessory.ApplyEffect();
        }
        else
        {
            accesoInventory.Add(accessory.GetAccessoryType(), 1);
            accessory.ApplyEffect();
        }
        InventoryChanged?.Invoke();
    }
}