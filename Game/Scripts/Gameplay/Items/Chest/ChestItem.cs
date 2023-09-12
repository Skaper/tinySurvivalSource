using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Health))]
public class ChestItem : Item
{
    [SerializeField] protected Health characterHealth;
    [SerializeField] protected float MaxHealth = 1f;

    public ChestDrop[] Drops;
    
    private void Awake()
    {
        if (characterHealth == null)
        {
            characterHealth = GetComponent<Health>();
        }
        Drops = Drops.OrderBy((d) => d.DropChance).ToArray();
        characterHealth.ReceivedDamage += CharacterHealthOnReceivedDamage;
    }

    private void CharacterHealthOnReceivedDamage(bool isDead, float damage, bool isKnockBack=true)
    {
        if (isDead == false)
        {
            return;
        }

        for (var i = 0; i < Drops.Length; i++)
        {
            var rnd = Random.value;
            if (rnd <= Drops[i].DropChance)
            {
                var dropItem = ItemsPool.instance.GetItem(Drops[i].SpawnItem);
                dropItem.transform.position = transform.position;
                dropItem.SetActive(true);
                break;
            }

            if (i == Drops.Length - 1)
            {
                var dropItem = ItemsPool.instance.GetItem(Drops[i].SpawnItem);
                dropItem.transform.position = transform.position;
                dropItem.SetActive(true);
            }
        }
        
        foreach (var drop in Drops)
        {
            var rnd = Random.value;
            if (rnd <= drop.DropChance)
            {
                var dropItem = ItemsPool.instance.GetItem(drop.SpawnItem);
                dropItem.transform.position = (Vector2)transform.position + Random.insideUnitCircle *0.5f;
                dropItem.SetActive(true);
                break;
            }
        }
        
        GetComponent<Collider2D>().enabled = false;
        ItemsPool.instance.ReturnToPool(gameObject, ItemType.Chest);
    }

    void OnEnable()
    {
        characterHealth.MaxHealth = MaxHealth;
        characterHealth.CurrentHealth = MaxHealth;
        GetComponent<Collider2D>().enabled = true;
    }
}

[System.Serializable]
public class ChestDrop
{
    public ItemType SpawnItem;
    [Range(0,1f)]
    public float DropChance;
}
