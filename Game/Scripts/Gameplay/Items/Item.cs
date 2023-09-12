using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] protected ItemType Type;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    private WaitForSecondsRealtime wait04 = new(0.4f);
    protected PlayerMove player
    {
        get
        {
            if (_playerMove == null)
            {
                _playerMove = Player.GetInstance().PlayerMove;
            }

            return _playerMove;
        }
    }

    private PlayerMove _playerMove;
    
    protected Rigidbody2D rigidbody;
    protected bool isCollided;
    protected int speed = 10;
    public virtual ItemType GetItemType()
    {
        return Type;
    }

    protected virtual void Initialize()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual IEnumerator ItemAnimation()
    {
        if (player == null)
            yield break;
        rigidbody.AddForce(new Vector2(player.GetHorizontal(), player.GetVertical()) * speed, ForceMode2D.Impulse);

        yield return wait04;

        isCollided=true;

        StartCoroutine(DisableWithDelay());

        while (true)
        {
            Vector2 direction = player.transform.position - transform.position;
            rigidbody.MovePosition(rigidbody.position + direction.normalized * Time.deltaTime * speed++);

            yield return null;
        }
    }
    
    protected virtual IEnumerator DisableWithDelay()
    {
        yield return null;
    }
}
