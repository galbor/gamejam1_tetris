using System;
using Effects;
using Interfaces;
using UnityEngine;

namespace Dishes
{
    public class BreakableBehavior : MonoBehaviour, IBreakable
    {
        public void Break()
        {
            foreach (Transform child in transform)
            {
                child.parent = null;
            }
        }
    }
}
