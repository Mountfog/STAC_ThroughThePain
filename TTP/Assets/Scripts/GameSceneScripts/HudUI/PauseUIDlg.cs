using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PauseUIDlg : MonoBehaviour
{
    public bool m_isPaused = false;
    public bool m_isOption = false;

    [Header("Pause Elements")]
    public GameObject m_PauseBG;
    public GameObject m_Pause;
    public GameObject m_btnGroupBG;
    public GameObject m_OptionBG;
    public Button m_btnOption;
    public Button m_btnRestart;
    public Button m_btnContinue;
    public Button m_btnExit;
    Tweener m_twAppear;

    [Space(20.0f)]

    [Header("Option")]
    public Slider sld_Sfx = null;
    public Slider sld_Bgm = null;
    public OptionToggle tglVib;
    public Button m_btnConfirm;



    private void Start()
    {
        m_btnOption.onClick.AddListener(OnClicked_Option);
        m_btnRestart.onClick.AddListener(OnClicked_Restart);
        m_btnContinue.onClick.AddListener(OnClicked_Continue);
        m_btnExit.onClick.AddListener(OnClicked_Exit);

        m_btnConfirm.onClick.AddListener(OnClicked_Confirm);

        sld_Sfx.onValueChanged.AddListener(OnValueChanged_VolumeSlider);
        sld_Bgm.onValueChanged.AddListener(OnValueChanged_VolumeSlider);

        sld_Sfx.value = GameMgr.Inst.gInfo.sfxVolume;
        sld_Bgm.value = GameMgr.Inst.gInfo.bgmVolume;
    }
    private void FixedUpdate()
    {
        if(Time.timeScale == 0)
        {
            InputSystem.Update();
        }
    }
    
    public void OnClicked_Option()
    {
        SetOption(true);
    }
    public void OnClicked_Continue()
    {
        Debug.Log("Wahta");
        SetPause(false);
    }
    public void OnClicked_Restart()
    {
        SceneMgr.Instance.LoadScene("GameScene");
    }
    public void OnClicked_Exit()
    {
        Debug.Log("Wahta");
        SceneMgr.Instance.LoadScene("TitleScene");
    }

    public void SetPause(bool bValue)
    {
        m_isPaused = bValue;
        SetOption(false);
        m_PauseBG.SetActive(bValue);
        if (m_isPaused)
        {
            Time.timeScale = 0;
            ResourceMgr.Instance.Blur(30, 0.5f);
            m_twAppear.Kill();
            m_twAppear = m_Pause.transform.DOLocalMoveY(0, 0.6f).From(-320).SetEase(Ease.OutBounce).SetUpdate(true);
            
        }
        else
        {
            Time.timeScale = 1;
            ResourceMgr.Instance.Blur(1, 0);
            m_twAppear.Kill();
        }
    }
    public void SetOption(bool bValue)
    {
        m_isOption = bValue;
        m_btnGroupBG.SetActive(!bValue);
        m_OptionBG.SetActive(bValue);
    }
    public void OnClicked_Confirm()
    {
        SetOption(false);
    }


    void OnValueChanged_VolumeSlider(float value)
    {
        GameMgr.Inst.gInfo.sfxVolume = sld_Sfx.value;
        GameMgr.Inst.gInfo.bgmVolume = sld_Bgm.value;

        AudioMgr.Instance.SetBgmVolume(GameMgr.Inst.gInfo.bgmVolume);
    }
}
