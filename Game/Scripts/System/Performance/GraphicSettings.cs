using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GraphicSettings", menuName = "Scriptable Object/Graphics/GraphicSettings", order = int.MinValue)]
public class GraphicSettings : ScriptableObject
{
    public GraphicPresset[] GraphicPressets;
    public enum GraphicsLevels
    {
        MobileWebGL,
        Low,
        Normal,
        Best
    }

    public GraphicPresset Get(GraphicsLevels level)
    {
        foreach (var presset in GraphicPressets)
        {
            if (level == presset.Level)
                return presset;
        }

        return GraphicPressets[0];
    }
}
