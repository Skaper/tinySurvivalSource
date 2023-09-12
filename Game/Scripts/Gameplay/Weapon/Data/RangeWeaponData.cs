using UnityEngine;

[CreateAssetMenu(fileName = "Range Weapon Data", menuName = "Scriptable Object/Range Weapon Data", order = int.MinValue)]
public class RangeWeaponData : EnemyWeaponData
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] EnemyWeaponData hitEffect;
    [SerializeField] EnemyWeaponData missEffect;
    [SerializeField] int attackPower;
    [SerializeField] float speed;
    [SerializeField] float cooldown;
    [SerializeField] float range;
    [SerializeField] float lifetime;
    [SerializeField] Sprite bulletSprite;

    
    public GameObject GetBulletPrefab => bulletPrefab;
    public EnemyWeaponData GetHitEffect => hitEffect;
    public EnemyWeaponData GetMissEffect => missEffect;
    public int GetAttackPower => attackPower;
    public float GetSpeed => speed;
    public float GetCooldown => cooldown;
    public float GetRange => range;
    public float GetLifetime => lifetime;
    
    public Sprite GetBulletSprite => bulletSprite;

    
}
