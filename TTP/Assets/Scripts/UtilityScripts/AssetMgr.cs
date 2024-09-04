using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetStage { 
    public int id = 0;
    public int stageEnemys = 2;
    public int coins;
}
public class AssetMonster
{
    public string name;
    public int health ;
    public int attack;
    public int speed;
}
public class AssetMgr
{
    static AssetMgr _inst = null;
    public static AssetMgr Inst
    {
        get
        {
            if (_inst == null)
                _inst = new AssetMgr();

            return _inst;
        }
    }
    public bool IsInstalled { get; set; }
    public List<AssetStage> m_assetStages = new List<AssetStage>();
    public List<AssetMonster> m_assetMonsters = new List<AssetMonster>();
    private AssetMgr()
    {
        IsInstalled = false;
    }
    public void InitializeMonsters(string m_tabledata)
    {
        List<string[]> dataList = CSVParser.Load(m_tabledata);

        for (int i = 1; i < dataList.Count; i++)
        {
            string[] data = dataList[i];
            AssetMonster kmon = new AssetMonster();

            kmon.name = data[0];
            kmon.health = int.Parse(data[1]);
            kmon.attack = int.Parse(data[2]);
            kmon.speed = int.Parse(data[3]);

            m_assetMonsters.Add(kmon);
        }
    }
    public void Initialize()
    {
        //InitializeMonsters("TableData/item");
        IsInstalled = true;
    }
}

