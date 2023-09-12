using System;
using TMPro;
using UnityEngine;

public class PlayerStatsPresenter : MonoBehaviour
{
    [SerializeField]
    private BasePlayerStatsData PlayerBaseData;
    public GameObject Context;
    [Header("Labels")] 
    [SerializeField] private TextMeshProUGUI Health;
    [SerializeField] private TextMeshProUGUI Armor;
    [SerializeField] private TextMeshProUGUI Attack;
    [SerializeField] private TextMeshProUGUI Revival;
    [SerializeField] private TextMeshProUGUI MoveSpeed;
    [SerializeField] private TextMeshProUGUI Luck;
    [SerializeField] private TextMeshProUGUI Duration;

    private const string COLOR_TAG = "<color=#AAFF00>+";
    
    void Start()
    {
        GameProgress.GetData().StatsUpdated += UpdateField;

        UpdateAllFields();
        
        Hide();
    }

    public void Hide()
    {
        Context.SetActive(false);
    }

    public void Show()
    {
        Context.SetActive(true);
        UpdateAllFields();
    }

    private void UpdateAllFields()
    {
        foreach (StatField.Names fieldType in Enum.GetValues(typeof(StatField.Names)))
        {
            UpdateField(fieldType, GameProgress.GetData().GetStatValue(fieldType));
        }
    }
    private void UpdateField(StatField.Names type, float value)
    {
        var baseValue = PlayerBaseData.GetFieldByName(type).value;
        switch (type)
        {
            case StatField.Names.Health:
                Health.text = (int)baseValue + COLOR_TAG + (int)value;
                break;
            case StatField.Names.Armor:
                Armor.text = (int)baseValue + COLOR_TAG + (int)value;
                break;
            case StatField.Names.Attack:
                Attack.text = (int)baseValue + COLOR_TAG + (int)value;
                break;
            case StatField.Names.Revival:
                Revival.text = (int)baseValue+ COLOR_TAG + (int)value;
                break;
            case StatField.Names.MoveSpeed:
                MoveSpeed.text = Math.Round(baseValue,2) + COLOR_TAG + Math.Round(value, 2);
                break;
            case StatField.Names.Luck:
                Luck.text = Math.Round(baseValue,2) + "%" + COLOR_TAG + Math.Round(value, 2) + "%";
                break;
            case StatField.Names.Duration:
                Duration.text = Math.Round(baseValue,2) + "%" + COLOR_TAG + Math.Round(value, 2) + "%";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}
