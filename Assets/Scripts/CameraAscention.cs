using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAscention : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _dropper;
    [SerializeField] private GameObject _cameraObject;
    [SerializeField] private float _ascensionSpeed = 1f;

    [SerializeField] private float _playerMaxHeightPercent = 0.75f;
    
    private Camera _camera;
    private Vector3 _ascensionVector;

    private float _playerMaxHeightScreen;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("CameraAscention Start");
        _camera = _cameraObject.GetComponent<Camera>();
        _playerMaxHeightScreen = _camera.pixelHeight * _playerMaxHeightPercent;
        _ascensionVector = new Vector3(0, _ascensionSpeed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (_camera.WorldToScreenPoint(_player.transform.position).y > _playerMaxHeightScreen)
        {
            _cameraObject.transform.position += _ascensionVector * Time.deltaTime;
            // _dropper.transform.position += _ascensionVector * Time.deltaTime;
        }
    }
}