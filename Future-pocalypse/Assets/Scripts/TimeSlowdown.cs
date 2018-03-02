using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TimeSlowdown : MonoBehaviour
{
    public GameObject enemyHealthbar;
    public GameObject TargetButton;
    [SerializeField]
    private Camera mycam;
    private bool targetting;
    public Transform target;
    private bool transitioning;
    [SerializeField]
    private CarDriving cd;
    public bool tickdown;
    public float currentcharge;
    public bool canturnon = true;




    public void Missle()
    {
        currentcharge = 0;
        cancelpower();
    }

    public void Missledash()
    {
        transform.LookAt(target);
        GetComponent<Rigidbody>().AddForce(transform.forward * 300, ForceMode.VelocityChange);
        targetting = false;
        tickdown = false;
        enemyHealthbar.SetActive(false);
        TargetButton.SetActive(false);
        Time.timeScale = 1f;
        Application.targetFrameRate =  Mathf.RoundToInt(60 / Time.timeScale);
        currentcharge = 10;
        cancelpower();
    }

    public void MobilityDash()
    {
        transform.LookAt(target);
        transform.position += transform.up;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().AddForce(transform.forward * 200, ForceMode.VelocityChange);
        enemyHealthbar.SetActive(false);
        TargetButton.SetActive(false);
        Time.timeScale = 0.25f;
        Application.targetFrameRate = Mathf.RoundToInt(60 / Time.timeScale);
        cancelpower();
    }

    private void showenemyhealth(healthTest enemy)
    {
        enemyHealthbar.GetComponent<Image>().fillAmount = enemy.Returnhealth() / enemy.ReturnMaxhealth();
    }

    public void machinegunfire(GameObject machinegun)
    {
        StartCoroutine(actualfire(machinegun));
    }

    public IEnumerator actualfire(GameObject machinegun)
    {
        tickdown = false;
        canturnon = false;
        

        RaycastHit hit;
        machinegun.transform.LookAt(target);
        healthTest enemyhealth = target.GetComponent<healthTest>();
        tickdown = false;
        int currenergy = Mathf.RoundToInt(currentcharge)/2;
        for (int i = 0; i < currenergy; i++)
        {
            yield return new WaitForSeconds(0.025f);
            Physics.Raycast(machinegun.transform.position, machinegun.transform.forward, out hit, 300);
            if (hit.transform != null)
            {
                if (hit.transform.tag == "Enemy" && hit.transform == target)
                {
                    enemyhealth.takedamage(5);
                    showenemyhealth(enemyhealth);
                }
                else if (hit.transform.tag == "Enemy" && hit.transform != target)
                {
                    hit.transform.GetComponent<healthTest>().takedamage(3);
                }
                else
                {
                    //Miss
                }
                currentcharge -= 2;
            }
            else
            {
                i = currenergy;
            }
            
        }
        cancelpower();
        machinegun.transform.rotation = Quaternion.Euler(0, 0, 0);

        yield return 0;
    }

    private void cancelpower()
    {
        Time.timeScale = 1f;
        Application.targetFrameRate = Mathf.RoundToInt(60 / Time.timeScale);
        canturnon = false;
        StartCoroutine(zoomout());
        target = null;
    }

    void Update()
    {

        if (tickdown == true)
        {
            currentcharge -= 10 * Time.deltaTime;

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
                Physics.Raycast(ray, out hit, 1200f);

                    
                //   Physics.Raycast(mycam.transform.position, mycam.ViewportToWorldPoint(Input.mousePosition), out hit, 100f);

                    if (hit.transform != null)
                    {
                        if (hit.transform.tag == "Enemy")
                        {
                            Debug.DrawRay(mycam.transform.position, ray.direction * hit.distance, Color.green, 3);
                            target = hit.transform;
                        }
                        if (hit.transform.tag == "DashPanel" && canturnon == true)
                        {
                            Debug.DrawRay(mycam.transform.position, ray.direction * hit.distance, Color.green, 3);
                            target = hit.transform;
                            currentcharge = 29;
                            
                            MobilityDash();
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
                    enemyHealthbar.SetActive(true);
                    enemyHealthbar.GetComponent<Image>().fillAmount = target.GetComponent<healthTest>().Returnhealth() / target.GetComponent<healthTest>().ReturnMaxhealth();
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
                    enemyHealthbar.SetActive(false);
                    TargetButton.SetActive(false);
                    Time.timeScale = 1;
                    Application.targetFrameRate = Mathf.RoundToInt(60 / Time.timeScale);
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
                canturnon = false;
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
        enemyHealthbar.SetActive(false);
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

