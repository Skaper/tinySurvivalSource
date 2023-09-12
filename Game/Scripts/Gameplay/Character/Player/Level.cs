using System;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Action<int, int> ExpChanged;
    public Action<int> LevelChanged;
    [SerializeField] private bool CanLevelUp = true;
    
    int maxExpValue;
    int curExpValue;
    static int level;
    static bool isLevelUpTime;
    
    void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        maxExpValue = 50;
        curExpValue = 0;
        level = 1;
        isLevelUpTime = false;
    }

    public int GetMaxExp()
    {
        return maxExpValue;
    }

    public bool GetIsLevelUpTime()
    {
        return isLevelUpTime;
    }

    public void SetIsLevelUpTime(bool value)
    {
        isLevelUpTime = value;
    }

    public void GetExp(int value)
    {
        if (curExpValue + value >= maxExpValue)
        {
            curExpValue += value - maxExpValue;
            LevelUp();
        }
        else
            curExpValue += value;

        ExpChanged?.Invoke(curExpValue, maxExpValue);
    }

    void LevelUp()
    {
        if(CanLevelUp == false)
            return;
        isLevelUpTime = true;

        level+=1;
        LevelChanged?.Invoke(level);

        maxExpValue = 50 * level;
        ExpChanged?.Invoke(curExpValue, maxExpValue);
    }
}
