using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteRenderer : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private PlayerMovement _movement;

    [SerializeField] private Sprite idle;
    [SerializeField] private Sprite jump;
    [SerializeField] private Sprite turn;
    [SerializeField] private Sprite run;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _movement = GetComponent<PlayerMovement>();
    }

    private void LateUpdate()
    {
        if (_movement.Jumping) {
            _spriteRenderer.sprite = jump;
        } else if (_movement.Turning)
        {
            _spriteRenderer.sprite = turn;
        } else if (_movement.Running)
        {
            _spriteRenderer.sprite = run;
        } else
        {
            _spriteRenderer.sprite = idle;
        }
    }
}
