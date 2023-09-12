using UnityEngine;

public class RangerAttack : EnemyAttack
{
    public RangeWeaponData WeaponData;
    private static readonly int AttackAnimName = Animator.StringToHash("Attack");
    private RangerMove _skeletonBowMove;
    private float cooldownTimer;

    protected override void Awake()
    {
        base.Awake();
        _skeletonBowMove = GetComponent<RangerMove>();
    }

    private void OnEnable()
    {
        _skeletonBowMove.RangedDistanceTriggered.AddListener(ChargeBow);
    }

    private void OnDisable()
    {
        _skeletonBowMove.RangedDistanceTriggered.RemoveListener(ChargeBow);
    }

    private void Update()
    {
        if (_skeletonBowMove.isDead)
        {
            return;
        }

        cooldownTimer += Time.deltaTime;
    }

    private void ChargeBow(float dist)
    {
        if (cooldownTimer >= WeaponData.GetCooldown)
        {
            character.GetAnimator().SetTrigger(AttackAnimName);
            cooldownTimer = 0f;
        }
    }

    //called from animation
    public void ShootArrow()
    {
        var muzzle = _skeletonBowMove.GetMuzzlePoint().position;
        var target = Player.GetInstance().GetRandomInsideCircle();
        
        float angle = Mathf.Atan2(target.y - transform.position.y, 
            target.x - transform.position.x) * Mathf.Rad2Deg + 180;

        var arrow = WeaponPool.instance.GetEnemyWeapon(WeaponData);
        arrow.SetParameters(target, muzzle, Quaternion.Euler(0,0,angle));
        arrow.gameObject.SetActive(true);
        
        
    }
}
