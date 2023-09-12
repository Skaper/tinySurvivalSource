using UnityEngine;

public class SimpleArrow : EnemyWeapon
{
    public LayerMask TargetLayers;
    
    private Vector2 targetPos;
    private float speed;
    private bool bulletInited;
    private Vector3 previusPosition;
    private bool isArrowFlying;

    private RangeWeaponData rangerWeaponData; 

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sortingOrder = 0;
    }


    public override void SetParameters(Vector2 target, Vector2 position, Quaternion rotation)
    {
        base.SetParameters(target, position, rotation);
        rangerWeaponData = (RangeWeaponData) enemyWeaponData;
        
        targetPos = target;
        _waitForLifeTime = new WaitForSeconds(rangerWeaponData.GetLifetime);
        speed = rangerWeaponData.GetSpeed;
        _spriteRenderer.sprite = rangerWeaponData.GetBulletSprite;
        bulletInited = true;
        isArrowFlying = true;
        
        OnSpawn();
    }

    protected override void InactiveWeapon()
    {
        base.InactiveWeapon();
        isArrowFlying = false;
        bulletInited = false;
        WeaponPool.instance.ReturnToPool(this, rangerWeaponData.GetWeaponType());
    }

    private void FixedUpdate()
    {
        if(isActiveAndEnabled && bulletInited && isArrowFlying)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.fixedDeltaTime);

            if (Vector3.SqrMagnitude(transform.position - previusPosition) < 0.0001)
            {
                isArrowFlying = false;
                WeaponPool.instance.ReturnToPool(this, rangerWeaponData.GetWeaponType());
                if (rangerWeaponData.GetMissEffect != null)
                {
                    var missEffect = WeaponPool.instance.GetEnemyWeapon(rangerWeaponData.GetMissEffect);
                    missEffect.SetParameters(Vector2.zero, transform.position, Quaternion.identity);
                    missEffect.gameObject.SetActive(true);
                }
                
                //gameObject.SetActive(false);
            }
            previusPosition = transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger && isArrowFlying && ((TargetLayers & (1 << other.gameObject.layer)) != 0))
        {
            GiveDamage();
            if (rangerWeaponData.GetHitEffect != null) 
            {
                var hitEffect = WeaponPool.instance.GetEnemyWeapon(rangerWeaponData.GetHitEffect);
                hitEffect.SetParameters(Vector2.zero, targetPos, Quaternion.identity);
                hitEffect.gameObject.SetActive(true);
            }
            WeaponPool.instance.ReturnToPool(this, rangerWeaponData.GetWeaponType());
        }
    }

    protected virtual void GiveDamage()
    {
        Player.GetInstance().MakeDamage(rangerWeaponData.GetAttackPower);
    }
}

