using UnityEngine;

public static class InputProvider
{
    private static Joystick _joystick;
    private static bool _isMobile;
    
    public static void SetJoystick(Joystick joystick)
    {
        _joystick = joystick;
        _isMobile = true;
    }
    
    public static Vector2 GetMoveVector()
    {
        if (_isMobile)
            return new Vector2(_joystick.Horizontal, _joystick.Vertical);
        
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
}
