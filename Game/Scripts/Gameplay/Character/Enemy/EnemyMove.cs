using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public Transform player;
    public bool isDead;
    public bool RevertFlip = true;
    
    protected Enemy character;
    protected Vector2 direction;
    protected Rigidbody2D _rigidbody2D;
    protected float distanceToPlayer;
    
    private SpriteRenderer spriteRenderer;
    private bool isSkinFliped;
    private Vector3 skinInitialScale;
    private WaitForSeconds wait05 = new(0.5f);

    void Awake()
    {
        Initialize();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        player = Player.GetInstance().transform;
        
    }

    protected virtual void OnEnable()
    {
        isDead = false;
    }
    
    private void FixedUpdate()
    {
        if(isDead)
            return;
        direction = (player.position - transform.position).normalized;
        CalculateDistance();
        Movement();
        FlipSprite();
        
        if (distanceToPlayer >  EnemySpawner.GetInstance().maxDistanceToEnemy)
        {
            transform.position = EnemySpawner.GetInstance().RandomPosition();
        }
    }

    protected virtual void CalculateDistance()
    {
        distanceToPlayer = Vector2.Distance(player.position, transform.position);
    }

    protected virtual void Movement()
    {
        if (!isDead)
            _rigidbody2D.MovePosition(_rigidbody2D.position + direction * character.GetSpeed()/15f * Time.fixedDeltaTime);
    }

    protected virtual void FlipSprite()
    {
        if (direction.x >= 0)
            spriteRenderer.flipX = RevertFlip;
        else
            spriteRenderer.flipX = !RevertFlip;
    }

    void Initialize()
    {
        character = GetComponent<Enemy>();
        direction = new Vector2();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public Vector2 GetDirection()
    {
        return direction;
    }

    public Transform GetTarget()
    {
        return player;
    }
}
