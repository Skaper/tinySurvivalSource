using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : Character
{
    public float DamageShowDelay = 0.4f;
    private float damageTimer;
    private float damageValueToShow;
    private Shader shaderGUItext;
    private Shader shaderSpritesDefault;
    private EnemyMove enemyMove;
    private Rigidbody2D rigidbody;
    private Material _material;
    private static readonly int DieAnimName = Animator.StringToHash("Die");
    private WaitForSeconds wait02 = new(0.2f);
    private WaitForSeconds wait1 = new(1f);
    void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        damageTimer += Time.deltaTime;
    }

    void OnEnable()
    {
        InitHealthPoint();
        GetComponent<CapsuleCollider2D>().enabled = true;
        spriteRenderer.material.shader = shaderSpritesDefault;
        hitCoroutine = null;
    }

    protected override void Initialize()
    {
        base.Initialize();
        
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default");
        enemyMove = GetComponent<EnemyMove>();
        rigidbody = GetComponent<Rigidbody2D>();
        _material = spriteRenderer.material;
    }

    public void SetSpawnData(int level, Vector2 position)
    {
        this.level = level;
        transform.position = position;
        
        if (characterHealth == null)
        {
            characterHealth = GetComponent<Health>();
        }
        
        characterHealth.CurrentHealth = characterData.GetHealthPoint(level);
        characterHealth.MaxHealth = characterData.GetHealthPoint(level);
        attackPower = characterData.GetAttackPower(level);
        defencePower = characterData.GetDefencePower(level);
        speed = characterData.GetSpeed(level);
        sprite = characterData.GetSprite(level);
        controller = characterData.GetController(level);

        
    }

    protected override void OnReceivedDamage(bool isDead, float damage, bool isKnockBack=true)
    {
        base.OnReceivedDamage(isDead, damage, isKnockBack);
        if (hitCoroutine == null)
            hitCoroutine = StartCoroutine(UnderAttack());

        if(isKnockBack)
            KnockBack();

        damageValueToShow += damage;
        if (damageTimer >= DamageShowDelay)
        {
            ShowFloatingDamage(damageValueToShow);
            damageTimer = 0;
            damageValueToShow = 0;
        }
    }

    protected override IEnumerator UnderAttack()
    {
        
        _material.shader = shaderGUItext;

        yield return wait02;

        _material.shader = shaderSpritesDefault;
        hitCoroutine = null;
    }

    void KnockBack()
    {
        rigidbody.AddForce(enemyMove.GetDirection() * -2f, ForceMode2D.Impulse);
    }

    public override void Die()
    {
        EnemySpawner.GetInstance().IncreaseKillCount();
        EnemySpawner.GetInstance().RemoveFromList(this);
        if (Random.Range(0, 10) > 5)
            DropCrystral();

        StartCoroutine(DieAnimation());
    }

    void DropCrystral()
    {
        foreach (var itemType in characterData.GetDropItems(level))
        {
            GameObject item = ItemsPool.instance.GetItem(itemType);
            item.transform.position = (Vector2)transform.position + Random.insideUnitCircle * 0.5f;
            item.SetActive(true);
        }
    }

    protected override IEnumerator DieAnimation()
    {
        GetAnimator().SetBool(DieAnimName, true);
        GetComponent<EnemyMove>().isDead = true;
        GetComponent<CapsuleCollider2D>().enabled = false;

        yield return wait1;

        
        GetAnimator().SetBool(DieAnimName, false);
        spriteRenderer.color = Color.white;
        transform.rotation = Quaternion.identity;
        EnemyPool.instance.ReturnToPool(this, GetCharacterType());
    }
}