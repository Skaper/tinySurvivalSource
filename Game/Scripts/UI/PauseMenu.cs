using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject PauseWindow;
    public static bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !Player.instance.Level.GetIsLevelUpTime())
        {
            if (!isPaused)
                Pause();
            else
                Resume();
        }
    }

    public void Pause()
    {
        if (!Player.instance.Level.GetIsLevelUpTime())
        {
            PauseWindow.SetActive(true);
            PauseManager.instance.PauseGame();
            isPaused = true;
        }
    }

    public void Resume()
    {
        if (!Player.instance.Level.GetIsLevelUpTime())
        {
            PauseWindow.SetActive(false);
            PauseManager.instance.UnpauseGame();
            isPaused = false;
        }
    }
}