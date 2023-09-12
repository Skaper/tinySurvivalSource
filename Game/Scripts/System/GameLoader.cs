using System;
using System.Collections;
using Assets.SimpleLocalization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using YG;

public class GameLoader : MonoBehaviour
{
    public int MainMenuSceneIndex = 1;
    public GameObject YandexGamePrefab;
    public static GameLoader instance;

    private PlatformSettingsAdjaster _renderPipeline;

    public UnityEvent OnLocalizationLoadingComplited;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);
        var settings = SystemTools.GetBuildSettings();
        
        
        
        switch (settings.TargetPlatform)
        {
            case BuildSettings.Platforms.Yandex:
                StartCoroutine(LoadYandexSDK());
                break;
            case BuildSettings.Platforms.Android:
                break;
            case BuildSettings.Platforms.Vk:
                break;
            case BuildSettings.Platforms.PC:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        
        
    }
    
  
    
    void Update()
    {
        
    }

    private void SetupLocalization()
    {
        LocalizationManager.Read();
        var lang = GameSystem.SystemLanguage.Get();
        switch (lang)
        {
            case "ru":
                LocalizationManager.Language = "Russian";
                break;
            case "en":
                LocalizationManager.Language = "English";
                break;
            case "tr":
                LocalizationManager.Language = "Turkish";
                break;
            default:
                LocalizationManager.Language = "English";
                break;
        }
        
        Debug.Log("System Language: " + lang + " Localization: " + LocalizationManager.Language);
    }

    private IEnumerator LoadYandexSDK()
    {
        Instantiate(YandexGamePrefab);
        yield return new WaitWhile(() => YandexGame.SDKEnabled == false);
        Debug.Log("YANDEX SDK LOADED");
        PerformanceOptimizer.instance.SetByDeviceType();
        SetupLocalization();
        OnLocalizationLoadingComplited?.Invoke();
        
        yield return LoadSounds();
        Destroy(gameObject, 0.1f);
        AdProvider.Initialization();
        SceneManager.LoadScene(MainMenuSceneIndex);
    }


    private IEnumerator LoadSounds()
    {
        var masterBankLoader = new FMODLoadMasterBank();
        yield return new WaitUntil(() => masterBankLoader.IsLoaded());
    }
    
}
