using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelsItemsObserver : MonoBehaviour, IShopItemsObserver
{
    public ShopLevelItemData LevelsDataObject;
    public GameObject ItemPrefab;
    public Transform Context;
    public CustomToggleGroup ToggleGroup;
    private bool canUseButtons;
    private ShopItem activeItem;

    private Dictionary<ShopItem, LevelData> itemsLevelData = new Dictionary<ShopItem, LevelData>();
    private void Start()
    {
        foreach (var levelInfo in LevelsDataObject.GetItems())
        {
            if(levelInfo.isActive == false)
                continue;
            var saved_id = GameProgress.GetData().GetPurchasedLevel(levelInfo.index);
            var isBought = saved_id != null && saved_id.Equals(levelInfo.id);
            var item = Instantiate(ItemPrefab, Context);
            var shopItem = item.GetComponent<ShopItem>();
            shopItem.Init(this, levelInfo, ToggleGroup, isBought);
            
            itemsLevelData.Add(shopItem, ((ItemLevelInfo)levelInfo).LevelData);
        }
        itemsLevelData.Keys.First().gameObject.GetComponent<ToggleButton>().ToggleSetState(true, true);
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
        var info = (ItemLevelInfo)item.info;

        ShopAcceptWindow.instance.Show(item.info.GetName(), info.priceDiamond, info.priceYan);
        ShopAcceptWindow.instance.UserMadeAction += OnUserMadeAction;
    }

    public void OnUsePressed(ShopItem item)
    {
        /*
        var levelData = itemsLevelData[item];
        var info = (ItemLevelInfo) item.info;
        LevelSceneLoader.instance.LoadGameLevel(info.levelName, levelData);
        */
    }

    public void LoadLevel()
    {
        var toggle = ToggleGroup.GetSelectedToggle();
        var item = toggle.gameObject.GetComponent<ShopItem>();
        var info = (ItemLevelInfo) item.info;
        var levelData = itemsLevelData[item];
        LevelSceneLoader.instance.LoadGameLevel(info.levelName, levelData);
    }
    
    private void OnUserMadeAction(AcceptWindowResult result)
    {
        ShopAcceptWindow.instance.UserMadeAction -= OnUserMadeAction;
        
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
            //GameProgress.GetData().purchasedLevels[activeItem.info.index] = activeItem.info.id;
            GameProgress.GetData().SetPurchasedLevel(activeItem.info.index, activeItem.info.id);
            GameProgress.GetData().SubtractDiamonds(activeItem.info.priceDiamond);
            
            GameProgress.Save();
        }
    }
    
    private void BuyWithYan()
    {
        //TODO: MAKE PAYMENTS
    }
}
