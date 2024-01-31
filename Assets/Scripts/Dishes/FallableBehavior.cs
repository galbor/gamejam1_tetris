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

        private void Awake()
        {
            _waterLayer = LayerMask.NameToLayer("Water");
            _borderLayer = LayerMask.NameToLayer("Border");
            _playerLayer = LayerMask.NameToLayer("Player");
            _isFalling = true;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            EventManagerScript.Instance.StartListening(EventManagerScript.PlayerHit, HitHead);
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
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log("Fallable collision enter with " + other.gameObject.layer);
            if (other.gameObject.layer == _waterLayer || other.gameObject.layer == _borderLayer)
            {
                return;
            }
            if (!_isFalling || (other.gameObject.TryGetComponent(out IFallable fallable) && fallable.IsFalling()))
            {
                return;
            }
            var velocity = _rigidbody2D.velocity;
            ScreenEffects.Shake(.5f, velocity.magnitude * velocity.magnitude/400);
            if (other.gameObject.layer != _playerLayer)
            {
                Debug.Log("not falling");
                _isFalling = false;
            }

        }
    }
}
