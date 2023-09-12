using System.Collections.Generic;
using UnityEngine;

public class CustomToggleGroup : MonoBehaviour
{
    private List<ToggleButton> toggles = new List<ToggleButton>();

    public void AddToggle(ToggleButton toggle)
    {
        toggles.Add(toggle);
    }

    public void OnToggleValueChanged(ToggleButton toggle)
    {
        foreach (ToggleButton otherToggle in toggles)
        {
            if (otherToggle != toggle)
            {
                otherToggle.ToggleSetState(false);
            }
        }
    }

    public ToggleButton GetSelectedToggle()
    {
        foreach (ToggleButton toggle in toggles)
        {
            if (toggle.IsToggled)
            {
                return toggle;
            }
        }

        return toggles[0];
    }
}