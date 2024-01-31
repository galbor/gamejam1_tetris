using System;
using Interfaces;
using UnityEngine;

namespace Dishes
{
    public class MovableBehavior : MonoBehaviour, IMovable
    {
        private Rigidbody2D _rigidbody2D;
        private int _waterLayer;
        private WaterShapeController waterShapeController;
        private bool _hitWater;
        
        public void Init(WaterShapeController wsc)
        {
            waterShapeController = wsc;
        }

        private void Awake()
        {
            _waterLayer = LayerMask.NameToLayer("Water");
            _hitWater = false;
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public Vector2 GetVelocity()
        {
            return _rigidbody2D.velocity;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_hitWater && other.gameObject.layer == _waterLayer)
            {
                var position = transform.position;
                var bottomY = position.y - GetComponent<SpriteRenderer>().bounds.extents.y;
                var splashPosition = new Vector3(position.x, bottomY, position.z);
                waterShapeController.Splash(splashPosition, GetVelocity());
                _hitWater = true;
            }
        }
    }
}
