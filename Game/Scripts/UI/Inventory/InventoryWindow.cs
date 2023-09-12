using UnityEngine;

public class InventoryWindow : MonoBehaviour
{
    public InventoryItemIU WeaponSlotTemplate;
    public InventoryItemIU AccessorySlotTemplate;

    private Player _player;
    private const int slotNum = 6;
    InventoryItemIU[] weaponSlots = new InventoryItemIU[slotNum];
    InventoryItemIU[] accessorySlots = new InventoryItemIU[slotNum];
    
    [SerializeField] Transform weaponSlotParent;
    [SerializeField] Transform accessorySlotParent;
    
    public void Initialize(Player player)
    {
        _player = player;
        SlotInitial();
    }

    void SlotInitial()
    {
        for (int i = 0; i < slotNum; i++)
        {
            weaponSlots[i] = Instantiate(WeaponSlotTemplate, weaponSlotParent);
            weaponSlots[i].Show();

            accessorySlots[i] = Instantiate(AccessorySlotTemplate, accessorySlotParent);
            accessorySlots[i].Show();
        }
    }

    
    public void UpdateView()
    {
        int count = 0;
        foreach(WeaponData.WeaponType weapon in _player.Inventory.GetWeaponInventory().Keys)
        {
            weaponSlots[count].FillData(_player.Inventory.GetWeaponDataAsset(weapon).GetSprite(), 
                _player.Inventory.GetWeaponInventory()[weapon].ToString());
            if(count < weaponSlots.Length - 1)
                count+=1;
        }

        count = 0;
        foreach (AccessoryData.AccessoryType accessory in _player.Inventory.GetAccInventory().Keys)
        {
            accessorySlots[count].FillData(_player.Inventory.GetAccessoryDataAsset(accessory).GetSprite(), 
                _player.Inventory.GetAccInventory()[accessory].ToString());
            if(count < accessorySlots.Length - 1)
                count+=1;
        }
    }
}
