using UnityEngine;
[CreateAssetMenu(fileName = "Player Data", menuName = "Scriptable Object/Player Data", order = int.MinValue )]

public class PlayerData : ScriptableObject
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int armor;
    [SerializeField] private int moveSpeed;
    [SerializeField] private int attackPower;
    [SerializeField] private float duration;
    [SerializeField] private float revival;
    [SerializeField] RuntimeAnimatorController controller;
    public int MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    public int Armor
    {
        get => armor;
        set => armor = value;
    }

    public int MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }

    public int AttackPower
    {
        get => attackPower;
        set => attackPower = value;
    }

    public float Duration
    {
        get => duration;
        set => duration = value;
    }

    public float Revival
    {
        get => revival;
        set => revival = value;
    }

    public RuntimeAnimatorController Controller => controller;
}
