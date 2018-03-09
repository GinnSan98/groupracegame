using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TimeSlowdown : MonoBehaviour
{
    public GameObject enemyHealthbar;
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
    [SerializeField]
    private RaceSystem rs;

    //For machine gun sound fx
    AudioSource audioSource;
    public AudioClip soundGun;
    [SerializeField]
    private GameObject machinegun;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void Missle()
    {
        currentcharge = 0;
        cancelpower();
    }


    public void Missledash()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * 300, ForceMode.VelocityChange);
        Application.targetFrameRate = Mathf.RoundToInt(60 / Time.timeScale);
        currentcharge = 10;
    }

    private IEnumerator timeslowon(bool activate, float time)
    {
        
        switch (activate)
        {
            case (false):
                {
                    Time.timeScale = 0.15f;
                    Application.targetFrameRate = Mathf.RoundToInt(60 / Time.timeScale);
                    StartCoroutine(timeslowon(true, 2));
                    break;
                }
            case (true):
                {
                    yield return new WaitForSeconds(time);
                    Time.timeScale = 1f;
                    Application.targetFrameRate = Mathf.RoundToInt(60 / Time.timeScale);

                    break;
                }

        }
        yield return 0;
    }

    private void showenemyhealth(healthTest enemy)
    {
        enemyHealthbar.GetComponent<Image>().fillAmount = enemy.Returnhealth() / enemy.ReturnMaxhealth();
    }

    public void machinegunfire()
    {
        
        StartCoroutine(actualfire());
    }

    public IEnumerator actualfire()
    {
        tickdown = false;
        canturnon = false;
        Lapcheckpoint me = GetComponent<Lapcheckpoint>();
   
        Transform temptransform = rs.returnplayerahead(me);
        target = temptransform;

        if (target != null)
        {
            print(target.name);
            audioSource.PlayOneShot(soundGun);
            RaycastHit hit;
            machinegun.transform.LookAt(target);
            healthTest enemyhealth = target.GetComponent<healthTest>();
            tickdown = false;
            int currenergy = Mathf.RoundToInt(currentcharge) / 2;
            enemyHealthbar.SetActive(true);
            for (int i = 0; i < currenergy; i++)
            {
                yield return new WaitForSeconds(0.025f);
                Physics.Raycast(machinegun.transform.position, machinegun.transform.forward, out hit, 90);
                if (hit.transform != null)
                {
                    if (hit.transform.tag == "Enemy" && hit.transform == target)
                    {
                        enemyhealth.takedamage(7);
                        showenemyhealth(enemyhealth);
                    }
                    else if (hit.transform.tag == "Enemy" && hit.transform != target)
                    {
                        healthTest tempenemey = hit.transform.GetComponent<healthTest>();
                        tempenemey.takedamage(3);
                        showenemyhealth(enemyhealth);
                    }
                    else
                    {
                        //Miss
                    }
                    currentcharge -= 1;
                }
                else
                {
                    i = currenergy;
                }

            }
            enemyHealthbar.SetActive(false);
            machinegun.transform.localRotation = Quaternion.Euler(0, 0, 0);
            audioSource.Stop();

        }
        else
        {
            cancelpower();
        }

        yield return 0;
    }

    private void cancelpower()
    {
        Time.timeScale = 1f;
        Application.targetFrameRate = Mathf.RoundToInt(60 / Time.timeScale);
        canturnon = false;
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
            if (Input.GetKeyDown(KeyCode.J) == true)
            {
                currentcharge = 0;
                Missledash();
                canturnon = false;
                //Boost
            }
            else if (Input.GetKeyDown(KeyCode.K) == true)
            {
                StartCoroutine(timeslowon(false, 0));
                currentcharge = 0;
                canturnon = false;
            }
            else if (Input.GetKeyDown(KeyCode.L) == true)
            {
                machinegunfire();
                currentcharge = 0;
                canturnon = false;
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
}

