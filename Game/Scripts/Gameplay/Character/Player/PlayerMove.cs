using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private static PlayerMove instance;
    public bool isDead;
    public Animator animator;
    private Player character;
    private float horizontal;
    private float vertical;
    private Vector2 moveVector;
    private bool lookingLeft;
    private Rigidbody2D _rigidbody2D;
    private bool isSkinFliped;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        character = GetComponent<Player>();
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        lookingLeft = false;
        instance = this;
        isDead = false;
    }

    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            if (!Player.instance.Level.GetIsLevelUpTime())
            {
                moveVector = InputProvider.GetMoveVector();
                horizontal = moveVector.x;
                vertical = moveVector.y;
            }
            if (horizontal != 0f || vertical != 0f)
            {
                if (horizontal > 0f)
                {
                    animator.SetInteger("Direction", 1);
                    lookingLeft = false;
                }
                else if (horizontal < 0f)
                {
                    animator.SetInteger("Direction", -1);
                    lookingLeft = true;
                }
                else
                {
                    if (vertical > 0f)
                    {
                        animator.SetInteger("Direction", 1);
                        lookingLeft = false;
                    }
                    else if (vertical < 0f)
                    {
                        animator.SetInteger("Direction", -1);
                        lookingLeft = true;
                    }
                }
            }
            else if(horizontal == 0f && vertical == 0f)
            {
                animator.SetInteger("Direction", 0);
            }

            
        }
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            _rigidbody2D.MovePosition(_rigidbody2D.position + moveVector * character.GetSpeed()/15f * Time.fixedDeltaTime);
        }
    }

    public static PlayerMove GetInstance()
    {
        return instance;
    }

    public bool GetLookingLeft()
    {
        return lookingLeft;
    }

    public float GetHorizontal()
    {
        return horizontal;
    }

    public float GetVertical()
    {
        return vertical;
    }
}