using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemIU : MonoBehaviour
{
    [SerializeField] private Image Item;
    [SerializeField] private TextMeshProUGUI Level;
    [SerializeField] private GameObject Context;

    private void Awake()
    {
        Item.enabled = false;
    }

    public void Show()
    {
        Context.SetActive(true);
    }

    public void FillData(Sprite sprite, string lvl)
    {
        Item.enabled = true;
        Item.sprite = sprite;
        Level.text = lvl;
    }
}
