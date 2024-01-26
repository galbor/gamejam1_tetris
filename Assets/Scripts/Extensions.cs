using UnityEngine;

public static class Extensions
{
    private static LayerMask _defaultLayerMask = LayerMask.GetMask("Default");
    
    /**
     * Raycast from the center of the rigidbody in the given direction.
     * Used for example to check if the player is touching the ground.
     */
    public static bool Raycast(this Rigidbody2D rigidbody, Vector2 direction)
    {
        if (rigidbody.isKinematic)
        {
            return false;
        }

        float radius = .05f;
        float distance = .35f;

        RaycastHit2D hit = Physics2D.CircleCast(rigidbody.position, radius, direction.normalized, distance, _defaultLayerMask);
        return hit.collider != null && hit.rigidbody != rigidbody;
    }
    
    /**
     * Check if the vector between the two transforms is pointing in the given direction.
     * Used for example to check if the player is hit from above.
     */
    public static bool IsDirectionFrom(this Transform transform, Transform other, Vector2 direction)
    {
        Vector2 toOther = other.position - transform.position;
        return Vector2.Dot(toOther, direction) > .25f;
    }
}
