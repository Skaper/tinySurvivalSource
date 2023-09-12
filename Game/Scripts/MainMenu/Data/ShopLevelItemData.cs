using UnityEngine;

[CreateAssetMenu(fileName = "Shop Level item Data", menuName = "Scriptable Object/Shop/Shop Level item Data", order = int.MinValue )]
public class ShopLevelItemData : ShopItemData
{
    public ItemLevelInfo[] itemLevelList;
    
    public override ItemInfo[] GetItems()
    {
        return itemLevelList;
    }
}
[System.Serializable]
public class ItemLevelInfo: ItemInfo
{
    public string levelName;
    public LevelData LevelData;
}