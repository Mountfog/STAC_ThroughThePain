using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudUI : MonoBehaviour
{
    public ActionDlg actionDlg = null;
    public UnitHealthDlg playerHealthDlg = null;
    public UnitHealthDlg playerPowerDlg = null;
    [Space(20.0f)]
    public ButtonUI btn_Attack = null;
    public ButtonUI btn_Rolling = null;
    public ButtonUI btn_Skill = null;



    // Start is called before the first frame update
    void Start()
    {
        playerHealthDlg.Init(100, 100);
        playerPowerDlg.Init(0, 100);

        btn_Attack.Init(ButtonType.Attack, true, 0.75f);
        btn_Rolling.Init(ButtonType.Rolling, true, 1.25f);
        btn_Skill.Init(ButtonType.Skill, false, 0.0f);

        StartCoroutine(Enum_SkillTest());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Enum_SkillTest()
    {
        while (true)
        {
            playerPowerDlg.SetHp(5);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
