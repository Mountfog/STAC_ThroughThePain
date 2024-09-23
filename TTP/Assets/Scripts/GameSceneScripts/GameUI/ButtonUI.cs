using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum ButtonType
{
    Attack,
    Jump,
    Rolling,
    Skill
}

public class ButtonUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Image img_CoverIcon = null;
    public Image img_Icon = null;
    Material mat = null;

    [Space(10.0f)]

    [Header("Info")]
    public ButtonType type = ButtonType.Attack;
    public bool useCooltime = false;

    public float pressDelay = 0.0f;

    private void Start()
    {
        img_CoverIcon.fillAmount = type == ButtonType.Skill ? 1 : 1;
        if (useCooltime) mat = img_CoverIcon.material;
    }

    public void Init(ButtonType type, bool useCooltime, float coolTime)
    {
        this.type = type;
        this.useCooltime = useCooltime;
        pressDelay = coolTime;
    }

    public void OnPressed()
    {
        StopAllCoroutines();

        if (useCooltime) StartCoroutine(Enum_ButtonCooltime());
        if(img_Icon != null) img_Icon.color = Color.clear;
    }



    IEnumerator Enum_ButtonCooltime()
    {
        float lerpTime = pressDelay;
        float curTime = 0.0f;

        img_CoverIcon.fillAmount = 0;

        while (curTime != lerpTime)
        {
            curTime = curTime > lerpTime ? lerpTime : curTime += Time.deltaTime;
            float t = curTime / lerpTime;

            img_CoverIcon.fillAmount = t;

            yield return null;
        }

        StartCoroutine(Enum_CoolEndEffect());
    }

    IEnumerator Enum_CoolEndEffect()
    {
        if (img_Icon != null) StartCoroutine(Enum_ShowIcon());

        float lerpTime = 0.8f;
        float curTime = 0.0f;

        while (curTime != lerpTime)
        {
            curTime = curTime > lerpTime ? lerpTime : curTime += Time.deltaTime;
            float t = curTime / lerpTime;

            mat.SetFloat("_Value", t);

            yield return null;
        }

        mat.SetFloat("_Value", 0);
    }

    IEnumerator Enum_ShowIcon()
    {
        yield return new WaitForSeconds(0.3f);

        float lerpTime = 0.2f;
        float curTime = 0.0f;

        while (curTime != lerpTime)
        {
            curTime = curTime > lerpTime ? lerpTime : curTime += Time.deltaTime;
            float t = curTime / lerpTime;
            t = 1 - Mathf.Pow(1 - t, 4);

            img_Icon.color = Color.Lerp(Color.clear, Color.white, t);

            yield return null;
        }
    }
}
