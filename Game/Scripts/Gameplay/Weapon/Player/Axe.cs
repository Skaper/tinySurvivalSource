using UnityEngine;

public class Axe : PlayerWeapon
{
    [SerializeField] private WeaponSprite[] LevelsSprites;
    [SerializeField] private Rigidbody2D rigidbody;
    const float speed = 10f;
    int hitCount;

    void OnEnable()
    {
        StartCoroutine(StartDestroy());
        hitCount = 0;
        Throw();
    }

    protected override void OnSpawn()
    {
        base.OnSpawn();
        if (level > LevelsSprites.Length-1)
        {
            _spriteRenderer.sprite = LevelsSprites[LevelsSprites.Length - 1].Image;
        }
        else
        {
            foreach (var levelSprite in LevelsSprites)
            {
                if (level == levelSprite.Level)
                    _spriteRenderer.sprite = levelSprite.Image;
            }
        }
        
    }

    void Throw()
    {
        switch (GetDirection())
        {
            case WeaponSpawner.Direction.Self:
                if (PlayerMove.GetInstance().GetLookingLeft())
                    rigidbody.AddForce(new Vector2(-0.3f, 1) * speed, ForceMode2D.Impulse);
                else
                    rigidbody.AddForce(new Vector2(0.3f, 1) * speed, ForceMode2D.Impulse);
                break;
            case WeaponSpawner.Direction.Opposite:
                if (PlayerMove.GetInstance().GetLookingLeft())
                    rigidbody.AddForce(new Vector2(0.3f, 1) * speed, ForceMode2D.Impulse);
                else
                    rigidbody.AddForce(new Vector2(-0.3f, 1) * speed, ForceMode2D.Impulse);
                break;
            case WeaponSpawner.Direction.Left:
                rigidbody.AddForce(new Vector2(-0.3f, 1) * speed, ForceMode2D.Impulse);
                break;
            case WeaponSpawner.Direction.Right:
                rigidbody.AddForce(new Vector2(0.3f, 1) * speed, ForceMode2D.Impulse);
                break;
        }

        rigidbody.AddTorque(speed / 2, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            var hp = collision.GetComponent<Health>();
            if(hp)
                hp.ReduceHealth(RandomDamage(attackPower));
            if (hitCount >= 1)
                InactiveWeapon();
            hitCount++;
        }
    }
}