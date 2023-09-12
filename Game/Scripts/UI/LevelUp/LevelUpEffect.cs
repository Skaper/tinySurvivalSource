using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem[] particles = new ParticleSystem[3];
    [SerializeField] Image levelUpBar;
    static bool isLevelUpTime;
    private WaitForSecondsRealtime wait01 = new(0.1f);
    public void Show()
    {
        //StartCoroutine(LevelUpEffects());
        Debug.Log("LevelUpEffect: particles is off");
    }
    
    private IEnumerator LevelUpEffects()
    {
        levelUpBar.gameObject.SetActive(true);
        StartParticles();

        foreach (ParticleSystem particle in particles)
        {
            particle.gameObject.SetActive(true);
            particle.Play();
        }

        while (true)
        {
            if (!isLevelUpTime) break;

            for (float i = 0f; i < 1f; i += 0.1f)
            {
                levelUpBar.color = Color.Lerp(new Color(1f, 0f, 1f), new Color(0f, 1f, 1f), i);
                yield return wait01;
                if (!isLevelUpTime) break;
            }

            for (float i = 0f; i < 1f; i += 0.1f)
            {
                levelUpBar.color = Color.Lerp(new Color(0f, 1f, 1f), new Color(1f, 0f, 1f), i);
                yield return wait01;
                if (!isLevelUpTime) break;
            }
        }

        levelUpBar.gameObject.SetActive(false);
        StopParticles();
    }
    
    void StartParticles()
    {
        foreach (ParticleSystem particle in particles)
        {
            particle.gameObject.SetActive(true);
            particle.Play();
        }
    }

    void StopParticles()
    {
        foreach (ParticleSystem particle in particles)
        {
            particle.Stop();
            particle.gameObject.SetActive(false);
        }
    }
}
