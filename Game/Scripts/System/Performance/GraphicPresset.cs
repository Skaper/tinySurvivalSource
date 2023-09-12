using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "GraphicPresset", menuName = "Scriptable Object/Graphics/GraphicPresset", order = int.MinValue)]
public class GraphicPresset : ScriptableObject
{
    [Header("Render")]
    public GraphicSettings.GraphicsLevels Level;
    [SerializeField] private RenderPipelineAsset  renderPipeline;
    [Header("Physics")]
    [SerializeField] private float fixedDeltaTime;
    [Header("Dynamic Resolution")]
    [SerializeField] float maxDPI = 0.95f; // Не рекомендую её ставить на более чем 1
    [SerializeField] float minDPI = 0.55f;
    [SerializeField] float dampen = 0.02f; // На какое кол-во мы повышаем\уменьшаем разрешение
    // Пока фпс между ними - разрешение меняться не будет.
    [SerializeField] float maxFps = 75;
    [SerializeField] float minFps = 55;
    [SerializeField] float renderScale = 1f;
    [SerializeField] float refreshResolutionTime = 1f;
    [Header("Ad settings")]
    [SerializeField] float disableStickyAdFpsLimit = 30;
    [SerializeField] float enableStickyAdFpsLimit = 40;
    [Header("Music settings")]
    [SerializeField] float disableBackgroundMusicFpsLimit = 35;
    [SerializeField] float enableBackgroundMusicFpsLimit = 35;



    [Header("Materials")] 
    [SerializeField] private Material playerMaterial;

    public Material PlayerMaterial => playerMaterial;
    public float EnableStickyAdFpsLimit => enableStickyAdFpsLimit;

    public float EnableBackgroundMusicFpsLimit => enableBackgroundMusicFpsLimit;
    public float MaxDPI => maxDPI;
    
    public float DisableStickyAdFpsLimit => disableStickyAdFpsLimit;

    public float DisableBackgroundMusicFpsLimit => disableBackgroundMusicFpsLimit;
    public float MinDPI => minDPI;

    public float Dampen => dampen;

    public float MaxFps => maxFps;

    public float MinFps => minFps;

    public float RenderScale => renderScale;

    public float RefreshResolutionTime => refreshResolutionTime;
    public RenderPipelineAsset RenderPipeline => renderPipeline;
    public float FixedDeltaTime => fixedDeltaTime;
}
