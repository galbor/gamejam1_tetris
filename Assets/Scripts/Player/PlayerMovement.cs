using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float maxJumpHeight = 3f;
    [SerializeField] private float maxJumpTime = 1f;
    
    private float _inputAxis;
    private Vector2 _velocity;
    private float JumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    private float Gravity => (-2f * maxJumpHeight) / Mathf.Pow(maxJumpTime / 2f, 2);
    
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
    }

    private void HorizontalMovement()
    {
        _inputAxis = Input.GetAxis("Horizontal");
        _velocity.x = Mathf.MoveTowards(_velocity.x, _inputAxis * moveSpeed, moveSpeed * Time.deltaTime);
        if (_rigidBody.Raycast(Vector2.right * _velocity.x))
        {
            _velocity.x = 0f;
        }

        if (_velocity.x > 0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (_velocity.x < 0f)
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
        if (other.gameObject.CompareTag("rock"))
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
