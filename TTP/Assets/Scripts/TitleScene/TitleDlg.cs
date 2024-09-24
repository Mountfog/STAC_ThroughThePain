using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleDlg : MonoBehaviour
{
    [SerializeField] Button m_btnStart = null;
    private void Start()
    {
        AudioMgr.Instance.LoadClip_BGM("titleBgm");
        m_btnStart.onClick.AddListener(OnClicked_Start);
    }
    
    void OnClicked_Start()
    {
        AudioMgr.Instance.LoadClip_SFX("startButton");
        SceneMgr.Instance.LoadScene("GameScene");
    }
}
