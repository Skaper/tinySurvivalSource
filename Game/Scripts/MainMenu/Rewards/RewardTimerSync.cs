using System;
using UnityEngine;

public class RewardTimerSync : MonoBehaviour
{
    public ChestReward[] Chests;
    private bool isTimeUpdated;
    
    private void Start()
    {
        
        ServerTime.instance.DateUpdatedEvent += UpdateTime;
        ServerTime.instance.RequestDate();

        
    }

    private void OnDestroy()
    {
        ServerTime.instance.DateUpdatedEvent -= UpdateTime;
    }

    private void UpdateTime(DateTime dateTime)
    {
        var collectedTimes = GameProgress.GetData().chestsRewardCollectedTime;
        
        foreach (var chest in Chests)
        {
            chest.SetServerTime(dateTime);
        }
        isTimeUpdated = true;
    }
}
