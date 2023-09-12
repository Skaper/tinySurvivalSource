using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

namespace GameSystem
{
    public static class SystemLanguage
    {
        public static string Get()
        {
            var settings = SystemTools.GetBuildSettings();
            if (settings.TargetPlatform == BuildSettings.Platforms.Yandex)
            {
#if !UNITY_EDITOR
                return YandexGame.EnvironmentData.language;
#else
                return YandexGame.savesData.language;
#endif
            }

            return "en";
        }
    }
}

