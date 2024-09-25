using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;

public class BtnPauseMenu : OnScreenControl, IPointerDownHandler
{
    [SerializeField] EventTrigger.Entry eventTrigger;
    public delegate void onBtnPress();
    public onBtnPress OnBtnPress;
    
    public void OnPointerDown(PointerEventData data) => eventTrigger.callback.Invoke(data);

    [InputControl(layout = "Button")]
    [SerializeField]
    private string m_ControlPath;

    protected override string controlPathInternal
    {
        get => m_ControlPath;
        set => m_ControlPath = value;
    }
}
