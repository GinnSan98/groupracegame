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
    [SerializeField]
    private BoxCollider mybox;
	// Use this for initialization
	void Start ()
    {
        rb.centerOfMass = -transform.up;	
	}

    private void OnCollisionEnter(Collision collision)
    {

    }


    // Update is called once per frame
    void FixedUpdate ()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, -transform.up, out hit, mybox.size.y/2);

        


        if (Input.GetAxis("Vertical") != 0 && rb.velocity.magnitude < topspeed && rb.velocity.magnitude > (-topspeed/2) && hit.distance != 0)
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
                transform.Rotate(transform.up, Input.GetAxis("Horizontal") * (turnspeed - (Input.GetAxis("Vertical")*2)));
            }
        }



    }
}
