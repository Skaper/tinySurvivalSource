using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class DebuggingGame : MonoBehaviour
{
    public TMP_InputField input;
    
    public void ResetProgress()
    {
        Debug.Log("ResetProgress");
        YandexGame.ResetSaveProgress();
        YandexGame.SaveProgress();
        SceneManager.LoadScene(0);
    }
    public void SaveProgress()
    {
        YandexGame.SaveProgress();
    }

    public void AddDiamonds()
    {
        GameProgress.GetData().AddDiamonds(int.Parse(input.text));
    }
    
    public void RemoveDiamonds()
    {
        GameProgress.GetData().SubtractDiamonds(int.Parse(input.text));
    }

}
