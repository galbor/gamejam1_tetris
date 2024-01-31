using System;
using UnityEngine;

public static class Helpers
{
    public static Vector3[] GetSpriteCorners(SpriteRenderer renderer)
    {
        Vector3 topRight = renderer.transform.TransformPoint(renderer.sprite.bounds.max);
        var sprite = renderer.sprite;
        Vector3 topLeft = renderer.transform.TransformPoint(new Vector3(sprite.bounds.max.x, sprite.bounds.min.y, 0));
        Vector3 botLeft = renderer.transform.TransformPoint(renderer.sprite.bounds.min);
        Vector3 botRight = renderer.transform.TransformPoint(new Vector3(renderer.sprite.bounds.min.x, sprite.bounds.max.y, 0));
        return new Vector3[] { topRight, topLeft, botLeft, botRight };
    }
    
    /**
     * Check if the vector between the two transforms is pointing in the given direction.
     * Used for example to check if the player is hit from above.
     */
    public static bool IsDirectionFrom(this Transform transform, Transform other, Vector2 direction)
    {
        Vector2 toOther = other.position - transform.position;
        return Vector2.Dot(toOther.normalized, direction) > .6f;
    }
}
