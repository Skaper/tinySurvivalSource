using UnityEngine;

public class PlayerWeapon : Weapon
{
    protected override void InactiveWeapon()
    {
        base.InactiveWeapon();
        WeaponPool.instance.ReturnToPool(this, weaponData.GetWeaponType());
    }
    public virtual void SetParameters(WeaponData weaponData, int attackPower, WeaponSpawner.Direction direction,int level)
    {
        this.level = level;
        this.weaponData = weaponData;
        this.attackPower = attackPower;
        this.inactiveDelay = weaponData.GetInactiveDelay();
        this.direction = direction;
        OnSpawn();
    }
    
    protected override int RandomDamage(int damage)
    {
        int minDamage = (int)(damage * 0.8f);
        int maxDamage = (int)(damage * 1.2f);

        damage = Random.Range(minDamage, maxDamage + 1);

        return damage;
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 6)
        {
            var hp = collision.GetComponent<Health>();
            if (hp)
            {
                hp.ReduceHealth( RandomDamage(attackPower));
            }
        }
    }
}
