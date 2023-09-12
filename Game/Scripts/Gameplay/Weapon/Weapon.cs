using System;
using System.Collections;
using MyBox;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponOwner Owner = WeaponOwner.Player;
    [SerializeField]protected SpriteRenderer _spriteRenderer;
    [SerializeField]protected Animator _animator;
    [ConditionalField(nameof(Owner), false, WeaponOwner.Player)]
    public WeaponData weaponData;
    protected WeaponSpawner.Direction direction;
    protected int attackPower;
    protected int level;
    protected float inactiveDelay;
    protected WaitForSeconds waitInactiveDelay = new(0.1f);

    public event Action<Weapon> WeaponDeactivated;
    

    protected virtual void Awake()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
        
    }

    protected virtual void OnEnable()
    {
        StartCoroutine(StartDestroy());
    }
    
    public virtual void SetParameters(){}

    public void FlipSpriteX(bool flipX)
    {
        _spriteRenderer.flipX = flipX;
    }
    
    public void FlipSpriteY(bool flipY)
    {
        _spriteRenderer.flipY = flipY;
    }

    protected virtual void OnSpawn(){}

    protected WeaponSpawner.Direction GetDirection()
    {
        return direction;
    }

    protected virtual IEnumerator StartDestroy()
    {
        if (weaponData.IsAnimated)
        {
            _animator.Play(0);
            
        }

        if (weaponData.UseAnimationTime)
        {
            yield return new WaitUntil((() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f));
        }
        else
        {
            yield return new WaitForSeconds(inactiveDelay);
        }
        
        InactiveWeapon();
    }

    protected virtual void InactiveWeapon()
    {
        BeforeInactiveWeapon();
        WeaponDeactivated?.Invoke(this);
    }

    public void RemoveAllListeners()
    {
        WeaponDeactivated = null;
    }

    protected virtual void BeforeInactiveWeapon(){}

    protected virtual int RandomDamage(int damage)
    {
        return damage;
    }
}

public enum WeaponOwner
{
    Player,
    Enemy
}

[Serializable]
public class WeaponSprite
{
    public int Level;
    public Sprite Image;
}
