using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Object = System.Object;

public class LossTrigger : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private SpriteRenderer _gameOverText;
    [SerializeField] private SpriteRenderer[] _restartText;
    [SerializeField] private Camera _camera;
    
    [SerializeField] private float _fadeTime = 2f;
    [SerializeField] private float _restartFadeDelay = 3f;

    private Color gameOverTextColor;
    private ChromaticAberration _chromaticAberration;
    private bool _canLose;

    private void Awake()
    {
        gameOverTextColor = _gameOverText.color;
        _gameOverText.color = new Color(gameOverTextColor.r, gameOverTextColor.g, gameOverTextColor.b, 0);
        _chromaticAberration = _camera.GetComponent<PostProcessVolume>().profile.GetSetting<ChromaticAberration>();
        _canLose = true;
        EventManagerScript.Instance.StartListening(EventManagerScript.StartGame, turnOffRestartText);
        EventManagerScript.Instance.StartListening(EventManagerScript.PlayerHit, Lose);
        EventManagerScript.Instance.StartListening(EventManagerScript.PlayerDrowned, Lose);
        EventManagerScript.Instance.StartListening(EventManagerScript.Win, DisableLoss);
    }

    private void turnOffRestartText(object arg0)
    {
        for (int i = 0; i < _restartText.Length; ++i)
        {
            _restartText[i].DOFade(0, 0);
        }
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
        _chromaticAberration.enabled.value = true;
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
