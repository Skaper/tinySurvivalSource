using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public enum ElementType
    {
        Toggle,
        Button
    }
    public bool IsToggled { get; private set; }
    public ElementType CurrentType { get; private set; } = ElementType.Button;
    
    public Color normalColor = Color.white;
    public Color toggledColor = Color.green;
    public Color unavailableColor = Color.gray;
    public Color clickedColor = Color.yellow;

    public UnityEvent OnToggleOn;
    public UnityEvent OnToggleOff;
    public UnityEvent OnClicked;

    [SerializeField]private CustomToggleGroup toggleGroup;

    public Image image;

    private void Awake()
    {
        if (image == null)
        {
            Debug.LogError("CustomToggle requires a SpriteRenderer component.");
            enabled = false;
            return;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (CurrentType == ElementType.Toggle)
        {
            Toggle();
        }
        else
        {
            Clicked();
            StartCoroutine(OnFinishClick());
        }
        
        
    }
    
    private IEnumerator OnFinishClick()
    {
        var fadeTime = 0.2f;
        var elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        image.color = normalColor;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if (CurrentType == ElementType.Button)
        {
            image.color = normalColor;
        }
    }

    public void SetType(ElementType targetType)
    {
        CurrentType = targetType;
        image.color = normalColor;
    }

    public void SetToggleGroup(CustomToggleGroup toggleGroup)
    {
        toggleGroup.AddToggle(this);
        this.toggleGroup = toggleGroup;
    }

    public void Toggle()
    {
        IsToggled = true;
        image.color = IsToggled ? toggledColor : normalColor;

        if (IsToggled)
        {
            OnToggleOn?.Invoke();
        }
        else
        {
            OnToggleOff?.Invoke();
        }

        if (toggleGroup != null)
        {
            toggleGroup.OnToggleValueChanged(this);
        }
    }

    public void ToggleSetState(bool state, bool updateGroup=false)
    {
        IsToggled = state;
        image.color = IsToggled ? toggledColor : normalColor;
        if (IsToggled)
        {
            OnToggleOn?.Invoke();
        }
        else
        {
            OnToggleOff?.Invoke();
        }
        
        if (toggleGroup != null && updateGroup)
        {
            toggleGroup.OnToggleValueChanged(this);
        }
    }

    public void Clicked()
    {
        image.color = clickedColor;
        OnClicked?.Invoke();
    }


    
}
