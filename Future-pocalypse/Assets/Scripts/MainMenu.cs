using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void LoadScean(string sceanName)
    {
        SceneManager.LoadScene(sceanName);
    }


    public void QuitGame()
    {
        Debug.Log("Game will have closed.");
        Application.Quit();
    }
}
