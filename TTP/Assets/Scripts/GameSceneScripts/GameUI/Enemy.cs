using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class Enemy : Unit
{
    public Animator m_animator = null;
    public SpriteRenderer _sr = null;
    public CapsuleCollider2D _col = null;
    public Rigidbody2D _rigidBody = null;
    public float forceMult = 5f;
    public Vector2 pos1;
    public Vector2 pos2;
    public Transform player;
    private Vector2 _frameVelocity = Vector2.zero;
    [SerializeField] private bool _grounded = true;
    public LayerMask mask;
    [SerializeField] private GameObject currentOneWayPlatform;
    public EnemyState curEnemyState = EnemyState.Stop;

    private float attackTime = 0f;
    private bool isAttacked = false;
    private bool attackPerOnce = false;
    private bool attackInCoolDown = false;

    [Header("Attack attributes")]
    public float findDist;
    public float speed = 10f;
    public float attackDelay = 0.6f;
    public float attackAfter = 0.5f;
    public Vector2 offSet = Vector2.zero;
    public Vector2 HitboxSize = Vector2.one;
    public int enemyAttackValue = 10;
    public float enemyAttackSpeed = 1.5f;
    public float attackDist = 3f;
    public float notAnymore = 3f;
    public float notAnymoreScale = 0.8f;
    public int enemyHealth = 50;

    public enum EnemyState
    {
        Stop = 0,
        Follow = 1,
        Attack = 2,
        AttackEnd = 3,
        Hit = 4,
        Death = 5,
    }

    private void Awake()
    {
        Initialize(enemyHealth, 2, 3);
    }
    private void FixedUpdate()
    {
        CheckCollisions();
        HandleMove();
        HandleGravity();
        ApplyMovement();
    }
    private void HandleMove()
    {
        if (curEnemyState == EnemyState.Stop)
        {
            if (Vector2.Distance(transform.position, player.position) <= findDist)
            {
                curEnemyState = EnemyState.Follow;
                m_animator.SetBool("isMove", true);
            }
            //Move();
        }
        else if(curEnemyState == EnemyState.Follow)
        {
            float notTooClose = 1f;
            if (Vector2.Distance(transform.position, player.position) < notAnymore)
            {
                notTooClose = notAnymoreScale;
            }
            _frameVelocity.x = ((transform.position.x - player.position.x < 0f) ? 1f : -1f) * speed * notTooClose;
            _sr.flipX = ((transform.position.x - player.position.x < 0f));

            if (Mathf.Abs(transform.position.x - player.position.x) <= attackDist)
            {
                if (!_grounded) return;
                float jumpFallValue = 1f;
                float heightDiff = (transform.position.y - player.position.y);
                if (Mathf.Abs(heightDiff) < jumpFallValue)
                {
                    if (attackInCoolDown) return;
                    m_animator.SetBool("isMove", false);
                    curEnemyState = EnemyState.Attack;
                    attackTime = 0f;
                    isAttacked = false;
                    attackPerOnce = false;
                    attackInCoolDown = true;
                    Invoke(nameof(AttackCool),1.5f);
                    _frameVelocity.x = 0f;
                }
                else if (heightDiff <= jumpFallValue)
                {
                    _frameVelocity.y = 22f;
                    _grounded = false;
                }
                else if (heightDiff > jumpFallValue)
                {
                    if (currentOneWayPlatform == null) return;
                    CompositeCollider2D tc = currentOneWayPlatform.GetComponent<CompositeCollider2D>();
                    Physics2D.IgnoreCollision(tc, _col);
                    _grounded = false;

                }
            }
        }
        else if(curEnemyState == EnemyState.Attack)
        {
            attackTime += Time.deltaTime;
            if(attackTime >= attackDelay && !isAttacked)
            {
                isAttacked =true;
                m_animator.SetTrigger("attacktrig");
                _frameVelocity.x = (_sr.flipX ? 1f : -1f) * speed * enemyAttackSpeed;
            }
            else if (isAttacked && !attackPerOnce)
            {
                Vector2 localPos = (Vector2)transform.position;
                Vector2 hitPoint = localPos + new Vector2(offSet.x * (_sr.flipX ? 1f : -1f), offSet.y) ;
                LayerMask layerMask = LayerMask.GetMask("Unit");
                Collider2D[] player = Physics2D.OverlapBoxAll(hitPoint, HitboxSize, 0f, layerMask);
                if (player.Length == 0) return;
                for (int i = 0; i < player.Length; i++)
                {
                    if (player[i].CompareTag("Player"))
                    {
                        player[i].GetComponent<Unit>().OnHit(hitPoint, enemyAttackValue);
                        attackPerOnce = true;
                        break;
                    }
                }
            }
        }
    }
    public void AttackCool()
    {
        attackInCoolDown = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 me = (Vector2)transform.position;
        float flip = (_sr.flipX ? 1f : -1f);
        Vector2 start =  (new Vector2(offSet.x * flip, offSet.y));
        Vector2 size = flip * HitboxSize;
        Vector2 startPos = me + start - (size / 2);
        Gizmos.DrawLine(startPos, startPos + new Vector2(0, size.y));
        Gizmos.DrawLine(startPos, startPos + new Vector2(size.x, 0));
        Gizmos.DrawLine(startPos + new Vector2(size.x, 0), startPos + size);
        Gizmos.DrawLine(startPos + new Vector2(0, size.y), startPos + size);
    }
    public void AttackEnd()
    {
        curEnemyState = EnemyState.AttackEnd;
        _frameVelocity.x = 0f;
        Invoke(nameof(BackToFollow), attackAfter);
    }
    public void BackToFollow()
    {
        m_animator.SetBool("isMove", true);
        curEnemyState = EnemyState.Follow;
    }
    private void ApplyMovement() => _rigidBody.velocity = _frameVelocity;
    private void HandleGravity()
    {
        if (_grounded && _frameVelocity.y <= 0f)
        {
            _frameVelocity.y = -0.15f;
        }
        else
        {
            var inAirGravity = 70f;
            _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -20f, inAirGravity * Time.fixedDeltaTime);
        }
    }
    void Move()
    {
        Vector3 dir = new Vector3(Mathf.PingPong(Time.time * speed, pos2.x - pos1.x) + pos1.x, pos1.y, 0f);
        _rigidBody.MovePosition(dir);
    }
    private void CheckCollisions()
    {
        Physics2D.queriesStartInColliders = false;

        int layermask = (-1) - (1 << LayerMask.NameToLayer("Unit"));

        // Ground and Ceiling
        RaycastHit2D groundRayCast = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.down, 0.05f, mask);
        RaycastHit2D ceilingRayCast = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.down, 0.05f, mask);

        bool groundHit = groundRayCast;
        bool ceilingHit = ceilingRayCast;
        //Hit a Ceiling
        if (ceilingHit)
        {
            if (ceilingRayCast.collider.CompareTag("OneWayPlatform"))
            {
                Physics2D.IgnoreCollision(ceilingRayCast.collider, _col, false);
            }
        }
        //Landed on the Ground
        if (!_grounded && groundHit)
        {
            Physics2D.IgnoreCollision(groundRayCast.collider, _col, false);
            _grounded = true;
            //if (Physics2D.GetIgnoreCollision(groundRayCast.collider, _col))
            //{

            //}
            //else
            //{
            //    Physics2D.IgnoreCollision(groundRayCast.collider, _col, false);
            //    _grounded = true;
            //}
        }
        // Left the Ground
        else if (_grounded && !groundHit)
        {
            _grounded = false;
        }
        Physics2D.queriesStartInColliders = true;
    }
    public override void Initialize(int khealth, int kspeed, int kdamage)
    {
        base.Initialize(khealth, kspeed, kdamage);
    }
    public override void OnHit(Vector2 hitPoint, int damage)
    {
        GetComponentInChildren<Animator>().SetTrigger("hittrig");
        base.OnHit(hitPoint, damage);
        GameMgr.Inst.damageTextMgr.CreateDamageText(damage, this.transform, hitPoint);
        Camera.main.transform.GetComponent<CameraShake>().ShakeCamera();
        AudioMgr.Instance.LoadClip_SFX("enemyHit");
    }
    public void HitStop()
    {
        m_animator.SetBool("isMove", true);
        if (curEnemyState == EnemyState.Death) return;
        curEnemyState = EnemyState.Follow;
        attackInCoolDown = true;
        Invoke(nameof(AttackCool), 0.5f);
        _frameVelocity.x = 0f;
    }
    public override void OnDeath()
    {
        GetComponentInChildren<Animator>().SetTrigger("deathtrig");
        GameMgr.Inst.gameScene.gameUI.EnemyKilled(this);
        curEnemyState = EnemyState.Death;
        _frameVelocity.x = 0;
        AudioMgr.Instance.LoadClip_SFX("enemyDie");
        Destroy(gameObject, 1.2f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
        else
        {
            if(currentOneWayPlatform != null)
            {
                CompositeCollider2D tc = currentOneWayPlatform.GetComponent<CompositeCollider2D>();
                Physics2D.IgnoreCollision(tc, _col, false);
                currentOneWayPlatform = null;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fall"))
        {
            OnDeath();
        }
    }
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("OneWayPlatform"))
    //    {
    //        Debug.Log("No");
    //        currentOneWayPlatform = null;
    //    }
    //}
}
