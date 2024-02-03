using System;
using Effects;
using Interfaces;
using UnityEngine;

namespace Dishes
{
    public class BrokenBehavior : MonoBehaviour
    {
        private BreakableBehavior _parent;
        private FixedJoint2D _joint;
        private float _timeToAllowBreak = 0.5f;
        private bool _isJointNull;
        private const float BreakForce = 150f;
        private const double Tolerance = .01f;

        private void Awake()
        {
            _parent = GetComponentInParent<BreakableBehavior>();
            _joint = GetComponent<FixedJoint2D>();
        }
        
        private void Start()
        {
            _isJointNull = _joint == null;
        }
        
        private void Update()
        {
            if (_isJointNull || Math.Abs(_joint.breakForce - BreakForce) < Tolerance) return;
            if (_timeToAllowBreak > 0)
            {
                _timeToAllowBreak -= Time.deltaTime;
            }
            if (_timeToAllowBreak <= 0)
            {
                _joint.breakForce = BreakForce;
            }
        }

        private void OnJointBreak2D(Joint2D brokenJoint)
        {
            _parent.Break();
            _isJointNull = true;
        }
    }
}
