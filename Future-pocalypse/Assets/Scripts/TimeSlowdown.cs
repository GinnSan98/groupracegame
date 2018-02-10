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
    [SerializeField]
    private CarDriving cd;

    void Update()
    {
        if (cd.cameracontrol == true)
        {
            if (Input.GetMouseButtonDown(0) == true)
            {
                RaycastHit hit;
                Ray ray = mycam.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out hit);
                //   Physics.Raycast(mycam.transform.position, mycam.ViewportToWorldPoint(Input.mousePosition), out hit, 100f);
                Debug.DrawRay(mycam.transform.position, ray.direction * hit.distance, Color.green, Mathf.Infinity);
                if (hit.transform.tag == "Enemy")
                {
                    target = hit.transform;
                }
            }
        }


        if (target != null)
        {
            if (Input.GetKeyDown("v") && targetting == false && transitioning == false)
            {
                targetting = true;
                TargetButton.SetActive(true);
                Time.timeScale = 0.5f;
                Mathf.RoundToInt(60 / Time.timeScale);
                cd.cameracontrol = false;
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
        mycam.fieldOfView = 70;
        cd.cameracontrol = true;
    }

    private IEnumerator zoomin()
    {
        transitioning = true;
        mycam.fieldOfView = 70;
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

