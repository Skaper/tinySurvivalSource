using UnityEngine;

[CreateAssetMenu(fileName = "Damage Zone Data", menuName = "Scriptable Object/Damage Zone Data", order = int.MinValue)]
public class DamageZoneData : EnemyWeaponData
{
    [SerializeField] GameObject prefab;
    [SerializeField] int damagePerContact;
    [SerializeField] float cooldown;
    [SerializeField] float lifetime = 1f;
    [SerializeField] Sprite sprite;
    
    public GameObject GetPrefab => prefab;

    public int GetDamagePerContact => damagePerContact;

    public float GetCooldown => cooldown;
    public float GetLifetime => lifetime;

    public Sprite GetSprite => sprite;

    
}
