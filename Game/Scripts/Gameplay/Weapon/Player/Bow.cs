using UnityEngine;

public class Bow : PlayerWeapon
{
    Vector2 destination;
    [SerializeField]Rigidbody2D rigidbody;
    float speed = 300;

    void OnEnable()
    {
        StartCoroutine(StartDestroy());
        destination = EnemySpawner.GetInstance().GetNearestEnemyPosition();
        var target = destination - (Vector2) transform.position;
        transform.right = (Vector2)transform.position - destination;
        rigidbody.AddForce((target).normalized * speed, ForceMode2D.Force);
    }

    private void OnDisable()
    {
        rigidbody.velocity = Vector2.zero;
        rigidbody.angularVelocity = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            var health = collision.GetComponent<Health>();
            if(health)
                health.ReduceHealth(RandomDamage(attackPower));
            InactiveWeapon();
        }
    }
}