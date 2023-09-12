using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "BuildSettings", menuName = "Game")]
public class BuildSettings : ScriptableObject
{
    public enum Platforms
    {
        Yandex,
        Android,
        Vk,
        PC,
    }

    public Platforms TargetPlatform;
    public bool IngamePurchases = true;
    public bool IsAdOn;
    public string[] ScenesPaths;

}
