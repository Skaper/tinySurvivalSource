using UnityEngine.Events;
using UnityEngine;

public class RangerMove : EnemyMove
{
    public RangeWeaponData WeaponData;
    public UnityEvent<float> RangedDistanceTriggered;

    [SerializeField] private Transform LeftMuzzlePonint;
    [SerializeField] private Transform RightMuzzlePonint;
    [SerializeField] private bool HaveIdleAnimation = true;
    private static readonly int IdleAnimName = Animator.StringToHash("Idle");

    protected override void Movement()
    {
        if (isDead)
        {
            return;
        }

        if (distanceToPlayer <= WeaponData.GetRange)
        {
            RangedDistanceTriggered?.Invoke(distanceToPlayer);
            if(HaveIdleAnimation)
                character.GetAnimator().SetBool(IdleAnimName, true);
        }
        else
        {
            _rigidbody2D.MovePosition(_rigidbody2D.position + direction * character.GetSpeed()/15f * Time.fixedDeltaTime);
            if(HaveIdleAnimation)    
                character.GetAnimator().SetBool(IdleAnimName, false);
        }
    }

    public Transform GetMuzzlePoint()
    {
        if (direction.x >= 0)
            return RightMuzzlePonint;
        
        return LeftMuzzlePonint;
    }
}
