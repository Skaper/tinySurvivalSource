using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Timer : MonoBehaviour
{
    private TextMeshProUGUI timeText;
    private float timer;
    private WaitForSeconds wait1 = new(1f);
    void Awake()
    {
        timeText = GetComponent<TextMeshProUGUI>();
    }
    
    private void OnEnable()
    {
        StartCoroutine(CountTime());
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    private void DisplayTime(float timeToDisplay)
    {
        var minutes = Mathf.FloorToInt(timeToDisplay / 60);  
        var seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = $"{minutes:00}:{seconds:00}";
    }

    private IEnumerator CountTime()
    {
        while (gameObject.activeSelf)
        {
            yield return wait1;

            DisplayTime(timer);
        }
    }
}
