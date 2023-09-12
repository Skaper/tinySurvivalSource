using System.Collections.Generic;
using UnityEngine;

public class AccessoriesCreator : MonoBehaviour
{
    private Dictionary<AccessoryData.AccessoryType, Accessory> _availableAccessories = new Dictionary<AccessoryData.AccessoryType, Accessory>();

    public void Initialize(Player player)
    {
        foreach (var accessoryData in player.LevelData.PlayerAccessoriesData)
        {
            if (_availableAccessories.ContainsKey(accessoryData.GetAccessoryType()) == false)
            {
                var accessoryController = Instantiate(accessoryData.AccessoryController, transform);
                accessoryController.Initialize(accessoryData, player);
                _availableAccessories.Add(accessoryData.GetAccessoryType(),  accessoryController);
            }
            else
            {
                Debug.LogError("Dictionary already contains accessory with key: " +  accessoryData.GetAccessoryType()
                                                                                + " Accessory name: " + accessoryData.GetName());
            }
        }
        
    }

    public Accessory GetAccessory(AccessoryData.AccessoryType type)
    {
        return _availableAccessories[type];
    }
}
