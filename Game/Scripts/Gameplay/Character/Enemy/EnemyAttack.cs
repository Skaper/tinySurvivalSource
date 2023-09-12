using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    protected Enemy character;
    protected Coroutine coroutine;
    private static readonly int AttackAnimName = Animator.StringToHash("Attack");
    private WaitForSeconds wait02 = new(0.2f);
    protected virtual void Awake()
    {
        character = GetComponent<Enemy>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            if (gameObject.activeSelf)
            {
                OnPlayerCollisionEnter();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            OnPlayerCollisionExit();
        }
    }

    protected virtual void OnPlayerCollisionEnter()
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(Attack());
        }
    }
    
    protected virtual void OnPlayerCollisionExit()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    protected virtual IEnumerator Attack()
    {
        while (true)
        {
            GiveDamage();
            character.GetAnimator().SetTrigger(AttackAnimName);
            yield return wait02;
        }
    }

    protected virtual void GiveDamage()
    {
        Player.GetInstance().MakeDamage(character.GetAttackPower());
    }
}