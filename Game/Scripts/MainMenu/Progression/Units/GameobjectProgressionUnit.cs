using UnityEngine;

public class GameobjectProgressionUnit : ProgressionUnit
{
    public GameObject targetObject;
    
    public override void Activate()
    {
        targetObject.SetActive(true);
    }

    public override void ActivateWithAnimation()
    {
        //TODO PLAY OPEN ANIMATION
        Activate();
    }

    public override void Deactivate()
    {
        targetObject.SetActive(false);
    }
    
    
}
