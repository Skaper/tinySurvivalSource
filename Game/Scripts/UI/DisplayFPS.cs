using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DisplayFPS : MonoBehaviour
{
    [Header("Secret buttons for showing fps")]
    public Button button1;
    public Button button2;
    public UnityEvent OnSecretKeyPressed;
    public UnityEvent OnSecretKeyRealize;
    private float fps;
    private float fpsSum;
    private int counter;
    private float lastValue;
    [SerializeField] public TextMeshProUGUI Label;
    private WaitForSeconds wait01 = new(0.1f);
    private bool isShow;
    private bool isButton1Pressed = false;
    private IEnumerator Start()
    {
        button1?.onClick.AddListener(OnButton1Clicked);
        button2?.onClick.AddListener(OnButton2Clicked);
        Label.gameObject.SetActive(false);
        while (true)
        {
            fpsSum += 1f / Time.unscaledDeltaTime;

            if (counter == 10)
            {
                fps = fpsSum / 10;
                counter = 0;
                fpsSum = 0;
                if (Mathf.Abs(fps - lastValue) > 0.1f)
                {
                    fps = Mathf.Round(fps);
                    if(isShow)
                        Label.text = fps.ToString();
                    lastValue = fps;
                }
            }
            counter++;
            

            yield return wait01;

        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.M))
        {
            isShow = !isShow;
            Label.gameObject.SetActive(isShow);
        }
    }
    
    private void OnButton1Clicked()
    {
        isButton1Pressed = true;
    }

    private void OnButton2Clicked()
    {
        if (isButton1Pressed)
        {
            isShow = !isShow;
            Label.gameObject.SetActive(isShow);
            isButton1Pressed = false; // сброс после успешной реакции
            OnSecretKeyPressed?.Invoke();
        }
        else
        {
            isButton1Pressed = false; // обеспечивает сброс, если Button2 нажата первой
            OnSecretKeyRealize?.Invoke();
        }
    }
}
