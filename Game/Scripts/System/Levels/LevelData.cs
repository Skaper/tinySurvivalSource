using UnityEngine;
[CreateAssetMenu(fileName = "Level Data", menuName = "Scriptable Object/Gameplay/Level Data", order = int.MinValue )]
public class LevelData : ScriptableObject
{
    public GameObject World;
    public GameObject Player;
    public GameObject EnemySpawner;
    public GameObject InGameUI;

    public Item[] Items;
    public PlayerWeapon[] PlayerWeapons;
    public EnemyWeapon[] EnemyWeapons;
    public AccessoryData[] PlayerAccessoriesData;
    public ParticleItem[] ParticleItems;
    public Stage[] Stages;

    private void OnValidate()
    {
        int index = 1;
        
        foreach (var stage in Stages)
        {
            string enemies = "E: ";
            string bosses = "B: ";
            foreach (var stageEnemy in stage.Enemies)
            {
                var gameObject = stageEnemy.Unit.gameObject;
                stageEnemy.name = gameObject.name;
                enemies += gameObject.name + ",";
            }
            foreach (var stageBoss in stage.Bosses)
            {
                var gameObject = stageBoss.Unit.gameObject;
                stageBoss.name = gameObject.name;
                bosses += gameObject.name + ",";
            }

            stage.name = "Stage " + index+ "| "+ enemies + " | " + bosses;
            index+=1;
        }
    }
}
