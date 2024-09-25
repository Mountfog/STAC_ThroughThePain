using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageTextMgr : MonoBehaviour
{
    public AnimationCurve lerpAnim = null;
    [Space(10.0f)]
    public GameObject damageTextPrefab = null;
    public Transform m_parent = null;
    public Vector3 offset = Vector3.zero;
    
    private void Awake()
    {   
        GameMgr.Inst.damageTextMgr = this;
    }
    

    public void CreateDamageText(int damage,Transform _enemy, Vector3 pos)
    {
        
        GameObject go = Instantiate(damageTextPrefab, m_parent);
        go.GetComponentInChildren<DamageTextItem>().Init(damage, pos + offset, lerpAnim);
        go.GetComponent<GUIPosition>().enemyTr = _enemy;
    }
    public void CreateDodgeText(string ktext, Color kcolor, Transform _enemy, Vector3 pos)
    {

        GameObject go = Instantiate(damageTextPrefab, m_parent);
        go.GetComponentInChildren<DamageTextItem>().Init(ktext, kcolor, pos + offset, lerpAnim);
        go.GetComponent<GUIPosition>().enemyTr = _enemy;
    }
}
