using System.Collections;
using MyBox;
using UnityEngine;

public class EnemyWeapon : Weapon
{
    [ConditionalField(nameof(Owner), false, WeaponOwner.Enemy)]
    public EnemyWeaponData enemyWeaponData;
    protected WaitForSeconds _waitForLifeTime;

    public virtual void SetParameters(Vector2 target, Vector2 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }

    protected override IEnumerator StartDestroy()
    {
        yield return _waitForLifeTime;
        InactiveWeapon();
    }
    
    
}
