using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSceneLoader : MonoBehaviour
{
    public static LevelSceneLoader instance;

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
    
    public void LoadGameLevel(string levelName, LevelData levelData)
    {
        var levelLoaderObj = new GameObject("Level Loader");
        var levelLoader = levelLoaderObj.AddComponent<LevelLoader>();
        levelLoader.Initialization(levelData);
        if (levelData == null)
        {
            Debug.Log("LevelData is null for level \""+levelName+"\"");
        }
        else
        {
            GameProgress.GetData().totalGameSessions += 1;
            GameProgress.Save();
            SceneManager.LoadScene(levelName);
        }
    }

}
