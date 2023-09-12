using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private List<EnemyPoolInfo> EnemyList;

    private Dictionary<CharacterData.CharacterType, Enemy> _enemies;
    private Dictionary<CharacterData.CharacterType, Queue<Enemy>> _freePool;
    private Transform playerTransform;
    
    public void Initialize(LevelData levelData, Transform playerTransform)
    {
        this.playerTransform = playerTransform;
        if (levelData != null)
        {
            EnemyList = new List<EnemyPoolInfo>();
            foreach (var stage in levelData.Stages)
            {
                foreach (var boss in stage.Bosses)
                {
                    var enemyPoolInfo = new EnemyPoolInfo(boss.Unit, boss.CountInPool);
                    EnemyList.Add(enemyPoolInfo);
                }
                foreach (var enemy in stage.Enemies)
                {
                    var enemyPoolInfo = new EnemyPoolInfo(enemy.Unit, enemy.CountInPool);
                    EnemyList.Add(enemyPoolInfo);
                }
            }
        }
        
        _enemies = new Dictionary<CharacterData.CharacterType, Enemy>();
        _freePool = new Dictionary<CharacterData.CharacterType, Queue<Enemy>>();
        
        foreach (var enemy in EnemyList)
        {
            enemy.EnemeUnit.gameObject.SetActive(false);
            if (_enemies.ContainsKey(enemy.EnemeUnit.GetCharacterType()) == false)
            {
                _enemies.Add(enemy.EnemeUnit.GetCharacterType(), enemy.EnemeUnit);
                
                Queue<Enemy> newQue = new Queue<Enemy>();

                for (int j = 0; j < enemy.EnemyCount; j++)
                {
                    newQue.Enqueue(CreateEnemy(enemy.EnemeUnit.GetCharacterType()));
                }
                _freePool.Add(enemy.EnemeUnit.GetCharacterType(), newQue);
            }
        }
        
        EnemyList.Clear();

    }

    public static EnemyPool instance;

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


    private Enemy CreateEnemy(CharacterData.CharacterType type)
    {
        var newObject = Instantiate(_enemies[type], instance.transform);
        newObject.GetComponent<EnemyMove>().player = playerTransform;
        newObject.SetActive(false);

        return newObject;
    }
    
    public Enemy GetEnemy(CharacterData.CharacterType type)
    {
        if (_freePool[type].Count > 0)
        {
            return _freePool[type].Dequeue();
        }
        else
        {
            return CreateEnemy(type);
        }
    }

    public void ReturnToPool(Enemy enemy, CharacterData.CharacterType type)
    {
        enemy.SetActive(false);
        _freePool[type].Enqueue(enemy);
    }
}

[System.Serializable]
public class EnemyPoolInfo
{
    public Enemy EnemeUnit;
    public int EnemyCount = 50;

    public EnemyPoolInfo(Enemy unit, int count)
    {
        EnemeUnit = unit;
        EnemyCount = count;
    }
}
