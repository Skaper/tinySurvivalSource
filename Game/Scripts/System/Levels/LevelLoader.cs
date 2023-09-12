using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private LevelData LevelData;
    
    [SerializeField] private InGameUI InGameUI;

    private GameObject _levelObjects;
    
    [Header("Debug")]
    [SerializeField] private bool DebugMode;
    [SerializeField] private bool Debug_DisableSpawnEnemies;
    private void Awake()
    {
        if (DebugMode)
        {
            StartLoading();
            Time.timeScale = 1f;
        }
    }

    public void Initialization(LevelData levelData)
    {
        LevelData = levelData;
        Destroy(_levelObjects);
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
    }

    private void SceneManagerOnsceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        Debug.Log(scene.name);
        StartLoading();
        SceneManager.sceneLoaded -= SceneManagerOnsceneLoaded;
    }

    public void StartLoading()
    {
        _levelObjects = new GameObject("Level Objects");
        
        Instantiate(LevelData.World, _levelObjects.transform);

        bool isDeviceMobile = DeviceInfo.IsMobile();
        if (isDeviceMobile)
        {
            
        }
        else
        {
            
        }

        var playerGameObject = Instantiate(LevelData.Player, _levelObjects.transform);
        
        var weaponPool = _levelObjects.AddComponent<WeaponPool>();
        weaponPool.Initialize(LevelData, playerGameObject.transform);

        var enemyPool = _levelObjects.AddComponent<EnemyPool>();
        enemyPool.Initialize(LevelData, playerGameObject.transform);

        var itemPool = _levelObjects.AddComponent<ItemsPool>();
        itemPool.Initialize(LevelData);
        
        var particleItemsPool = _levelObjects.AddComponent<ParticleItemsPool>();
        particleItemsPool.Initialize(LevelData);
        
        var enemySpawner = Instantiate(LevelData.EnemySpawner, _levelObjects.transform).GetComponent<EnemySpawner>();
        if (DebugMode)
        {
            enemySpawner.Initialize(playerGameObject, LevelData, !Debug_DisableSpawnEnemies);
        }
        else
        {
            enemySpawner.Initialize(playerGameObject, LevelData);
        }
        
        
        var player = playerGameObject.GetComponent<Player>();
        player.Initialize(LevelData, weaponPool, enemySpawner);

        if (InGameUI == null)
        {
            var UI = Instantiate(LevelData.InGameUI, _levelObjects.transform).GetComponent<InGameUI>();
            UI.Initialize(player, enemySpawner);
        }
        else
        {
            InGameUI.Initialize(player, enemySpawner);
        }
        
        Destroy(gameObject);

    }
    
    
}
