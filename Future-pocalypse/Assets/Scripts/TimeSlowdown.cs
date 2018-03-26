using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using weapons;

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
    [SerializeField]
    private AudioSource EngineSource;
    public AudioSource soundGun;
    [SerializeField]
    private GameObject machinegun;


    [SerializeField]
    private PlayerWeapons.Weapontypes[] myweapons = new PlayerWeapons.Weapontypes[3];


    //For Shielding
    public healthTest hpTest;
    public int shieldHp;
    public bool isShielding;

    private void Start()
    {
        hpTest = GetComponent<healthTest>();
        shieldHp = hpTest.health;

        PlayerWeapons.Weapontypes[] temp = new PlayerWeapons.Weapontypes[3];
        temp[0] = PlayerWeapons.Weapontypes.TimeSlow;
        temp[1] = PlayerWeapons.Weapontypes.Machinegunfire;
        temp[2] = PlayerWeapons.Weapontypes.Missiledash;
        SettingWeapons(temp);

    }

    public void SettingWeapons(PlayerWeapons.Weapontypes[] newweapons)
    {
        for (int i = 0; i < 3; i++)
        {
            myweapons[i] = newweapons[i];
        }
    }

    private void Newcontrols()
    {
        if (Input.GetKeyDown(KeyCode.J) == true)
        {
            Invoke(myweapons[0].ToString(), 0);
            if (myweapons[1].ToString() == "Machinegunfure")
            {
                currentcharge -= 1;
                canturnon = false;
            }
            else
            {
                currentcharge = 0;
            }
            canturnon = false;
            //Boost
        }
        else if (Input.GetKeyDown(KeyCode.K) == true)
        {
            Invoke(myweapons[1].ToString(), 0);
            if (myweapons[1].ToString() == "Machinegunfire")
            {
                currentcharge -= 1;
                canturnon = false;
            }
            else
            {
                currentcharge = 0;
            }

            canturnon = false;
        }
        else if (Input.GetKeyDown(KeyCode.L) == true)
        {
            Invoke(myweapons[2].ToString(), 0);
            if (myweapons[1].ToString() == "Machinegunfire")
            {
                currentcharge -= 1;
                canturnon = false;
            }
            else
            {
                currentcharge = 0;
            }
            canturnon = false;
        }
    }

    public void Missile()
    {
        currentcharge = 0;
        Cancelpower();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Ramp")
        {
          //  Missiledash(1f,other.transform);
        }
    }

    public void Missiledash()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * 100, ForceMode.VelocityChange);
        Application.targetFrameRate = Mathf.RoundToInt(60 / Time.timeScale);
        currentcharge = 10;
    }

    public void Missiledash(float Bonus,Transform rotate)
    {
        GetComponent<Rigidbody>().AddForce(rotate.forward * 100 * Bonus, ForceMode.VelocityChange);
        Application.targetFrameRate = Mathf.RoundToInt(60 / Time.timeScale);
    }

    private IEnumerator Timeslow(bool activate, float time)
    {
        
        switch (activate)
        {
            case (false):
                {
                    Time.timeScale = 0.15f;
                    Application.targetFrameRate = Mathf.RoundToInt(60 / Time.timeScale);
                    StartCoroutine(Timeslow(true, 2));
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

    private void Showenemyhealth(healthTest enemy)
    {
        enemyHealthbar.GetComponent<Image>().fillAmount = enemy.Returnhealth() / enemy.ReturnMaxhealth();
    }

    public void Machinegunfire()
    {
        
        StartCoroutine(Actualfire());
    }

    public IEnumerator Actualfire()
    {
        tickdown = false;
        canturnon = false;
        Lapcheckpoint me = GetComponent<Lapcheckpoint>();
   
        Transform temptransform = rs.Returnplayerahead(me);
        target = temptransform;

        if (target != null)
        {
           
            RaycastHit hit;
            machinegun.transform.LookAt(target);
            healthTest enemyhealth = target.GetComponent<healthTest>();
            tickdown = false;
            int currenergy = Mathf.RoundToInt(currentcharge) / 2;
            enemyHealthbar.SetActive(true);
            Image Ehealth = enemyHealthbar.GetComponent<Image>();
            for (int i = 0; i < currenergy; i++)
            {
                soundGun.Play();
                yield return new WaitForSeconds(0.2f);
                Physics.Raycast(machinegun.transform.position, machinegun.transform.forward, out hit, 90);
                if (hit.transform != null)
                {
                    Ehealth.fillAmount = enemyhealth.Returnhealth() / enemyhealth.ReturnMaxhealth();
                    if (hit.transform.tag == "Enemy" && hit.transform == target)
                    {
                        enemyhealth.takedamage(7);
                        Showenemyhealth(enemyhealth);
                        machinegun.transform.LookAt(hit.transform);
                        currentcharge -= 2;
                    }
                    else if (hit.transform.tag == "Enemy" && hit.transform != target)
                    {
                        healthTest tempenemey = hit.transform.GetComponent<healthTest>();
                        tempenemey.takedamage(2);
                        Showenemyhealth(tempenemey);
                        machinegun.transform.LookAt(hit.transform);
                        currentcharge -= 2;
                    }
                    else
                    {

                        //Miss
                    }
                    
                }
                else
                {
                    i = currenergy;
                }

            }
            enemyHealthbar.SetActive(false);
            machinegun.transform.localRotation = Quaternion.Euler(0, 0, 0);
            canturnon = false;
            soundGun.Stop();

        }
        else
        {
            Cancelpower();
        }

        yield return 0;
    }

    public IEnumerator Shield()
    {
        //What is this doing?
        shieldHp = hpTest.health;
        isShielding = true;
        yield return new WaitForSeconds(3);
        isShielding = false;
        yield return 0;
    }

    private void Cancelpower()
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
                Cancelpower();
            }
        }

        if (canturnon == true)
        {

            Newcontrols();
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

