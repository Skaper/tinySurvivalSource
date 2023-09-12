using System.Collections;
using UnityEngine;

public class DamageZone : EnemyWeapon
{
    public LayerMask TargetLayers;
    private DamageZoneData _data;
    
    protected Coroutine coroutine;
    private WaitForSeconds waitCooldown;

    protected override void Awake()
    {
        base.Awake();
        _data = (DamageZoneData) enemyWeaponData;
    }

    protected override void OnEnable()
    {
        
        waitCooldown = new WaitForSeconds(_data.GetCooldown);
        _waitForLifeTime = new WaitForSeconds(_data.GetLifetime);
        base.OnEnable();
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((TargetLayers & (1 << other.gameObject.layer)) != 0)
        {
            if (coroutine == null)
            {
                coroutine = StartCoroutine(Attack());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if ((TargetLayers & (1 << other.gameObject.layer)) != 0)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }
    }

    private void OnDisable()
    {
        if(coroutine != null) StopCoroutine(coroutine);
    }

    protected virtual IEnumerator Attack()
    {
        while (true)
        {
            GiveDamage();
            yield return waitCooldown;
        }
    }

    protected override void InactiveWeapon()
    {
        base.InactiveWeapon();
        WeaponPool.instance.ReturnToPool(this, _data.GetWeaponType());
    }

    protected virtual void GiveDamage()
    {
        Player.GetInstance().MakeDamage(_data.GetDamagePerContact);
    }
}
