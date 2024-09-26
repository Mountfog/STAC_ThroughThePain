using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public GameUI gameUI = null;
    public HudUI hudUI = null;
    public BattleFSM battleFSM = new BattleFSM();
    public float gameTime = 0f;
    //FSM
    //CSVParser
    // Start is called before the first frame update
    private void Awake()
    {
        GameMgr.Inst.gameScene = this;
        GameMgr.Inst.Initialize();
        AssetMgr.Inst.Initialize();
        gameTime = 0f;
        battleFSM.Initialize(Ready, Wave, Game, Result);
        battleFSM.SetReadyState();
    }
    private void Update()
    {
        gameTime += Time.deltaTime;
        if (battleFSM != null)
            battleFSM.OnUpdate();
    }

    public void Ready()
    {
        hudUI.SetReadyState();
    }
    public void Wave()
    {

    }
    public void Game()
    {

    }
    public void Result()
    {
        gameUI.SetResultState();
    }
    
}
