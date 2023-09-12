using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsPool : MonoBehaviour
{
    [SerializeField] List<Item> ItemsList;

    private Dictionary<ItemType, GameObject> _items;
    private Dictionary<ItemType, Queue<GameObject>> _freePool;
    public int ItemCopies = 50;
    public static ItemsPool instance;
    
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
    }
    
    public void Initialize(LevelData levelData)
    {
        if (levelData != null)
        {
            ItemsList = new List<Item>();
            foreach (var item in levelData.Items)
            {
                ItemsList.Add(item);
            }
        }
        _items = new Dictionary<ItemType, GameObject>();
        _freePool = new Dictionary<ItemType, Queue<GameObject>>();
        foreach (var item in ItemsList)
        {
            if(item.GetItemType() == ItemType.None)
                continue;
            
            if (_items.ContainsKey(item.GetItemType()) == false)
            {
                _items.Add(item.GetItemType(), item.gameObject);
            }
            else
            {
                Debug.LogError("An item with the same key has already been added");
                Debug.LogError("Enemy type: " + item.GetItemType() + 
                               " ContainsKey: TRUE" +
                               " Gameobject name: " + item.gameObject);
            }
            
        }

        CreatePool();
        ItemsList.Clear();
    }

    public void CreatePool()
    {
        foreach(ItemType item in Enum.GetValues(typeof(ItemType)))
        {
            if(item == ItemType.None)
                continue;
            
            if (_items.ContainsKey(item) == false)
            {
                continue;
            }
            Queue<GameObject> newQue = new Queue<GameObject>();

            for (int j = 0; j < ItemCopies; j++)
            {
                newQue.Enqueue(CreateItem(item));
            }
            _freePool.Add(item, newQue);
        }
    }
    
    private GameObject CreateItem(ItemType type)
    {
        var newObject = Instantiate(_items[type], instance.transform);
        
        newObject.SetActive(false);

        return newObject;
    }
    
    public  GameObject GetItem(ItemType type)
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

    public void ReturnToPool(GameObject item, ItemType type)
    {
        item.SetActive(false);
        _freePool[type].Enqueue(item);
    }
    
}
