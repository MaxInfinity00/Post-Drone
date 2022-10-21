using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private WinConditionUI _winConditionUI;
    [SerializeField]
    private GameObject _pauseMenuGUI;
    [SerializeField]
    private GameObject _pauseTintImg;
    [SerializeField]
    private GameObject _resumeButton;
    [SerializeField]
    private GameObject _winLoseScreenGUI;
    [SerializeField]
    private GameObject _restartButton;
    [SerializeField]
    private TextMeshProUGUI _winLoseText;
    [SerializeField]
    private string _WinSound;
    [SerializeField]
    private string _LoseSound;

    public static UIManager instance;
    private bool isPaused;
    private AudioManager _audioManager;

    void Awake()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        isPaused = false;
    }

    public void SendWinScore()
    {
        _winConditionUI.AddScore();
    }

    public void TogglePauseMenu()
    {
        Debug.Log("PAUSING");
        if (!isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    public void WinLoseScreen(bool hasWon, float totalTimePlayed)
    {
        Time.timeScale = 0f;
        if (hasWon)
        {
            if (!string.IsNullOrEmpty(_WinSound))
            {
                _audioManager.PlaySound(_WinSound);
            }
            isPaused = true;
            var eventSystem = EventSystem.current;
            eventSystem.SetSelectedGameObject(_restartButton, new BaseEventData(eventSystem));
            _winLoseScreenGUI.SetActive(true);
            _winLoseText.text = "YOU DELIVERED ALL PACKAGES IN " + totalTimePlayed + " SECONDS!";
        }
        else
        {
            if (!string.IsNullOrEmpty(_LoseSound))
            {
                _audioManager.PlaySound(_LoseSound);
            }
            isPaused = true;
            var eventSystem = EventSystem.current;
            eventSystem.SetSelectedGameObject(_restartButton, new BaseEventData(eventSystem));
            _winLoseScreenGUI.SetActive(true);
            _winLoseText.text = "TIMES UP\nYOU FAILED!";
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(_resumeButton, new BaseEventData(eventSystem));
        _pauseMenuGUI.SetActive(true);
        _pauseTintImg.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        _pauseTintImg.SetActive(false);
        _pauseMenuGUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartGame() // Loads the current scene again
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu() // Quit game button, Load into Main menu scene
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public bool IsGamePaused()
    {
        return isPaused;
    }
}
