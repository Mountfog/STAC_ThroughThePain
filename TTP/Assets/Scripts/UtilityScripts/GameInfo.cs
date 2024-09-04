using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo
{
    private int coins = 0;
    public int Coins => coins;
    public List<PlayerUpgrade> upgrades = new List<PlayerUpgrade>();
    public void AddUpgrade(PlayerUpgrade playerupgrade)
    {
        upgrades.Add(playerupgrade);
    }
}
public class PlayerUpgrade
{
    string name; 
    string desc;
    string upgradeType;

}
