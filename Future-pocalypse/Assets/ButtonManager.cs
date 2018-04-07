using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{

    // Exit the pause menu
    public void Resume()
    {
        GameObject pauseObj = GameObject.FindGameObjectWithTag("PauseMenu");
        PauseManager pauseMgr = pauseObj.GetComponent<PauseManager>();

        if (pauseMgr != null)
        {
            pauseMgr.GetComponent<PauseManager>().Resume();
        }
    }

    // Open the pause menu
    void PauseGame()
    {
        GameObject pauseObj = GameObject.FindGameObjectWithTag("PauseMenu");
        PauseManager pauseMgr = pauseObj.GetComponent<PauseManager>();

        if (pauseMgr != null)
        {
            pauseMgr.GetComponent<PauseManager>().Resume();
        }
    }

    // Loads a new sceane, use string to select which sceane to load
    public void LoadScean(string sceanName)
    {
        SceneManager.LoadScene(sceanName);
    }

    // Reloads the current scene
    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Exits the game
    public void QuitGame()
    {
        Debug.Log("Game will have closed.");
        Application.Quit();
    }
}