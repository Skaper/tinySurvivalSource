using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelUpItem : MonoBehaviour
{
    [SerializeField] private Image WeaponIcon;
    [SerializeField] private TextMeshProUGUI NameText;
    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private Button Button;
    [SerializeField] private GameObject Context;
    [SerializeField] private Image Background;
    [SerializeField] private Color SelectedColor = Color.green;
    [SerializeField] private Color NormalColor = Color.white;

    private UnityAction _clickFunction;
    public void Select()
    {
        Background.color = SelectedColor;
    }
    
    public void Deselect()
    {
        Background.color = NormalColor;
    }
    
    public void Show()
    {
        Context.SetActive(true);
    }

    public void FillData(Sprite sprite, string itemName, string description, string level)
    {
        WeaponIcon.sprite = sprite;
        NameText.text = itemName;
        Description.text = description;
        LevelText.text = level;
        Deselect();
    }

    public void ResetOnClickListener(UnityAction func)
    {
        _clickFunction = func;
        Button.onClick.RemoveAllListeners();
        Button.onClick.AddListener(func);
    }

    public void Click()
    {
        _clickFunction.Invoke();
    }
}
