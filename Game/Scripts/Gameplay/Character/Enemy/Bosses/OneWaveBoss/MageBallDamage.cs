using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageBallDamage : MonoBehaviour
{
    public ObjectDamageData DamageData;

    private bool didDamage;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (didDamage)
        {
            return;
        }
        Player.GetInstance().MakeDamage(DamageData.GetAttackPower);
        didDamage = true;
    }
}
