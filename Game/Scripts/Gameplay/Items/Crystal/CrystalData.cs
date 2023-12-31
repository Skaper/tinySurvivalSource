using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "Scriptable Object/Crystal Data", order = int.MinValue)]
public class CrystalData : ScriptableObject
{
    public enum CrystalType
    {
        blue,
        green,
        red
    }

    [SerializeField] Sprite sprite;
    [SerializeField] float lifeTime = 30f;
    [SerializeField] Color color;
    [SerializeField] RuntimeAnimatorController controller;
    [SerializeField] CrystalType crystalType;
    [SerializeField] int expValue;

    public int GetExpValue()
    {
        return expValue;
    }

    public float GetLifeTime()
    {
        return lifeTime;
    }

    public CrystalType GetCristalType()
    {
        return crystalType;
    }

    public RuntimeAnimatorController GetController()
    {
        return controller;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }

    public Color GetColor()
    {
        return color;
    }
}