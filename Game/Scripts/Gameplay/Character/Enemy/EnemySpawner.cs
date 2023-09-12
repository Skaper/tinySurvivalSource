using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public bool Debug_DrawSpawnZone;
    public Vector2 spawnZoneScreenOffset = new Vector2(0.5f, 0.5f);
    public Vector2 spawnZoneSize = new Vector2(12, 10);
    [Tooltip("If distance from player to enemy more than value, enemy will teleport to nearest player position")]
    public float maxDistanceToEnemy = 20f;
    
    private static EnemySpawner instance;
    private List<Enemy> enemyList = new List<Enemy>(500);
    private const float DecreseSpawnDelayTime = 0.8f;
    private float spawnDelay;
    private int stage;
    private int killCount;
    private int stageNamber;
    private float stageTimer;

    private bool BossSpawnedOnStage;
    private Camera MainCamera;
    private Transform playerTransform;
    private LevelData LevelData;
    private float maxDistanceToEnemySqr;
    private WaitForSeconds wait05 = new(0.5f);
    private WaitForSeconds waitSpawnDelay;
    private Vector2 CameraSize;
    private float lastNearestDistScanTime;
    private Vector2 lastNearestDist = Vector2.down;

    public event Action<int> KillCountChangedEvent; 

    enum Direction
    {
        North,
        South,
        West,
        East
    }
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
    }

    public void Initialize(GameObject player, LevelData levelData, bool isSpawnEnable=true)
    {
        
        playerTransform = player.transform;
        LevelData = levelData;
        
        stage = 0;
        killCount = 0;

        stageNamber = 0;
        
        maxDistanceToEnemySqr = maxDistanceToEnemy * maxDistanceToEnemy;
        MainCamera = Camera.main;
        spawnDelay = levelData.Stages[0].SpawnDelay;
        waitSpawnDelay = new(spawnDelay);
        if(isSpawnEnable)
            StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            foreach (StageEnemy enemy in LevelData.Stages[stageNamber].Enemies)
            {
                if (Random.Range(0f, 1f) <= enemy.Percent)
                {
                    var newEnemy = EnemyPool.instance.GetEnemy(enemy.Unit.GetCharacterType());
                    newEnemy.SetSpawnData(enemy.Level, RandomPosition());
                    newEnemy.SetActive(true);
                    enemyList.Add(newEnemy);
                }
            }
            
            if (BossSpawnedOnStage == false && LevelData.Stages[stageNamber].Bosses.Length > 0)
            {
                var id = Random.Range(0, LevelData.Stages[stageNamber].Bosses.Length);
                var boss = LevelData.Stages[stageNamber].Bosses[id];
                var newBoss = EnemyPool.instance.GetEnemy(boss.Unit.GetCharacterType());
                newBoss.SetSpawnData(boss.Level, RandomPosition());
                newBoss.SetActive(true);
                enemyList.Add(newBoss);
                BossSpawnedOnStage = true;
            }

            yield return waitSpawnDelay;

            if (stageTimer >= LevelData.Stages[stageNamber].Duration && stageNamber + 1 < LevelData.Stages.Length)
            {
                stageTimer = 0f;
                stageNamber++;
                BossSpawnedOnStage = false;
                spawnDelay = LevelData.Stages[stageNamber].SpawnDelay;
                waitSpawnDelay = new(spawnDelay);
            }
        }
        
        
    }

    private float t_simple_maxTime = float.MinValue;
    private float t_alloc_maxTime = float.MinValue;
    private void Update()
    {
        stageTimer += Time.deltaTime;
    }

   
    private Vector2 GetRandomPositionNorth(Vector2 rightTop, Vector2 leftBottom)
    {
        var lTop = new Vector2(leftBottom.x - spawnZoneSize.x, rightTop.y + spawnZoneSize.y);
        var north_lBottom = new Vector2(leftBottom.x - spawnZoneScreenOffset.x, rightTop.y + spawnZoneScreenOffset.y);
        
        return RandomVectorInQuad(lTop, rightTop + spawnZoneSize, north_lBottom, rightTop + spawnZoneScreenOffset);
    }
    
    private Vector2 GetRandomPositionSouth(Vector2 rightTop, Vector2 leftBottom)
    {
        var rBottom = new Vector2(rightTop.x + spawnZoneSize.x, leftBottom.y - spawnZoneSize.y);
        var south_rTop = new Vector2(rightTop.x + spawnZoneScreenOffset.x, leftBottom.y - spawnZoneScreenOffset.y);

        return RandomVectorInQuad(leftBottom - spawnZoneSize, rBottom, leftBottom - spawnZoneScreenOffset, south_rTop);
    }
    
    private Vector2 GetRandomPositionEast(Vector2 rightTop, Vector2 leftBottom)
    {
        var rBottom = new Vector2(rightTop.x + spawnZoneSize.x, leftBottom.y - spawnZoneSize.y);
        var east_lBottom = new Vector2(rightTop.x + spawnZoneScreenOffset.x, leftBottom.y - spawnZoneScreenOffset.y);

        return RandomVectorInQuad(rightTop + spawnZoneSize, rBottom, rightTop + spawnZoneScreenOffset, east_lBottom);
    }
    
    private Vector2 GetRandomPositionWest(Vector2 rightTop, Vector2 leftBottom)
    {
        var lTop = new Vector2(leftBottom.x - spawnZoneSize.x, rightTop.y + spawnZoneSize.y);
        var west_rTop = new Vector2(leftBottom.x - spawnZoneScreenOffset.x, rightTop.y + spawnZoneScreenOffset.y);

        return RandomVectorInQuad(lTop, leftBottom - spawnZoneSize, west_rTop, leftBottom - spawnZoneScreenOffset);
    }
    
    /// <summary>
    /// Отрисовка полей спавна, для деббагинга
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (Debug_DrawSpawnZone == false)
        {
            return;
        }
        
        if (MainCamera == null)
        {
            MainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        }

        Vector2 rightTop = MainCamera.ViewportToWorldPoint(new Vector3(1, 1, MainCamera.nearClipPlane));
        Vector2 leftBottom = MainCamera.ViewportToWorldPoint(new Vector3(0, 0, MainCamera.nearClipPlane));
        
        Debug.Log("Draw camera");

        var rTop = new Vector2(rightTop.x + spawnZoneSize.x, rightTop.y + spawnZoneSize.y);
        var rBottom = new Vector2(rightTop.x + spawnZoneSize.x, leftBottom.y - spawnZoneSize.y);
        var lTop = new Vector2(leftBottom.x - spawnZoneSize.x, rightTop.y + spawnZoneSize.y);
        var lBottom = new Vector2(leftBottom.x - spawnZoneSize.x, leftBottom.y - spawnZoneSize.y);
        
        Gizmos.DrawSphere(rTop, 0.5F);
        Gizmos.DrawSphere(rBottom, 0.5F); //right bottom
        Gizmos.DrawSphere(lTop, 0.5F);
        Gizmos.DrawSphere(lBottom, 0.5F); //left top
        
        Gizmos.DrawLine(rTop, rBottom);
        Gizmos.DrawLine(rBottom, lBottom);
        Gizmos.DrawLine(lBottom, lTop);
        Gizmos.DrawLine(lTop, rTop);
        
        
        Gizmos.color = Color.blue;
        //North
        //lTop, rTop
        var north_lBottom = new Vector2(leftBottom.x - spawnZoneScreenOffset.x, rightTop.y + spawnZoneScreenOffset.y);
        var north_rBottom = new Vector2(rightTop.x + spawnZoneScreenOffset.x, rightTop.y + spawnZoneScreenOffset.y);
        Gizmos.DrawLine(lTop, rTop);
        Gizmos.DrawLine(rTop, north_rBottom);
        Gizmos.DrawLine(north_rBottom, north_lBottom);
        Gizmos.DrawLine(north_lBottom, lTop);
        
        //South
        //lBottom, rBottom
        var south_lTop = new Vector2(leftBottom.x - spawnZoneScreenOffset.x, leftBottom.y - spawnZoneScreenOffset.y);
        var south_rTop = new Vector2(rightTop.x + spawnZoneScreenOffset.x, leftBottom.y - spawnZoneScreenOffset.y);
        Gizmos.DrawLine(lBottom, rBottom);
        Gizmos.DrawLine(rBottom, south_rTop);
        Gizmos.DrawLine(south_rTop, south_lTop);
        Gizmos.DrawLine(south_lTop, lBottom);
        
        //East
        //rTop, rBottom
        var east_lTop = new Vector2(rightTop.x + spawnZoneScreenOffset.x, rightTop.y + spawnZoneScreenOffset.y);
        var east_lBottom = new Vector2(rightTop.x + spawnZoneScreenOffset.x, leftBottom.y - spawnZoneScreenOffset.y);
        Gizmos.DrawLine(rTop, rBottom);
        Gizmos.DrawLine(rBottom, east_lBottom);
        Gizmos.DrawLine(east_lBottom, east_lTop);
        Gizmos.DrawLine(east_lTop, rTop);
        
        //west
        //lTop, lBottom
        var west_rTop = new Vector2(leftBottom.x - spawnZoneScreenOffset.x, rightTop.y + spawnZoneScreenOffset.y);
        var west_rBottom = new Vector2(leftBottom.x - spawnZoneScreenOffset.x, leftBottom.y - spawnZoneScreenOffset.y);
        Gizmos.DrawLine(lTop, west_rTop);
        Gizmos.DrawLine(west_rTop, west_rBottom);
        Gizmos.DrawLine(west_rBottom, lBottom);
        Gizmos.DrawLine(lBottom, lTop);
       
    }
    
    private Vector2 RandomVectorInQuad(Vector2 a, Vector2 b, Vector2 c, Vector2 d) {
        var s = Random.Range(0f, 1f);
        Vector2 e = s*a + (1-s)*b;
        Vector2 f = s*c + (1-s)*d;
        float t = Random.Range(0f,1f);
        return t*e + (1-t)*f;
    }
    
    public Vector3 RandomPosition()
    {
        Vector3 pos = new Vector3();
        Direction direction = (Direction)Random.Range(0, 4);
        
        Vector2 rightTop = MainCamera.ViewportToWorldPoint(new Vector3(1, 1, MainCamera.nearClipPlane));
        Vector2 leftBottom = MainCamera.ViewportToWorldPoint(new Vector3(0, 0, MainCamera.nearClipPlane));
        
        switch (direction)
        {
            case Direction.North:
                pos = GetRandomPositionNorth(rightTop, leftBottom);
                break;
            case Direction.South:
                pos = GetRandomPositionSouth(rightTop, leftBottom);
                break;
            case Direction.West:
                pos = GetRandomPositionWest(rightTop, leftBottom);
                break;
            case Direction.East:
                pos = GetRandomPositionEast(rightTop, leftBottom);
                break;
        }

        return pos;
    }

    public Vector2 GetRandomPositionInScreen()
    {
        Vector2 rightTop = MainCamera.ViewportToWorldPoint(new Vector3(1, 1, MainCamera.nearClipPlane));
        Vector2 leftBottom = MainCamera.ViewportToWorldPoint(new Vector3(0, 0, MainCamera.nearClipPlane));

        return new Vector2(Random.Range(leftBottom.x, rightTop.x), Random.Range(leftBottom.y, rightTop.y));
    }

    public Vector2 GetNearestEnemyPosition()
    {
        if (Time.timeSinceLevelLoad - lastNearestDistScanTime < 0.8f)
        {
            return lastNearestDist;
        }
        CameraSize = new Vector2(MainCamera.orthographicSize * MainCamera.aspect * 2f, MainCamera.orthographicSize * 2f);
        Collider2D[] colliders = new Collider2D[16];
        var collidersCount = Physics2D.OverlapBoxNonAlloc(
            playerTransform.position, CameraSize, playerTransform.localPosition.x, colliders, LayerMask.GetMask("Enemy"));
        Collider2D nearestCollider = null;
        float minSqrDistance = Mathf.Infinity;
        for (int i = 0; i < collidersCount; i++)
        {
            float sqrDistanceToCenter = (playerTransform.position - colliders[i].transform.position).sqrMagnitude;
            if (sqrDistanceToCenter < minSqrDistance)
            {
                minSqrDistance = sqrDistanceToCenter;
                nearestCollider = colliders[i];
            }
        }

        lastNearestDistScanTime = Time.timeSinceLevelLoad;
        if (nearestCollider)
            return nearestCollider.transform.position;
        return new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    public Vector2 GetRandomEnemyPosition()
    {
        int random = Random.Range(0, enemyList.Count);

        return enemyList[random].transform.position;
    }

    public void RemoveFromList(Enemy enemy)
    {
        enemyList.Remove(enemy);
    }
    
    public void IncreaseKillCount()
    {
        killCount++;
        KillCountChangedEvent?.Invoke(killCount);
    }

    public static EnemySpawner GetInstance()
    {
        return instance;
    }

    public int GetListCount()
    {
        return enemyList.Count;
    }
}
