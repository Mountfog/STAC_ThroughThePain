using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UnitHpDlg : MonoBehaviour
{
    //유닛의 체력 표시 UI
    public GameObject m_fill = null;

    public void UpdateHealth(int curHP, int maxHP)
    {
        float percent = (float)curHP / maxHP;
        float x = Mathf.Lerp(-1.032f, 0, percent);
        Vector3 vec = m_fill.transform.localPosition;
        vec = new Vector3(x, vec.y, vec.z);
        m_fill.transform.localPosition = vec;
    }
}
