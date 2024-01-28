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
    [SerializeField] private TextMeshProUGUI _gameOverText;
    [SerializeField] private TextMeshProUGUI _restartText;

    private void Awake()
    {
        _gameOverText.alpha = 0;
        _restartText.alpha = 0;
        EventManagerScript.Instance.StartListening("PlayerHit", Lose);
    }

    public void Lose(Object obj)
    {
        Debug.Log("Game Over");
        StartCoroutine(GameOverCoroutine());
    }
    
    private IEnumerator GameOverCoroutine()
    {
        _playerMovement.CanMove = false;
        _gameOverText.DOFade(1, 2f);
        yield return new WaitForSeconds(3f);
        _restartText.DOFade(1, 2f);
    }
    
}
