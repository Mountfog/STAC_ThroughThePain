using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public Player player = null;
    public List<Enemy> enemyList = null;

    public bool EnemyAllKilled => enemyList.Count == 0;

    // Start is called before the first frame update
    void Start()
    {
        LoadStage();
    }
    public void LoadStage()
    {
        
    }

    public void SpawnEnemy()
    {


    }
    public void EnemyKilled(Enemy e)
    {
        enemyList.Remove(e);
    }
}
