using PlayerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    PlayerMovement playerMoveMent = null;
    void Awake()
    {
        playerMoveMent = GetComponent<PlayerMovement>();
    }
    public void LoadStage()
    {

    }
    public override void OnHit(Vector2 hitPoint, int damage)
    {
        AudioMgr.Instance.LoadClip_SFX("hit");
        throw new System.NotImplementedException();
    }
    public override void OnDeath()
    {
        AudioMgr.Instance.LoadClip_SFX("die");
        throw new System.NotImplementedException();
    }
    public override void Initialize(int khp, int kspeed, int atk)
    {
        throw new System.NotImplementedException();
    }
}
