using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class FMODInstancePlay : MonoBehaviour
{
    public EventReference Event;
    public bool PlayOnStart;
    public bool PlayShotToEnd;

    private EventInstance _eventInstance;

    private void Awake()
    {
        if (Event.IsNull == false)
        {
            Initialize(Event);
        }
    }

    public void Initialize(EventReference eventReference)
    {
        Event = eventReference;
        _eventInstance = RuntimeManager.CreateInstance(Event);
    }

    public void Play()
    {
        if((IsPlaying() == false && PlayShotToEnd) || PlayShotToEnd == false)
            _eventInstance.start();
    }

    public void Stop()
    {
        _eventInstance.stop(STOP_MODE.ALLOWFADEOUT);
    }

    public void Pause()
    {
        _eventInstance.setPaused(true);
    }

    private void Start()
    {
        if (PlayOnStart)
            Play();
    }

    private void OnDestroy()
    {
        _eventInstance.stop(STOP_MODE.IMMEDIATE);
        _eventInstance.release();
    }


    private bool IsPlaying() {
        _eventInstance.getPlaybackState(out PLAYBACK_STATE state);
        return state != PLAYBACK_STATE.STOPPED;
    }
}