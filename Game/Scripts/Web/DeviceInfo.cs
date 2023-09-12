using YG;

public static class DeviceInfo
{
    public static bool IsMobile()
    {
        return SystemTools.GetBuildSettings().TargetPlatform switch
        {
            BuildSettings.Platforms.Yandex => YandexGame.EnvironmentData.isMobile,
            BuildSettings.Platforms.Android => true,
            BuildSettings.Platforms.Vk => false,
            BuildSettings.Platforms.PC => false,
            _ => false
        };
    }
    
    public static bool IsTablet()
    {
        return SystemTools.GetBuildSettings().TargetPlatform switch
        {
            BuildSettings.Platforms.Yandex => YandexGame.EnvironmentData.isTablet,
            BuildSettings.Platforms.Android => true,
            BuildSettings.Platforms.Vk => false,
            BuildSettings.Platforms.PC => false,
            _ => false
        };
    }
    
    public static bool IsMobileBrowser()
    {
        return SystemTools.GetBuildSettings().TargetPlatform switch
        {
            BuildSettings.Platforms.Yandex => YandexGame.EnvironmentData.isDesktop == false,
            BuildSettings.Platforms.Android => false,
            BuildSettings.Platforms.Vk => false,
            BuildSettings.Platforms.PC => false,
            _ => false
        };
    }
    
    public static string DeviceType()
    {
        return SystemTools.GetBuildSettings().TargetPlatform switch
        {
            BuildSettings.Platforms.Yandex => YandexGame.EnvironmentData.deviceType,
            BuildSettings.Platforms.Android => "",
            BuildSettings.Platforms.Vk => "",
            BuildSettings.Platforms.PC => "",
            _ => ""
        };
    }
}
