using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Object = System.Object;

public class LossTrigger : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private SpriteRenderer _gameOverText;
    [SerializeField] private SpriteRenderer[] _restartText;
    
    [SerializeField] private float _fadeTime = 2f;
    [SerializeField] private float _restartFadeDelay = 3f;

    private Color gameOverTextColor;
    private Color[] restartTextColors;
    
    private void Awake()
    {
        restartTextColors = new Color[_restartText.Length];
        gameOverTextColor = _gameOverText.color;
        _gameOverText.color = new Color(gameOverTextColor.r, gameOverTextColor.g, gameOverTextColor.b, 0);
        for (int i = 0; i < restartTextColors.Length; ++i)
        {
            restartTextColors[i] = _restartText[i].color;
            _restartText[i].color = new Color(restartTextColors[i].r, restartTextColors[i].g, restartTextColors[i].b, 0);
        }
        EventManagerScript.Instance.StartListening("PlayerHit", Lose);
        EventManagerScript.Instance.StartListening("PlayerDrowned", Lose);
    }

    public void Lose(Object obj=null)
    {
        Debug.Log("Game Over");
        StartCoroutine(GameOverCoroutine());
    }
    
    private IEnumerator GameOverCoroutine()
    {
        _playerMovement.CanMove = false;
        _gameOverText.DOFade(1, _fadeTime);
        yield return new WaitForSeconds(_restartFadeDelay);
        foreach (SpriteRenderer text in _restartText)
        {
            text.DOFade(1, _fadeTime);
        }
    }
    
}
