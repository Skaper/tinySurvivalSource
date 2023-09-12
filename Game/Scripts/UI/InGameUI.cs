using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{

    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider expSlider;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image levelUpBar;
    [SerializeField] private LevelUpEffect _levelUpEffect;
    [SerializeField] private LevelUpWindow _levelUpWindow;
    [SerializeField] private InventoryWindow _inventoryWindow;
    [SerializeField] private FullscreenAdWindow _fullscreenAdWindow;
    [SerializeField] private GameOverWindow _gameOverWindow;
    [SerializeField] private FMODInstancePlay _backgroundMusic;
    [SerializeField] private TextMeshProUGUI KillCountLabel;
    private Player _player;
    private EnemySpawner _enemySpawner;


    public void Initialize(Player player, EnemySpawner enemySpawner)
    {
        _player = player;
        _enemySpawner = enemySpawner;
        var canvas = GetComponent<Canvas>();
        canvas.worldCamera = _player.MainCamera;
        _inventoryWindow.Initialize(player);
        _levelUpWindow.Initialize(player);
        _gameOverWindow.Initialize(player);
        SetupHPSlider();
        SetupExpSlider();
        
        _enemySpawner.KillCountChangedEvent += OnKillCountChangedEvent;
        _player.PlayerDead += OnPlayerDeath;
        _player.Inventory.InventoryChanged += OnInventoryChanged;
        _fullscreenAdWindow.ReadyToLevelUpEvent += FullscreenAdWindowOnReadyToLevelUpEvent;

    }

    private void OnKillCountChangedEvent(int value)
    {
        KillCountLabel.text = value.ToString();
    }

    private void OnPlayerDeath()
    {
        _backgroundMusic.Pause();
        _gameOverWindow.Show();
    }
    

    private void SetupHPSlider()
    {
        hpSlider.maxValue = _player.GetHealthPoint();
        hpSlider.value = _player.GetHealthPoint();
        _player.HealthChanged += PlayerHealthChanged;
    }
    
    private void PlayerHealthChanged(float value)
    {
        hpSlider.value = value;
        if(_player.isDead)
            hpSlider.gameObject.SetActive(false);
    }

    
    private void SetupExpSlider()
    {
        expSlider.maxValue = _player.Level.GetMaxExp();
        expSlider.value = 0;
        _player.Level.ExpChanged += PlayerLevelExpChanged;
        _player.Level.LevelChanged += PlayerLevelChanged;
    }

    private void PlayerLevelChanged(int level)
    {
        levelText.text = level.ToString();
        PauseManager.instance?.PauseGame();
        _fullscreenAdWindow.ShowAd();
    }
    
    private void FullscreenAdWindowOnReadyToLevelUpEvent()
    {
        PauseManager.instance?.UnpauseGame();
        _levelUpEffect.Show();
        _levelUpWindow.Show();
    }

    private void PlayerLevelExpChanged(int value, int maxValue)
    {
        expSlider.maxValue = maxValue;
        expSlider.value = value;
    }

    private void OnInventoryChanged()
    {
        _inventoryWindow.UpdateView();
    }
    
    
}
