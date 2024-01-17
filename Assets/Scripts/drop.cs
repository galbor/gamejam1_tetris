using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drop : MonoBehaviour
{
    [SerializeField] private GameObject _dropper;
    [SerializeField] private Transform _drop_parent;
    
    [SerializeField] private KeyCode _dropKey = KeyCode.Space;
    [SerializeField] private float _maxdroprotation = 45f;
 
    private GameObject _dropPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        _dropPrefab = Resources.Load<GameObject>("rock1");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(_dropKey))
        {
            GameObject drop = Instantiate(_dropPrefab, _dropper.transform.position, Quaternion.identity, _drop_parent);
            drop.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-_maxdroprotation, _maxdroprotation));
        }
    }
}
