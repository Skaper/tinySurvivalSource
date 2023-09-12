using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSwitch : MonoBehaviour
{
    private Button _button;
    [SerializeField]private bool state;

    public UnityEvent StateOn;
    public UnityEvent StateOff;
    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(SwitchState);
    }

    private void SwitchState()
    {
        state = !state;
        if(state)
            StateOn?.Invoke();
        else
            StateOff.Invoke();
    }

    public void SetState(bool st)
    {
        state = st;
        if(state)
            StateOn?.Invoke();
        else
            StateOff.Invoke();
    }
}
