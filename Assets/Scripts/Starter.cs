using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;
using UnityEngine.Serialization;

public class Starter : MonoBehaviour
{
    [SerializeField] private KeyCode _startKey = KeyCode.Return; //Enter
    
    [SerializeField] private GameObject[] _objectsToActivateDelayed;
    [SerializeField] private GameObject[] _objectsToActivateImmediately;
    [SerializeField] private GameObject[] _objectsToDeactivateImmediately;
    [SerializeField] private CameraAscention _cameraAscender;
    [SerializeField] private drop _drop;
    
    
    [SerializeField] private WaterFilling _faucet;
    [SerializeField] private float _faucetStartDelay = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Starter Start");
        if (EventManagerScript.Instance.isRestarted())
        {
            OnStartPressed();
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(_startKey))
        {
            OnStartPressed();
            _startKey = KeyCode.None;
        }
    }
    
    public void OnStartPressed()
    {
        _startKey = KeyCode.None; //in case it's restarted
        EventManagerScript.Instance.TriggerEvent(EventManagerScript.StartGame, null);
        AudioManager.PlayStartButtonPressed();
        AudioManager.StopStartBackground();
        AudioManager.PlayHomeBackground();
        AudioManager.PlayRoomTone();
        AudioManager.PlayPeopleFolies();
        // _drop.SwitchAutomatic();
        
        foreach (GameObject obj in _objectsToDeactivateImmediately)
        {
            obj.SetActive(false);
        }
        foreach(GameObject obj in _objectsToActivateImmediately)
        {
            obj.SetActive(true);
        }
        StartDelayedActivation();
    }
    
    private void StartDelayedActivation()
    {
        StartCoroutine(StartDelayed());
    }
    
    IEnumerator StartDelayed()
    {
        yield return new WaitForSeconds(_faucetStartDelay);
        foreach (GameObject obj in _objectsToActivateDelayed)
        {
            obj.SetActive(true);
        }
        _faucet.OpenFaucet();
        _cameraAscender.GameStarted();
        // gameObject.SetActive(false);
    }
}
