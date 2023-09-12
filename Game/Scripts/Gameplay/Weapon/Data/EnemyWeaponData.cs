using UnityEngine;

public class EnemyWeaponData : ScriptableObject
{ 
    public enum WeaponType
    {
        Arrow,
        Fireball,
        FireballBlue,
        DamageZoneFireRed,
        DamageZoneFireBlue,
        GreenCreeperExplosion
    }
    
    public enum Parent
    {
        Self,
        Enemy
    }
    
    [SerializeField] Parent parent;
    [SerializeField] WeaponType weaponType;

    public WeaponType GetWeaponType()
    {
        return weaponType;
    }
    
    public Parent GetParent() => parent;
}
