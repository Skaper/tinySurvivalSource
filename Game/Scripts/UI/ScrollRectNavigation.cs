using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectNavigation : MonoBehaviour
{
    [Range(0f, 1f)]
    public float ScrollDelta = 0.1f;
    
    public RectDimensionsChanged CanvasRect;
    public RectTransform[] NavigationOreder;

    public Button UpArrow;
    public Button DownArrow;
    
    public ScrollRect scrollRect;
    public RectTransform contentPanel;

    private int _currentIndexPosition;

    private IEnumerator Start()
    {
        UpArrow.onClick.AddListener(MoveUp);
        DownArrow.onClick.AddListener(MoveDown);
        
        UpArrow.gameObject.SetActive(false);
        DownArrow.gameObject.SetActive(true);
        
        CanvasRect.RectChanged += RectChanged; 
        yield return new WaitForEndOfFrame();
        
        
        
        if (AreAllActiveElementsFullyVisible())
        {
            DownArrow.gameObject.SetActive(false);
        }
        scrollRect.onValueChanged.AddListener(OnScrollRectValueChanged);
    }

    private void OnScrollRectValueChanged(Vector2 value)
    {
        var vertical = Mathf.Clamp(scrollRect.verticalNormalizedPosition, 0f, 1f);
        
        if (vertical > 0.9f)
        {
            UpArrow.gameObject.SetActive(false);
            DownArrow.gameObject.SetActive(true);
        }
        
        
        if (vertical < 0.1f)
        {
            UpArrow.gameObject.SetActive(true);
            DownArrow.gameObject.SetActive(false);
        }
    }

    private void RectChanged()
    {
        DownArrow.gameObject.SetActive(!AreAllActiveElementsFullyVisible());
    } 

    private void MoveUp()
    {
        scrollRect.verticalNormalizedPosition += ScrollDelta;
        scrollRect.verticalNormalizedPosition = Mathf.Clamp(scrollRect.verticalNormalizedPosition, 0f, 1f);
        if (scrollRect.verticalNormalizedPosition > 0.9f)
        {
            UpArrow.gameObject.SetActive(false);
            DownArrow.gameObject.SetActive(true);
        }
    }

    private void MoveDown()
    {
        scrollRect.verticalNormalizedPosition -= ScrollDelta;
        scrollRect.verticalNormalizedPosition = Mathf.Clamp(scrollRect.verticalNormalizedPosition, 0f, 1f);
        if (scrollRect.verticalNormalizedPosition < 0.1f)
        {
            UpArrow.gameObject.SetActive(true);
            DownArrow.gameObject.SetActive(false);
        }
    }
    
    bool AreAllActiveElementsFullyVisible()
    {
        RectTransform scrollRectTransform = scrollRect.GetComponent<RectTransform>();
        Rect scrollRectRect = scrollRectTransform.rect;

        foreach (RectTransform element in NavigationOreder)
        {
            // Skip inactive elements
            if (!element.gameObject.activeInHierarchy) continue;

            Vector3[] elementCorners = new Vector3[4];
            element.GetWorldCorners(elementCorners);

            bool isElementInsideScrollRect = true;
            for (int i = 0; i < 4; i++)
            {
                Vector3 localPoint = scrollRectTransform.InverseTransformPoint(elementCorners[i]);
                if (!scrollRectRect.Contains(localPoint))
                {
                    isElementInsideScrollRect = false;
                    break;
                }
            }

            if (!isElementInsideScrollRect)
            {
                return false;
            }
        }
        return true;
    
    }

    public void SnapTo(int index)
    {
        Canvas.ForceUpdateCanvases();

        // Calculate the position we want to scroll to
        float targetPosition = -NavigationOreder[index].anchoredPosition.y / scrollRect.content.sizeDelta.y;

        // Scroll to the target position
        scrollRect.verticalNormalizedPosition = targetPosition;
    }
}
