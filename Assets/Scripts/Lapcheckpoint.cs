﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//For Text Mesh Pro stuff
using TMPro;
public class Lapcheckpoint : MonoBehaviour {

   
    public Vector3 currentRespawn;
    public int Lap = 1;
    public int Checkpoint = 0;
    [SerializeField]
    private int maxcheckpoints;
    [SerializeField]
    private GameObject endscreen;
    [SerializeField]
    private TextMeshProUGUI endtext;
    private float timestart;
    public float totalracevalue;
    [SerializeField]
    private RaceSystem rs;


    public bool CheckLapProgress()
    {
        return Checkpoint == maxcheckpoints - 1;
    }

    public int ReturnmyPlace()
    {
        return Checkpoint;
    }

    public void Setmyplace(int newnumber)
    {
        Checkpoint = newnumber;
    }

    void Start ()
    {
        //maxcheckpoints = GameObject.FindObjectsOfType<Checkpoint>().Length;
        InvokeRepeating("Distancetonextcp", 4, 0.1f);
    }

    private float Distancepercent()
    {
        float distance;

        if (Checkpoint > 0)
        {
            distance = Vector3.Distance(GameObject.Find("Checkpoint " + Checkpoint).transform.position, GameObject.Find("Checkpoint " + (Checkpoint - 1)).transform.position);
           // Debug.DrawLine(GameObject.Find("Checkpoint " + Checkpoint).transform.position, GameObject.Find("Checkpoint " + (Checkpoint - 1)).transform.position, Color.red);

        }
        else
        {
            distance = Vector3.Distance(GameObject.Find("Checkpoint " + Checkpoint).transform.position, GameObject.Find("Checkpoint " + (maxcheckpoints - 1)).transform.position);
          //  Debug.DrawLine(GameObject.Find("Checkpoint " + Checkpoint).transform.position, GameObject.Find("Checkpoint " + (maxcheckpoints - 1)).transform.position, Color.red);
        }
        float curr = Vector3.Distance(this.transform.position,GameObject.Find("Checkpoint " + Checkpoint).transform.position);

        //Debug.DrawLine(transform.position, GameObject.Find("Checkpoint " + Checkpoint).transform.position, Color.green);
        distance = curr/distance;
        Mathf.Clamp(distance, 0, 1);
        return distance/10f;
    }

    void Distancetonextcp()
    {
        totalracevalue = (Lap * maxcheckpoints) + Checkpoint - Distancepercent();
        //Lap 2: 200 * 10 = 2000.  5/10 *100 = 50. - 30.
    }

    public void Showmyprogress()
    {
        print("Name: " + name + "\n Checkpoint: " + Checkpoint + "\n Lap: " + Lap + "\n Distance: " + Distancepercent());
    }

    void Update()
    {

        if (transform.tag == "Player")
        {
            if (endscreen.activeSelf == false)
            {
                timestart += Time.deltaTime;
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        
        if (collider.transform.tag == "Checkpoint" && Checkpoint == collider.GetComponent<Checkpoint>().checkpointnum)
        {
            Checkpoint++;

            if (Checkpoint == maxcheckpoints)
            {
                Checkpoint = 0;
                Lap++;
            }
        }
        if (Lap == 4 && endscreen.activeSelf == false && transform.tag == "Player")
        {
            showendscreen();
        }
        else if (Lap == 4 && transform.tag == "Enemy")
        {
            GetComponent<AI>().enabled = false;
        }
		
	}


    void showendscreen()
    {
        endscreen.SetActive(true);
        endtext.text = "You finished! " + System.Environment.NewLine + rs.Returnposition(this) + "place";
        if(GetComponent<CarDriving>() == true)
        {
            GetComponent<CarDriving>().canDrive = false;
        }
        else if (GetComponent<AI>() == true)
        {
            GetComponent<AI>().canmove = false;
        }
    }

    
}
