using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Dishes;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class drop : MonoBehaviour
{
    [SerializeField] private Transform _drop_parent; //for organization's sake
    [SerializeField] private GameObject _dropperPointer; //the little triangle that shows where the drop will be
    
    [SerializeField] private GameObject[] _dropPrefabs;
    [SerializeField] private GameObject[] _utenstilDropPrefabs;
    [SerializeField] private GameObject[] _heroDropPrefabs;
    [SerializeField] private int HeroDropInverval = 30;
    [SerializeField] private float _utensilDropChance = 0.25f;

    [SerializeField] private GameObject _rightWall;
    [SerializeField] private GameObject _leftWall;
    [SerializeField] private float _distanceFromWall = 0.5f; //distance from the wall to not spawn
    [SerializeField] private float _maxDropRotation = 45f; //random rotation when dropping
    [SerializeField] private float _maxDropForce = 10f; //random downwards force when dropping

    [SerializeField] private float _moveDelay = 0.25f;
    [SerializeField] private float _minDropInterval = 1f;
    [SerializeField] private float _maxDropInterval = 5f;
    [SerializeField] private float _accelerationRate = 0.5f; //how much the drop interval decreases when dropping with warning

    [SerializeField] private Color _pointerColorWarning = Color.red; //when dropping with warning, final color
    [SerializeField] private float _pointerSizeWarning = 1.5f; //when dropping with warning, final size
    [SerializeField] private float _pointerTimeWarning = 0.5f; //when dropping with warning, time to reach final color and size
    [SerializeField] private float dropRandomVariance = 2f;
    
    private GameObject _nextDropPrefab;
    private SpriteRenderer _dropperPointerSpriteRenderer;
    private float rightBoundX;
    private float leftBoundX;
    private bool isWarning = false;
    private bool isAutomatic = false;
    private float dropTimer;
    private int _dropCounter = 0;
    private RandomNormalDistribution _rand;
    [SerializeField] private WaterShapeController wsc;

    void Start()
    {
        Debug.Log("drop start");
        _dropperPointerSpriteRenderer = _dropperPointer.GetComponent<SpriteRenderer>();
        dropTimer = GetDropInterval();
        
        rightBoundX = _rightWall.transform.position.x - _distanceFromWall - _rightWall.transform.localScale.x / 2;
        leftBoundX = _leftWall.transform.position.x + _distanceFromWall + _leftWall.transform.localScale.x / 2;
        var mean = (leftBoundX + rightBoundX) / 2;
        _rand = new RandomNormalDistribution(mean, dropRandomVariance, leftBoundX, rightBoundX);
        
        _nextDropPrefab = _dropPrefabs[0];
        EventManagerScript.Instance.StartListening(EventManagerScript.PlayerFirstLand, SwitchAutomatic);
        EventManagerScript.Instance.StartListening(EventManagerScript.Win, SwitchAutomatic);
    }

    // Update is called once per frame
    void Update()
    {

        //every _dropInterval seconds, drop a random prefab (if automatic)
        if (isAutomatic)
        {
            if (dropTimer > 0)
            {
                dropTimer -= Time.deltaTime;
            }
            else
            {
                if (!isWarning)
                {
                    StartCoroutine(DropAnimation(_pointerColorWarning, _pointerSizeWarning, _pointerTimeWarning));
                }
            }
        }
    }

    /**
     * Drops a gameobject from the dropper, with a given force and rotation
     * @param force is downwards force
     */
    private void Drop(GameObject prefab, float force, float torque)
    {
        GameObject newDrop = Instantiate(prefab, transform.position,
            prefab.transform.rotation, _drop_parent);
        List<MovableBehavior> movable = new List<MovableBehavior>();
        List<Rigidbody2D> dropRb = new List<Rigidbody2D>();
        if (newDrop.TryGetComponent(out IBreakable breakable))
        {
            foreach (var variable in newDrop.GetComponentsInChildren<MovableBehavior>())
            {
                movable.Add(variable);
            }
            foreach (var variable in newDrop.GetComponentsInChildren<Rigidbody2D>())
            {
                dropRb.Add(variable);
            }
        } else
        { 
            movable.Add(newDrop.GetComponent<MovableBehavior>());
            dropRb.Add(newDrop.GetComponent<Rigidbody2D>());
        } 
        foreach (var variable in movable)
        {
            variable.Init(wsc);
        }
        foreach (var variable in dropRb)
        {
            variable.AddForce(-force * transform.up, ForceMode2D.Impulse);
            variable.AddTorque(torque, ForceMode2D.Impulse);
        }
        // also add drop sound
        AudioManager.PlayFallingObject();
        ++_dropCounter;
    }
    
    //drops a gameobject from the dropper, with a random rotation and a random force
    private void Drop(GameObject prefab)
    {
        float force = prefab.CompareTag("gentlerock") ? 0 : Random.Range(-_maxDropForce, 0);
        float torque = prefab.CompareTag("gentlerock") ? 0 : Random.Range(-_maxDropRotation, _maxDropRotation);
        Drop(prefab, force, torque);
    }

    //drops a random gameobject from the prefab list
    private void Drop()
    {
        Drop(_nextDropPrefab);
        if (_dropCounter % HeroDropInverval == 0 && _heroDropPrefabs.Length != 0)
        {
            _nextDropPrefab = _heroDropPrefabs[Random.Range(0, _heroDropPrefabs.Length)];
        }
        else if (Random.Range(0, 100) <= _utensilDropChance*100)
        {
            _nextDropPrefab = _utenstilDropPrefabs[Random.Range(0, _utenstilDropPrefabs.Length)];
        }
        else
        {
            _nextDropPrefab = _dropPrefabs[Random.Range(0, _dropPrefabs.Length)];
        }
    }

    /**
     * small animation to warn when dropping
     * drops a random prefab when the animation is done
     * changes the dropper's position and rotation when done
     */
    IEnumerator DropAnimation(Color final_color, float final_size_multiplier, float time)
    {
        isWarning = true;
        float time_elapsed = 0f;
        Color initial_color = _dropperPointerSpriteRenderer.color;
        Vector3 initial_size = _dropperPointer.transform.localScale;
        while (time_elapsed < time)
        {
            _dropperPointerSpriteRenderer.color = Color.Lerp(initial_color, final_color, time_elapsed / time);
            _dropperPointer.transform.localScale = 
                Vector3.Lerp(initial_size, initial_size * final_size_multiplier, time_elapsed / time);
            time_elapsed += Time.deltaTime;
            yield return null;
        }

        Drop();
        _dropperPointerSpriteRenderer.color = initial_color;
        _dropperPointer.transform.localScale = initial_size;
        dropTimer = GetDropInterval();
        StartCoroutine(ChangeToNewRandomLocation());
        isWarning = false;
    }


    //moves the dropper and rotates it randomly
    IEnumerator ChangeToNewRandomLocation()
    {
        yield return new WaitForSeconds(Math.Min(_moveDelay, _minDropInterval));
        transform.position = new Vector3(_rand.GenerateRandomNumber(), transform.position.y, 0);//Random.Range(leftBoundX, rightBoundX), transform.position.y, 0);
    }

    public void SwitchAutomatic(object obj)
    {
        isAutomatic = !isAutomatic;
        _dropperPointer.SetActive(isAutomatic);
        
    }
    
    private float GetDropInterval()
    {
        float res = Random.Range(_minDropInterval, _maxDropInterval);
        _minDropInterval = Math.Max(_minDropInterval - _accelerationRate, 0);
        _maxDropInterval = Math.Max(_maxDropInterval - _accelerationRate, _minDropInterval);
        return res;
    }
}
