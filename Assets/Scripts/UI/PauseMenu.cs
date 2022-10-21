using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseTintImg;

    [SerializeField]
    private GameObject _resumeButton;

    public void PauseGame()
    {
        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(_resumeButton, new BaseEventData(eventSystem));
        _pauseTintImg.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        _pauseTintImg.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartGame() // Loads the current scene again
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu() // Quit game button, Load into Main menu scene
    {
        SceneManager.LoadScene(0);
    }
}
