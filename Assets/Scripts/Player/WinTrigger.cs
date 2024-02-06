using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    [SerializeField] private GameObject[] _winSignifiers;
    [SerializeField] private drop _dropper;
    [SerializeField] private WaterFilling _faucet;
    [SerializeField] private SpriteRenderer _winScreen;
    [SerializeField] private float _winScreenFadeTime = 2f;

    void Start()
    {
        Debug.Log("WinTrigger Start");
        // _winScreen.color = new Color(_winScreen.color.r, _winScreen.color.g, _winScreen.color.b, 0);
        _winScreen.DOFade(0, 0);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            Debug.Log("WinTrigger OnTriggerEnter2D Finish");
            EventManagerScript.Instance.TriggerEvent(EventManagerScript.Win, null);
            foreach (GameObject signifier in _winSignifiers)
            {
                signifier.SetActive(true);
            }
            _winScreen.DOFade(1, _winScreenFadeTime);
            _dropper.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("faucet stopper"))
        {
            _faucet.CloseFaucet();
        }
    }
}
