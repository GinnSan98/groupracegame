using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;    //For Text Mesh Pro stuff

public class ThreeDText : MonoBehaviour {

    public TextMeshPro displayText;

    public GameObject playerObject;
    public Transform playerPosition;
    public float dist;

    //For rotating towards
    public float speed = 50;
    private Quaternion lookRotation;
    private Vector3 direction;
    // Use this for initialization
    void Start ()
    {
        displayText = GetComponent<TextMeshPro>();
        playerObject = GameObject.Find("Main Camera");
        playerObject = GameObject.Find("John shmid");
        playerPosition = playerObject.transform;

    }
	
	// Update is called once per frame
	void Update () {
        dist = Vector3.Distance(playerPosition.position, transform.position);
        if(dist <=200 )
        {
            displayText.text = gameObject.transform.parent.name;
        }
        else if(dist > 200)
        {
            displayText.text = "";
        }

        //For rotating towards
        direction = (playerPosition.position - transform.position).normalized;
        lookRotation = Quaternion.LookRotation(direction);
        lookRotation *= Quaternion.Euler(0, 180, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);
    }
}
