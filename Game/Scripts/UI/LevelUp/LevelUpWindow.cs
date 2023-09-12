using System;
using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelUpWindow : MonoBehaviour
{
    [SerializeField] GameObject Context;
    [SerializeField] Transform ItemsColumn;

    public LevelUpItem LevelUpItemTemplate;
    private LevelUpItem[] _levelUpItems = new LevelUpItem[slotNum];

    private const int slotNum = 4;
    private Player _player;

    private void Start()
    {
        Context.SetActive(false);
    }

    public void Initialize(Player player)
    {
        _player = player;
        for (int i = 0; i < slotNum; i++)
        {
            _levelUpItems[i] = Instantiate(LevelUpItemTemplate, ItemsColumn);
        }
    }

    public void Show()
    {
        StartCoroutine(GetNewItem());
    }
    
    IEnumerator GetNewItem()
    {
        PauseManager.instance.PauseGame();
        ShowSelectWindow();

        while (true)
        {
            if (!_player.Level.GetIsLevelUpTime()) break;

            yield return null;
        }


        Context.SetActive(false);
        PauseManager.instance.UnpauseGame();
    }
    private int activeSlots = 3;
    private int _keyboardNavigationIndex = -1;
    private int _previousNavigationIndex = -1;
    private int _selectedIndex = -1;

    private void KeyboardNavigation(int direction)
    {
        
        _previousNavigationIndex = _keyboardNavigationIndex;
    
        _keyboardNavigationIndex += direction;
        if (_keyboardNavigationIndex < 0)
        {
            _keyboardNavigationIndex = activeSlots - 1;
        }
        else if (_keyboardNavigationIndex >= activeSlots)
        {
            _keyboardNavigationIndex = 0;
        }

        if (_previousNavigationIndex != -1)
        {
            _levelUpItems[_previousNavigationIndex].Deselect();
        }
        
        _levelUpItems[_keyboardNavigationIndex].Select();
        _selectedIndex = _keyboardNavigationIndex;
    }
    
    void Update()
    {
        CheckForKeyPress();
    }

    private void CheckForKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            KeyboardNavigation(-1);
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            KeyboardNavigation(1);
        }

        if (_selectedIndex > -1 && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return)))
        {
            _levelUpItems[_selectedIndex].Click();
        }
    }
    
    
    void ShowSelectWindow()
    {
        Context.SetActive(true);
        _selectedIndex = -1;
        _keyboardNavigationIndex = -1;
        _previousNavigationIndex = -1;
        List<string> checkDuplicate = new List<string>(3);

        for (int i = 0; i < slotNum; i++)
        {
            string itemName, itemDescription, itemLevelText;
            Sprite sprite;
            if (Random.Range(0, 10) < 4) 
            {
                WeaponData.WeaponType weapon;

                do weapon = GetRandomWeapon();
                while (checkDuplicate.Contains(weapon.ToString()));
                checkDuplicate.Add(weapon.ToString());

                sprite = _player.Inventory.GetWeaponDataAsset(weapon).GetSprite();
                itemName = _player.Inventory.GetWeaponDataAsset(weapon).GetName();
                itemDescription =  _player.Inventory.GetWeaponDataAsset(weapon).GetDescription();
                
                if (_player.Inventory.GetWeaponInventory().TryGetValue(weapon, out var itemLevel))
                {
                    itemLevelText = LocalizationManager.Localize("LevelUp.Level")+ " " + (itemLevel + 1);
                }
                else
                {
                    itemLevelText = LocalizationManager.Localize("LevelUp.New");
                }

                _levelUpItems[i].ResetOnClickListener(delegate { 
                    _player.Inventory.AddWeapon(weapon); 
                    _player.Level.SetIsLevelUpTime(false); 
                });
            }
            else 
            {
                AccessoryData.AccessoryType accessory;
                
                do accessory = GetRandomAccessory();
                while(checkDuplicate.Contains(accessory.ToString()));
                checkDuplicate.Add(accessory.ToString());
                
                sprite = _player.Inventory.GetAccessoryDataAsset(accessory).GetSprite();
                itemName = _player.Inventory.GetAccessoryDataAsset(accessory).GetName();
                itemDescription =  _player.Inventory.GetAccessoryDataAsset(accessory).GetDescription();
                
                if (_player.Inventory.GetAccInventory().TryGetValue(accessory, out var itemLevel))
                {
                    itemLevelText = LocalizationManager.Localize("LevelUp.Level") + " " + (itemLevel+1);
                }
                else
                {
                    itemLevelText = LocalizationManager.Localize("LevelUp.New");
                }
                
                _levelUpItems[i].ResetOnClickListener(delegate { 
                    _player.Inventory.AddAccessory(accessory); 
                    _player.Level.SetIsLevelUpTime(false); 
                });
            }
            _levelUpItems[i].FillData(sprite, itemName, itemDescription, itemLevelText);
        }

        if (Random.Range(0, 100) < _player.GetLuck())
        {
            _levelUpItems[3].gameObject.SetActive(true);
            activeSlots = 4;
        }
        else
        {
             _levelUpItems[3].gameObject.SetActive(false);
             activeSlots = 3;
        }
           
    }
    
    WeaponData.WeaponType GetRandomWeapon()
    {
        return (WeaponData.WeaponType)Random.Range(0, Enum.GetValues(typeof(WeaponData.WeaponType)).Length);
    }

    AccessoryData.AccessoryType GetRandomAccessory()
    {
        return (AccessoryData.AccessoryType)Random.Range(0, Enum.GetValues(typeof(AccessoryData.AccessoryType)).Length);
    }
}
