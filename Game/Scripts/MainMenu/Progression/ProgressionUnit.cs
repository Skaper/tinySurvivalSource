using UnityEngine;

public class ProgressionUnit : MonoBehaviour
{
    public string UnitName;
    public int Index;
    public int SessionsTarget;
    public ConditionTypes ConditionType;
    public virtual bool ConditionMet(int sessionsCount)
    {
        switch (ConditionType)
        {
            case ConditionTypes.MoreEqual:
                return sessionsCount >= SessionsTarget;
            case ConditionTypes.LessEqual:
                return sessionsCount <= SessionsTarget;
            case ConditionTypes.Less:
                return sessionsCount < SessionsTarget;
            case ConditionTypes.More:
                return sessionsCount > SessionsTarget;
            case ConditionTypes.Equal:
                return sessionsCount == SessionsTarget;
            default:
                return false;
        }
    }
    public virtual void Activate()
    {
        
    }

    public virtual void ActivateWithAnimation()
    {
        
    }
    
    public virtual void Deactivate()
    {
        
    }
    
    public enum ConditionTypes
    {
        MoreEqual, 
        LessEqual, 
        Less,
        More,
        Equal
    }
}
