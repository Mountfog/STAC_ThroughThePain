using JetBrains.Annotations;
using System.Collections;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UnitHealthDlg : MonoBehaviour
{
    #region Parameters

    [Header("Info")]
    public int maxValue = 0;
    public int value = 0;

    [Space(10.0f)]

    [Header("UI Elements")]
    public GameObject m_backGround = null;
    public Slider sld_Main = null;
    public Slider sld_Sub = null;
    public Text txt_Value = null;
    [Space(10.0f)]

    [Header("Hp Color")]
    public Color highColor = new Color();
    public Color lowColor = new Color();


    public enum HpShowType
    {
        OnlyText = 0,
        WithBar = 1,
    }
    public HpShowType curType = HpShowType.WithBar;
    #endregion

    public void Init(int kvalue)
    {
        maxValue = kvalue;
        value = maxValue;
        if (curType == HpShowType.WithBar)
        {
            sld_Main.fillRect.gameObject.SetActive(true);
            sld_Sub.fillRect.gameObject.SetActive(true);
            m_backGround.SetActive(true);
            sld_Main.fillRect.GetComponent<Image>().color = Color.Lerp(lowColor, highColor, GetSldValue());
            if (sld_Main != null) sld_Main.value = GetSldValue();
            if (sld_Sub != null) sld_Sub.value = GetSldValue();
        }
        else
        {
            sld_Main.fillRect.gameObject.SetActive(false);
            sld_Sub.fillRect.gameObject.SetActive(false);
            m_backGround.SetActive(false);

        }
        ShowText(value, maxValue);
    }
    public void Init(int kvalue, int kmaxvalue)
    {
        maxValue = kmaxvalue;
        value = kvalue;
        if (curType == HpShowType.WithBar)
        {
            sld_Main.fillRect.gameObject.SetActive(true);
            sld_Sub.fillRect.gameObject.SetActive(true);
            if(m_backGround!=null) m_backGround.SetActive(true);
            sld_Main.fillRect.GetComponent<Image>().color = Color.Lerp(lowColor, highColor, GetSldValue());
            if (sld_Main != null) sld_Main.value = GetSldValue();
            if (sld_Sub != null) sld_Sub.value = GetSldValue();
        }
        else
        {
            sld_Main.fillRect.gameObject.SetActive(false);
            sld_Sub.fillRect.gameObject.SetActive(false);
            m_backGround.SetActive(false);

        }
        ShowText(value, maxValue);
    }
    public void ShowText(int value, int mvalue)
    {
        if(curType == HpShowType.WithBar)
        {
            if (txt_Value != null) txt_Value.text = $"{value}/{mvalue}";
        }
        else
        {
            if (txt_Value != null) txt_Value.text = $"{value}";
        }
    }

    public void UpdateHealth(int _value)
    {
        int prevHp = value;

        value = Mathf.Clamp( _value, 0, maxValue);

        int changedValue = value - prevHp;

        SetUI(changedValue >= 0);
    }

    public bool SetHp(int _value)
    {
        int prevHp = value;

        value = Mathf.Clamp(value + _value, 0, maxValue);

        int changedValue = value - prevHp;

        SetUI(changedValue >= 0);

        return value == 0;
    }

    void SetUI(bool isPositive)
    {
        ShowText(value, maxValue);
        if (sld_Main == null || sld_Sub == null) return;

        if(curType == HpShowType.WithBar)
        {
            StopAllCoroutines();
            if (isPositive) StartCoroutine(Enum_GetHeal());
            else StartCoroutine(Enum_GetDamage());
            StartCoroutine(Enum_SetColor());
        }
        else
        {
            txt_Value.transform.DOShakePosition(0.3f,10);
           
        }
    }
    
    float GetSldValue() => (float)value / maxValue;

    IEnumerator Enum_GetDamage()
    {
        float lerpTime = 0.5f;
        float curTime = 0.0f;

        if (sld_Main.value >= sld_Sub.value) sld_Sub.value = sld_Main.value;

        float sv = sld_Sub.value;
        float ev = GetSldValue();

        sld_Main.value = ev;

        yield return new WaitForSeconds(0.15f);

        while (curTime != lerpTime)
        {
            curTime = curTime > lerpTime ? lerpTime : curTime += Time.deltaTime;

            float t = 1 - Mathf.Pow(1 - (curTime / lerpTime), 4);

            sld_Sub.value = Mathf.Lerp(sv, ev, t);

            yield return null;
        }
    }
    IEnumerator Enum_GetHeal()
    {
        float lerpTime = 0.5f;
        float curTime = 0.0f;

        float sv = sld_Main.value;
        float ev = GetSldValue();

        float subValue = sld_Sub.value;

        while (curTime != lerpTime)
        {
            curTime = curTime > lerpTime ? lerpTime : curTime += Time.deltaTime;

            float t = 1 - Mathf.Pow(1 - (curTime / lerpTime), 4);

            sld_Main.value = Mathf.Lerp(sv, ev, t);
            sld_Sub.value = Mathf.Lerp(subValue, ev, t);

            yield return null;
        }
    }
    IEnumerator Enum_SetColor()
    {
        float lerpTime = 0.4f;
        float curTime = 0.0f;

        Image img = sld_Main.fillRect.GetComponent<Image>();
        Color sc = img.color;
        Color ec = Color.Lerp(lowColor, highColor, GetSldValue());

        while (curTime != lerpTime)
        {
            curTime = curTime > lerpTime ? lerpTime : curTime += Time.deltaTime;

            float t = 1 - Mathf.Pow(1 - curTime / lerpTime, 4);

            img.color = Color.Lerp(sc, ec, t);

            yield return null;
        }
    }
}
