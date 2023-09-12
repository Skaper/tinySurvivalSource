using System;
using UnityEngine;

public class PlayerStatsLoader : MonoBehaviour
{
    [SerializeField]
    private BasePlayerStatsData PlayerBaseData;
    
    [SerializeField]
    public BasePlayerStatsData InGamePlayerStatsData;

    public void Awake()
    {
        GameProgress.GetData().StatsUpdated += UpdateStats;
        InitPlayerStats();
    }

    public void InitPlayerStats()
    {
        foreach (StatField.Names fieldType in Enum.GetValues(typeof(StatField.Names)))
        {
            //var value = PlayerBaseData.GetFieldByName(fieldType).value + GameProgress.GetData().GetStatValue(fieldType);
            var value = GameProgress.GetData().GetStatValue(fieldType);
            InGamePlayerStatsData.SetFieldByName(fieldType, value);
        }
    }


    private void UpdateStats(StatField.Names type, float value)
    {
        //var result = PlayerBaseData.GetFieldByName(type).value + value;
        var result = value;
        InGamePlayerStatsData.SetFieldByName(type, result);
    }
}
