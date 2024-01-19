using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killrocks : MonoBehaviour
{
    [SerializeField] private string _rockTag = "rock";
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("killrocks start");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(_rockTag))
        {
            Destroy(other.gameObject);
        }
    }
    
    
}
