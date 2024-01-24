using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoldableBehavior : MonoBehaviour, IHoldable
{
    public GameObject GetObject()
    {
        return gameObject;
    }
}
