using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponPool : MonoBehaviour
{
    public int CopiesPerWeapon = 25;
    [FormerlySerializedAs("WeaponsList")] [SerializeField] List<PlayerWeapon> PlayerWeaponsList;
    [SerializeField] List<EnemyWeapon> EnemyWeaponsList;
    private Dictionary<WeaponData.WeaponType, PlayerWeapon> _playerWeapons;
    private Dictionary<WeaponData.WeaponType, Queue<PlayerWeapon>> _playerWeaponFreePool;
    private Dictionary<EnemyWeaponData.WeaponType, EnemyWeapon> _enemyWeapons;
    private Dictionary<EnemyWeaponData.WeaponType, Queue<EnemyWeapon>> _enemyWeaponFreePool;
    private Transform player;
    public static WeaponPool instance; 
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    public void Initialize(LevelData levelData, Transform player)
    {
        this.player = player;
        if (levelData != null)
        {
            PlayerWeaponsList = new List<PlayerWeapon>();
            foreach (var weapon in levelData.PlayerWeapons)
            {
                PlayerWeaponsList.Add(weapon);
            }
        }
        
        if (levelData != null)
        {
            EnemyWeaponsList = new List<EnemyWeapon>();
            foreach (var weapon in levelData.EnemyWeapons)
            {
                EnemyWeaponsList.Add(weapon);
            }
        }
        
        _playerWeapons = new Dictionary<WeaponData.WeaponType, PlayerWeapon>();
        _playerWeaponFreePool = new Dictionary<WeaponData.WeaponType, Queue<PlayerWeapon>>();

        _enemyWeapons = new Dictionary<EnemyWeaponData.WeaponType, EnemyWeapon>();
        _enemyWeaponFreePool = new Dictionary<EnemyWeaponData.WeaponType, Queue<EnemyWeapon>>();
        
        foreach (PlayerWeapon weapon in PlayerWeaponsList)
        {
            if (_playerWeapons.ContainsKey(weapon.weaponData.GetWeaponType()) == false)
            {
                _playerWeapons.Add(weapon.weaponData.GetWeaponType(), weapon);
                
                Queue<PlayerWeapon> newQue = new Queue<PlayerWeapon>();

                for (int j = 0; j < CopiesPerWeapon; j++)
                {
                    newQue.Enqueue(CreatePlayerWeapon(weapon.weaponData));
                }
                _playerWeaponFreePool.Add(weapon.weaponData.GetWeaponType(), newQue);
            }
        }
        
        PlayerWeaponsList.Clear();

        foreach (EnemyWeapon eWeapon in EnemyWeaponsList)
        {
            if (_enemyWeapons.ContainsKey(eWeapon.enemyWeaponData.GetWeaponType()) == false)
            {
                _enemyWeapons.Add(eWeapon.enemyWeaponData.GetWeaponType(), eWeapon);
                
                Queue<EnemyWeapon> newQue = new Queue<EnemyWeapon>();

                for (int j = 0; j < CopiesPerWeapon; j++)
                {
                    newQue.Enqueue(CreateEnemyWeapon(eWeapon.enemyWeaponData));
                }
                _enemyWeaponFreePool.Add(eWeapon.enemyWeaponData.GetWeaponType(), newQue);
            }
        }
        
        EnemyWeaponsList.Clear();
    }

    private PlayerWeapon CreatePlayerWeapon(WeaponData data)
    {
        var parent = data.GetParent() == WeaponData.Parent.Player ? player : transform;
        var newObject = Instantiate(_playerWeapons[data.GetWeaponType()], parent);
        
        newObject.gameObject.SetActive(false);

        return newObject;
    }
    
    private EnemyWeapon CreateEnemyWeapon(EnemyWeaponData data)
    {
        var parent = transform; // TODO support for ENEMY transform;
        var newObject = Instantiate(_enemyWeapons[data.GetWeaponType()], parent);
        
        newObject.gameObject.SetActive(false);

        return newObject;
    }
    
    public PlayerWeapon GetPlayerWeapon(WeaponData data)
    {
        if (_playerWeaponFreePool[data.GetWeaponType()].Count > 0)
        {
            return _playerWeaponFreePool[data.GetWeaponType()].Dequeue();
        }
        else
        {
            return CreatePlayerWeapon(data);
        }
    }
    
    public EnemyWeapon GetEnemyWeapon(EnemyWeaponData data)
    {
        if (_enemyWeaponFreePool[data.GetWeaponType()].Count > 0)
        {
            return _enemyWeaponFreePool[data.GetWeaponType()].Dequeue();
        }
        else
        {
            return CreateEnemyWeapon(data);
        }
    }
    
    public void ReturnToPool(PlayerWeapon weapon, WeaponData.WeaponType type)
    {
        weapon.gameObject.SetActive(false);
        _playerWeaponFreePool[type].Enqueue(weapon);
    }
    
    public void ReturnToPool(EnemyWeapon weapon, EnemyWeaponData.WeaponType type)
    {
        weapon.gameObject.SetActive(false);
        _enemyWeaponFreePool[type].Enqueue(weapon);
    }
    

}
