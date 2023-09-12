using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private Image CoverImage;
    [SerializeField] private GameObject PriceYan;
    [SerializeField] private GameObject PriceDiamond;
    [SerializeField] private ToggleButton ToggleButtonElement;
    
    [SerializeField] private GameObject UseIcon;
    [SerializeField] private TextMeshProUGUI NameLabel;
    [SerializeField] private TextMeshProUGUI PriceYanLabel;
    [SerializeField] private TextMeshProUGUI PriceDiamondLabel;
    [HideInInspector] public ItemInfo info;

    public UnityEvent OnUseClick;

    private IShopItemsObserver _observer;
    public void Init(IShopItemsObserver observer, ItemInfo itemInfo, CustomToggleGroup toggleGroup, bool isBought=false)
    {
        info = itemInfo;
        _observer = observer;
        
        CoverImage.sprite = itemInfo.icon;
        NameLabel.text = itemInfo.GetName(GameSystem.SystemLanguage.Get());

        ToggleButtonElement.SetToggleGroup(toggleGroup);
        ToggleButtonElement.OnClicked.RemoveAllListeners();
        ToggleButtonElement.OnToggleOn.RemoveAllListeners();
        ToggleButtonElement.OnToggleOff.RemoveAllListeners();

        if (isBought)
        {
            UseIcon.SetActive(true);
            PriceDiamond.SetActive(false);
            PriceYan.SetActive(false);
            ToggleButtonElement.SetType(ToggleButton.ElementType.Toggle);
            ToggleButtonElement.OnToggleOn.AddListener(delegate { observer.OnUsePressed(this);});
        }
        else
        {
            UseIcon.SetActive(false);
            
            if (itemInfo.priceYan != 0 && SystemTools.GetBuildSettings().IngamePurchases)
            {
                PriceYanLabel.text = itemInfo.priceYan.ToString();
                
            }
            else
            {
                PriceYan.gameObject.SetActive(false);
            }
            PriceDiamondLabel.text = itemInfo.priceDiamond.ToString();
            
            ToggleButtonElement.OnClicked.AddListener(delegate { observer.OnPurchasePressed(this); });
        }
        
        ToggleButtonElement.OnToggleOn.AddListener(() => OnUseClick?.Invoke());
    }

    public void SuccessfullyPurchased()
    {
        PriceDiamond.gameObject.SetActive(false);
        PriceYan.gameObject.SetActive(false);
        
        UseIcon.SetActive(true);
        
        ToggleButtonElement.SetType(ToggleButton.ElementType.Toggle);
        ToggleButtonElement.OnToggleOn.RemoveAllListeners();
        ToggleButtonElement.OnToggleOn.AddListener(delegate { _observer.OnUsePressed(this);});
        ToggleButtonElement.OnToggleOn.AddListener((() => OnUseClick?.Invoke()));
    }
}
