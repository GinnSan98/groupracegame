using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDriving : MonoBehaviour
{
    public bool candrive;
    public bool isdead;
    [SerializeField]
    public Rigidbody rb;
    [SerializeField]
    private int accel;
    [SerializeField]
    public int topspeed;
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
    private float driftClamp = 15f;

    public bool cameracontrol;
    [SerializeField]
    private healthTest ht;

    //For Controlling Sound
    [SerializeField]
    private AudioSource audioSource;

    public bool isDrifting = false;


    //public AudioClip soundCollision;

    
    // Use this for initialization
    void Start ()
    {
        
        audioSource = GetComponent<AudioSource>();

        Application.targetFrameRate = 60;
        rb.centerOfMass = -transform.up;	
	}

    public int returnmaxspeed()
    {
        return topspeed;
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
            if (Physics.Raycast(transform.position, -transform.up, out hit, (mybox.size.y / 2)*transform.localScale.magnitude))
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


            }

            if (rb.velocity.magnitude != 0)
            {
                if (Input.GetAxis("Horizontal") != 0)
                {



                    actualturnspeed = (turnspeed * 1f);

                    actualturnspeed = (turnspeed);

                    if (Input.GetButton("Drift"))
                    {
                        actualturnspeed = (turnspeed * 1.5f);
                        isDrifting = true;
                        // Play particle system
                        if (sparks[0].isPlaying == false)
                        {
                            sparks[0].Play();
                            sparks[1].Play();
                        }


                        // Rotate mesh by the horizontal input
                        carRotation += Input.GetAxis("Horizontal") * 1;

                        carMesh.transform.localRotation = Quaternion.Euler(new Vector3(0, carRotation, 0));
                    }
                    else if (!Input.GetButton("Drift"))     //Make correction when player lets go of the drifting key
                    {

                        isDrifting = false;
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

                    carRotation = Mathf.Clamp(carRotation, -driftClamp, driftClamp);
                }

                float tempfloat = Mathf.Clamp((Input.GetAxis("Vertical") / (rb.velocity.magnitude)), 1, 10);
                // Chanage the transfrom roation
                transform.Rotate(transform.up, Input.GetAxis("Horizontal") * (actualturnspeed - tempfloat));
            }
           
            mycam.fieldOfView = ((rb.velocity.magnitude/2) ) + 70;

            if (mycam.fieldOfView >= 105)
            {
                mycam.fieldOfView = 105;
            }
            else if (mycam.fieldOfView < 70)
            {
                mycam.fieldOfView = 70;
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
    

