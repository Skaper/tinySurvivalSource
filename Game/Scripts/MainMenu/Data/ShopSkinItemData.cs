using UnityEngine;
[CreateAssetMenu(fileName = "Shop Skin item Data", menuName = "Scriptable Object/Shop/Shop Skin item Data", order = int.MinValue )]
public class ShopSkinItemData : ShopItemData
{
    [SerializeField]
    private ItemSkinInfo[] itemSkinList;
    
    public override ItemInfo[] GetItems()
    {
        return itemSkinList;
    }
}
[System.Serializable]
public class ItemSkinInfo: ItemInfo
{
    public Texture2D image;
}