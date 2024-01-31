using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameramovement : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private KeyCode _goDownKey = KeyCode.DownArrow;
    private EventManagerScript _eventManager;
    private bool goingdown = false;
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("cameramovement start");
        _eventManager = EventManagerScript.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(_goDownKey) && goingdown)
        {
            goingdown = false;
            _eventManager.TriggerEvent(EventManagerScript.StopDescent, _speed);
        }
        else if (goingdown || Input.GetKeyDown(_goDownKey))
        {
            goingdown = true;
            _eventManager.TriggerEvent(EventManagerScript.StartDescent, _speed);
        }
    }
}
