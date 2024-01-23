using UnityEngine;

// IHoldable interface
public interface IHoldable
{
    public bool IsColliding { get; }    // Property to check if the object is colliding with something
    void PickUp(Transform parent);    // Method to be called when the object is picked up
    void Release();   // Method to be called when the object is released
    void OnMove();    // Method to be called when the holder is moving
    GameObject GetObject();  // Method to return the object
}