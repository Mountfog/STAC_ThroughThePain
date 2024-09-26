using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

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
    public Text m_stageText = null;
    public Text m_monsterText = null;

    public void SetReadyState()
    {
        m_monsterText.text = "남은 적 : " + GameMgr.Inst.gameScene.gameUI.enemyList.Count;
        StageNum().Play();
    }
    Sequence StageNum()
    {
        Sequence seq = DOTween.Sequence().SetUpdate(true);
        seq.AppendCallback(() => { Time.timeScale = 0f; });
        seq.AppendInterval(1.0f);
        seq.AppendCallback(()=> { m_stageText.rectTransform.DOAnchorPosY(-130f, 1f).SetRelative().SetEase(Ease.OutBack).SetUpdate(true); ; });
        seq.AppendCallback(() => { m_monsterText.rectTransform.DOAnchorPosY(-130f, 1f).SetRelative().SetEase(Ease.OutBack).SetUpdate(true); ; });
        seq.AppendInterval(1.0f);
        seq.AppendCallback(() => { m_stageText.rectTransform.DOAnchorPosY(130f, 1f).SetRelative().SetEase(Ease.OutBack).SetUpdate(true); ; });
        seq.AppendCallback(() => { m_monsterText.rectTransform.DOAnchorPosY(130f, 1f).SetRelative().SetEase(Ease.OutBack).SetUpdate(true); ; });
        seq.AppendInterval(0.5f);
        seq.AppendCallback(() => { Time.timeScale = 1f; });
        return seq;
    }
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

        //StartCoroutine(Enum_SkillTest());
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
