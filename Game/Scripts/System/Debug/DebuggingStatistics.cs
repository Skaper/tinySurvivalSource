using System.Linq;
using System.Text;
using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class DebuggingStatistics : MonoBehaviour
{
    public GameObject Context;
    public GameObject MainMenuScroll;
    public TextMeshProUGUI Label;
    private void Awake()
    {
        Context.SetActive(false);
        
    }

    public void Hide()
    {
        Context.SetActive(false);
        MainMenuScroll.SetActive(true);
    }
    
    public void Show()
    {
        Context.SetActive(true);
        MainMenuScroll.SetActive(false);
        Label.text = CreateStatistics();
    }
    
    public string CreateStatistics()
    {
        StringBuilder text = new StringBuilder();
        text.AppendLine("\t System:");
        text.AppendLine("app version: " + Application.version);
        text.AppendLine("currentRenderPipeline: " + GraphicsSettings.currentRenderPipeline.name);
        text.AppendLine("CurrentRenderScale: " + PerformanceOptimizer.instance.CurrentRenderScale);
        text.AppendLine("fixedDeltaTime: " + Time.fixedDeltaTime);
        text.AppendLine("IsMobileBrowser: " + DeviceInfo.IsMobileBrowser());
        text.AppendLine("DeviceType: " + DeviceInfo.DeviceType());
        text.AppendLine("ActiveGraphicPresset: " + PerformanceOptimizer.instance.ActiveGraphicPresset.Level);
        
        text.AppendLine("\t User info:");
        text.AppendLine("totalGameSessions: " + GameProgress.GetData().totalGameSessions);
        text.AppendLine("firstLoginDate: " + GameProgress.GetData().firstLoginDate);
        var purchasedSkinsCount = GameProgress.GetData().purchasedSkins.Count(level => level.IsNullOrEmpty() == false);
        text.AppendLine("purchasedSkins count: " + purchasedSkinsCount);
        var purchasedLevelsCount = GameProgress.GetData().purchasedLevels.Count(level => level.IsNullOrEmpty() == false);
        text.AppendLine("purchasedLevels count: " + purchasedLevelsCount);

        return text.ToString();
    }
}
