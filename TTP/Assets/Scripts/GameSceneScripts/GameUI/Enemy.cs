using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public Animator m_animator = null;
    public Collider2D _col = null;
    public Rigidbody2D _rigidBody = null;
    public float forceMult = 5f;
    private void Awake()
    {
        Initialize(30,2,3);
    }
    public override void Initialize(int khealth,int kspeed, int kdamage)
    {
        base.Initialize(khealth, kspeed, kdamage);
    }
    public override void OnHit(Vector2 hitPoint, int damage)
    {
        Debug.Log("Hit");
        Vector2 pos = transform.position;
        //³Ë¹é°ü·Ã
        //Vector2 forceDirection = hitPoint - pos;
        //Vector2 forceDirection = Vector2.up;
        //forceDirection.Normalize();
        //forceDirection *= forceMult;
        //_rigidBody.AddForce(forceDirection, ForceMode2D.Impulse);
        GetComponentInChildren<Animator>().SetTrigger("hittrig");
        base.OnHit(hitPoint, damage);
        GameMgr.Inst.damageTextMgr.CreateDamageText(damage,this.transform,hitPoint);
        Camera.main.transform.GetComponent<CameraShake>().ShakeCamera();
    }
    public override void OnDeath()
    {
        GetComponentInChildren<Animator>().SetTrigger("deathtrig");
        GameMgr.Inst.gameScene.gameUI.EnemyKilled(this);
        Destroy(gameObject, 1.2f);
    }
}
