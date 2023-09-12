using UnityEngine;

public class FireShield : PlayerWeapon
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            var health = collision.GetComponent<Health>();
            if (health)
            {
                var damage = RandomDamage(attackPower) * Time.deltaTime;
                health.ReduceHealth(damage, false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
    }
}
