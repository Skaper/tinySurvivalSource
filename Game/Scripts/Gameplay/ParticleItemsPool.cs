using System.Collections.Generic;
using UnityEngine;

public class ParticleItemsPool : MonoBehaviour
{
    public int CopiesPerItem = 25;
    [SerializeField] List<ParticleItem> ItemsList;
    private Dictionary<ParticleItem.ParticleItemType, GameObject> _items;
    private Dictionary<ParticleItem.ParticleItemType, Queue<GameObject>> _freePool;
    
    public static ParticleItemsPool instance; 
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        //Initialize(null);
    }
    
    public void Initialize(LevelData levelData)
    {
        if (levelData != null)
        {
            ItemsList = new List<ParticleItem>();
            foreach (var item in levelData.ParticleItems)
            {
                ItemsList.Add(item);
            }
        }
        
        _items = new Dictionary<ParticleItem.ParticleItemType, GameObject>();
        _freePool = new Dictionary<ParticleItem.ParticleItemType, Queue<GameObject>>();
        
        foreach (var item in ItemsList)
        {
            if (_items.ContainsKey(item.Type) == false)
            {
                _items.Add(item.Type, item.gameObject);
                
                Queue<GameObject> newQue = new Queue<GameObject>();

                for (int j = 0; j < CopiesPerItem; j++)
                {
                    newQue.Enqueue(CreateItem(item.Type));
                }
                _freePool.Add(item.Type, newQue);
            }
            
        }
        
        ItemsList.Clear();
    }
    
    private GameObject CreateItem(ParticleItem.ParticleItemType type)
    {
        var newObject = Instantiate(_items[type], transform);
        
        newObject.SetActive(false);

        return newObject;
    }
    
    public  GameObject GetItem(ParticleItem.ParticleItemType type)
    {
        if (_freePool[type].Count > 0)
        {
            return _freePool[type].Dequeue();
        }
        else
        {
            return CreateItem(type);
        }
    }
    
    public void ReturnToPool(GameObject item, ParticleItem.ParticleItemType type)
    {
        _freePool[type].Enqueue(item);
    }
}
