using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SystemTools
{
    private static BuildSettings _buildSettings;
    public static BuildSettings GetBuildSettings()
    {
        if (_buildSettings == null)
        {
            _buildSettings = Resources.Load("Settings/BuildSettings") as BuildSettings;
        }

        return _buildSettings;
    }
}
