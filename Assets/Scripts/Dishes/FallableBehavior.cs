using System;
using Interfaces;
using UnityEngine;

namespace Dishes
{
    public class FallableBehavior : MonoBehaviour, IFallable
    {
        private bool _isFalling;

        private void Awake()
        {
            _isFalling = true;
        }

        public bool IsFalling()
        {
            return _isFalling;
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!_isFalling || (other.gameObject.TryGetComponent(out IFallable fallable) && fallable.IsFalling()))
            {
                return;
            }
            Debug.Log("not falling");
            _isFalling = false;
            if (other.gameObject.CompareTag("Player"))
            {
                EventManagerScript.Instance.TriggerEvent("PlayerHit", null);
            }
        }
    }
}
