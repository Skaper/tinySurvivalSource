using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void ToStartScreen()
    {
        SceneManager.LoadScene("Start");
        PauseManager.instance.UnpauseGame();
        PauseMenu.isPaused = false;
    }

    public void ToGameScreen()
    {
        SceneManager.LoadScene("Game");
    }

    public void Pause()
    {
        PauseMenu.isPaused = true;
    }

    public void Resume()
    {
        PauseMenu.isPaused = false;
    }
}
