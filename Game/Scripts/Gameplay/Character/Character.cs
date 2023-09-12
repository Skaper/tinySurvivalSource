using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Health))]
public abstract class Character : MonoBehaviour
{
    [SerializeField] protected CharacterData characterData;
    protected Sprite sprite;
    protected RuntimeAnimatorController controller;
    public Animator animator;
    protected int attackPower;
    protected int defencePower;
    protected int speed;
    protected int level = 0;
    internal Coroutine hitCoroutine;
    internal SpriteRenderer spriteRenderer;

    [SerializeField] protected Health characterHealth;

    protected virtual void Initialize()
    {
        if (characterHealth == null)
        {
            characterHealth = GetComponent<Health>();
        }
        
        characterHealth.ReceivedDamage += OnReceivedDamage;
        spriteRenderer = GetComponent<SpriteRenderer>();
        GetComponent<SpriteRenderer>().sprite = GetSprite();
        
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        
        hitCoroutine = null;
    }

    protected virtual void ShowFloatingDamage(float damage)
    {
        var floatingDamage = ParticleItemsPool.instance.GetItem(ParticleItem.ParticleItemType.DamageText).GetComponent<FloatingDamage>();
        floatingDamage.gameObject.SetActive(true);
        var roundedDamage = (int)damage;
        if (roundedDamage < 1)
            roundedDamage = 1;

        floatingDamage.Show(roundedDamage.ToString(), transform.position);
    }

    public void SetActive(bool state)
    {
        gameObject.SetActive(state);
    }
    
    protected void InitHealthPoint()
    {
        characterHealth.ResetHealth();
    }

    public CharacterData.CharacterType Type()
    {
        return characterData.GetCharacterType(level);
    }

    public float GetHealthPoint()
    {
        return characterHealth.CurrentHealth;
    }

    public int GetAttackPower()
    {
        return attackPower;
    }

    public int GetDefencePower()
    {
        return defencePower;
    }

    public int GetSpeed()
    {
        return speed;
    }

    public RuntimeAnimatorController GetController()
    {
        return controller;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Animator GetAnimator()
    {
        return animator;
    }

    protected virtual void OnReceivedDamage(bool isDead, float damage, bool isKnockBack=true)
    {
        if (isDead)
        {
            Die();
        }
    }
    
    public void IncreaseAttackPower(int value)
    {
        attackPower += value;
    }

    public void IncreaseDefencePower(int value)
    {
        defencePower += value;
    }

    public void IncreaseSpeed(int value)
    {
        speed += speed * value / 100;
    }

    public CharacterData.CharacterType GetCharacterType()
    {
        return characterData.GetCharacterType(level);
    }

    public abstract void Die();

    protected abstract IEnumerator DieAnimation();

    protected abstract IEnumerator UnderAttack();
}