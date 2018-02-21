using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lapcheckpoint : MonoBehaviour {

   
    public Vector3 currentRespawn;
    public int Lap = 1;
    public int Checkpoint = 0;
    [SerializeField]
    private int maxcheckpoints;
    [SerializeField]
    private GameObject endscreen;
    [SerializeField]
    private Text endtext;
    private float timestart;

    public float totalracevalue;

    void Start ()
    {
        maxcheckpoints = GameObject.FindObjectsOfType<Checkpoint>().Length;
        InvokeRepeating("distancetonextcp", 4, 0.1f);
    }

    void distancetonextcp()
    {
        float distance = Vector3.Distance(transform.position, GameObject.Find("Checkpoint " + Checkpoint).transform.position)/500;
        totalracevalue = (Lap * (10 * maxcheckpoints)) + (Checkpoint * 10) - distance;
        //Lap 2: 200 * 10 = 2000.  5/10 *100 = 50. - 30.
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
        endtext.text = "You finished! " + System.Environment.NewLine + "You did all the laps in " + timestart + " seconds!";
        if(GetComponent<CarDriving>() == true)
        {
            GetComponent<CarDriving>().candrive = false;
        }
        else if (GetComponent<AI>() == true)
        {
            GetComponent<AI>().canmove = false;
        }
    }
}
