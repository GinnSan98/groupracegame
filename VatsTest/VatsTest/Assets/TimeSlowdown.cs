using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlowdown : MonoBehaviour
{
    public GameObject TargetButton;
    [SerializeField]
    private Camera mycam;
    private bool targetting;
    public Transform target;
    private bool transitioning;

    void Update()
    {

        if (Input.GetKeyDown("v") && targetting == false && transitioning == false)
        {
            targetting = true;
            TargetButton.SetActive(true);
            Time.timeScale = 0.5f;
            Mathf.RoundToInt(60 / Time.timeScale);
            StartCoroutine(zoomin());

        }
        else if (Input.GetKeyDown("v") && targetting == true && transitioning == false)
        {
            targetting = false;
            TargetButton.SetActive(false);
            Time.timeScale = 1f;
            Mathf.RoundToInt(60 / Time.timeScale);
            StartCoroutine(zoomout());

        }


    }

    private IEnumerator zoomout()
    {
        transitioning = true;
        for (int i = 0; i < 30; i++)
        {
            mycam.fieldOfView++;
            yield return new WaitForEndOfFrame();
        }
        mycam.transform.localRotation =  Quaternion.Euler(new Vector3(10, 0, 0)) ;
        transitioning = false;
    }

    private IEnumerator zoomin()
    {
        transitioning = true;
        mycam.transform.LookAt(target);
        for (int i = 0; i < 30;i++)
        {
            mycam.fieldOfView--;
            yield return new WaitForEndOfFrame();
        }
        transitioning = false;
        yield return 0;
    }

}

