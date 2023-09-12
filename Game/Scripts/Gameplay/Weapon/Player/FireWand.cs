using System.Collections;
using UnityEngine;

public class FireWand : PlayerWeapon
{
    private static readonly int Explosion = Animator.StringToHash("Explosion");
    private Rigidbody2D _rigidbody2D;
    private WaitForSeconds wait01 = new WaitForSeconds(0.1f);
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            var health = collision.GetComponent<Health>();
            if (health)
            {
                StartCoroutine(PendingExplosion(health));
            }
        }
    }

    private IEnumerator PendingExplosion(Health health)
    {
        yield return wait01;
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.angularVelocity = 0f;
        _animator.SetTrigger(Explosion);
        health.ReduceHealth(RandomDamage(attackPower));
    }

    //Animation call
    public void Explode()
    {
        InactiveWeapon();
    }
}