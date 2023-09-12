using System;
using YG;

public static class GameProgress
{
    public static GameProgressData GetData()
    {
        return SystemTools.GetBuildSettings().TargetPlatform switch
        {
            BuildSettings.Platforms.Yandex => YandexGame.savesData.ProgressData,
            BuildSettings.Platforms.Android => new GameProgressData(),
            BuildSettings.Platforms.Vk => new GameProgressData(),
            BuildSettings.Platforms.PC => new GameProgressData(),
            _ => new GameProgressData()
        };
    }

    public static void Save()
    {
        switch (SystemTools.GetBuildSettings().TargetPlatform)
        {
            case BuildSettings.Platforms.Yandex:
                YandexGame.SaveProgress();
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
