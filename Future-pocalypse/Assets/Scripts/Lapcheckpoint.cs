using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Lapcheckpoint : MonoBehaviour {

   
    public Vector3 currentRespawn;
    public int Lap = 0;
    public int Checkpoint = 0;
    [SerializeField]
    private int maxcheckpoints;

    void Start ()
    {
        maxcheckpoints = GameObject.FindObjectsOfType<Checkpoint>().Length;
    }


    void OnTriggerEnter(Collider collider)
    {
        
        if (collider.transform.tag == "Checkpoint" && Checkpoint == collider.GetComponent<Checkpoint>().checkpointnum)
        {
            Debug.Log("Collision detected");
            Checkpoint++;

            if (Checkpoint == maxcheckpoints)
            {
                Checkpoint = 0;
                Lap++;
            }
        }
		
	}
}
