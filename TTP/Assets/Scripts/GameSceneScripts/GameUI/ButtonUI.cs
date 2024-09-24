using System.Collections;
using System.Reflection.Emit;
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
    Collider2D coll = null;

    [Space(15.0f)]

    [Header("Info")]
    public ButtonType type = ButtonType.Attack;
    [Space(10.0f)]
    public bool isAble = true;
    public bool useCooltime = false;
    public float pressDelay = 0.0f;
    public UnitHealthDlg skillEnergy = null;

    bool ableSkill = false;
    bool fullEnergy = false;

    private void Update()
    {
        if (type == ButtonType.Skill)
        {
            img_CoverIcon.fillAmount = skillEnergy.GetSldValue();

            if (skillEnergy.GetSldValue() == 1 && !fullEnergy)
            {
                fullEnergy = true;
                ableSkill = true;

                StartCoroutine(Enum_CoolEnd());
            }

            coll.enabled = ableSkill;
        }
    }

    public void Init(ButtonType type, bool useCooltime, float coolTime)
    {
        this.type = type;
        this.useCooltime = useCooltime;
        pressDelay = coolTime;

        img_CoverIcon.fillAmount = 1f;
        mat = img_CoverIcon.material;
        coll = GetComponent<CircleCollider2D>();

        if(type == ButtonType.Skill)
        {
            img_CoverIcon.fillAmount = skillEnergy.GetSldValue();
            img_Icon.color = Color.clear;
            coll.enabled = false;
        }
    }

    public void OnPressed()
    {
        if(type == ButtonType.Skill && ableSkill)
        {
            skillEnergy.SetHp(-skillEnergy.maxValue);
            ableSkill = false;
            fullEnergy = false;
        }

        StopAllCoroutines();
        mat.SetFloat("_Value", 0);

        if (useCooltime) StartCoroutine(Enum_CoolDown());
        if (img_Icon != null) img_Icon.color = Color.clear;

    }

    IEnumerator Enum_CoolDown()
    {
        float coolTime = pressDelay;

        coll.enabled = false;

        StartCoroutine(Enum_ButtonCooltime(coolTime - 0.5f));
        yield return new WaitForSeconds(coolTime - 0.5f);
        StartCoroutine(Enum_CoolEndEffect());
        yield return new WaitForSeconds(0.5f);

        coll.enabled = true;
    }
    IEnumerator Enum_CoolEnd()
    {
        StartCoroutine(Enum_CoolEndEffect());
        yield return new WaitForSeconds(0.5f);

        coll.enabled = true;
    }

    IEnumerator Enum_ButtonCooltime(float time)
    {
        float lerpTime = time;
        float curTime = 0.0f;

        img_CoverIcon.fillAmount = 0;

        while (curTime != lerpTime)
        {
            curTime = curTime > lerpTime ? lerpTime : curTime += Time.deltaTime;
            float t = curTime / lerpTime;

            img_CoverIcon.fillAmount = t;

            yield return null;
        }
    } // 쿨타임 + 0.5초 = 진짜 쿨타임
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
    } // 0.8초 기준 0.55초 걸림
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
    }  // 얘도 0.5초 걸림
}
