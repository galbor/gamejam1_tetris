using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleColliderToButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ScaleColliderToButton Start");
        gameObject.GetComponent<BoxCollider2D>().size = new Vector2 (
            gameObject.GetComponent<RectTransform>().sizeDelta.x,
            gameObject.GetComponent<RectTransform>().sizeDelta.y
        );
    }
}
