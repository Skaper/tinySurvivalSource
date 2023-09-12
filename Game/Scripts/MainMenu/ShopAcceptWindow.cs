using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopAcceptWindow : MonoBehaviour
{
    
    public static ShopAcceptWindow instance;

    public GameObject Context;
    public GameObject Background;
    
    public TextMeshProUGUI ItemNameLabel;
    public TextMeshProUGUI DiamondPriceLabel;
    public Button DiamondButton;
    public TextMeshProUGUI YanPriceLabel;
    public Button YanButton;
    public GameObject NotEnoughDiamonds;
    public SkinUpdater PlayerSkin;
    
    public UnityEvent OnShowEvent;
    public UnityEvent OnDiamonPurchaseEvent;
    public UnityEvent OnYanPurchaseEvent;
    public UnityEvent OnCancelEvent;
    public event Action<AcceptWindowResult> UserMadeAction;
    
    private bool _isActive;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Context.SetActive(false);
        Background.SetActive(false);
        
        
    }

    public void Show(string itemName, int diamondPrice, int yanPrice)
    {
        PlayerSkin.HideSkin();
        ItemNameLabel.text = itemName;

        if (GameProgress.GetData().diamonds >= diamondPrice)
        {
            NotEnoughDiamonds.SetActive(false);
            DiamondButton.gameObject.SetActive(true);
            DiamondPriceLabel.text = diamondPrice.ToString();
            DiamondButton.onClick.AddListener(PurchaseWithDiamonds);
        }
        else
        {
            NotEnoughDiamonds.SetActive(true);
            DiamondButton.gameObject.SetActive(false);
        }
        

        if (yanPrice != 0 && SystemTools.GetBuildSettings().IngamePurchases)
        {
            YanButton.gameObject.SetActive(true);
            YanPriceLabel.text = yanPrice.ToString();
            YanButton.onClick.AddListener(PurchaseWithYan);
        }
        else
        {
            YanButton.gameObject.SetActive(false);
        }
        
        Context.SetActive(true);
        Background.SetActive(true);
        OnShowEvent?.Invoke();
    }

    private void PurchaseWithDiamonds()
    {
        HideWindow();
        
        UserMadeAction?.Invoke(new AcceptWindowResult(true, false));
        OnDiamonPurchaseEvent?.Invoke();
    }
    
    private void PurchaseWithYan()
    {
        HideWindow();
        
        UserMadeAction?.Invoke(new AcceptWindowResult(true, true));
        OnYanPurchaseEvent?.Invoke();
    }
    
    public void Close()
    {
        HideWindow();
        
        UserMadeAction?.Invoke(new AcceptWindowResult(false));
        OnCancelEvent?.Invoke();
    }
    
    private void HideWindow()
    {
        PlayerSkin.ShowSkin();
        DiamondButton.onClick.RemoveAllListeners();
        YanButton.onClick.RemoveAllListeners();
        
        Context.SetActive(false);
        Background.SetActive(false);
    }
}

public class AcceptWindowResult
{
    public bool isPurchased;
    public bool isDonation;

    public AcceptWindowResult(bool isPurchased, bool isDonation=false)
    {
        this.isPurchased = isPurchased;
        this.isDonation = isDonation;
    }
}
