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
        AudioMgr.Instance.LoadClip_BGM("gameBgm");
    }
    private void Start()
    {
    }
    public void LoadStage()
    {

    }
    public override void OnHit(Vector2 hitPoint, int damage)
    {
        AudioMgr.Instance.LoadClip_SFX("playerHit");
        throw new System.NotImplementedException();
    }
    public override void OnDeath()
    {
        AudioMgr.Instance.LoadClip_SFX("playerDie");
        throw new System.NotImplementedException();
    }
    public override void Initialize(int khp, int kspeed, int atk)
    {
        throw new System.NotImplementedException();
    }
}
