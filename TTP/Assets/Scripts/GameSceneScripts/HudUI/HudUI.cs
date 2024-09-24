using UnityEngine;
using UnityEngine.UI;

public class HudUI : MonoBehaviour
{
    public PauseUIDlg pauseDlg = null;
    public ResultUIDlg resultDlg = null;
    public UnitHealthDlg playerHealthDlg = null;
    public UnitHealthDlg playerPowerDlg = null;

    public PlayerInputActions _playerActions = null;

    private void Awake()
    {
        _playerActions = new PlayerInputActions();
        _playerActions.Game.Enable();
    }
    void Start()
    {
        pauseDlg.SetPause(false);
        playerPowerDlg.Init(0, 100);
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
}
