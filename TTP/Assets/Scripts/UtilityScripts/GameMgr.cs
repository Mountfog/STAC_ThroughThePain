using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr
{
    private static GameMgr instance;
    public static GameMgr Inst
    {
        get
        {
            if (instance == null)
                instance = new GameMgr();
            return instance;
        }
    }

    public GameScene gameScene = null;
    public GameInfo gInfo = new GameInfo();
    public DamageTextMgr damageTextMgr = null;
    public void Initialize()
    {
        Application.targetFrameRate = 60;
    }

}
