using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        Application.focusChanged += ApplicationOnfocusChanged;
        UnpauseGame();
    }
    
    private void ApplicationOnfocusChanged(bool focus)
    {
        if(AdProvider.IsAdActive())
            return;
        if (focus)
        {
            UnpauseGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
    }
}
