public interface IShopItemsObserver
{
    public abstract void OnPurchasePressed(ShopItem item);
    public abstract void OnUsePressed(ShopItem item);
}
