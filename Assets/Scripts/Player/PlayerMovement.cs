using System;
using System.Linq.Expressions;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Interfaces;
using UnityEngine;
using Object = System.Object;

public class PlayerMovement : MonoBehaviour, IMovable
{
    public bool CanMove { get; set; }

    private LayerMask _waterLayerMask;
    private LayerMask _borderLayerMask;
    private LayerMask _playerLayerMask;
    private LayerMask _ignorePlayerCollisionLayerMask;
    private LayerMask _ignoreHoldableLayerMask;
    
    private Rigidbody2D _rigidBody;
    private IHoldable _holdable;

    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float maxJumpHeight = 3f;
    [SerializeField] private float maxJumpTime = 1f;
    [SerializeField] private float holdDistance = .5f;
    [SerializeField] private float timeInWaterUntilStop = 10f;

    private float _inputAxis;
    private Vector2 _velocity;
    private float _velocityMultiplier = 1f;
    private float _timeInWater;
    private float JumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    private float Gravity => (-2f * maxJumpHeight) / Mathf.Pow(maxJumpTime / 2f, 2);
    private FixedJoint2D _fixedJoint;
    private const float JointBreakForce = 600f;
    private float _tweenYOffset;
    private TweenerCore<Vector3,Vector3,VectorOptions> _holdableTween;
    private bool _submerged;
    private SpriteRenderer _spriteRenderer;
    private bool _lost;
    [SerializeField] private float waterSoakCurve = 3f;

