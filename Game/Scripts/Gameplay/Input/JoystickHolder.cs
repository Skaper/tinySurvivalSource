using UnityEngine;

public class JoystickHolder : MonoBehaviour
{
    [SerializeField] private Joystick _joystick;

    private void Awake()
    {
        if (DeviceInfo.IsMobileBrowser())
        {
            _joystick.gameObject.SetActive(true);
            InputProvider.SetJoystick(_joystick);
        }
        else
        {
            _joystick.gameObject.SetActive(false);
        }
    }
}
