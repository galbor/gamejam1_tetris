using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedStarter : MonoBehaviour
{
    [SerializeField] private GameObject[] _objectsToActivate;
    [SerializeField] private WaterFilling _faucet;
    [SerializeField] private float _delay = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("DelayedStarter Start");
    }
    
    public void StartDelayedActivation()
    {
        StartCoroutine(StartDelayed());
    }
    
    IEnumerator StartDelayed()
    {
        yield return new WaitForSeconds(_delay);
        foreach (GameObject obj in _objectsToActivate)
        {
            obj.SetActive(true);
        }
        _faucet.OpenFaucet();
    }
}
