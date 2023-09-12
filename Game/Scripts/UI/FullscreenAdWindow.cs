using System;
using System.Collections;
using FMODUnity;
using TMPro;
using UnityEngine;

public class FullscreenAdWindow : MonoBehaviour
{
    public GameObject Context;
    public TextMeshProUGUI TimerLabel;
    public EventReference LevelUpSound;

    public event Action ReadyToLevelUpEvent; 
    private WaitForSecondsRealtime wait1 = new(1f);
    private void OnEnable()
    {
        Context.SetActive(false);
        AdProvider.CloseFullAdEvent += CloseAd;
    }

    private void OnDisable()
    {
        AdProvider.CloseFullAdEvent -= CloseAd;
    }

    public void ShowAd()
    {
        RuntimeManager.PlayOneShot(LevelUpSound);
        if (AdProvider.IsReadyFullscreenAd())
        {
            Context.SetActive(true);
            StartCoroutine(ShowAdWaitCoroutine());
        }
        else
        {
            CloseAd();
        }
        
    }
    private void CloseAd()
    {
        ReadyToLevelUpEvent?.Invoke();
    }

    private IEnumerator ShowAdWaitCoroutine()
    {
        for (int i = 3; i >= 1; i--)
        {
            TimerLabel.text = i.ToString();
            yield return wait1;
        }
        
        Context.SetActive(false);
        AdProvider.FullscreenShow();

        yield return null;
    }
}