    public bool Grounded { get; private set; }
    public bool Jumping { get; private set; }
    public bool Turning => (_inputAxis > 0f && _velocity.x < 0f) || (_inputAxis < 0f && _velocity.x > 0f);
    public bool Running => Mathf.Abs(_velocity.x) > .25f || Mathf.Abs(_inputAxis) > .25f;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _waterLayerMask = LayerMask.GetMask("Water");
        _borderLayerMask = LayerMask.GetMask("Border");
        _playerLayerMask = LayerMask.GetMask("Player");
        _ignorePlayerCollisionLayerMask = _waterLayerMask | _playerLayerMask;
        _ignoreHoldableLayerMask = _waterLayerMask | _borderLayerMask | _playerLayerMask;
        _timeInWater = 0f;
        CanMove = true;
        _lost = false;
    }

    private void Update()
    {
        HorizontalMovement();

        var transform1 = transform;
        var scale = transform1.localScale;
        var localScale = scale;
        var distance = 3f * localScale.x;

        // cast ray from the player's feet
        // get the down-left corner of the player
        var position = transform1.position;     
        var spriteBounds = _spriteRenderer.sprite.bounds;
        var playerBottomCenter = position + new Vector3(0, spriteBounds.min.y, 0) * scale.x;
        var playerBottomLeftCorner = playerBottomCenter + new Vector3(-.2f, .15f, 0);
        var playerBottomRightCorner = playerBottomCenter + new Vector3(.2f, .15f, 0);
        var leftHit = Physics2D.Raycast(playerBottomLeftCorner, Vector2.down, distance, ~_ignorePlayerCollisionLayerMask);
        var rightHit = Physics2D.Raycast(playerBottomRightCorner, Vector2.down, distance, ~_ignorePlayerCollisionLayerMask);
        // Debug.DrawRay(playerBottomLeftCorner, Vector2.down * distance, Color.red);
        // Debug.DrawRay(playerBottomRightCorner, Vector2.down * distance, Color.red);
        
        var touchesGround = (leftHit.collider != null && leftHit.rigidbody != _rigidBody) 
                            || (rightHit.collider != null && rightHit.rigidbody != _rigidBody);
        if (!Grounded && touchesGround)
        {
            Debug.Log("Player touches the ground");
        }
        Grounded = touchesGround;
        if (Grounded) {
            
            GroundedMovement();
        }

        ApplyGravity();
        
        ApplyWaterSoak();
        
        // CheckHoldAndRelease();
    }

    private void ApplyWaterSoak()
    {
        if (!_submerged && _timeInWater > 0f)
        {
            _timeInWater -= Time.deltaTime;
        }
        else if (_submerged && _timeInWater < timeInWaterUntilStop)
        {
            _timeInWater += Time.deltaTime;
        }
        var easeInCubic = Mathf.Pow(_timeInWater / timeInWaterUntilStop, waterSoakCurve);
        _velocityMultiplier = Mathf.Lerp(1f, 0f, easeInCubic);
        
        // if _velocityMultiplier reached 0, lose
        if (!_lost && _velocityMultiplier <= 0f)
        {
            _lost = true;
            EventManagerScript.Instance.TriggerEvent("PlayerDrowned", null);
        }
    }

    private void CheckHoldAndRelease()
    {
        if (CanMove && Input.GetButtonDown("Fire1") && _holdable == null)
        {
            var transform1 = transform;
            var hitInfo = Physics2D.Raycast(transform1.position, transform1.right, holdDistance * transform1.localScale.x, ~_ignoreHoldableLayerMask);
            if (hitInfo.collider != null && hitInfo.collider.TryGetComponent(out IHoldable holdable))
            {
                // _holdable.PickUp(transform);
                Debug.Log("Player picked up " + holdable);
                _holdable = holdable;
                _fixedJoint = gameObject.AddComponent<FixedJoint2D>();
                _fixedJoint.connectedBody = _holdable.GetObject().GetComponent<Rigidbody2D>();
                _fixedJoint.breakForce = JointBreakForce;
                // do local move y to the holdable over .3 seconds to make it look like the player is picking it up
                var localPosition = _holdable.GetObject().transform.localPosition;
                _tweenYOffset = localPosition.y;
                _holdableTween = _holdable.GetObject().transform.DOLocalMoveY(_tweenYOffset + .3f, .15f);
            }
        }
        if (CanMove && Input.GetButtonUp("Fire1") && _holdable != null)
        {
            // _holdable.Release();
            Debug.Log("Player released " + _holdable);
            _holdable = null;
            Destroy(_fixedJoint);
            _holdableTween?.Kill();
        }
    }

    private void HorizontalMovement()
    {
        _inputAxis = CanMove? Input.GetAxis("Horizontal"): 0;
        
        _velocity.x = Mathf.MoveTowards(_velocity.x, _inputAxis * moveSpeed, moveSpeed * Time.deltaTime);
        _velocity.x *= _velocityMultiplier;
    
        // player's facing direction
        if (_holdable != null) {  // we want to not turn if we're holding something
            return;
        }
        if (_velocity.x > 0f || (_velocity.x == 0f && _inputAxis > 0f))
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (_velocity.x < 0f || (_velocity.x == 0f && _inputAxis < 0f))
        {
            transform.eulerAngles = Vector3.up * 180f;
        }
        // if player is turning, add drag to make him turn faster
        if (Turning)
        {
            _velocity.x *= .98f;
        }
    }

    private void GroundedMovement()
    {
        _velocity.y = Mathf.Max(_velocity.y, 0f);
        Jumping = _velocity.y > 0f;
        
        if (CanMove && Input.GetButtonDown("Jump"))
        {
            _velocity.y = JumpForce * _velocityMultiplier;
            Jumping = true;
            if (_holdable != null)
            {
                _holdableTween?.Kill();
            }
        }
    }
    
    private void ApplyGravity()
    {
        bool falling = _velocity.y < 0f || !Input.GetButton("Jump");
        float multiplier = falling ? 2f : 1f;
        _velocity.y += multiplier * Gravity * Time.deltaTime;
        _velocity.y = Mathf.Max(_velocity.y, Gravity / 2f);
    }
    
    private void FixedUpdate()
    {
        Vector2 position = _rigidBody.position;
        position += _velocity * Time.fixedDeltaTime;
        _rigidBody.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.IsChildOf(transform))
        {
            return;
        }
        if (other.gameObject.TryGetComponent(out IFallable fallable))
        {
            if (transform.IsDirectionFrom(other.transform, Vector2.up))
            {
                Debug.Log("player's head touches rock");
                _velocity.y = 0f;
            }
            if (fallable.IsFalling())
            {
                EventManagerScript.Instance.TriggerEvent("PlayerHit", fallable);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("water"))
        {
            _submerged = true;
            Debug.Log("Player enters water");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("water"))
        {
            _submerged = false;
            Debug.Log("Player exits water");
        }
    }

    public Vector2 GetVelocity()
    {
        return _velocity;
    }

    public float GetSoakness()
    {
        return _timeInWater / timeInWaterUntilStop;
    }
}
