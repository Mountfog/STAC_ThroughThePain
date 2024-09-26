using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextItem : MonoBehaviour
{
    public Text txt = null;
    AnimationCurve lerpAnim = null;

    public void Awake()
    {
        txt = GetComponent<Text>();
    }

    public void Init(int damage,Vector3 pos, AnimationCurve _lerpAnim)
    {
        lerpAnim = _lerpAnim;
        transform.localPosition = pos;
        txt.text = damage.ToString();
        if (damage > 50)
        {
            txt.text += "!!";
        }
        if (damage > 30)
        {
            txt.transform.parent.localScale += Vector3.one * 0.1f;
        }

        
        StartCoroutine(Enum_TextEffect());
    }
    public void Init(string ktext, Color kcolor, Vector3 pos, AnimationCurve _lerpAnim)
    {
        lerpAnim = _lerpAnim;
        transform.localPosition = pos;
        txt.text = ktext;
        txt.color = kcolor;

        StartCoroutine(Enum_TextEffect());
    }

    IEnumerator Enum_TextEffect()
    {
        float lerpTime = 1.35f;
        float curTime = 0.0f;

        Vector3 startPos = Vector3.zero;
        Vector3 endPos = startPos;

        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;

        float textSpeedX = 150f;
        float textSpeedY = 125f;
        float textPowerY = 125f;
        float xRdx = Random.Range(textSpeedX, textSpeedX * -1);
        float yRdx = Random.Range(textSpeedY, textSpeedY + textPowerY);
        endPos.x += xRdx;
        endPos.y += yRdx;

        while (curTime != lerpTime)
        {
            curTime = curTime > lerpTime ? lerpTime : curTime += Time.deltaTime;

            float t = curTime / lerpTime;
            float _t = lerpAnim.Evaluate(t);
            t = 1 - Mathf.Pow(1 - t, 4);

            transform.localScale = Vector3.Lerp(startScale, endScale, _t);
            transform.localPosition = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        Destroy(transform.parent.gameObject, 0.2f);
        
    }
}
