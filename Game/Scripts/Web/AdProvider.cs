using System;
using YG;

public static class AdProvider
{
    public static event Action OpenFullAdEvent;
    public static event Action CloseFullAdEvent;

    private static bool _isInitialized;
    private static bool _isAdActive;

    public static bool IsAdActive()
    {
        return _isAdActive;
    }
    
    public static void Initialization()
    {
        switch (SystemTools.GetBuildSettings().TargetPlatform)
        {
            case BuildSettings.Platforms.Yandex:
                if (!_isInitialized)
                {
                    YandexGame.OpenFullAdEvent += OnOpenFullAdEvent;
                    YandexGame.CloseFullAdEvent += OnCloseFullAdEvent;
                }
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

    private static void OnCloseFullAdEvent()
    {
        _isAdActive = false;
        FMODVolumeControl.Instance.UnmuteAllAfterAd();
        CloseFullAdEvent?.Invoke();
    }
    private static void OnOpenFullAdEvent()
    {
        _isAdActive = true;
        FMODVolumeControl.Instance.MuteAllBeforeAd();
        OpenFullAdEvent?.Invoke();
    }
    
    public static void FullscreenShow()
    {
        switch (SystemTools.GetBuildSettings().TargetPlatform)
        {
            case BuildSettings.Platforms.Yandex:
                YandexGame.FullscreenShow();
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

    public static bool IsReadyFullscreenAd()
    {
        var currentSessionNumber = GameProgress.GetData().totalGameSessions;
        //Dont show AD first 3 sessions
        if (currentSessionNumber < 3)
            return false;
        //Increase time between ADs for first 10 sessions
        if (SystemTools.GetBuildSettings().TargetPlatform == BuildSettings.Platforms.Yandex)
        {
            //YandexGame.Instance.AdditionalTimerShowAd = currentSessionNumber < 10 ? 60f : 0f;
        }

        return SystemTools.GetBuildSettings().TargetPlatform switch
        {
            BuildSettings.Platforms.Yandex => YandexGame.Instance.IsReadyToShowFullscreenAd() && SystemTools.GetBuildSettings().IsAdOn,
            BuildSettings.Platforms.Android => false,
            BuildSettings.Platforms.Vk => false,
            BuildSettings.Platforms.PC => false,
            _ => false
        };
    }

    public static void StickyAdActivity(bool state)
    {
        switch (SystemTools.GetBuildSettings().TargetPlatform)
        {
            case BuildSettings.Platforms.Yandex:
                YandexGame.StickyAdActivity(state);
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
}
