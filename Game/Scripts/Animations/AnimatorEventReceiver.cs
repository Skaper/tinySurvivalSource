using UnityEngine;
using UnityEngine.Events;

public class AnimatorEventReceiver : MonoBehaviour
{
    public UnityEvent AnimationEnded;

    public void OnAnimationEnd() 
    {
        AnimationEnded?.Invoke();
    }
}
