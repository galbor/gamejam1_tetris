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
    private bool _canLose;

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
        _canLose = true;
        EventManagerScript.Instance.StartListening(EventManagerScript.PlayerHit, Lose);
        EventManagerScript.Instance.StartListening(EventManagerScript.PlayerDrowned, Lose);
        EventManagerScript.Instance.StartListening(EventManagerScript.Win, DisableLoss);
    }

    private void DisableLoss(object arg0)
    {
        _canLose = false;
    }

    public void Lose(Object obj=null)
    {
        if (!_canLose) return;
        Debug.Log("Game Over");
        EventManagerScript.Instance.TriggerEvent(EventManagerScript.Lose, null);
        StartCoroutine(GameOverCoroutine());
    }
    
    private IEnumerator GameOverCoroutine()
    {
        _playerMovement.CanMove = false;
        _gameOverText.DOFade(1, _fadeTime);
        yield return new WaitForSeconds(_restartFadeDelay);
        foreach (SpriteRenderer text in _restartText)
        {
            text.transform.gameObject.SetActive(true);
            text.DOFade(1, _fadeTime);
        }
    }
    
}
