using System;
using System.Collections;
using FMODUnity;
using UnityEngine;

public abstract class WeaponSpawner : MonoBehaviour
{
    public WeaponData weaponData;
    protected Player _player;
    protected WeaponPool _weaponPool;
    protected int level;
    protected EventReference _weaponSoundRef;
    int attackPower;
    float attackSpeed;
    int finalAttackPower;
    float finalAttackSpeed;
    float lastAttackSpeed;
    float inactiveDelay;
    float additionalScale;
    Sprite weaponIcon;

    protected WaitForSeconds waitAtackSpeed;
    
    protected EnemySpawner EnemySpawner;

    public enum Direction
    {
        Self,
        Opposite,
        Left,
        Right,
        Up,
        Down
    }

    public void Initialize(WeaponData weaponData, Player player, WeaponPool weaponPool, EnemySpawner enemySpawner)
    {
        this.weaponData = weaponData;
        _player = player;
        _weaponPool = weaponPool;
        _weaponSoundRef = weaponData.GetWeaponSoundRef();
        EnemySpawner = enemySpawner;
        
        weaponIcon = weaponData.GetSprite();
        attackPower = weaponData.GetAttackPower();
        attackSpeed = weaponData.GetAttackSpeed();
        inactiveDelay = weaponData.GetInactiveDelay();
        level = 1;
        additionalScale = 100f;
        AfterInitialization();
    }

    protected virtual void AfterInitialization()
    {
        
    }

    protected virtual void SpawnWeapon(Direction direction)
    {
        var weapon= _weaponPool.GetPlayerWeapon(weaponData);
        
        var weaponGo= weapon.gameObject;

        weaponGo.transform.localPosition = weaponData.GetBasePosition();
        weaponGo.transform.localScale = weaponData.GetBaseScale();

        switch (direction)
        {
            case Direction.Self:
                if (_player.PlayerMove.GetLookingLeft())
                {
                    weaponGo.transform.localPosition = new Vector3(-weaponGo.transform.localPosition.x, weaponGo.transform.localPosition.y, 0f);
                    weapon.FlipSpriteX(true);
                }
                else
                {
                    weapon.FlipSpriteX(false);
                }
                weapon.FlipSpriteY(false);
                break;

            case Direction.Opposite:
                if (!_player.PlayerMove.GetLookingLeft())
                {
                    weaponGo.transform.localPosition = new Vector3(-weaponGo.transform.localPosition.x, weaponGo.transform.localPosition.y - 1f, 0f);
                    weapon.FlipSpriteX(true);
                }
                else
                {
                    weaponGo.transform.localPosition = new Vector3(weaponGo.transform.localPosition.x, weaponGo.transform.localPosition.y - 1f, 0f);
                    weapon.FlipSpriteX(false);
                }
                weapon.FlipSpriteY(true);
                break;

            case Direction.Left:
                weaponGo.transform.localPosition = new Vector3(-weaponGo.transform.localPosition.x, weaponGo.transform.localPosition.y, 0f);
                weapon.FlipSpriteX(true);
                weapon.FlipSpriteY(false);
                break;

            case Direction.Right:
                weapon.FlipSpriteX(false);
                weapon.FlipSpriteY(false);
                break;

            case Direction.Up:
                weaponGo.transform.localPosition = new Vector3(0f, weaponGo.transform.localPosition.y + 3f, 0f);
                weapon.FlipSpriteX(false);
                weapon.FlipSpriteY(false);
                break;

            case Direction.Down:
                weaponGo.transform.localPosition = new Vector3(0f, weaponGo.transform.localPosition.y - 3.5f, 0f);
                weapon.FlipSpriteX(false);
                weapon.FlipSpriteY(false);
                break;
        }

        if (weaponData.GetParent().Equals(WeaponData.Parent.Self))
            weaponGo.transform.position += _player.GetPosition();

        
        weaponGo.transform.localScale = new Vector2(weaponData.GetBaseScale().x * (additionalScale / 100f), weaponData.GetBaseScale().y * (additionalScale / 100f));
        weapon.SetParameters(weaponData, finalAttackPower, direction, level);
        weapon.WeaponDeactivated += OnWeaponDeactivated;
        weaponGo.SetActive(true);
    }

    protected virtual void OnWeaponDeactivated(Weapon weapon)
    {
        weapon.RemoveAllListeners();
    }

    public WeaponData.WeaponType GetWeaponType()
    {
        return weaponData.GetWeaponType();
    }

    public WeaponData GetWeaponData()
    {
        return weaponData;
    }

    public int GetAttackPower()
    {
        return finalAttackPower;
    }

    public float GetAttackSpeed()
    {
        return finalAttackSpeed;
    }

    public int GetLevel()
    {
        return level;
    }

    public float GetInactiveDelay()
    {
        return inactiveDelay;
    }

    public float GetAdditionalScale()
    {
        return additionalScale;
    }

    public Sprite GetSprite()
    {
        return weaponIcon;
    }
    
    public void DecreaseInactiveDelay(float percent)
    {
        if (percent > 1f)
        {
            percent /= 100f;
        }
        inactiveDelay -= inactiveDelay*percent;
    }
    
    public void IncreaseAdditionalScale(float value)
    {
        additionalScale += value;
    }

    public void IncreaseAttackPower(int value)
    {
        attackPower += value;
    }

    public void DecreaseAttackSpeed(float value)
    {
        attackSpeed -= attackSpeed * value / 100f;
    }

    public void IncreaseLevel()
    {
        level++;
        LevelUp();
    }

    public void UpdateAttackSpeed()
    {
        finalAttackSpeed = attackSpeed * _player.GetAttackSpeed() / 100f;
        if (Math.Abs(lastAttackSpeed - finalAttackSpeed) > 0.01f)
        {
            lastAttackSpeed = finalAttackSpeed;
            waitAtackSpeed = new WaitForSeconds(attackSpeed);
        }
    }

    public void UpdateAttackPower()
    {
        finalAttackPower = attackPower * (int)(_player.GetAttackPower() / 100);
    }

    public void StartWeapon()
    {
        StartCoroutine(StartAttack());
    }

    public virtual void LevelUp()
    {
        switch (GetLevel())
        {
            case 3:
                IncreaseAttackPower(5);
                break;
            case 4:
                IncreaseAttackPower(5);
                IncreaseAdditionalScale(10f);
                break;
            case 5:
                DecreaseAttackSpeed(10f);
                break;
            case 6:
                IncreaseAttackPower(10);
                break;
        }
    }

    protected abstract IEnumerator StartAttack();
}
