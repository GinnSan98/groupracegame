using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Test : MonoBehaviour
{
    [SerializeField]
    private Vector3 startlocation;
    [SerializeField]
    private NavMeshAgent mynavmesh;

	// Use this for initialization
	void Start ()
    {
        mynavmesh.destination = startlocation;
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Checkpoint")
        {
            mynavmesh.destination = GameObject.Find("Checkpoint " + GetComponent<Lapcheckpoint>().Checkpoint).transform.position;
        }
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
