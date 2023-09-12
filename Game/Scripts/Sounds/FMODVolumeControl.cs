using FMOD.Studio;
using UnityEngine;

public class FMODVolumeControl : MonoBehaviour
{
    public static FMODVolumeControl Instance;
    [SerializeField] private string MasterBusRef = "bus:/";
    [SerializeField] private string BackgroundBusRef = "bus:/Game/BackgroundMusic";
    private Bus _masterBus;
    private Bus _backgroundMusicBus;
    private float _masterBusVolume;
    [SerializeField] private bool _isGameMutedByUser;
    private bool _isMutedNow;

    private void Awake()
    {
        PauseManager.instance.UnpauseGame();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(this);
        _masterBus = FMODUnity.RuntimeManager.GetBus(MasterBusRef);
        _backgroundMusicBus = FMODUnity.RuntimeManager.GetBus(BackgroundBusRef);
        _masterBus.getVolume(out _masterBusVolume);

        Application.focusChanged += ApplicationOnfocusChanged;
    }

    private void CheckForFocus(bool focus)
    {
        if(AdProvider.IsAdActive())
            return;
        if (focus == false && _isGameMutedByUser == false)
        {
            _masterBus.setVolume(0f);
        }
        else if (focus && _isGameMutedByUser == false && _isMutedNow == false)
        {
            _masterBus.setVolume(_masterBusVolume);
        }
    }

    private void ApplicationOnfocusChanged(bool focus)
    {
        CheckForFocus(focus);
    }

    public bool IsMuted()
    {
        return _isGameMutedByUser;
    }

    public void MuteGame()
    {
        MuteAll(true);
        _isGameMutedByUser = true;
    }

    public void UnMuteGame()
    {
        _isGameMutedByUser = false;
        MuteAll(false);
    }

    public void MuteAll(bool isMuted)
    {
        if (_isGameMutedByUser)
        {
            return;
        }

        _isMutedNow = isMuted;
        if (isMuted)
        {
            _masterBus.setVolume(0f);
        }
        else
        {
            _masterBus.setVolume(_masterBusVolume);
        }
    }

    public void MuteAllBeforeAd()
    {
        _masterBus.setVolume(0f);
    }

    public void UnmuteAllAfterAd()
    {
        _masterBus.setVolume(_masterBusVolume);
    }

    public void SetBackgroundMusicVolume(float volume)
    {
        _backgroundMusicBus.setVolume(volume);
    }

    public void SetMasterVolume(float volume)
    {
        _masterBusVolume = volume;
        _masterBus.setVolume(_masterBusVolume);
    }
}
