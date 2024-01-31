using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class descender : MonoBehaviour, IMovable
{
    [SerializeField] private string _destroyerTag = "rockdestroyer";

    private Rigidbody2D _descender;
    private EventManagerScript _eventManager;
    private bool goingdown = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("descender start");
        _descender = GetComponent<Rigidbody2D>();
        
        _eventManager = EventManagerScript.Instance;
        _eventManager.StartListening(EventManagerScript.StartDescent, StartDescent);
        _eventManager.StartListening(EventManagerScript.StopDescent, StopDescent);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("descender trigger enter");
        if (other.gameObject.CompareTag(_destroyerTag))
        {
            // Debug.Log("descender in kill zone");
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("tokinematic"))
        {
            // Debug.Log("descender enter tokinematic");
            _descender.isKinematic = true;
            _descender.velocity = Vector2.zero;
            _descender.angularVelocity = 0f;
            goingdown = false;
        }
    }
    
    private void StartDescent(object obj)
    {
        if (goingdown)
        {
            return;
        }

        // Debug.Log("descender start descent");
        _descender.velocity -= new Vector2(0, (float)obj);
        goingdown = true;
    }

    private void StopDescent(object obj)
    {
        if (goingdown)
        {
            // Debug.Log("descender stop descent");
            _descender.velocity += new Vector2(0, (float)obj);;
            goingdown = false;
        }
    }

    public Vector2 GetVelocity()
    {
        return _descender.velocity;
    }
}
