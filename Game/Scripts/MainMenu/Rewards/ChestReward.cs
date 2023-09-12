using System;
using System.Collections;
using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestReward : MonoBehaviour
{
    [SerializeField] private GameObject LockFrame;
    [SerializeField] private Button LockFrameButton;
    public ChestsRewardData ChestsData;
    public ChestsRewardData.RewardType RewardType;
    public Button UseButton;
    public GameObject RewardDiamonds;
    public GameObject Timer;
    [Header("Text")] 
    public TextMeshProUGUI RewardAmount;
    public TextMeshProUGUI NameLabel;
    public TextMeshProUGUI Hourse;
    public GameObject HourseSpliter;
    public TextMeshProUGUI Minutes;
    public GameObject MinutesSpliter;
    public TextMeshProUGUI Seconds;
    [Header("Tooltip")] 
    public GameObject Tooltip;
    public TextMeshProUGUI TooltipTargetLabel;

    private ChestRewaedProgressionUnit _progressionUnit;
    private ChestData _chestData;
    private bool isLocked;
    private DateTime sceneStartServerTime;

    private bool isServerTimeSetted;
    private double totalSecondForCollect;
    private double leftSecondForCollect;
    private float elapsedSeconds = 0;

    private WaitForSeconds wait1sec = new WaitForSeconds(1f);
    private WaitForSeconds waitTooltip = new WaitForSeconds(3f);
    
    private void Awake()
    {
        UseButton.interactable = false;
        _chestData = ChestsData.GetChestData(RewardType);
        NameLabel.text = ChestsData.GetName(_chestData);

        if (_chestData.Hours == 0)
        {
            Hourse.gameObject.SetActive(false);
            HourseSpliter.SetActive(false);
        }
        UseButton.onClick.AddListener(OnClickCollect);

        RewardAmount.text = _chestData.Diamonds.ToString();
        RewardDiamonds.SetActive(false);
        Timer.SetActive(false);

        _progressionUnit = GetComponent<ChestRewaedProgressionUnit>();
        Tooltip.SetActive(false);
        if (_progressionUnit)
        {
            int totalGameSessions = GameProgress.GetData().totalGameSessions;
            if (totalGameSessions < _progressionUnit.SessionsTarget)
            {
                TooltipTargetLabel.text = GameProgress.GetData().totalGameSessions + "/" + _progressionUnit.SessionsTarget;
                LockFrameButton.onClick.AddListener((() => StartCoroutine(ShowTooltip())));
            }
            
        }
    }

    private void Update()
    {
        if(isLocked || isServerTimeSetted == false)
            return;
        
        leftSecondForCollect -= Time.unscaledDeltaTime;
        
        
    }

    private IEnumerator ShowTooltip()
    {
        Tooltip.SetActive(true);
        yield return waitTooltip;
        Tooltip.SetActive(false);
    }

    private IEnumerator UpdateUITimer()
    {
        while (true)
        {
            var leftSecond = (int) leftSecondForCollect;
            if (leftSecond >= 0)
            {
                int hours = leftSecond / 3600;
                int minutes = (leftSecond % 3600) / 60;
                int seconds = leftSecond % 60;

                Hourse.text = hours > 9 ? hours.ToString() : "0"+hours;
                Minutes.text = minutes > 9 ? minutes.ToString() : "0"+minutes;
                Seconds.text = seconds > 9 ? seconds.ToString() : "0"+seconds;
            }
            else
            {
                ReadyToCollect();
            }

            yield return wait1sec;
        }
    }

    private void OnClickCollect()
    {
        //TODO VERIFY TIME WITH SERVER
        
        ServerTime.instance.DateUpdatedEvent += delegate(DateTime time)
        {
            var lastTimeString = GameProgress.GetData().chestsRewardCollectedTime[GetRewardIndexInSave()];
            var secondsDiff = double.PositiveInfinity;
            if (lastTimeString.IsNullOrEmpty() == false)
            {
                var lastTime = GetLastCollectedTime();
                secondsDiff = (time - lastTime).TotalSeconds;
            }
            
            if (secondsDiff >= totalSecondForCollect)
            {
                RewardWindow.instance.Show(RewardType);
                var data = GameProgress.GetData();
                data.chestsRewardCollectedTime[GetRewardIndexInSave()] = ServerTime.instance.ConvertToString(time);
                data.AddDiamonds(_chestData.Diamonds);
                GameProgress.Save();
                RewardDiamonds.SetActive(false);
                Timer.SetActive(true);
            }
            UpdateLeftTimer();
        };
        ServerTime.instance.RequestDate();
        UseButton.interactable = false;
    }

    private void UpdateLeftTimer()
    {
        totalSecondForCollect = _chestData.Hours * 3600 + _chestData.Minutes * 60;
        leftSecondForCollect = totalSecondForCollect;
    }
    
    public void SetServerTime(DateTime dateTime)
    {
        if(isLocked)
            return;
        
        UpdateLeftTimer();

        //Check if reward could be collected on start    
        var lastTimeString = GameProgress.GetData().chestsRewardCollectedTime[GetRewardIndexInSave()];
        
        //It's first time when player can collect reward
        if (lastTimeString.IsNullOrEmpty())
        {
            ReadyToCollect();
            
        }
        else
        {
            var lastTime = GetLastCollectedTime();
            var secondsDiff = (dateTime - lastTime).TotalSeconds;
        
            if (secondsDiff >= totalSecondForCollect)
            {
                ReadyToCollect();
            }
            else
            {
                Timer.SetActive(true);
                leftSecondForCollect = totalSecondForCollect - secondsDiff;
                UseButton.interactable = false;
            }
        }

        isServerTimeSetted = true;
        StartCoroutine(UpdateUITimer());
    }

    private void ReadyToCollect()
    {
        UseButton.interactable = true;
        RewardDiamonds.SetActive(true);
        Timer.SetActive(false);
    }

    public void Unlock()
    {
        LockFrame.SetActive(false);
        isLocked = false;
    }

    public void Lock()
    {
        LockFrame.SetActive(true);
        UseButton.interactable = false;
        isLocked = true;
    }

    private DateTime GetLastCollectedTime()
    {
        var collectedTimes = GameProgress.GetData().chestsRewardCollectedTime;
        return ServerTime.instance.ConvertToDateTime(collectedTimes[GetRewardIndexInSave()]);
    }
    
    private int GetRewardIndexInSave()
    {
        return RewardType switch
        {
            ChestsRewardData.RewardType.CommonChest => 0,
            ChestsRewardData.RewardType.RareChest => 1,
            ChestsRewardData.RewardType.EpicChest => 2,
            ChestsRewardData.RewardType.LegendaryChest => 3,
            _ => throw new ArgumentOutOfRangeException(nameof(RewardType), RewardType, null)
        };
    }

}
