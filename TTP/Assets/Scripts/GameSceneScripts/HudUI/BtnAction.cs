using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BtnAction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Image[] img_Elements = null;
    public delegate void onBtnPress();
    public onBtnPress OnBtnPress;
    public ButtonUI btnUI = null;

    public void OnPointerDown(PointerEventData data)
    {
        foreach (Image item in img_Elements)
            item.rectTransform.anchoredPosition -= new Vector2(0, 3);
    }
    public void OnPointerUp(PointerEventData data)
    {
        foreach (Image item in img_Elements)
            item.rectTransform.anchoredPosition += new Vector2(0, 3);

        if (OnBtnPress != null)
            OnBtnPress();

        btnUI?.OnPressed();  
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
