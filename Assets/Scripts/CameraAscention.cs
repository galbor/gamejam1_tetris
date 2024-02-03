using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAscention : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _dropper;
    [SerializeField] private GameObject _cameraObject;
    [SerializeField] private float _ascensionSpeed = 1f;
    [SerializeField] private float _descentSpeed = 4f;

    [SerializeField] private float _playerMaxHeightPercent = 0.75f;
    
    private Camera _camera;
    private Rigidbody2D _cameraBody;
    private Vector3 _ascensionVector;
    private Vector3 _descentVector;
    private SpriteRenderer _playerSpriteRenderer;
    private bool started = false;

    private float _playerMaxHeightScreen;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("CameraAscention Start");
        _camera = _cameraObject.GetComponent<Camera>();
        _playerSpriteRenderer = _player.GetComponent<SpriteRenderer>();
        _cameraBody = _cameraObject.GetComponent<Rigidbody2D>();
        _playerMaxHeightScreen = _camera.pixelHeight * _playerMaxHeightPercent;
        _ascensionVector = new Vector3(0, _ascensionSpeed, 0);
        _descentVector = new Vector3(0, -_descentSpeed, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    { 
        if (started && _camera.WorldToScreenPoint(_player.transform.position).y > _playerMaxHeightScreen)
        {
            _cameraBody.velocity = _ascensionVector;
            return;
        }
        if (_camera.WorldToScreenPoint(_playerSpriteRenderer.bounds.min).y <= 0)
        {
            _cameraBody.velocity = _descentVector;
        }
    }
    
    public void GameStarted()
    {
        started = true;
    }
}
