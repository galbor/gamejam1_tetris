using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class HoldableBehavior : MonoBehaviour, IHoldable
{
    [SerializeField] private GameObject player;
    [SerializeField] private float distance = .2f;
    private Transform _parent;
    private TweenerCore<Vector3,Vector3,VectorOptions> _tween;
    private SpriteRenderer _spriteRenderer;
    private float _raycastDistance = 0f;
    private bool _isHeld;
    private Rigidbody2D _rigidbody2D;

    public bool IsColliding { get; private set; }

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // check if the object collides with something in front of it
        if (_isHeld)
        {
            var transform1 = transform;
            var position = transform1.position;
            var hitInfo = Physics2D.Raycast(position, _parent.right, _raycastDistance);
            // draw the raycast
            Debug.DrawRay(position, _parent.right * _raycastDistance, Color.red);
            
            // if it does, stop moving the player(parent)
            IsColliding = hitInfo.collider != null && !transform.IsChildOf(hitInfo.collider.transform);
            if (IsColliding)
            {
                Debug.Log("Colliding with " + hitInfo.collider.gameObject);
            }
        }

    }
    
    public void PickUp(Transform parent)
    {
        // disable physics and attach it to the player's hand
        _rigidbody2D.isKinematic = true;
        float yOffset = _spriteRenderer.bounds.extents.y;
        _tween = transform.DOMoveY(parent.position.y + yOffset, .3f);
        transform.SetParent(parent);
        _raycastDistance = _spriteRenderer.bounds.extents.x + distance;
        _parent = parent;
        _isHeld = true;
    }

    public void Release()
    {
        // enable physics and detach it from the player's hand
        _parent = null;
        transform.parent = null;
        _rigidbody2D.isKinematic = false;
        _raycastDistance = 0f;
        _isHeld = false;
    }

    public void OnMove()
    {
        _tween.Kill();
    }
    
    public GameObject GetObject()
    {
        return gameObject;
    }
}
