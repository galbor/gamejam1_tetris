using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private int maxScore = 1000;
    [SerializeField] private Transform endPoint;
    [SerializeField] private Color scoreIncreaseColor;
    [SerializeField] private Color highScorePassedColor;
    [SerializeField] private GameObject scoreCanvas;
    
    private int _score;
    private float _scoreScale;
    private int _highScore;
    private float _highScoreScale;
    private Color _scoreColor;
    private Color _highScoreColor;
    private bool _started;
    private float _startPointY;
    private int CurScore => Mathf.Min(maxScore, (int) (player.position.y - _startPointY) * maxScore / (int) (endPoint.position.y - _startPointY));
    
    private void Start()
    {
        _started = false;
        _score = 0;
        _highScore = PlayerPrefs.GetInt("HighScore", 0);
        scoreText.text = _score.ToString();
        highScoreText.text = _highScore.ToString();
        _scoreScale = scoreText.transform.localScale.x;
        _highScoreScale = highScoreText.transform.localScale.x;
        _scoreColor = scoreText.color;
        _highScoreColor = highScoreText.color;
        EventManagerScript.Instance.StartListening(EventManagerScript.PlayerFirstLand, OnStart);
    }

    private void OnStart(object arg0)
    {
        scoreText.transform.localScale = new Vector3(_scoreScale, _scoreScale, _scoreScale);
        highScoreText.transform.localScale = new Vector3(_highScoreScale, _highScoreScale, _highScoreScale);
        scoreText.color = _scoreColor;
        highScoreText.color = _highScoreColor;
        scoreText.text = "0";
        _started = true;
        _startPointY = player.position.y;
        scoreCanvas.SetActive(true);
    }

    private void Update()
    {
        if (!_started) return;
        if (CurScore > _score)
        {
            _score = CurScore>=maxScore?maxScore:CurScore;
            scoreText.text = _score.ToString();
            scoreText.transform.localScale *= 1.3f;
            scoreText.color = scoreIncreaseColor;
            if (_score > _highScore)
            {
                PlayerPrefs.SetInt("HighScore", _score);
                highScoreText.color = highScorePassedColor;
            }
            AudioManager.PlayScoreUp();
        }
        else // if the score is not increasing, decrease the scale
        {
            if (scoreText.transform.localScale.x > _scoreScale)
            {
                scoreText.transform.localScale /= 1.01f;
            }

            if (highScoreText.transform.localScale.x > _highScoreScale)
            {
                highScoreText.transform.localScale /= 1.01f;
            }
        }
    }
}
