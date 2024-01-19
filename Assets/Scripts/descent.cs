using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class descent : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _descender;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private KeyCode _descendKey = KeyCode.LeftShift;
    [SerializeField] private string _destroyerTag = "rockdestroyer";

    private Vector2 _startPosition;

    private Vector2 _moveVelocity;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("descent start");
        _startPosition = _descender.position;
        _moveVelocity = new Vector2(0, -_speed);
    }
    void FixedUpdate()
    {
        if (Input.GetKey(_descendKey))
        {
            _descender.velocity = _moveVelocity;
        }
        else
        {
            _descender.velocity = Vector2.zero;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("descent trigger enter");
        if (other.gameObject.CompareTag(_destroyerTag))
        {
            Debug.Log("descent resetting position");
            _descender.transform.position = _startPosition;
        }
    }
}
