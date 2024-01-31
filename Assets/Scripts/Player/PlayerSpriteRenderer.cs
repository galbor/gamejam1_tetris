using System;
using UnityEngine;

namespace Player
{
    public class PlayerSpriteRenderer : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private PlayerMovement _movement;
        private float _scaleX;
        private float _scaleY;
        private bool _drown = false;
        private bool _hit = false;

        [SerializeField] private Sprite idle;
        [SerializeField] private Sprite jump;
        [SerializeField] private Sprite turn;
        [SerializeField] private AnimatedSprite run;
        [SerializeField] private Sprite soak;
        [SerializeField] private Sprite hit;
        [SerializeField] private float _expansionRate = .05f;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _movement = GetComponent<PlayerMovement>();
            var localScale = transform.localScale;
            _scaleX = localScale.x;
            _scaleY = localScale.y;
            EventManagerScript.Instance.StartListening(EventManagerScript.PlayerHit, Hit);
            EventManagerScript.Instance.StartListening(EventManagerScript.PlayerDrowned, Drown);
        }

        private void OnEnable()
        {
            _spriteRenderer.enabled = true;
        }
        
        private void OnDisable()
        {
            _spriteRenderer.enabled = false;
        }

        private void Drown(object arg0)
        {
            _drown = true;
        }

        private void Hit(object arg0)
        {
            _hit = true;
        }

        private void LateUpdate()
        {
            run.enabled = _movement.Running;
            if (_drown)
            {
                _spriteRenderer.sprite = soak;
                var transform2 = _spriteRenderer.transform;
                var localScale1 = transform2.localScale;
                localScale1 = new Vector3(localScale1.x, _scaleY + _expansionRate, localScale1.z);
                transform2.localScale = localScale1;
            } else if (_hit)
            {
                _spriteRenderer.sprite = hit;
            } else if (_movement.Jumping) {
                _spriteRenderer.sprite = jump;
            } else if (_movement.Turning)
            {
                _spriteRenderer.sprite = turn;
            } else if (!_movement.Running)
            {
                _spriteRenderer.sprite = idle;
            }
            // change sprite x scale & blueness based on soakness
            var soakness = _drown? 1f : _movement.GetSoakness();
            _spriteRenderer.color = new Color(1f - soakness, 1f, 1f, 1f);
            var transform1 = _spriteRenderer.transform;
            var localScale = transform1.localScale;
            var scale = localScale;
            scale.x = _scaleX + soakness * _expansionRate;
            localScale = scale;
            transform1.localScale = localScale;
        }
    }
}
