using Assets.SimpleLocalization;
using FMODUnity;
using MyBox;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Scriptable Object/Weapon Data", order = int.MinValue)]
public class WeaponData : ScriptableObject
{
    public enum WeaponType
    {
        Whip,
        Axe,
        Beholder,
        Lightning,
        Bow,
        FireWand,
        Spikes,
        FireShield
    }

    public WeaponSpawner Spawner;

    public enum Parent
    {
        Self,
        Player
    }

    [SerializeField] private string weaponNameKey;
    [SerializeField] Parent parent;
    [SerializeField] WeaponType weaponType;
    [SerializeField] int attackPower;
    [SerializeField] float attackSpeed;
    [SerializeField] Sprite weaponSprite;
    [SerializeField] string descriptionKey;
    [SerializeField] string description;
    [SerializeField] Vector2 basePosition;
    [SerializeField] Vector2 baseScale;
    
    [Header("Sound")]
    [SerializeField] private EventReference WeaponSound;
    
    [Header("Animation")]
    [SerializeField] private bool isAnimated;
    [ConditionalField(nameof(isAnimated))] [SerializeField] private bool useAnimationTime;
    [ConditionalField(nameof(useAnimationTime), true)] [SerializeField] float inactiveDelay;
    
    public bool IsAnimated => isAnimated;
    public bool UseAnimationTime => useAnimationTime;

    public string GetName()
    {
        return LocalizationManager.Localize(weaponNameKey);;
    }

    public EventReference GetWeaponSoundRef()
    {
        return WeaponSound;
    }
    
    public Parent GetParent()
    {
        return parent;
    }

    public WeaponType GetWeaponType()
    {
        return weaponType;
    }

    public int GetAttackPower()
    {
        return attackPower;
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }

    public float GetInactiveDelay()
    {
        return inactiveDelay;
    }

    public Sprite GetSprite()
    {
        return weaponSprite;
    }

    public string GetDescription()
    {
        return LocalizationManager.Localize(descriptionKey);
    }

    public Vector2 GetBasePosition()
    {
        return basePosition;
    }

    public Vector2 GetBaseScale()
    {
        return baseScale;
    }
}
