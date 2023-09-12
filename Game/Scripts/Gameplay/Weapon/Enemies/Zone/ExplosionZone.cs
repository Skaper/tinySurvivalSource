using UnityEngine;

public class ExplosionZone : EnemyWeapon
{
    public LayerMask TargetLayers;
    public ParticleSystem Effect;
    private DamageZoneData _data;
    private bool isDidDamage;
    protected override void Awake()
    {
        base.Awake();
        _data = (DamageZoneData) enemyWeaponData;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((TargetLayers & (1 << other.gameObject.layer)) != 0)
        {
            if(!isDidDamage)GiveDamage();
        }
    }

    protected override void OnEnable()
    {
        isDidDamage = false;
        Effect.Play();
        _waitForLifeTime = new WaitForSeconds(_data.GetLifetime);
        base.OnEnable();
    }
    
    protected virtual void GiveDamage()
    {
        Player.GetInstance().MakeDamage(_data.GetDamagePerContact);
        isDidDamage = true;
    }
    
}
