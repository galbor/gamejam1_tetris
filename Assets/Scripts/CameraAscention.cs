using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAscention : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _dropper;
    [SerializeField] private GameObject _cameraObject;
    [SerializeField] private float _descentSpeedStart;
    [SerializeField] private float _ascensionSpeed;
    [SerializeField] private float _descentSpeed;
    [SerializeField] private float _minCameraY;

    [SerializeField] private float _playerMaxHeightPercent = 0.75f;
    [SerializeField] private float _playerMinHeightPercent = 0.25f;
    
    private Camera _camera;
    private Rigidbody2D _cameraBody;
    private Vector3 _ascensionVector;
    private Vector3 _descentVector;
    private SpriteRenderer _playerSpriteRenderer;
    private bool started = false;

    private float _playerMaxHeightScreen;
    private float _playerMinHeightScreen;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("CameraAscention Start");
        _camera = _cameraObject.GetComponent<Camera>();
        _playerSpriteRenderer = _player.GetComponent<SpriteRenderer>();
        _cameraBody = _cameraObject.GetComponent<Rigidbody2D>();
        var pixelHeight = _camera.pixelHeight;
        _playerMaxHeightScreen = pixelHeight * _playerMaxHeightPercent;
        _playerMinHeightScreen = pixelHeight * _playerMinHeightPercent;
        _ascensionVector = new Vector3(0, _ascensionSpeed, 0);
        _descentVector = new Vector3(0, -_descentSpeedStart, 0);
        EventManagerScript.Instance.StartListening(EventManagerScript.PlayerFirstLand, GameCameraSpeeds);
    }

    private void GameCameraSpeeds(object arg0)
    {
        _descentVector = new Vector3(0, -_descentSpeed, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    { 
        if (_camera.transform.position.y < _minCameraY)
        {
            Debug.Log("CameraAscention Reset");
            var transform1 = _camera.transform;
            var position = transform1.position;
            position = new Vector3(position.x, _minCameraY, position.z);
            transform1.position = position;
            _cameraBody.velocity = Vector2.zero;
        } else if (started && _camera.WorldToScreenPoint(_player.transform.position).y > _playerMaxHeightScreen)
        {
            Debug.Log("CameraAscention Ascend");
            _cameraBody.velocity = _ascensionVector;
        } else if (_camera.transform.position.y > _minCameraY && _camera.WorldToScreenPoint(_playerSpriteRenderer.bounds.min).y <= _playerMinHeightScreen)
        {
            Debug.Log("CameraAscention Descend");
            _cameraBody.velocity = _descentVector;
        }
    }
    
    public void GameStarted()
    {
        started = true;
    }
}
