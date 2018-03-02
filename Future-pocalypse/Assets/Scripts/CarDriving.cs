using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDriving : MonoBehaviour
{
    public bool candrive;
    public bool isdead;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private int accel;
    [SerializeField]
    private int topspeed;
    [SerializeField]
    private float turnspeed;

    private float actualturnspeed;
    [SerializeField]
    private BoxCollider mybox;

    [SerializeField]
    private Camera mycam;

    [SerializeField]
    private GameObject carMesh;

    [SerializeField]
    private float carRotation;

    [SerializeField]
    private ParticleSystem[] sparks;

    [SerializeField]
    private float driftClamp = 25f;

    public bool cameracontrol;
    [SerializeField]
    private healthTest ht;

    //For Controlling Sound
    [SerializeField]
    private AudioSource audioSource;
    // Use this for initialization
    void Start ()
    {

        Application.targetFrameRate = 60;
        rb.centerOfMass = -transform.up;	
	}

    public int returnmaxspeed()
    {
        return topspeed;
    }

    private void OnCollisionEnter(Collision collision)
    {

    }


    // Update is called once per frame
    void Update()
    {
        if (candrive)
        {
            //Sound of the engine
            EngineSound();

            RaycastHit hit;
            bool onfloor;

            // Check if the car is grounded
            if (Physics.Raycast(transform.position, -transform.up, out hit, mybox.size.y / 2))
            {
                onfloor = true;
            }
            else
            {
                onfloor = false;
            }

            // Check for player depth input, the car is movement, and the car is on the ground.
            if (Input.GetAxis("Vertical") != 0 && onfloor == true)
            {
                // Add force to car
                if (Input.GetAxis("Vertical") > 0 && rb.velocity.magnitude < topspeed / 2)
                {
                    rb.AddForce(transform.forward * (accel), ForceMode.Acceleration);
                   


                }
                else if (Input.GetAxis("Vertical") < 0 && rb.velocity.magnitude > -topspeed/2)
                {
                    rb.AddForce(-transform.forward * (accel / 2), ForceMode.Acceleration);
               

                }

                if (Input.GetAxis("Horizontal") != 0)
                {
                    actualturnspeed = turnspeed * 2;
                    if (Input.GetButton("Drift"))
                    {

                        // Play partical system
                        if (sparks[0].isPlaying == false)
                        {
                            sparks[0].Play();
                            sparks[1].Play();
                        }

                        // Rotate mesh by the horizontal input
                        carRotation += Input.GetAxis("Horizontal") * 1;
                        carRotation = Mathf.Clamp(carRotation, -driftClamp, driftClamp);
                        carMesh.transform.localRotation = Quaternion.Euler(new Vector3(0, carRotation, 0));
                    }
                }
                else if (!Input.GetButton("Drift"))     //Make correction when player lets go of the drifting key
                {
                    actualturnspeed = turnspeed;
                    // Stop partical system
                    if (sparks[0].isPlaying == true)
                    {
                        sparks[0].Stop();
                        sparks[1].Stop();
                    }

                    // Make corection
                    if (carRotation > 0)
                    {
                        carRotation--;
                    }
                    else if (carRotation < 0)
                    {
                        carRotation++;
                    }

                    // Roate mesh back
                    carRotation = Mathf.RoundToInt(carRotation);
                    carMesh.transform.localRotation = Quaternion.Euler(new Vector3(0, carRotation, 0));
                }

                // Chanage the transfrom roation
                transform.Rotate(transform.up, Input.GetAxis("Horizontal") * (actualturnspeed - (Input.GetAxis("Vertical"))));
            }

            if (rb.velocity.z >= 0 && mycam.fieldOfView < 125)
                mycam.fieldOfView = (int)((rb.velocity.magnitude) ) + 70;

            if (mycam.fieldOfView >= 125)
            {
                mycam.fieldOfView = 125;
            }
            else if (mycam.fieldOfView < 0)
            {
                mycam.fieldOfView = 0;
            }
        }
        else
        {
            if (isdead == true)
            {
                if (ht.addhealth() == true)
                {
                    isdead = false;
                    candrive = true;
                }
            }
        }
    }
    //The faster you move, the higher the pitch
    void EngineSound()
    {
        audioSource.pitch = rb.velocity.magnitude / topspeed + 1;
    }
}
