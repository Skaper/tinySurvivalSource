using UnityEngine;

public class CreeperAttack : EnemyAttack
{
    public EnemyWeaponData WeaponData;
    private static readonly int ExplosionAnimName = Animator.StringToHash("IsBoom");

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 3)
        {
            if (gameObject.activeSelf)
            {
                character.GetAnimator().SetBool(ExplosionAnimName, true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 3)
        {
            character.GetAnimator().SetBool(ExplosionAnimName, false);
        }
    }

    protected override void GiveDamage()
    {
        Player.GetInstance().MakeDamage(character.GetAttackPower());
    }

    public void Explosion()
    {
        var explosion = WeaponPool.instance.GetEnemyWeapon(WeaponData);
        explosion.SetParameters(Vector2.zero, transform.position, Quaternion.identity);
        explosion.gameObject.SetActive(true);
        character.Die();
    }
}
