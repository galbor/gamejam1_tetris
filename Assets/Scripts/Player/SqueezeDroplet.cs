using System.Collections;
using Interfaces;
using UnityEngine;
using UnityEngine.Pool;

namespace Player
{
    public class SqueezeDroplet : MonoBehaviour
    {
        [SerializeField] private Color colorBright;
        [SerializeField] private Color colorDark;
        [SerializeField] private float sizeSmall;
        [SerializeField] private float sizeBig;
        [SerializeField] private float speedSlow;
        [SerializeField] private float speedFast;
        [SerializeField] private float fadeStartTime;
        [SerializeField] private float angleFirst;
        [SerializeField] private float angleLast;
        [SerializeField] private float force;
        [SerializeField] private float spawnRadius;
    
        private float _lifeTime;
        private SpriteRenderer _spriteRenderer;
        private float _size;
        private float _speed;
        private float _angle;
        private ObjectPool<GameObject> _pool;
        private bool _released;

        public void Init(Vector3 position, float lifetime, ObjectPool<GameObject> objPool)
        {
            Debug.Log("droplet Init");
            _lifeTime = lifetime;
            _pool = objPool;
            _released = false;
        
            // color
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.color = Color.Lerp(colorBright, colorDark, Random.Range(0f, 1f));
            // size
            _size = Random.Range(sizeSmall, sizeBig);
            transform.localScale = new Vector3(_size, _size, 1);
            // speed
            _speed = Random.Range(speedSlow, speedFast);
            // angle
            _angle = Random.Range(angleFirst, angleLast);
            transform.position = position + new Vector3(Mathf.Cos(_angle * Mathf.Deg2Rad), Mathf.Sin(_angle * Mathf.Deg2Rad)) * spawnRadius;
        
            StartCoroutine(Fade());
        }

        private IEnumerator Fade()
        {
            var time = 0f;
            while (time < _lifeTime)
            {
                time += Time.deltaTime;
                if (time > fadeStartTime)
                {
                    var color = _spriteRenderer.color;
                    color.a = 1 - (time - fadeStartTime) / (_lifeTime - fadeStartTime);
                    _spriteRenderer.color = color;
                }
                transform.position += new Vector3(Mathf.Cos(_angle * Mathf.Deg2Rad), Mathf.Sin(_angle * Mathf.Deg2Rad)) * (_speed * Time.deltaTime);
                yield return null;
            }
            Release();
        }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IFallable fallable))
            {
                var angleVector = new Vector2(Mathf.Cos(_angle * Mathf.Deg2Rad), Mathf.Sin(_angle * Mathf.Deg2Rad));
                fallable.AddForce(angleVector * force);
                Release();
            }
        }

        public void Release()
        {
            if (_released) return;
            _released = true;
            _pool.Release(this.gameObject);
        }
    }
}
