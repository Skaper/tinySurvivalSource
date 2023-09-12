using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeItemToolTip : MonoBehaviour
{
    public GameObject Context;
    public TextMeshProUGUI FirstValueText;
    public TextMeshProUGUI FirstValue;
    public TextMeshProUGUI SecondValueText;
    public TextMeshProUGUI SecondValue;

    public UnityEvent OnHide;
    public UnityEvent OnShow;
    
    [SerializeField]private string _firstField;
    [SerializeField]private string _firstValue;
    [SerializeField]private string _secondValue;
    [SerializeField]private string _secondField;
    private void Start()
    {
        Hide();
    }

    public void UpdateFields(string firstField, string firstValue, string secondField, string secondValue)
    {
        _firstField = firstField;
        _firstValue = firstValue;
        _secondField = secondField;
        _secondValue = secondValue;

        UpdateUI();
    }

    private void UpdateUI()
    {
        FirstValueText.text = _firstField;
        FirstValue.text = "+"+_firstValue;

        SecondValueText.text = _secondField;
        SecondValue.text = "+"+_secondValue;
    }
    public void Hide()
    {
        OnHide?.Invoke();
        Context.SetActive(false);
    }

    public void Show()
    {
        OnShow?.Invoke();
        Context.SetActive(true);
        UpdateUI();
    }
}
