using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDriving : MonoBehaviour
{

    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private int accel;
    [SerializeField]
    private int topspeed;
    [SerializeField]
    private float turnspeed;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        
        if (Input.GetAxis("Vertical") != 0 && rb.velocity.magnitude < topspeed && rb.velocity.magnitude > (-topspeed/2))
        {
            if (Input.GetAxis("Vertical") > 0)
            {
                rb.AddForce(transform.forward * accel -(transform.up/2), ForceMode.Acceleration);
            }
            else
            {
                rb.AddForce(-transform.forward * (accel*2) - (transform.up / 2), ForceMode.Acceleration);
            }
                
            if (Input.GetAxis("Horizontal") != 0)
            {
                transform.Rotate(transform.up,Input.GetAxis("Horizontal") * turnspeed);
            }
        }
	}
}
