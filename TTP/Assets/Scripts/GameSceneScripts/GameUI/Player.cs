using PlayerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy;

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
        if (!GameMgr.Inst.gameScene.battleFSM.IsGameState()) return;
        if (!playerMoveMent.isRoll)
        {
            curHealth -= damage;
            unitHealthDlg.UpdateHealth(curHealth);
            if (curHealth <= 0)
            {
                curHealth = 0;
                OnDeath();
            }
            else
            {
                GetComponentInChildren<Animator>().SetTrigger("hittrig");
            }
            GameMgr.Inst.damageTextMgr.CreateDamageText(damage, this.transform, hitPoint);
            Camera.main.transform.GetComponent<CameraShake>().ShakeCamera();
            AudioMgr.Instance.LoadClip_SFX("playerHit");
        }
        else
        {
            GameMgr.Inst.damageTextMgr.CreateDodgeText("È¸ÇÇÇÔ",Color.red, this.transform, hitPoint);
            GameMgr.Inst.gameScene.hudUI.playerPowerDlg.SetHp(10);
            AudioMgr.Instance.LoadClip_SFX("Jump");
        }
    }
    public override void OnDeath()
    {
        GetComponentInChildren<Animator>().SetTrigger("deathtrig");
        playerMoveMent.OnDeath();
        AudioMgr.Instance.LoadClip_SFX("playerDie");
        float gameTime = GameMgr.Inst.gameScene.gameTime;
        GameMgr.Inst.gameScene.hudUI.resultDlg.Init(false, gameTime);
        GetComponent<CapsuleCollider2D>().enabled = false;
    }
    public override void Initialize(int khp, int kspeed, int atk)
    {
        unitHealthDlg.Init(khp);
        maxHealth = khp;
        curHealth = maxHealth;
        moveSpeed = kspeed;
        attack = khp;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fall"))
        {
            OnDeath();
        }
    }
    public void SetResultState()
    {
        //GetComponent<CapsuleCollider2D>().enabled = false;
    }
}
