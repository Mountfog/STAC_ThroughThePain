using System.Collections;
using UnityEngine;

public class HudUI : MonoBehaviour
{
    public PauseUIDlg pauseDlg = null;
    public ResultUIDlg resultDlg = null;
    public CanvasGroup inGameUIAlpha = null;
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

    public void HideInGameUI() { StartCoroutine(Enum_HideInGameUI()); } // resultUI에서 처리하게 해둿음 Fsm에서 하도록 바꾸셈
    IEnumerator Enum_HideInGameUI()
    {
        float lerpTime = 1.25f;
        float curTime = 0.0f;

        while(curTime != lerpTime)
        {
            curTime = curTime > lerpTime ? lerpTime : curTime += Time.deltaTime;
            float t = curTime / lerpTime;
            t = 1 - Mathf.Pow(1 - t, 4);

            inGameUIAlpha.alpha = Mathf.Lerp(1, 0, t);

            yield return null;
        }
    }
}
