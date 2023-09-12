using Assets.SimpleLocalization;
using UnityEngine;

[CreateAssetMenu(fileName = "Accessory Data", menuName = "Scriptable Object/Accessory Data", order = int.MinValue)]
public class AccessoryData : ScriptableObject
{
    public Accessory AccessoryController;
    [SerializeField] private string accessoryNameKey;
    [SerializeField] AccessoryType accessoryType;
    [SerializeField] Sprite accessorySprite;
    [SerializeField] string descriptionKey;
    [SerializeField] string description;

    public enum AccessoryType
    {
        Spinach,
        Armor,
        EmptyTome,
        Wings,
        Clover,
        Crown
    }

    public string GetName()
    {
        return LocalizationManager.Localize(accessoryNameKey);;
    }

    public AccessoryType GetAccessoryType()
    {
        return accessoryType;
    }

    public Sprite GetSprite()
    {
        return accessorySprite;
    }

    public string GetDescription()
    {
        return LocalizationManager.Localize(descriptionKey);
    }
}
