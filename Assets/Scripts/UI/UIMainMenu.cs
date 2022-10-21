using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    [Header("Lerp")]
    [SerializeField] private GameObject drones;

    private float desiredDuration = 6f;
    private float elapsedTime;
    private float timer = 6.2f;

    private void Update()
    {
        DroneAnimation();
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Loads next index scene
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    private void DroneAnimation()
    {
        timer -= Time.deltaTime;
        elapsedTime += Time.deltaTime;
        float percentageComplete = elapsedTime / desiredDuration;
        drones.transform.position = Vector2.Lerp(new Vector2(1100, 600), new Vector2(-6426, 600), percentageComplete);
        
        if (timer <= 0)
        {
            drones.transform.position = new Vector2(1100, 600);
            timer = 6.2f;
            elapsedTime = 0;
        }
    }
}
