using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public GameUI gameUI = null;
    public HudUI hudUI = null;
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
    }
    private void Update()
    {
        gameTime += Time.deltaTime;
    }
}
