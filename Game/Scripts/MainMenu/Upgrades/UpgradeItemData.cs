using System;
using Assets.SimpleLocalization;
using UnityEngine;
[CreateAssetMenu(fileName = "Upgrade item Data", menuName = "Scriptable Object/Shop/Upgrade item Data", order = int.MinValue )]
public class UpgradeItemData : ScriptableObject
{
    public UpgradeType type;
    public Sprite startIcon;
    [SerializeField] 
    private string LocalizationKey;

    public UpgradeStageInfo[] upgrades;

    public UpgradeStageInfo GetStageByName(string stageName)
    {
        for (var i = 0; i < upgrades.Length; i++)
        {
            if (upgrades[i].name.Equals(stageName))
            {
                return upgrades[i];
            }
        }
        return null;
        
    }

    public string[] GetAllStagesNames()
    {
        var names = new string[upgrades.Length];
        for (var i = 0; i < upgrades.Length; i++)
        {
            names[i] = upgrades[i].name;
        }

        return names;
    }
    
    public string GetName()
    {
        return LocalizationManager.Localize(LocalizationKey);
    }
    
    private void OnValidate()
    {
        foreach (var vUpgrade in upgrades)
        {
            if (vUpgrade.UpgrededFields.Length != vUpgrade.MaxFieldsCount)
            {
                Debug.LogWarning("Don't change the 'UpgrededFields' field's array size!");
                Array.Resize(ref vUpgrade.UpgrededFields, vUpgrade.MaxFieldsCount);
            }
        }
    }
}

public enum UpgradeType
{
    Helmet,
    Chestplate,
    Leggings,
    Boots,
    Shield,
    Sword
}

[System.Serializable]
public class UpgradeStageInfo
{
    public string name;
    public int levelsOnStage;
    public Sprite icon;
    [Header("Price (fixed per stage)")]
    public int priceDiamond;
    public int priceYan;
    
    private const int MAX_FIELDS = 2;
    public StatField[] UpgrededFields = new StatField[MAX_FIELDS];

    public int MaxFieldsCount => MAX_FIELDS;
}