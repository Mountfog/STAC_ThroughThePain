using UnityEngine;
using UnityEngine.UI;

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

    // Start is called before the first frame update
    void Start()
    {
                pauseDlg.SetPause(false);
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
