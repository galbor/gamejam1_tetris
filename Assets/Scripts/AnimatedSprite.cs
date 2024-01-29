using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AnimatedSprite : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float frameRate = 1f / 6f;
    
    private SpriteRenderer _spriteRenderer;
    private int _currentFrame = 0;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(Animate), frameRate, frameRate);
    }
    
    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Animate()
    {
        _currentFrame++;
        if (_currentFrame >= sprites.Length)
        {
            _currentFrame = 0;
        }

        if (_currentFrame >= 0 && _currentFrame < sprites.Length)
        {
            _spriteRenderer.sprite = sprites[_currentFrame];
        }
    }
}
