using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class DynamicScaleAdjuster : MonoBehaviour
{
    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale; // сохраняем исходный масштаб объекта

        Scale();
    }

    private void Update()
    {
        Scale();
    }

    private void Scale()
    {
        float widthRatio = (float)Screen.width / 1920f; // здесь 1920 - это ваше базовое разрешение по ширине
        float heightRatio = (float)Screen.height / 1080f; // здесь 1080 - это ваше базовое разрешение по высоте

        // меняем масштаб, используя среднее значение между отношениями ширины и высоты
        float scaleRatio = (widthRatio + heightRatio) / 2f;
        transform.localScale = originalScale * scaleRatio;
    }
}
