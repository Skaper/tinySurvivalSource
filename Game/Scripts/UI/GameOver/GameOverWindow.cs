using FMODUnity;
using UnityEngine;

public class GameOverWindow : MonoBehaviour
{
    public GameObject Context;
    public EventReference GameOverSound;
    private Player _player;
    
    public void Initialize(Player player)
    {
        _player = player;
    }

    private void Awake()
    {
        Context.SetActive(false);
    }

    public void Show()
    {
        RuntimeManager.PlayOneShot(GameOverSound);
        Context.SetActive(true);
    }

    public void RespawnPlayer()
    {
        //TODO PLAYER RESPAWN
        
    }
}
