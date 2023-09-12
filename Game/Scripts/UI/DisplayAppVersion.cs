using System;
using TMPro;
using UnityEngine;

public class DisplayAppVersion : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    void Start()
    {
        label.text = Application.version;
    }
}
