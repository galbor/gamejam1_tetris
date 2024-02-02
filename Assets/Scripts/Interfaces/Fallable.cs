using UnityEngine;

namespace Interfaces
{
    public interface IFallable
    {
        public bool IsFalling();
        public void AddForce(Vector2 force);
    }
}