using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BtnAction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Image m_image = null;
    public Image m_icon = null;
    public delegate void onBtnPress();
    public onBtnPress OnBtnPress;
    private void Awake()
    {
        
    }
    public void OnPointerDown(PointerEventData data)
    {
        m_image.rectTransform.anchoredPosition -= new Vector2(0, 3);
        m_icon.rectTransform.anchoredPosition -= new Vector2(0, 3);
    }
    public void OnPointerUp(PointerEventData data)
    {
        m_image.rectTransform.anchoredPosition += new Vector2(0, 3);
        m_icon.rectTransform.anchoredPosition += new Vector2(0, 3);
        if(OnBtnPress != null)
            OnBtnPress();
    }
    public void AddLinster(onBtnPress press)
    {
        OnBtnPress += press;
    }
    public void OnDestroy()
    {
        OnBtnPress = null;
    }
}
