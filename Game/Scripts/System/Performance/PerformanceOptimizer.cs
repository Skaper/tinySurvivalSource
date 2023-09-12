using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PerformanceOptimizer : MonoBehaviour
{
    public static PerformanceOptimizer instance;
    
    public bool EditorOnly;
    public GraphicSettings Settings;
    [SerializeField] private float _updateInterval = 1f;
    [SerializeField] private float _optimizePerformanceInterval = 5f;
    [HideInInspector] public GraphicPresset ActiveGraphicPresset;
    
    private UniversalRenderPipelineAsset urp;
    private float currentRenderScale;
    public float CurrentRenderScale => currentRenderScale;

    /// <summary>
    /// Skip some time at start to skip performance drop on game start
    /// and produce more accurate Avg FPS
    /// </summary>
    private float _idleTime = 5;
    private float _elapsed;
    private int _frames;
    private int _quantity;
    private float _fps;
    private float _averageFps;

    private WaitForSecondsRealtime _waitIdle;
    private WaitForSecondsRealtime _waitOptimizePerformanceInterval;
    private Coroutine CheckForLowFpsCoroutine;
    
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        _waitIdle = new WaitForSecondsRealtime(_idleTime);
        _waitOptimizePerformanceInterval = new WaitForSecondsRealtime(_optimizePerformanceInterval);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (CheckForLowFpsCoroutine == null)
        {
            CheckForLowFpsCoroutine = StartCoroutine(CheckForLowFps());
        }
        else
        {
            StopCoroutine(CheckForLowFpsCoroutine);
            CheckForLowFpsCoroutine = StartCoroutine(CheckForLowFps());
        }
    }
    

    private IEnumerator CheckForLowFps()
    {
        yield return _waitIdle;
        while (true)
        {
            yield return _waitOptimizePerformanceInterval;
            ResolutionUpdate();
            StickyAdUpdate();
            BackgroundMusicUpdate();
        }
    }

    public void SetByDeviceType()
    {
        if (DeviceInfo.IsMobileBrowser())
        {
            var mobile = Settings.Get(GraphicSettings.GraphicsLevels.MobileWebGL);
            Time.fixedDeltaTime = mobile.FixedDeltaTime;
            QualitySettings.renderPipeline = mobile.RenderPipeline;
            Debug.Log("Set render pipeline: " + mobile.RenderPipeline.name);
            ActiveGraphicPresset = mobile;
        }
        else
        {
            var best = Settings.Get(GraphicSettings.GraphicsLevels.Best);
            Time.fixedDeltaTime = best.FixedDeltaTime;
            QualitySettings.renderPipeline = best.RenderPipeline;
            ActiveGraphicPresset = best;
            Debug.Log("Set render pipeline: " + best.RenderPipeline.name);
        }
        Debug.Log("The active render pipeline is defined by " +GraphicsSettings.currentRenderPipeline.name);
        
        urp = (UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline;
        urp.renderScale = ActiveGraphicPresset.RenderScale;
        currentRenderScale = ActiveGraphicPresset.RenderScale;

        //StartCoroutine(CheckForLowFps());
    }
    
    private void Update()
    {
        if (EditorOnly && !Application.isEditor) return;

        if (_idleTime > 0)
        {
            _idleTime -= Time.deltaTime;
            return;
        }
		
        _elapsed += Time.deltaTime;
        ++_frames;
		
        if (_elapsed >= _updateInterval)
        {
            _fps = _frames / _elapsed;
            _elapsed = 0;
            _frames = 0;
        }
		
        _quantity++;
        _averageFps += (_fps - _averageFps) / _quantity;
    }
    
    private void ResolutionUpdate()
    {
        if (ActiveGraphicPresset.MaxFps < _fps)
        {
            ImproveResolution();
        }

        if (ActiveGraphicPresset.MinFps > _fps)
        {
            SubtractResolution();
        }

        currentRenderScale = urp.renderScale;
    }

    private void StickyAdUpdate()
    {
        if (_fps <= ActiveGraphicPresset.DisableStickyAdFpsLimit)
        {
            AdProvider.StickyAdActivity(false);
        }
        else if(_fps >= ActiveGraphicPresset.EnableStickyAdFpsLimit)
        {
            AdProvider.StickyAdActivity(true); 
        }
    }

    public void BackgroundMusicUpdate()
    {
        if (_fps <= ActiveGraphicPresset.DisableBackgroundMusicFpsLimit)
        {
            FMODVolumeControl.Instance.SetBackgroundMusicVolume(0f);
        }
        else if(_fps <= ActiveGraphicPresset.EnableBackgroundMusicFpsLimit)
        {
            FMODVolumeControl.Instance.SetBackgroundMusicVolume(1f);
        }
    }

    private void ImproveResolution()
    {
        if (currentRenderScale < ActiveGraphicPresset.MaxDPI)
        {
            urp.renderScale += ActiveGraphicPresset.Dampen;
        }
    }
    private void SubtractResolution()
    {
        if (currentRenderScale > ActiveGraphicPresset.MinDPI)
        {
            urp.renderScale -= ActiveGraphicPresset.Dampen;
        }
    }
}
