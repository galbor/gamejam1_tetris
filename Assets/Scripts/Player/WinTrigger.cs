using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    [SerializeField] private GameObject[] _winSignifiers;
    [SerializeField] private drop _dropper;
    [SerializeField] private WaterFilling _faucet;

    void Start()
    {
        Debug.Log("WinTrigger Start");
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            Debug.Log("WinTrigger OnTriggerEnter2D Finish");
            EventManagerScript.Instance.TriggerEvent("Win", null);
            foreach (GameObject signifier in _winSignifiers)
            {
                signifier.SetActive(true);
            }
            _dropper.gameObject.SetActive(false);
        }
    }
}
