using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "Scriptable Object/Character Data", order = int.MinValue )]
public class CharacterData : ScriptableObject
{
    [SerializeField] private CharacterType characterType;
    [SerializeField] private List<LevelCharacterData> Levels = new List<LevelCharacterData>();
    
    public enum CharacterType
    {
        Item = -1,
        FlyingEye = 0,
        Goblin = 1,
        Skeleton = 2,
        Mushroom = 3,
        Knight = 4,
        Bandit = 5,
        Zombie = 6,
        Creeper = 7,
        ZombieChicken = 8,
        SpiderBrown = 9,
        SlimeSmall = 10,
        SkeletonWizzard = 11,
        Bat = 12,
        SkeletonSimple = 13,
        Wolf = 14,
        Boar = 15,
        SlimeMiddle  = 16,
        Bee = 17,
        MageStaffBoss = 18,
        ZombieSword = 19,
        ZombieGold = 20,
        SkeletonWizzardBlack = 21,
        SkeletonWizzardGreen = 22,
        SkeletonWizzardElit = 23,
        SkeletonWizzardElitBlack = 24,
        SkeletonWizzardElitGreen = 25,
    }

    public int GetHealthPoint(int level = 0)
    {
        level = VerifyLevel(level);
        return Levels[level].healthPoint;
    }

    public int GetAttackPower(int level = 0)
    {
        level = VerifyLevel(level);
        return Levels[level].attackPower;
    }

    public int GetDefencePower(int level = 0)
    {
        level = VerifyLevel(level);
        return Levels[level].defencePower;
    }

    public int GetSpeed(int level = 0)
    {
        level = VerifyLevel(level);
        return Levels[level].speed;
    }

    public CharacterType GetCharacterType(int level = 0)
    {
        return characterType;   
    }

    public RuntimeAnimatorController GetController(int level = 0)
    {
        level = VerifyLevel(level);
        return Levels[level].controller;
    }

    public Sprite GetSprite(int level = 0)
    {
        level = VerifyLevel(level);
        return Levels[level].sprite;
    }

    public ItemType[] GetDropItems(int level = 0)
    {
        level = VerifyLevel(level);
        return Levels[level].dropItems;
    }

    private int VerifyLevel(int level)
    {
        return level >= Levels.Count ? Levels.Count - 1 : level;
    }
    
    private void OnValidate()
    {
        for (int i = 0; i < Levels.Count; i++)
        {
            Levels[i].name = "Level " + i;
        }
    }
}

[System.Serializable]
public class LevelCharacterData
{
    public string name;
    
    public Sprite sprite;
    public RuntimeAnimatorController controller;
    public int healthPoint;
    public int attackPower;
    public int defencePower;
    public int speed;
    public ItemType[] dropItems;
}