using System.Collections.Generic;
using Assets.SimpleLocalization;
using UnityEngine;
[CreateAssetMenu(fileName = "Base Player Stats Data", menuName = "Scriptable Object/Shop/Base Player Stats Data", order = int.MinValue )]
public class BasePlayerStatsData : ScriptableObject
{
    public StatField[] StatFields;

    private Dictionary<StatField.Names, StatField> fieldsDic;

    public StatField GetFieldByName(StatField.Names fieldName)
    {
        if (fieldsDic == null) CreateDictionary();

        return fieldsDic?[fieldName];
    }

    public void SetFieldByName(StatField.Names fieldName, float value)
    {
        if (fieldsDic == null) CreateDictionary();

        if (fieldsDic != null) fieldsDic[fieldName].value = value;
    }


    private void CreateDictionary()
    {
        fieldsDic = new Dictionary<StatField.Names, StatField>();
        
        foreach (var field in StatFields)
        {
            fieldsDic.Add(field.name, field);
        }
    }
}

[System.Serializable]
public class StatField
{
    public Names name;
    public string localizationKey;

    public float value;

    public string GetLocalization()
    {
        return LocalizationManager.Localize(localizationKey);
    }

    public enum Names
    {
        Health,
        Armor,
        Attack,
        Revival,
        MoveSpeed,
        Luck,
        Duration
    }
}