using System.Collections.Generic;
using System.Linq;
using Assets.SimpleLocalization;
using UnityEngine;
[CreateAssetMenu(fileName = "ChestRewardData", menuName = "Scriptable Object/Rewards/Chest", order = int.MinValue)]
public class ChestsRewardData : ScriptableObject
{
    public enum RewardType
    {
        CommonChest,
        RareChest,
        EpicChest,
        LegendaryChest
    }
    
    public List<ChestData> Rewards = new List<ChestData>();

    public ChestData GetChestData(RewardType type)
    {
        foreach (var reward in Rewards.Where(reward => reward.Type == type))
        {
            return reward;
        }

        return null;
    }
    
    public string GetName(ChestData chestData)
    {
        return LocalizationManager.Localize(chestData.LocalizationKey);
    }
}

[System.Serializable]
public class ChestData
{
    public string LocalizationKey;
    public ChestsRewardData.RewardType Type;
    public int Diamonds;
    public int Hours;
    public int Minutes;
}
