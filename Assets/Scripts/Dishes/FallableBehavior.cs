using System;
using Effects;
using Interfaces;
using UnityEngine;

namespace Dishes
{
    public class FallableBehavior : MonoBehaviour, IFallable
    {
        
        
        private bool _isFalling;
        private int _waterLayer;
        private int _borderLayer;
        private int _playerLayer;
        private Rigidbody2D _rigidbody2D;
        private EventManagerScript _eventManager;
        private int _squeezeDropletsLayer;

        private void Awake()
        {
            _waterLayer = LayerMask.NameToLayer("Water");
            _borderLayer = LayerMask.NameToLayer("Border");
            _playerLayer = LayerMask.NameToLayer("Player");
            _squeezeDropletsLayer = LayerMask.NameToLayer("SqueezeDroplets");
            _isFalling = true;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _eventManager = EventManagerScript.Instance;
            _eventManager.StartListening(EventManagerScript.PlayerHit, HitHead);
        }

        public bool IsFalling()
        {
            return _isFalling;
        }

        private void HitHead(object obj)
        {
            if (obj.Equals(this))
            {
                _isFalling = false;
            }
        }
        
        public void AddForce(Vector2 force)
        {
            _rigidbody2D.AddForce(force, ForceMode2D.Impulse);
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log("Fallable collision enter with " + other.gameObject.layer);
            if (other.gameObject.layer == _waterLayer || other.gameObject.layer == _borderLayer)
            {
                return;
            }
            if (other.gameObject.TryGetComponent(out IFallable fallable1))
            {
                AudioManager.PlayDishWithDishCollision();
                _eventManager.TriggerEvent(EventManagerScript.DishWithDishCollision, transform.position.y);
            }
            if (!_isFalling || (other.gameObject.TryGetComponent(out IFallable fallable) && fallable.IsFalling()))
            {
                return;
            }
            var velocity = _rigidbody2D.velocity;
            ScreenEffects.Shake(.5f, velocity.magnitude * velocity.magnitude/400);
            if (other.gameObject.layer != _playerLayer && other.gameObject.layer != _squeezeDropletsLayer)
            {
                Debug.Log("not falling");
                _isFalling = false;
            }
        }
    }
}
