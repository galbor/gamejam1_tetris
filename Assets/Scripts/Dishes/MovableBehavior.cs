using System;
using Interfaces;
using UnityEngine;

namespace Dishes
{
    public class MovableBehavior : MonoBehaviour, IMovable
    {
        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public Vector2 GetVelocity()
        {
            return _rigidbody2D.velocity;
        }
    }
}
