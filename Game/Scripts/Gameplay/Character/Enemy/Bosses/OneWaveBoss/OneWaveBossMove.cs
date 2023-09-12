using UnityEngine;
using UnityEngine.Events;

public class OneWaveBossMove : EnemyMove
{
    public float RangeDistance = 5f;
    public UnityEvent<float> RangedDistanceTriggered;
    public bool CanMove = true;
    protected override void Movement()
    {
        if (isDead)
        {
            return;
        }

        var distToPlayer = Vector2.Distance(player.position, transform.position);

        if (distToPlayer <= RangeDistance)
        {
            RangedDistanceTriggered?.Invoke(distToPlayer);
        }
        
        if(CanMove)
        {
            _rigidbody2D.MovePosition(_rigidbody2D.position + direction * character.GetSpeed()/15f * Time.fixedDeltaTime);
        }
    }
}
