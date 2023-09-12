using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float damageCooldown = 0.1f;
    private float _health;
    private float _maxHealth;
    private bool isDead;
    private float lastDamageTime;

    public bool IsDead => isDead;
    public event Action<bool, float, bool> ReceivedDamage;
    

    public float CurrentHealth
    {
        get => _health;
        set => _health = value;
    }

    public float MaxHealth
    {
        get => _maxHealth;
        set => _maxHealth = value;
    }

    public void ResetHealth()
    {
        _health = _maxHealth;
        isDead = false;
    }
    
    public void ReduceHealth(float damage, bool isKnockBack=true)
    {
        if (Time.time - lastDamageTime <= damageCooldown)
        {
            return;
        }
        lastDamageTime = Time.time;
        
        if(_health <= damage)
        {
            _health = 0;
            isDead = true;
        }
        else
        {
            _health -= damage;
        }
        
        ReceivedDamage?.Invoke(isDead, damage, isKnockBack);
    }
    
    public void RecoverHealthPoint(float amount)
    {
        if (_health + amount > _maxHealth)
        {
            _health = _maxHealth;
        }
        else
        {
            _health += amount;
        }
    }
}
