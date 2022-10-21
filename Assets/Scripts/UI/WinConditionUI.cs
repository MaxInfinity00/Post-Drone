using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinConditionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private int _scoreAmountToWin = 10;
    [SerializeField] private Animation _scoreAnimation;
    [SerializeField] private Animation _scoreAnimation2;

    private int _currentScore;

    void Awake()
    {
        _currentScore = 0;
        _scoreText.text = _currentScore.ToString() + "/" + _scoreAmountToWin.ToString();
    }


    public void AddScore() {
        _scoreAnimation.Play();
        _scoreAnimation2.Play();
        _currentScore++;
        UpdateScoreText();
        // TODO: Add happy animation to the UI
        if (_currentScore == _scoreAmountToWin)
        {
            UIManager.instance.WinLoseScreen(true, GameManager.instance.GetTimeElapsed());
        }
        
    }

    private void UpdateScoreText()
    {
        _scoreText.text = _currentScore.ToString() + "/" + _scoreAmountToWin.ToString();
    }
}
