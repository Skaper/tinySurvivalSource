using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeItemObserver : MonoBehaviour
{
    public UpgradeItemData ItemData;
    public TextMeshProUGUI ProgressLabel;
    public Button ButtonDiamond;
    public TextMeshProUGUI PriceDiamondLabel;
    public Button ButtonYan;
    public TextMeshProUGUI PriceYanLabel;
    public Image Icon;
    public UpgradeItemToolTip ToolTip;
    public GameObject LoadingIcon;
    
    [HideInInspector]public float PurchaseWaitTime = 0.0f;
    private WaitForSeconds waitPurchase;
    private int _currentStageLevel;
    private string _currentStageName;
    private string[] _stagesNames;
    private int _currentStageNameIndex;
    private UpgradeStageInfo _currentStageInfo;
    private bool canUseButtons;
    private void Start()
    {
        LoadingIcon.SetActive(false);
        waitPurchase = new(PurchaseWaitTime);
        _currentStageLevel = GameProgress.GetData().GetItemLevel(ItemData.type);
        _currentStageName = GameProgress.GetData().GetItemStage(ItemData.type);

        _currentStageInfo = ItemData.GetStageByName(_currentStageName);
        _stagesNames = ItemData.GetAllStagesNames();
        _currentStageNameIndex = Array.IndexOf(_stagesNames, _currentStageName);
        
        var isMaxLevel = CheckForMaxLevel();
        if (isMaxLevel == false)
        {
            ButtonDiamond.onClick.AddListener(OnButtonDiamondClicked);
        }

        if (SystemTools.GetBuildSettings().IngamePurchases)
        {
            ButtonYan.onClick.AddListener(OnButtonYanClicked);
        }
        GameProgress.GetData().DiamondsUpdated += OnDiamondsUpdated;
        OnDiamondsUpdated(GameProgress.GetData().diamonds);
        canUseButtons = true;
        UpdateFields();
    }

    private void OnDiamondsUpdated(int diamonds)
    {
        if (diamonds < _currentStageInfo.priceDiamond)
        {
            ButtonDiamond.interactable = false;
        }
        else
        {
            ButtonDiamond.interactable = true;
        }
    }
    
    

    private void UpdateFields()
    {
        Icon.sprite = _currentStageInfo.icon;
        PriceDiamondLabel.text = _currentStageInfo.priceDiamond.ToString();
        
        var priceYan = _currentStageInfo.priceYan;
        if (priceYan != 0 && SystemTools.GetBuildSettings().IngamePurchases)
        {
            ButtonYan.gameObject.SetActive(true);
            PriceYanLabel.text = _currentStageInfo.priceYan.ToString();
        }
        else
        {
            ButtonYan.gameObject.SetActive(false);
        }

        ProgressLabel.text = _currentStageLevel + "/" + _currentStageInfo.levelsOnStage;

        var fields = _currentStageInfo.UpgrededFields;
        
        ToolTip.UpdateFields(fields[0].GetLocalization(), fields[0].value.ToString(),
            fields[1].GetLocalization(), fields[1].value.ToString());
    }

    private void OnButtonDiamondClicked()
    {
        BuyWithDiamonds();
    }
    
    private void OnButtonYanClicked()
    {
        TryBuy();
    }
    
    private void TryBuy()
    {
        if (canUseButtons == false)
        {
            return;
        }
        canUseButtons = false;

        ShopAcceptWindow.instance.Show(ItemData.GetName(), _currentStageInfo.priceDiamond, _currentStageInfo.priceYan);
        ShopAcceptWindow.instance.UserMadeAction += OnUserMadeAction;
    }
    
    private void OnUserMadeAction(AcceptWindowResult result)
    {
        ShopAcceptWindow.instance.UserMadeAction -= OnUserMadeAction;
        
        if (result.isPurchased == false)
        {
            canUseButtons = true;
            return;
        }

        if (result.isDonation == false)
        {
            BuyWithDiamonds();
        }
        else
        {
            BuyWithYan();
        }

        canUseButtons = true;
    }
    
    private void BuyWithDiamonds()
    {
        if (GameProgress.GetData().diamonds >= _currentStageInfo.priceDiamond)
        {
            StartCoroutine(WaitPurchaseDelay());
            DoUpgrade();
        }
    }

    private void BuyWithYan()
    {
        StartCoroutine(WaitPurchaseDelay());
    }

    private IEnumerator WaitPurchaseDelay()
    {
        var diamondButtonIsActive = ButtonDiamond.isActiveAndEnabled;
        var yanButtonIsActive = ButtonYan.isActiveAndEnabled;
        
        if(diamondButtonIsActive)
            ButtonDiamond.gameObject.SetActive(false);
        if(yanButtonIsActive)
            ButtonYan.gameObject.SetActive(false);
        
        LoadingIcon.SetActive(true);
        
        if(diamondButtonIsActive)
            ButtonDiamond.gameObject.SetActive(true);
        if(yanButtonIsActive)
            ButtonYan.gameObject.SetActive(true);
        
        LoadingIcon.SetActive(false);
        yield return null;
    }
    
    private void DoUpgrade()
    {
        if (CheckForMaxLevel())
        {
            return;
        }
        
        _currentStageLevel += 1;
        
        if (_currentStageLevel > _currentStageInfo.levelsOnStage ||  _currentStageNameIndex == 0)
        {
            if (_currentStageNameIndex < _stagesNames.Length - 1)
            {
                _currentStageNameIndex += 1;
                _currentStageLevel = 0;
            }
            else
            {
                _currentStageLevel = _currentStageInfo.levelsOnStage;
                _currentStageNameIndex = _stagesNames.Length - 1;
            }

            _currentStageName = _stagesNames[_currentStageNameIndex];
            _currentStageInfo = ItemData.GetStageByName(_currentStageName);
        }

        UpdateFields();
        CheckForMaxLevel();
        SaveData();
    }
    
    private bool CheckForMaxLevel()
    {
        var isMaxLevel = _currentStageNameIndex == _stagesNames.Length - 1
                         && _currentStageLevel == _currentStageInfo.levelsOnStage;

        if (isMaxLevel)
        {
            ButtonYan.gameObject.SetActive(false);
            ButtonDiamond.gameObject.SetActive(false);
        }
        
        return isMaxLevel;
    }

    private void SaveData()
    {
        var data = GameProgress.GetData();
        data.SetItemLevel(ItemData.type, _currentStageLevel);
        data.SetItemStage(ItemData.type, _currentStageName);
        data.IncreaseStatValue(_currentStageInfo.UpgrededFields[0].name, _currentStageInfo.UpgrededFields[0].value);
        data.IncreaseStatValue(_currentStageInfo.UpgrededFields[1].name, _currentStageInfo.UpgrededFields[1].value);
        data.SubtractDiamonds(_currentStageInfo.priceDiamond);
        GameProgress.Save();
    }
}
