using System;
[Serializable]
public class GameProgressData
{
    public string currentSkinId = "skin_0";
    public string[] purchasedSkins;
    public string[] purchasedLevels;
    public string[] progressionCompleted;
    public int diamonds => _diamonds;
    
    public int _diamonds = 6;

    public string GetPurchasedLevel(int index)
    {
        return purchasedLevels[index];
    }
    
    public void SetPurchasedLevel(int index, string id)
    { 
        purchasedLevels[index] = id;
    }

    public string GetPurchasedSkins(int index)
    {
        return purchasedSkins[index];
    }
    
    public void SetPurchasedSkins(int index, string id)
    {
        purchasedSkins[index] = id;
    }
    public void SubtractDiamonds(int count)
    {
        if (diamonds >= count)
        {
            _diamonds -= count;
        }
        DiamondsUpdated?.Invoke(diamonds);
    }
    public void AddDiamonds(int addCount)
    {
        _diamonds += addCount;
        DiamondsUpdated?.Invoke(diamonds);
    }
    [field: NonSerialized]
    public event Action<int> DiamondsUpdated;
    
    [field: NonSerialized]
    public event Action<StatField.Names, float> StatsUpdated;
    
    public void InitDefault()
    {
        purchasedSkins = new string[200];
        purchasedSkins[0] = "skin_0";
        purchasedLevels = new string[100];
        purchasedLevels[0] = "level_0";
        lastTenSurviveTime = new float[10];
        progressionCompleted = new string[100];
        chestsRewardCollectedTime = new string[4];
    }

    //Upgrades
    public string swordStage = "stage_0";
    public int swordStageLevel;
    public string shieldStage = "stage_0";
    public int shieldStageLevel;
    public string helmetStage = "stage_0";
    public int helmetStageLevel;
    public string chestplateStage = "stage_0";
    public int chestplateStageLevel;
    public string leggingsStage = "stage_0";
    public int leggingsStageLevel;
    public string bootsStage = "stage_0";
    public int bootsStageLevel;
    
    //Extra stats
    public float extraHealth;
    public float extraArmor;
    public float extraAttack;
    public float extraRevival;
    public float extraMoveSpeed;
    public float extraLuck;
    public float extraDuration;

    //Game data
    public int totalGameSessions;
    public int totalDonations;
    public float maxSurviveTime;
    public float[] lastTenSurviveTime;
    
    //Activity data
    public string firstLoginDate;
    public string lastLoginDate;
    public string threeMinutesRewardCollectedDate;
    public string tenMinutesRewardCollectedDate;
    public string hourRewardCollectedDate;
    public string dailyRewardCollectedDate;
    
    //Ad settings
    public int personalAdDelay; //Personal time between ads
    
    //Rewards last Collected Time
    /// <summary>
    /// 0-common
    /// 1-rare
    /// 2-epic
    /// 3-legendary
    /// </summary>
    public string[] chestsRewardCollectedTime;
    
    
    public float GetStatValue(StatField.Names type)
    {
        return type switch
        {
            StatField.Names.Health => extraHealth,
            StatField.Names.Armor => extraArmor,
            StatField.Names.Attack => extraAttack,
            StatField.Names.Revival => extraRevival,
            StatField.Names.MoveSpeed => extraMoveSpeed,
            StatField.Names.Luck => extraLuck,
            StatField.Names.Duration => extraDuration,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
    
    public void IncreaseStatValue(StatField.Names type, float value)
    {
        var finalValue = 0f;
        switch (type)
        {
            case StatField.Names.Health:
                extraHealth += value;
                finalValue = extraHealth;
                break;
            case StatField.Names.Armor:
                extraArmor += value;
                finalValue = extraArmor;
                break;
            case StatField.Names.Attack:
                extraAttack += value;
                finalValue = extraAttack;
                break;
            case StatField.Names.Revival:
                extraRevival += value;
                finalValue = extraRevival;
                break;
            case StatField.Names.MoveSpeed:
                extraMoveSpeed += value;
                finalValue = extraMoveSpeed;
                break;
            case StatField.Names.Luck:
                extraLuck += value;
                finalValue = extraLuck;
                break;
            case StatField.Names.Duration:
                extraDuration += value;
                finalValue = extraDuration;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
        StatsUpdated?.Invoke(type, finalValue);
    }
    
    public int GetItemLevel(UpgradeType type)
    {
        return type switch
        {
            UpgradeType.Helmet => helmetStageLevel,
            UpgradeType.Chestplate => chestplateStageLevel,
            UpgradeType.Leggings => leggingsStageLevel,
            UpgradeType.Boots => bootsStageLevel,
            UpgradeType.Shield => shieldStageLevel,
            UpgradeType.Sword => swordStageLevel,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
    public void SetItemLevel(UpgradeType type, int level)
    {
        switch (type)
        {
            case UpgradeType.Helmet:
                helmetStageLevel = level;
                break;
            case UpgradeType.Chestplate:
                chestplateStageLevel = level;
                break;
            case UpgradeType.Leggings:
                leggingsStageLevel = level;
                break;
            case UpgradeType.Boots:
                bootsStageLevel = level;
                break;
            case UpgradeType.Shield:
                shieldStageLevel = level;
                break;
            case UpgradeType.Sword:
                swordStageLevel = level;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
    public string GetItemStage(UpgradeType type)
    {
        return type switch
        {
            UpgradeType.Helmet => helmetStage,
            UpgradeType.Chestplate => chestplateStage,
            UpgradeType.Leggings => leggingsStage,
            UpgradeType.Boots => bootsStage,
            UpgradeType.Shield => shieldStage,
            UpgradeType.Sword => swordStage,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
    public void SetItemStage(UpgradeType type, string stageName)
    {
        switch (type)
        {
            case UpgradeType.Helmet:
                helmetStage = stageName;
                break;
            case UpgradeType.Chestplate:
                chestplateStage = stageName;
                break;
            case UpgradeType.Leggings:
                leggingsStage = stageName;
                break;
            case UpgradeType.Boots:
                bootsStage = stageName;
                break;
            case UpgradeType.Shield:
                shieldStage = stageName;
                break;
            case UpgradeType.Sword:
                swordStage = stageName;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
    
    
}
