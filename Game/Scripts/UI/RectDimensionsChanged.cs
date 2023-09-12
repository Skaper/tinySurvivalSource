using System;
using UnityEngine;

public class RectDimensionsChanged : MonoBehaviour
{
    public Action RectChanged;
    private void OnRectTransformDimensionsChange()
    {
        RectChanged?.Invoke();
    }
}
