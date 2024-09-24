using System.Collections;
using System.Collections.Generic;
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
    public float dist;
    public float speed = 10f;
    private Vector2 _frameVelocity = Vector2.zero;
    [SerializeField] private bool _grounded = true;
    public LayerMask mask;
    [SerializeField] private GameObject currentOneWayPlatform;
    public EnemyState curEnemyState = EnemyState.Stop; 

    public enum EnemyState
    {
        Stop = 0,
        Follow = 1,
        Attack = 2,
        Hit = 3,
        Death = 4,
    }

    private void Awake()
    {
        Initialize(30, 2, 3);
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
            if (Vector2.Distance(transform.position, player.position) <= dist)
            {
                curEnemyState = EnemyState.Follow;
                m_animator.SetBool("isMove", true);
            }
            //Move();
        }
        else if(curEnemyState == EnemyState.Follow)
        {
            float notTooClose = 1f;
            float attackDist = 3f;
            if (Vector2.Distance(transform.position, player.position) < attackDist)
            {
                notTooClose =  0.1f;
            }
            _frameVelocity.x = ((transform.position.x - player.position.x < 0f) ? 1f : -1f) * speed * notTooClose;
            _sr.flipX = ((transform.position.x - player.position.x < 0f));

            if (Mathf.Abs(transform.position.x - player.position.x) <= attackDist)
            {
                if (!_grounded) return;
                if (transform.position.y - player.position.y <= 0f)
                {
                    _frameVelocity.y = 24f;
                    _grounded = false;
                }
                else if (transform.position.y - player.position.y > 0f)
                {
                    if (currentOneWayPlatform == null) return;
                    Debug.Log("GetLow");
                    CompositeCollider2D tc = currentOneWayPlatform.GetComponent<CompositeCollider2D>();
                    Physics2D.IgnoreCollision(tc, _col);
                    _grounded = false;

                }
            }
        }
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
        // Hit a Ceiling
        //if (ceilingHit)
        //{
        //    if (ceilingRayCast.collider.CompareTag("OneWayPlatform")) 
        //    {
        //        _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);
        //        Debug.Log("what;");
        //    }
        //}
        // Landed on the Ground
        if (!_grounded && groundHit)
        {
            if (Physics2D.GetIgnoreCollision(groundRayCast.collider, _col))
            {
            }
            else
            {
                Physics2D.IgnoreCollision(groundRayCast.collider, _col, false);
                _grounded = true;
            }
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
        GameMgr.Inst.damageTextMgr.CreateDamageText(damage, this.transform, hitPoint);
        Camera.main.transform.GetComponent<CameraShake>().ShakeCamera();

    }
    public override void OnDeath()
    {
        GetComponentInChildren<Animator>().SetTrigger("deathtrig");
        GameMgr.Inst.gameScene.gameUI.EnemyKilled(this);
        curEnemyState = EnemyState.Death;
        _frameVelocity = Vector2.zero;
        Destroy(gameObject, 1.2f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            Debug.Log("No");
            currentOneWayPlatform = null;
        }
    }
}
