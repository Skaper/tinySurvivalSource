using UnityEngine;

[CreateAssetMenu(fileName = "Object Damage Data", menuName = "Scriptable Object/Object Damage Data", order = int.MinValue)]
public class ObjectDamageData : ScriptableObject
{
    [SerializeField] int attackPower;
    public int GetAttackPower => attackPower;
}
