using UnityEngine;

public class ShopItemData : ScriptableObject
{
    public virtual ItemInfo[] GetItems()
    {
        return null;
    }
}


[System.Serializable]
public class ItemInfo
{
    public bool isActive = true;
    public int index;
    public string id;
    [SerializeField] 
    private LocalizeText itemName;
    public Sprite icon;
    public int priceDiamond;
    public int priceYan;

    public string GetName()
    {
        return GetName(GameSystem.SystemLanguage.Get());
    }
    
    public string GetName(string lang)
    {
        switch (lang)
        {
            case "ru":
                return itemName.name_ru;
            case "en":
                return itemName.name_en;
            case "tr":
                return itemName.name_tr;
            default:
                return itemName.name_en;
        }
    }
}

[System.Serializable]
public struct LocalizeText
{
    public string name_ru;
    public string name_en;
    public string name_tr;
}

