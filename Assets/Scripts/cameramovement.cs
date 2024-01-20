using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameramovement : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private KeyCode _goDownKey = KeyCode.DownArrow;
    private EventManagerScript _eventManager;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("cameramovement start");
        _eventManager = EventManagerScript.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(_goDownKey))
        {
            _eventManager.TriggerEvent("start descent", _speed);
        }
        else
        {
            _eventManager.TriggerEvent("stop descent", _speed);
        }
    }
}
