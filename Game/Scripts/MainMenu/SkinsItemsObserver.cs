using UnityEngine;

public class SkinsItemsObserver : MonoBehaviour, IShopItemsObserver
{
    public ShopSkinItemData SkinsDataObject;
    public GameObject ItemPrefab;
    public Transform Context;
    public SkinUpdater PlayerSkin;
    public SkinUpdater DummySkin;
    public CustomToggleGroup ToggleGroup;
    private bool canUseButtons;
    private ShopItem activeItem;
    private void Start()
    {
        DummySkin.HideSkin();
        foreach (ItemSkinInfo skinData in SkinsDataObject.GetItems())
        {
            var saved_id = GameProgress.GetData().GetPurchasedSkins(skinData.index);
            var isBought = saved_id != null && saved_id.Equals(skinData.id);
            var item = Instantiate(ItemPrefab, Context);
            var shopItem = item.GetComponent<ShopItem>();
            shopItem.Init(this, skinData, ToggleGroup, isBought);
            
            if (skinData.id.Equals(GameProgress.GetData().currentSkinId))
            {
                PlayerSkin.ChangeTexture(skinData.image);
                var toggle = shopItem.gameObject.GetComponent<ToggleButton>();
                toggle.ToggleSetState(true, true);
            }
        }

        canUseButtons = true;
    }

    public void OnPurchasePressed(ShopItem item)
    {
        PurchaseProcess(item);
    }

    private void PurchaseProcess(ShopItem item)
    {
        if (canUseButtons == false)
        {
            return;
        }
        canUseButtons = false;
        
        activeItem = item;
        var info = (ItemSkinInfo)item.info;
        
        
        DummySkin.ShowSkin();
        DummySkin.TryOnSkin(info.image);
        
        
        ShopAcceptWindow.instance.Show(item.info.GetName(), info.priceDiamond, info.priceYan);
        ShopAcceptWindow.instance.UserMadeAction += OnUserMadeAction;
    }

    public void OnUsePressed(ShopItem item)
    {
        var info = (ItemSkinInfo)item.info;
        PlayerSkin.ChangeTexture(info.image);
        GameProgress.GetData().currentSkinId = item.info.id;
        GameProgress.Save();
    }

    private void OnUserMadeAction(AcceptWindowResult result)
    {
        ShopAcceptWindow.instance.UserMadeAction -= OnUserMadeAction;
        
        DummySkin.HideSkin();
        PlayerSkin.ResetSkin();
        
        if (activeItem == null || result.isPurchased == false)
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
        activeItem = null;
    }

    private void BuyWithDiamonds()
    {
        if (GameProgress.GetData().diamonds >= activeItem.info.priceDiamond)
        {
            activeItem.SuccessfullyPurchased();
            GameProgress.GetData().SetPurchasedSkins(activeItem.info.index, activeItem.info.id);
            GameProgress.GetData().SubtractDiamonds(activeItem.info.priceDiamond);
            
            GameProgress.Save();
        }
    }
    
    private void BuyWithYan()
    {
        //TODO: MAKE PAYMENTS
    }

    
}
