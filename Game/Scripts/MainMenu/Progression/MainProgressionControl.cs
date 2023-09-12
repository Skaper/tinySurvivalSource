using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainProgressionControl : MonoBehaviour
{
    public List<ProgressionUnitInfo> ProgressionUnits = new List<ProgressionUnitInfo>();

    private void Start()
    {
        int totalGameSessions = GameProgress.GetData().totalGameSessions;
        string[] progressionCompleted = GameProgress.GetData().progressionCompleted;
        foreach (var unit in ProgressionUnits)
        {
            if (progressionCompleted.Contains(unit.ProgressionUnit.UnitName))
            {
                if (unit.ProgressionUnit.ConditionMet(totalGameSessions))
                {
                    unit.ProgressionUnit.Activate();
                }
                else
                {
                    unit.ProgressionUnit.Deactivate();
                }
            }
            else
            {
                if (unit.ProgressionUnit.ConditionMet(totalGameSessions))
                {
                    unit.ProgressionUnit.ActivateWithAnimation();
                    GameProgress.GetData().progressionCompleted[unit.ProgressionUnit.Index] = unit.ProgressionUnit.UnitName;
                }
                else
                {
                    unit.ProgressionUnit.Deactivate();
                }
            }
            
            
            
        }
    }

    private void OnValidate()
    {
        foreach (var unit in ProgressionUnits)
        {
            unit.name = unit.ProgressionUnit.UnitName;
        }
    }
}
[System.Serializable]
public class ProgressionUnitInfo
{
    public string name;
    public ProgressionUnit ProgressionUnit;
}
