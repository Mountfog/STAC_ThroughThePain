using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace PlayerController
{
    
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerMovement : MonoBehaviour, IPlayerController
    {
        [SerializeField] private ScriptableStats _stats;
        [SerializeField] private Animator _anim;
        private Rigidbody2D _rb;
        private CapsuleCollider2D _col;
        private FrameInput _frameInput;
        private Vector2 _frameVelocity;
        private bool _cachedQueryStartInColliders;
        public PlayerInputActions _playerActions = null;
        #region Interface

        public Vector2 FrameInput => _frameInput.Move;
        public event Action<bool, float> GroundedChanged;
        public event Action Jumped;

        #endregion

        private float _time;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<CapsuleCollider2D>();
            _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
            _playerActions = new PlayerInputActions();
            _playerActions.Game.Enable();
            _playerActions.Game.Jump.performed += OnJump;
            //_pia.Game.Move.performed += OnMove
        }

        private void Update()
        {
            _time += Time.deltaTime;
            GatherInput();
        }

        private void GatherInput()
        {
            _frameInput = new FrameInput
            {
                JumpDown = _playerActions.Game.Jump.WasPerformedThisFrame(),
                JumpHeld = _playerActions.Game.Jump.IsPressed(),
                Move = _playerActions.Game.Move.ReadValue<Vector2>(),
                RollDown = _playerActions.Game.Roll.triggered,
                AttackDown = _playerActions.Game.Attack.triggered,
                SkillDown = _playerActions.Game.Skill.triggered
            };
            if (_stats.SnapInput)
            {
                _frameInput.Move.x = Mathf.Abs(_frameInput.Move.x) < _stats.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.x);
                _frameInput.Move.y = Mathf.Abs(_frameInput.Move.y) < _stats.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.y);
            }

            if (_frameInput.JumpDown)
            {
                if (!HoldingDown)
                {
                    _jumpToConsume = true;
                    _timeJumpWasPressed = _time;
                }

            }
        }

        private void FixedUpdate()
        {
            CheckCollisions();

            HandleJump();
            HandleDirection();
            HandleGravity();
            HandleRoll();
            Attack();
            ApplyMovement();

        }
        public bool isAttack = false;
        public void Attack()
        {
            if(_frameInput.AttackDown && !isAttack && !isRoll)
            {
                Debug.Log("Attack"); 
                Vector2 localPos = transform.position;
                Vector2 hitPoint = localPos + new Vector2(1.15f, 0f) * (sr.flipX ? -1 : 1);
                LayerMask layerMask  = LayerMask.GetMask("Unit");
                Collider2D[] monsters = Physics2D.OverlapCapsuleAll(hitPoint,new Vector2(0.99f,0.68f),CapsuleDirection2D.Horizontal,180f, layerMask);

                Debug.DrawLine(hitPoint - new Vector2(0.5f,0), hitPoint + new Vector2(0.5f, 0));
                _anim.SetTrigger("attackTrig");
                if (monsters.Length == 0) return;
                for (int i=0;i<monsters.Length; i++)
                {
                    if (monsters[i] == _col) continue;
                    if (!monsters[i].GetComponent<Unit>().isAlive) continue;
                    monsters[i].GetComponent<Unit>().OnHit(hitPoint, 10);
                }
                isAttack = true;
                Invoke("AttackHide", 0.4f);
            }
        }
        public void AttackHide()
        {
            isAttack = false;
        }

        #region Collisions

        private float _frameLeftGrounded = float.MinValue;
        private bool _grounded;
        private bool _ignoringPlatform = false;

        private void CheckCollisions()
        {
            Physics2D.queriesStartInColliders = false;

            int layermask = (-1) - (1 << LayerMask.NameToLayer("Unit"));
            // Ground and Ceiling
            RaycastHit2D groundRayCast = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.down, _stats.GrounderDistance, layermask);
            RaycastHit2D ceilingRayCast = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.down, _stats.GrounderDistance, layermask);
            bool groundHit =groundRayCast;
            bool ceilingHit = ceilingRayCast;
            // Hit a Ceiling
            if (ceilingHit)
            {
                if (ceilingRayCast.collider.CompareTag("OneWayPlatform"))
                    _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);
            }
            // Landed on the Ground
            if (!_grounded && groundHit)
            {
                if (groundRayCast.collider.CompareTag("OneWayPlatform"))
                {
                    if (groundRayCast.point.y - (_col.bounds.center.y - _col.bounds.extents.y) > 0f)
                    {
                        Physics2D.IgnoreCollision(groundRayCast.collider, _col);
                    }
                    else
                    {
                        Physics2D.IgnoreCollision(groundRayCast.collider, _col, false);
                        _grounded = true;
                        _coyoteUsable = true;
                        _bufferedJumpUsable = true;
                        _endedJumpEarly = false;
                        GroundedChanged?.Invoke(true, Mathf.Abs(_frameVelocity.y));
                        _anim.SetTrigger("landTrig");
                        _anim.ResetTrigger("jumptrig");
                    }
                }
                else
                {
                    Physics2D.IgnoreCollision(groundRayCast.collider, _col, false);
                    _grounded = true;
                    _coyoteUsable = true;
                    _bufferedJumpUsable = true;
                    _endedJumpEarly = false;
                    GroundedChanged?.Invoke(true, Mathf.Abs(_frameVelocity.y));
                    _anim.SetTrigger("landTrig");
                    _anim.ResetTrigger("jumptrig");
                }
            }
            // Left the Ground
            else if (_grounded && !groundHit)
            {
                _grounded = false;
                _frameLeftGrounded = _time;
                GroundedChanged?.Invoke(false, 0);
            }
            else if(_grounded && groundHit)
            {
                if(HoldingDown && _frameInput.JumpDown)
                    if (groundRayCast.collider.CompareTag("OneWayPlatform"))
                    {
                        _grounded = false;
                        _ignoringPlatform = true;
                        Physics2D.IgnoreCollision(groundRayCast.collider, _col);
                    }
            }
            _anim.SetBool("isfalling", (!_grounded && !groundHit && _rb.velocity.y < 0)); //LeftTheGroundByPlatformEdge


            Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
        }

        #endregion

        #region Jumping

        private bool _jumpToConsume = false;
        private bool _bufferedJumpUsable;
        private bool _endedJumpEarly;
        private bool _coyoteUsable;
        private float _timeJumpWasPressed;

        private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + _stats.JumpBuffer && _timeJumpWasPressed != 0;
        private bool CanUseCoyote => _coyoteUsable && !_grounded && _time < _frameLeftGrounded + _stats.CoyoteTime;

        private bool HoldingDown => _frameInput.Move.y < -0.8f;
        private void HandleJump()
        {
            if (!_endedJumpEarly && !_grounded && !_frameInput.JumpHeld && _rb.velocity.y > 0) _endedJumpEarly = true;

            if (!_jumpToConsume && !HasBufferedJump) return; //�������� ����
            else if (_grounded || CanUseCoyote) ExecuteJump();
            _jumpToConsume = false;
        }

        private void ExecuteJump()
        {
            _endedJumpEarly = false;
            _timeJumpWasPressed = 0;
            _bufferedJumpUsable = false;
            _coyoteUsable = false;
            _frameVelocity.y = _stats.JumpPower;
            Jumped?.Invoke();
            _anim.SetTrigger("jumptrig");
        }

        #endregion

        #region Horizontal
        public SpriteRenderer sr = null;
        private float  rollDeceleration = 120f;
        private float rollSpeed = 30f;
        private void HandleDirection()
        {
            if (_frameInput.Move.x == 0 || isRoll)
            {
                var deceleration = _grounded ? _stats.GroundDeceleration : _stats.AirDeceleration;
                
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, (isRoll ? rollDeceleration : deceleration) * Time.fixedDeltaTime);
            }
            else
            {

                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeed, _stats.Acceleration * Time.fixedDeltaTime);
                sr.flipX = _frameInput.Move.x < 0;

            }
            _anim.SetBool("ismove", _frameInput.Move.x != 0);
        }

        #endregion
        #region Roll
        private bool isRoll = false;
        public void HandleRoll()
        {
            if (_frameInput.RollDown && !isRoll && _grounded)
            {
                Debug.Log("Roll");
                _anim.SetTrigger("rollTrig");
                _frameVelocity.x = rollSpeed * (sr.flipX ? -1 : 1);
                isRoll = true;
                Invoke("RollHide", 0.26f);
            }
        }
        public void RollHide()
        {
            isRoll = false;
            Debug.Log("RollHide");
        }
        #endregion
        #region Gravity

        private void HandleGravity()
        {
            if (_grounded && _frameVelocity.y <= 0f)
            {
                _frameVelocity.y = _stats.GroundingForce;
            }
            else
            {
                var inAirGravity = _stats.FallAcceleration;
                if (_endedJumpEarly && _frameVelocity.y > 0) inAirGravity *= _stats.JumpEndEarlyGravityModifier;
                _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_stats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
            }
        }

        #endregion
        #region UnityInputSystem
        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.duration < 0.2f)
            {
                Debug.Log("Short Jump");
            }
            else if(context.duration > 0.3f)
            {

            }
        }
        #endregion

        private void ApplyMovement() => _rb.velocity = _frameVelocity;
    }

    public struct FrameInput
    {
        public bool JumpDown;
        public bool JumpHeld;
        public bool AttackDown;
        public bool RollDown;
        public bool FallDown;
        public bool SkillDown;
        public Vector2 Move;
    }

    public interface IPlayerController
    {
        public event Action<bool, float> GroundedChanged;

        public event Action Jumped;
        public Vector2 FrameInput { get; }
    }
}