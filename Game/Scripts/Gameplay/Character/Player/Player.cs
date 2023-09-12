using System;
using System.Collections;
using BulletPro;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PlayerMove))]
public class Player : MonoBehaviour//Character
{
    public static Player instance;
    [Header("Player settings")]
    [SerializeField] private BasePlayerStatsData InGameplayerData;
    [SerializeField] private BasePlayerStatsData BasePlayerData;
    [SerializeField] private WeaponData.WeaponType BaseWeapon = WeaponData.WeaponType.Whip;

    [Header("Player components")] 
    public Camera MainCamera;
    public Animator animator;
    public Level Level;
    public Inventory Inventory;
    public PlayerMove PlayerMove;
    public LevelData LevelData;
    [SerializeField] ParticleSystem bleeding;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] private GameObject skin;
    
    public Action<float> HealthChanged;
    public Action PlayerDead;
    
    private float healthPoint;
    private float attackPower; 
    private float defencePower;
    private float speed;
    private float maxHealth;
    private float attackSpeed;
    private float expAdditional;
    private float luck;
    private bool isColliding;
    public bool isDead;
    internal Coroutine hitCoroutine;
    private WaitForSeconds wait1 = new(1f);
    private WaitForSeconds wait02 = new(0.2f);
   
    private Player() { }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public virtual void Initialize(LevelData levelData, WeaponPool weaponPool, EnemySpawner enemySpawner)
    {
        Debug.Log("Player initialization: " + gameObject.name + ", World: " + levelData.name);
        LevelData = levelData;
        Inventory.Initialize(this, weaponPool, enemySpawner);
        
        healthPoint = BasePlayerData.GetFieldByName(StatField.Names.Health).value + 
                      InGameplayerData.GetFieldByName(StatField.Names.Health).value;
        
        maxHealth = BasePlayerData.GetFieldByName(StatField.Names.Health).value + 
                    InGameplayerData.GetFieldByName(StatField.Names.Health).value;
        
        attackPower = BasePlayerData.GetFieldByName(StatField.Names.Attack).value + 
                      InGameplayerData.GetFieldByName(StatField.Names.Attack).value;
        
        defencePower = BasePlayerData.GetFieldByName(StatField.Names.Armor).value +
                       InGameplayerData.GetFieldByName(StatField.Names.Armor).value / 100f;
        
        speed = BasePlayerData.GetFieldByName(StatField.Names.MoveSpeed).value +
                InGameplayerData.GetFieldByName(StatField.Names.MoveSpeed).value / 2f;
        
        luck = BasePlayerData.GetFieldByName(StatField.Names.Luck).value +
            InGameplayerData.GetFieldByName(StatField.Names.Luck).value;
        
        attackSpeed = 100f;
        expAdditional = 100f;

        
        hitCoroutine = null;
        
        isColliding = false;

        StartCoroutine(GetFirstWeapon());
    }

    public static Player GetInstance()
    {
        return instance;
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }

    public float GetExpAdditional()
    {
        return expAdditional;
    }

    public float GetLuck()
    {
        return luck;
    }

    public void DecreaseAttackSpeed(float value)
    {
        attackSpeed -= value;
    }

    public void IncreaseExpAdditional(float value)
    {
        expAdditional += value;
    }

    public void IncreaseLuck(int value)
    {
        luck += value;
    }

    public void Die()
    {
        PlayerMove.isDead = true;
        isDead = true;
        StartCoroutine(DieAnimation());
    }

    protected IEnumerator DieAnimation()
    {
        animator.SetBool("IsDead", true);

        yield return wait1;

        PlayerDead?.Invoke();
        PauseManager.instance.PauseGame();
    }

    private IEnumerator GetFirstWeapon()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("Player default weapon: " + BaseWeapon.ToString());
        /*foreach (WeaponData.WeaponType weaponType in Enum.GetValues(typeof(WeaponData.WeaponType)))
        {
            for (int i = 0; i < 5; i++)
            {
                Inventory.AddWeapon(weaponType);
            }
        }*/
        Inventory.AddWeapon(BaseWeapon); 
    }

    private void ReduceHealthPoint(float damage)
    {
        if (!isDead)
        {
            if(healthPoint <= damage)
            {
                skin.SetActive(false);
                explosion.Play();
                healthPoint = 0;
                Die();
            }
            else
            {
                healthPoint -= damage;
            }

            bleeding.Play();

            isColliding = true;

            if (hitCoroutine == null)
                hitCoroutine = StartCoroutine(UnderAttack());
        }
        HealthChanged?.Invoke(GetHealthPoint());
    }

    public void MakeDamage(float enemyAttackPower)
    {
        ReduceHealthPoint(enemyAttackPower - enemyAttackPower * defencePower / 100f);
    }

    protected IEnumerator UnderAttack()
    {

        do
        {
            isColliding = false;
            yield return wait02;
        }
        while (isColliding);
        
        hitCoroutine = null;
    }
    
    public void HurtByBullet(Bullet bullet, Vector3 hitPoint)
    {
        if (isDead) return;
        var bulletDamage = bullet.moduleParameters.GetInt("_AttackPower");
        int damage = bulletDamage - (int)(bulletDamage * GetDefencePower()/100f);

        ReduceHealthPoint(damage);
    }

    public Vector2 GetRandomInsideCircle(float radius=2f)
    {
        return (Vector2)transform.position + Random.insideUnitCircle * radius;
    }

    public float GetDefencePower()
    {
        return defencePower;
    }
    
    public float GetHealthPoint()
    {
        return healthPoint;
    }
    
    public Vector3 GetPosition()
    {
        return transform.position;
    }
    
    public float GetAttackPower()
    {
        return attackPower;
    }
    
    public void RecoverHealthPoint(float amount)
    {
        if (healthPoint + amount > maxHealth)
        {
            healthPoint = maxHealth;
        }
        else
        {
            healthPoint += amount;
        }
        HealthChanged?.Invoke(GetHealthPoint());
    }
    
    public void RecoverHealthPercent(float percent)
    {
        var amount = maxHealth * percent;
        RecoverHealthPoint(amount);
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
    
    public float GetSpeed()
    {
        return speed;
    }
}