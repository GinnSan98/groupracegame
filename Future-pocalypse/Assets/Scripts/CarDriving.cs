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

    public bool cameracontrol = true;
    [SerializeField]
    private Camera mycam;
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
        bool onfloor;
        if (Physics.Raycast(transform.position, -transform.up, out hit, mybox.size.y/2))
        {
            onfloor = true;
        }
        else
        {
            onfloor = false;
        }

        


        if (Input.GetAxis("Vertical") != 0 && rb.velocity.magnitude < topspeed && rb.velocity.magnitude > (-topspeed/2) && onfloor == true)
        {
            if (Input.GetAxis("Vertical") > 0)
            {
                rb.AddForce(transform.forward * accel, ForceMode.Acceleration);
                if (cameracontrol == true)
                {
                    mycam.fieldOfView = (rb.velocity.magnitude / 1.8f) + 70;
                }

            }
            else
            {
                rb.AddForce(-transform.forward * (accel*2), ForceMode.Acceleration);
                if (cameracontrol == true)
                {
                    mycam.fieldOfView = 70;
                }
            }

            if (Input.GetAxis("Horizontal") != 0)
            {
                transform.Rotate(transform.up, Input.GetAxis("Horizontal") * (turnspeed - (Input.GetAxis("Vertical")*2)));
            }
        }
        else
        {
            if (cameracontrol == true)
            {
                mycam.fieldOfView = (rb.velocity.magnitude / 2f) + 70;
            }
            
        }



    }
}
