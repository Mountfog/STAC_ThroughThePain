using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public Player player = null;
    public List<Enemy> enemyList = null;
    public GameObject monsterParent = null;
    public bool EnemyAllKilled => enemyList.Count == 0;

    // Start is called before the first frame update
    void Start()
    {
        LoadStage();
        player.Initialize(100, 0, 0);
    }
    public void LoadStage()
    {
        foreach(Transform child in monsterParent.transform)
        {
            enemyList.Add(child.GetComponent<Enemy>());
        }
    }

    public void SpawnEnemy()
    {


    }
    public void SetResultState()
    {
        player.SetResultState();
    }
    public void EnemyKilled(Enemy e)
    {
        enemyList.Remove(e);
        if(enemyList.Count == 0)
        {
            GameMgr.Inst.gameScene.hudUI.resultDlg.Init(true,GameMgr.Inst.gameScene.gameTime);
            AudioMgr.Instance.LoadClip_SFX("win");
        }
    }
}
