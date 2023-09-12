using System;
using UnityEngine;
using UnityEngine.Rendering;

[Obsolete]
public class PlatformSettingsAdjaster : MonoBehaviour
{
    public GraphicSettings Settings;
    public void SetByDeviceType()
    {
        if (DeviceInfo.IsMobileBrowser())
        {
            var mobile = Settings.Get(GraphicSettings.GraphicsLevels.MobileWebGL);
            Time.fixedDeltaTime = mobile.FixedDeltaTime;
            QualitySettings.renderPipeline = mobile.RenderPipeline;
            Debug.Log("Set render pipeline: " + mobile.RenderPipeline.name);
        }
        else
        {
            var best = Settings.Get(GraphicSettings.GraphicsLevels.Best);
            Time.fixedDeltaTime = best.FixedDeltaTime;
            QualitySettings.renderPipeline = best.RenderPipeline;
            
            Debug.Log("Set render pipeline: " + best.RenderPipeline.name);
        }
        Debug.Log("The active render pipeline is defined by " +GraphicsSettings.currentRenderPipeline.name);
    }
}
