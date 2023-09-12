using UnityEngine;

public class SkinUpdater : MonoBehaviour
{
    public GameObject Skin;
    [SerializeField] private Material DefaultMaterial;
    [SerializeField] private Material MobileWebGLMaterial;
    

    private Texture2D _currentSkin;
    private static readonly int Default_SHADER_PROPERTY = Shader.PropertyToID("_Albedo");
    private static readonly int MobileWebGL_SHADER_PROPERY = Shader.PropertyToID("_BaseMap");

    private void Start()
    {
        var renderers = Skin.GetComponentsInChildren<MeshRenderer>();

        foreach (var renderer in renderers)
        {
            renderer.material = PerformanceOptimizer.instance.ActiveGraphicPresset.PlayerMaterial;
        }
    }

    public void ChangeTexture(Texture2D texture2D)
    {
        _currentSkin = texture2D;
        SetTexture(texture2D);
    }

    public void TryOnSkin(Texture2D texture2D)
    {
        SetTexture(texture2D);
    }

    public void ResetSkin()
    {
        SetTexture(_currentSkin);
    }

    public void HideSkin()
    {
        Skin.SetActive(false);
    }

    public void ShowSkin()
    {
        Skin.SetActive(true);
    }

    private void SetTexture(Texture2D texture2D)
    {
        var material = PerformanceOptimizer.instance.ActiveGraphicPresset.PlayerMaterial;
        if (material.Equals(MobileWebGLMaterial))
        {
            material.SetTexture(MobileWebGL_SHADER_PROPERY, texture2D);
        }
        else if(material.Equals(DefaultMaterial))
        {
            material.SetTexture(Default_SHADER_PROPERTY, texture2D);
        }
    }
}
