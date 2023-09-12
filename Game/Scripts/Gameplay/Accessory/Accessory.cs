using UnityEngine;

public class Accessory : MonoBehaviour
{
    public AccessoryData accessoryData;
    private Player _player;
    Sprite accessorySprite;
    private int level = 1;

    public void Initialize(AccessoryData accessoryData, Player player)
    {
        this.accessoryData = accessoryData;
        _player = player;
        accessorySprite = accessoryData.GetSprite();
    }

    public AccessoryData.AccessoryType GetAccessoryType()
    {
        return accessoryData.GetAccessoryType();
    }

    public Sprite GetSprite()
    {
        return accessorySprite;
    }

    public int GetLevel()
    {
        return level;
    }

    public void IncreaseLevel()
    {
        level++;
        ApplyEffect();
    }

    public void ApplyEffect()
    {
        switch (accessoryData.GetAccessoryType())
        {
            case AccessoryData.AccessoryType.Spinach:
                _player.IncreaseAttackPower(10);
                break;
            case AccessoryData.AccessoryType.Crown:
                _player.IncreaseExpAdditional(10);
                break;
            case AccessoryData.AccessoryType.Clover:
                _player.IncreaseLuck(20);
                break;
            case AccessoryData.AccessoryType.Wings:
                _player.IncreaseSpeed(10);
                break;
            case AccessoryData.AccessoryType.Armor:
                _player.IncreaseDefencePower(10);
                break;
            case AccessoryData.AccessoryType.EmptyTome:
                _player.DecreaseAttackSpeed(8);
                break;
        }
    }
}
