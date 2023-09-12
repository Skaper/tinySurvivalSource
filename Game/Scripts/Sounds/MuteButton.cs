using System;
using UnityEngine;

[RequireComponent(typeof(ButtonSwitch))]
public class MuteButton : MonoBehaviour
{
    private ButtonSwitch _switch;

    private void Awake()
    {
        _switch = GetComponent<ButtonSwitch>();
    }

    private void Start()
    {
        _switch.SetState(!FMODVolumeControl.Instance.IsMuted());
    }

    void OnEnable()
    {
        if(FMODVolumeControl.Instance)
            _switch.SetState(!FMODVolumeControl.Instance.IsMuted());
    }

    public void Mute()
    {
        FMODVolumeControl.Instance.MuteGame();
    }

    public void UnMute()
    {
        FMODVolumeControl.Instance.UnMuteGame();
    }
}
