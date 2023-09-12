using UnityEngine;

[System.Serializable]
public class Stage
{
    public string name = "Stage";
    public float SpawnDelay = 0.5f;
    [Tooltip("Stage duration in seconds")]
    public float Duration = 60;
    [Tooltip("One random boss")]
    public StageBoss[] Bosses;
    public StageEnemy[] Enemies;
}
[System.Serializable]
public class StageEnemy
{
    public string name;
    public int Level = 0;
    [Range(0f,1f)]
    public float Percent = 1f;
    public Enemy Unit;
    public int CountInPool = 25;
}

[System.Serializable]
public class StageBoss
{
    public string name;
    public int Level = 0;
    [Range(0f,1f)]
    public float Percent = 1f;
    public Enemy Unit;
    public int CountInPool = 1;
}