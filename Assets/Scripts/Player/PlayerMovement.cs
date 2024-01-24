using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    
    private Rigidbody2D _rigidBody;
    private IHoldable _holdable;

    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float maxJumpHeight = 3f;
    [SerializeField] private float maxJumpTime = 1f;
    [SerializeField] private float holdDistance = .5f;

    private float _inputAxis;
    private Vector2 _velocity;
    private float JumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    private float Gravity => (-2f * maxJumpHeight) / Mathf.Pow(maxJumpTime / 2f, 2);
    private FixedJoint2D _fixedJoint;
    private const float JointBreakForce = 600f;
    private float _tweenYOffset;
    private TweenerCore<Vector3,Vector3,VectorOptions> _holdableTween;

    public bool Grounded { get; private set; }
    public bool Jumping { get; private set; }
    public bool Turning => (_inputAxis > 0f && _velocity.x < 0f) || (_inputAxis < 0f && _velocity.x > 0f);
    public bool Running => Mathf.Abs(_velocity.x) > .25f || Mathf.Abs(_inputAxis) > .25f;
    
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HorizontalMovement();

        if (!Grounded && _rigidBody.Raycast(Vector2.down))
        {
            Debug.Log("Player touches the ground");
        }
        Grounded = _rigidBody.Raycast(Vector2.down);
        if (Grounded) {
            
            GroundedMovement();
        }

        ApplyGravity();
        
        CheckHoldAndRelease();
    }

    private void CheckHoldAndRelease()
    {
        if (Input.GetButtonDown("Fire1") && _holdable == null)
        {
            var hitInfo = Physics2D.Raycast(transform.position, transform.right, holdDistance);
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
        if (Input.GetButtonUp("Fire1") && _holdable != null)
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
        _inputAxis = Input.GetAxis("Horizontal");
        
        _velocity.x = Mathf.MoveTowards(_velocity.x, _inputAxis * moveSpeed, moveSpeed * Time.deltaTime);
    
    
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
    }

    private void GroundedMovement()
    {
        _velocity.y = Mathf.Max(_velocity.y, 0f);
        Jumping = _velocity.y > 0f;
        
        if (Input.GetButtonDown("Jump"))
        {
            _velocity.y = JumpForce;
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
        if (other.gameObject.CompareTag("rock") || other.gameObject.CompareTag("gentlerock"))
        {
            // TODO kill\lower HP of player
            if (transform.IsDirectionFrom(other.transform, Vector2.up))
            {
                Debug.Log("Rock hits player's head");
                _velocity.y = 0f;
            }
        }
    }
}
