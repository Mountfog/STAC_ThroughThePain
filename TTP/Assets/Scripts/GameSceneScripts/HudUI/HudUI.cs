using System.Collections;
using UnityEngine;

public class HudUI : MonoBehaviour
{
    public PauseUIDlg pauseDlg = null;
    public ResultUIDlg resultDlg = null;
    public UnitHealthDlg playerHealthDlg = null;
    public UnitHealthDlg playerPowerDlg = null;

    public PlayerInputActions _playerActions = null;



    [Space(20.0f)]
    public ButtonUI btn_Attack = null;
    public ButtonUI btn_Rolling = null;
    public ButtonUI btn_Skill = null;

    private void Awake()
    {
        _playerActions = new PlayerInputActions();
        _playerActions.Game.Enable();
    }

    void Start()
    {
        pauseDlg.SetPause(false);
        playerHealthDlg.Init(100, 100);
        playerPowerDlg.Init(0, 100);

        btn_Attack.Init(ButtonType.Attack, true, 0.7f);
        btn_Rolling.Init(ButtonType.Rolling, true, 0.7f);
        btn_Skill.Init(ButtonType.Skill, false, 0.0f);

        StartCoroutine(Enum_SkillTest());
    }

    void FixedUpdate()
    {
        if (_playerActions.Game.Pause.WasPerformedThisFrame())
        {
            if (pauseDlg.m_isPaused)
            {
                if (pauseDlg.m_isOption)
                {
                    pauseDlg.SetOption(false);
                }
                else
                    pauseDlg.SetPause(false);
            }
            else
                pauseDlg.SetPause(true);
        }
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
