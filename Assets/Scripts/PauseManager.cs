using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{

    public static bool isPaused = false;
    public GameObject pauseMenuUI;

    private void Start()
    {
        Resume();
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                PauseGame();
            }
        }
    }

    #region Pause and resume game

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    #endregion
}