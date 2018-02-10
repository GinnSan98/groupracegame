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
    private bool tickdown;
    [SerializeField]
    private float currentcharge;
    [SerializeField]
    private bool canturnon;




    public void missle()
    {
        cancelpower();
    }

    public void missledash()
    {
        transform.LookAt(target);
        transform.position += transform.up;
        GetComponent<Rigidbody>().AddForce(transform.forward * 500, ForceMode.VelocityChange);
        targetting = false;
        TargetButton.SetActive(false);
        Time.timeScale = 1f;
        Application.targetFrameRate =  Mathf.RoundToInt(60 / Time.timeScale);
        cancelpower();
    }

    private void cancelpower()
    {
        Time.timeScale = 1f;
        Application.targetFrameRate = Mathf.RoundToInt(60 / Time.timeScale);
        canturnon = false;
        currentcharge = 10;
        StartCoroutine(zoomout());
        target = null;
    }

    void Update()
    {

        if (tickdown == true)
        {
            currentcharge -= 30 * Time.deltaTime;

            if (currentcharge <= 0)
            {
                currentcharge = 0;
                tickdown = false;
                cancelpower();
            }
        }

        if (canturnon == true)
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
                    if (hit.transform != null)
                    {
                        if (hit.transform.tag == "Enemy")
                        {
                            target = hit.transform;
                        }
                    }
            }
        }
        else
        {
            if (target != null)
            mycam.transform.LookAt(target);
        }


            if (target != null)
            {
                if (Input.GetKeyDown("v") && targetting == false && transitioning == false)
                {
                    targetting = true;
                    TargetButton.SetActive(true);
                    Time.timeScale = 0.5f;
                    Application.targetFrameRate = Mathf.RoundToInt(60 / Time.timeScale);
                    cd.cameracontrol = false;
                    tickdown = true;
                    StartCoroutine(zoomin());

                }
                else if (Input.GetKeyDown("v") && targetting == true && transitioning == false)
                {
                    targetting = false;
                    TargetButton.SetActive(false);
                    canturnon = false;
                    currentcharge = 20;
                    tickdown = false;
                    StartCoroutine(zoomout());

                }
            }
        }
        else
        {
            if (currentcharge < 30)
            {
                currentcharge += Time.deltaTime;
            }
            else
            {
                currentcharge = 30;
                canturnon = true;
            }
        }

    }

    private IEnumerator zoomout()
    {
        targetting = false;
        TargetButton.SetActive(false);
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

