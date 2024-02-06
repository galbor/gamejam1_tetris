using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class WaterFilling : MonoBehaviour
{
    [SerializeField] private float _drainSpeed = 1f;
    [SerializeField] private float _fillSpeed = 1f;
    [SerializeField] private float _fastFillSpeed = 2f;
    
    [SerializeField] private float _faucetOpenTime = 0.5f;

    [SerializeField] private Transform _faucetStream;
    [SerializeField] private ParticleSystem[] _streamParticles;
    
    [SerializeField] private Camera _mainCamera;
    
    private Rigidbody2D _waterBody;
    private float _minWaterLevel;
    private float _streamWidth;
    private bool _isFaucetOpen = false;
    private bool _isFilling = false;
    private float _desiredWaterLevel = -30;
    private EventManagerScript _eventManager;
    private SpriteShapeRenderer _spriteRenderer;
    private Vector3 _screenEnd; //1920, 1080, 0
    
    public float WaterTop => _spriteRenderer.bounds.max.y;
    
    // Start is called before the first frame update
    void Start()
    {
        _waterBody = GetComponent<Rigidbody2D>();
        _minWaterLevel = transform.position.y;
        _streamWidth = _faucetStream.localScale.x;
        _faucetStream.localScale = new Vector3(0, _faucetStream.localScale.y, 1);
        _eventManager = EventManagerScript.Instance;
        _eventManager.StartListening(EventManagerScript.Win, CloseFaucet);
        _eventManager.StartListening(EventManagerScript.DishWithDishCollision, UpdateDesiredWaterLevel);
        _spriteRenderer = GetComponent<SpriteShapeRenderer>();
        _screenEnd = new Vector3(Screen.width, Screen.height, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.y <= _minWaterLevel && _waterBody.velocity.y < 0)
        {
            _waterBody.velocity = Vector2.zero;
            return;
        }

        if (_spriteRenderer.bounds.max.y >= _mainCamera.ScreenToWorldPoint(_screenEnd).y)
        {
            CloseFaucet(null);
        }

        if (!_isFaucetOpen || _isFilling)
            return;
        if (_spriteRenderer.bounds.max.y < _desiredWaterLevel)
        {
            _waterBody.velocity = new Vector2(0, _fastFillSpeed);
        }
        else
        {
            _waterBody.velocity = new Vector2(0, _fillSpeed);
        }
    }

    private void LateUpdate()
    {
        if (_isFaucetOpen)
        {
            if (_waterBody.position.y < _minWaterLevel + .5f)
            {
                AudioManager.StopWaterFallOnWater();
                AudioManager.PlayWaterFallOnSink();
            }
            else
            {
                AudioManager.StopWaterFallOnSink();
                AudioManager.PlayWaterFallOnWater();
            }

            AudioManager.PlayWaterFlow();
        }
    }

    private void WaterUp()
    {
        _waterBody.velocity = new Vector2(0, _fillSpeed);
    }
    
    private void WaterDown()
    {
        _waterBody.velocity = new Vector2(0, -_drainSpeed);
    }
    
    public void OpenFaucet()
    {
        _isFaucetOpen = true;
        AudioManager.PlayFaucetOpen();
        AudioManager.PlayFaucetOpenWaterPressure();
        for (int i = 0; i < _streamParticles.Length; i++)
            _streamParticles[i].Play();
        StartCoroutine(FaucetAnimationCoroutine(new Vector3(_streamWidth, _faucetStream.localScale.y, 1),
            new Vector2(0, _fillSpeed)));
    }
    
    public void CloseFaucet(object obj = null)
    {
        _isFaucetOpen = false;
        AudioManager.PlayFaucetClose();
        AudioManager.PlayFaucetCloseWaterPressure();
        for (int i = 0; i < _streamParticles.Length; i++)
            _streamParticles[i].Stop();
        StartCoroutine(FaucetAnimationCoroutine(new Vector3(0, _faucetStream.localScale.y, 1),
            new Vector2(0, -_drainSpeed)));
    }
    
    
    private void UpdateDesiredWaterLevel(object obj)
    {
        _desiredWaterLevel = Math.Max((float) obj-1.5f, _desiredWaterLevel);
    }
    
    
    IEnumerator FaucetAnimationCoroutine(Vector3 final_stream_size, Vector2 final_water_velocity)
    {
        _isFilling = true;
        float time_elapsed = 0f;
        Vector3 initial_size = _faucetStream.localScale;
        Vector2 initial_velocity = _waterBody.velocity;
        while (time_elapsed < _faucetOpenTime)
        {
            _faucetStream.localScale = Vector3.Lerp(initial_size, final_stream_size, time_elapsed / _faucetOpenTime);
            _waterBody.velocity =
                Vector2.Lerp(initial_velocity, final_water_velocity, time_elapsed / _faucetOpenTime);
            time_elapsed += Time.deltaTime;
            yield return null;
        }
        _faucetStream.localScale = final_stream_size;
        _waterBody.velocity = final_water_velocity;
        _isFilling = false;
    }
}
