using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int maxHealth { get; set; }
    public int curHealth { get; set; }
    public float moveSpeed { get; set; }
    public int attack { get; set; }
    public UnitHealthDlg unitHealthDlg = null;
    public bool isAlive => curHealth > 0;
    public virtual void Initialize(int khealth, int kspeed, int kattack) 
    {
        unitHealthDlg = GetComponentInChildren<UnitHealthDlg>();
        unitHealthDlg.Init(khealth);
        maxHealth = khealth;
        curHealth = maxHealth;
        moveSpeed = kspeed;
        attack = kattack;
    }
    public virtual void OnHit(Vector2 hitPoint, int damage) 
    {
        curHealth -= damage;
        unitHealthDlg.UpdateHealth(curHealth);
        if (curHealth <= 0)
        {
            curHealth = 0;
            OnDeath();
        }
    }
    public virtual void OnDeath() { }
}
