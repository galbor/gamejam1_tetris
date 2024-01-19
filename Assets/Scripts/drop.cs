using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drop : MonoBehaviour
{
    [SerializeField] private GameObject _dropper;
    [SerializeField] private Transform _drop_parent;
    
    [SerializeField] private KeyCode _dropKey = KeyCode.Space;
    [SerializeField] private float _maxdroprotation = 45f;
    [SerializeField] private float _maxdropforce = 10f;
 
    private GameObject _dropPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("drop start");
        _dropPrefab = Resources.Load<GameObject>("rock1");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(_dropKey))
        {
            GameObject drop = Instantiate(_dropPrefab, _dropper.transform.position, Quaternion.identity,
                _drop_parent);
            Rigidbody2D dropRb = drop.GetComponent<Rigidbody2D>();
            dropRb.AddTorque(Random.Range(-_maxdroprotation, _maxdroprotation), ForceMode2D.Impulse);
            dropRb.AddForce(new Vector2(Random.Range(-_maxdropforce, _maxdropforce), 0), ForceMode2D.Impulse);
        }
    }
}
