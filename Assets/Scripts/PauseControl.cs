using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseControl : MonoBehaviour
{
    [SerializeField]
    private GameObject pausemenu;
    [SerializeField]
    private GameObject pointer;
    [SerializeField]
    private GameObject pointerstart;
    [SerializeField]
    private int pauseselection = 0; //0 = resume; 1 = restart; 2 = quit to menu;

    private void Start()
    {
        StartCoroutine(MyUpdate());
    }


    public void TurnonMenu()
    {
        pausemenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void TurnoffMenu()
    {
        pauseselection = 0;
        Time.timeScale = 1;
        pausemenu.SetActive(false);
    }
    public void Quittomenu()
    {
        pausemenu.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void Restart()
    {
        pausemenu.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    private void Movecursor()
    {
        pointer.transform.localPosition = pointerstart.transform.localPosition - new Vector3(0,(70* pauseselection));
    }
    private IEnumerator MyUpdate()
    {
        while (true)
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Escape));
            TurnonMenu();
            bool canstop = false;
            while (canstop == false)
            {
                yield return new WaitUntil(() => Input.anyKeyDown);

                print(Input.anyKeyDown);

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    switch (pauseselection)
                    {
                        case (0):
                            {
                                TurnoffMenu();
                                break;
                            }
                        case (1):
                            {
                                Restart();
                                break;
                            }
                        case (2):
                            {
                                Quittomenu();
                                break;
                            }

                    }
                    canstop = true;
                }
                else if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    canstop = true;
                    TurnoffMenu();
                    
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow)||Input.GetKeyDown(KeyCode.W))
                {
                    pauseselection += 1;

                    if (pauseselection > 2)
                    {
                        pauseselection = 0;
                    }
                    Movecursor();
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.S))
                {
                    pauseselection -= 1;

                    if (pauseselection < 0)
                    {
                        pauseselection = 2;
                    }
                    Movecursor();
                }



            }
            
        }
    }
}

