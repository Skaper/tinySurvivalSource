using System;
using System.Collections;
using System.Collections.Generic;
using BulletPro;
using UnityEngine;

public class OneWaveBossAttack : EnemyAttack
{
    public GameObject FireOrigin;
    public BulletEmitter Emitter;
    public float FireTime;
    public float FireCooldown;
    private OneWaveBossMove _move;
    private static readonly int AttackAnimName = Animator.StringToHash("Attack");
    private bool isFiring;
    private float fireTimer;
    private float fireCooldownTimer;
    protected override void Awake()
    {
        base.Awake();
        FireOrigin.SetActive(false);
        _move = GetComponent<OneWaveBossMove>();
    }

    private void OnEnable()
    {
        fireTimer = 0f;
        fireCooldownTimer = 0f;
        isFiring = false;
        _move.CanMove = true;
        _move.RangedDistanceTriggered.AddListener(PrepareAttack);
    }

    private void OnDisable()
    {
        _move.RangedDistanceTriggered.RemoveListener(PrepareAttack);
    }


    private void Update()
    {
        if (_move.isDead)
        {
            return;
        }
        if (isFiring)
        {
            if (fireTimer >= FireTime)
            {
                FireOrigin.SetActive(false);
                isFiring = false;
                character.GetAnimator().SetBool(AttackAnimName, false);
                fireTimer = 0f;
                fireCooldownTimer = 0f;
                _move.CanMove = true;
            }
            fireTimer += Time.deltaTime;
        }
        else
        {
            fireCooldownTimer += Time.deltaTime;
        }
    }

    private void PrepareAttack(float dist)
    {
        if (fireCooldownTimer >= FireCooldown)
        {
            _move.CanMove = false;
            character.GetAnimator().SetBool(AttackAnimName, true);
        }
        
    }

    //Called by animation
    public void StartAttack()
    {
        FireOrigin.SetActive(true);
        isFiring = true;
        //Emitter.PlayPatternTag("Spiral");
    }
}
