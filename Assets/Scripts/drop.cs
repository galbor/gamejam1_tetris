using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Dishes;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class drop : MonoBehaviour
{
    [SerializeField] private Transform _drop_parent; //for organization's sake
    [SerializeField] private GameObject _dropperPointer; //the little triangle that shows where the drop will be
    
    [SerializeField] private GameObject[] _dropPrefabs;
    
    [SerializeField] private KeyCode _dropKey = KeyCode.Space; //manually drop
    [SerializeField] private KeyCode _moveLeftKey = KeyCode.A; //move the dropper left
    [SerializeField] private KeyCode _moveRightKey = KeyCode.D; //move the dropper right
    [SerializeField] private KeyCode _rotateLeftKey = KeyCode.Q; //rotate the dropper left 
    [SerializeField] private KeyCode _rotateRightKey = KeyCode.E; //rotate the dropper right
    [SerializeField] private KeyCode _switchAutomaticKey = KeyCode.F; //switch between automatic and manual dropping


    [SerializeField] private float _moveSpeed = 5f; //manual move speed
    [SerializeField] private float _rotateSpeed = 20f; //manual rotate speed
    [SerializeField] private float _maxDropperDirection = 65f; //max angle of the dropper
    [SerializeField] private GameObject _rightWall;
    [SerializeField] private GameObject _leftWall;
    [SerializeField] private float _distanceFromWall = 0.5f; //distance from the wall to not spawn
    [SerializeField] private float _maxDropRotation = 45f; //random rotation when dropping
    [SerializeField] private float _maxDropForce = 10f; //random downwards force when dropping
    
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
    private float originalRotation;
    private RandomNormalDistribution _rand;
    [SerializeField] private WaterShapeController wsc;

    void Start()
    {
        Debug.Log("drop start");
        _dropperPointerSpriteRenderer = _dropperPointer.GetComponent<SpriteRenderer>();
        dropTimer = GetDropInterval();
        originalRotation = transform.eulerAngles.z;
        
        rightBoundX = _rightWall.transform.position.x - _distanceFromWall - _rightWall.transform.localScale.x / 2;
        leftBoundX = _leftWall.transform.position.x + _distanceFromWall + _leftWall.transform.localScale.x / 2;
        var mean = (leftBoundX + rightBoundX) / 2;
        _rand = new RandomNormalDistribution(mean, dropRandomVariance, leftBoundX, rightBoundX);
        
        _nextDropPrefab = _dropPrefabs[0];
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(_dropKey))
        // {
        //     Drop();
        // }
        // //move
        // if (Input.GetKey(_moveLeftKey))
        // {
        //     transform.position = new Vector2(Math.Max(leftBoundX, transform.position.x - _moveSpeed * Time.deltaTime),
        //         transform.position.y);
        // }
        // else if (Input.GetKey(_moveRightKey))
        // {
        //     transform.position = new Vector2(Math.Min(rightBoundX, transform.position.x + _moveSpeed * Time.deltaTime),
        //         transform.position.y);
        // }
        
        //rotate
        // if (Input.GetKey(_rotateRightKey))
        // {
        //     transform.Rotate(0, 0, Math.Min(_rotateSpeed * Time.deltaTime, _maxDropperDirection));
        // }
        // else if (Input.GetKey(_rotateLeftKey))
        // {
        //     transform.Rotate(0, 0, Math.Max(-_rotateSpeed * Time.deltaTime, -_maxDropperDirection));
        // }
        //
        // if (Input.GetKeyDown(_switchAutomaticKey))
        // {
        //     SwitchAutomatic();
        // }

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
        GameObject drop = Instantiate(prefab, transform.position,
            transform.rotation, _drop_parent);
        drop.GetComponent<MovableBehavior>().Init(wsc);
        Rigidbody2D dropRb = drop.GetComponent<Rigidbody2D>();
        dropRb.AddForce(-force * transform.up, ForceMode2D.Impulse);
        dropRb.AddTorque(torque, ForceMode2D.Impulse);
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
        _nextDropPrefab = _dropPrefabs[Random.Range(0, _dropPrefabs.Length)];
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
        _dropperPointer.transform.localScale = 
            new Vector3(_nextDropPrefab.transform.localScale.x, initial_size.y, initial_size.z);
        ChangeToNewRandomLocation();
        dropTimer = GetDropInterval();
        isWarning = false;
    }


    //moves the dropper and rotates it randomly
    private void ChangeToNewRandomLocation()
    {
        transform.position = new Vector3(_rand.GenerateRandomNumber(), transform.position.y, 0);//Random.Range(leftBoundX, rightBoundX), transform.position.y, 0);
        transform.eulerAngles = new Vector3( 0,0, Random.Range(-_maxDropperDirection, _maxDropperDirection) + originalRotation);
    }

    public void SwitchAutomatic()
    {
        isAutomatic = !isAutomatic;
    }
    
    private float GetDropInterval()
    {
        float res = Random.Range(_minDropInterval, _maxDropInterval);
        _minDropInterval = Math.Max(_minDropInterval - _accelerationRate, 0);
        _maxDropInterval = Math.Max(_maxDropInterval - _accelerationRate, _minDropInterval);
        return res;
    }
}
