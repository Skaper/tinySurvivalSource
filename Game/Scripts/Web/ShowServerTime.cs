using TMPro;
using UnityEngine;

public class ShowServerTime : MonoBehaviour
{
    public TextMeshProUGUI label;
    
    
    public void UpdateLabel()
    {
        ServerTime.instance.RequestDate();
        ServerTime.instance.DateUpdatedEvent += date => label.text = date.ToString();
    }
}
