using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChestEnemy : Enemy
{
    public Item DropItem;
    
    void OnEnable()
    {
        InitHealthPoint();
        GetComponent<CapsuleCollider2D>().enabled = true;
    }
   
    
    public override void Die()
    {
        GetComponent<CapsuleCollider2D>().enabled = false;
        ItemsPool.instance.ReturnToPool(gameObject, ItemType.Chest);
    }

    protected override IEnumerator DieAnimation()
    {
        yield return null;
    }

    protected override IEnumerator UnderAttack()
    {
        yield return null;
    }
}
