using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public GameUI gameUI = null;
    public HudUI hudUI = null;
    //FSM
    //CSVParser
    // Start is called before the first frame update
    private void Awake()
    {
        GameMgr.Inst.gameScene = this;
        GameMgr.Inst.Initialize();
        AssetMgr.Inst.Initialize();
    }
    void Start()
    {
        Debug.Log(123);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
