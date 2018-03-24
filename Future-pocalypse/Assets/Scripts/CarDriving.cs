﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDriving : MonoBehaviour
{
    public bool canDrive, isDead, cameraControl, isDrifting,firstorthird;

    [SerializeField]
    private bool onFloor;

    [SerializeField]
    private int accel;


    [SerializeField]
    private float turnSpeed, actualTurnSpeed, carRotation, driftClamp, midAirPitch, midAirYaw, topSpeed;

    [SerializeField]
    public Rigidbody rb;

    [SerializeField]
    private BoxCollider myBox;

    [SerializeField]
    private Camera myCam;

    [SerializeField]
    private GameObject carMesh;

    [SerializeField]
    private ParticleSystem[] sparks;

    [SerializeField]
    private healthTest ht;

    //For Controlling Sound
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private CameraChange camchange;

   
    

    //public AudioClip soundCollision;


    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        


        myBox = GetComponent<BoxCollider>();
        myCam = GetComponentInChildren<Camera>();

        Application.targetFrameRate = 60;
        rb.centerOfMass = -transform.up;
    }

    public int TopSpeed
    {
        get { return (int)topSpeed; }
    }


    // Update is called once per frame
    void Update()
    {
        if (canDrive)
        {
           
            EngineSound();         // Sound of the engine
            FloorCheck();          // Check if car is on the ground
            Acceleration();        // Acceleration and Deceleration with keyboard
            Steering();            // Sterring left and right with keyboard. Includes power slide.
            MidAirControl();
            CameraMethod();        // Changes the camra FOV when accelerating
        }
        else
        {
            
            if (isDead == true)
            {
                if (ht.addhealth() == true)
                {
                    isDead = false;
                    canDrive = true;
                    
                }
            }
        }
    }

    //The faster you move, the higher the pitch
    void EngineSound()
    {
        audioSource.pitch = rb.velocity.magnitude / topSpeed + 1;
        audioSource.volume = 0.200f - (rb.velocity.magnitude / topSpeed)/5;
        if(audioSource.volume > 0.180f)
        {
            audioSource.volume = 0.180f;
        }
        else if(audioSource.volume < 0.120f)
        {
            audioSource.volume = 0.120f;
        }

    }

    void FloorCheck()
    {
        RaycastHit hit;

        // Check if the car is grounded
        if (Physics.Raycast(transform.position, -transform.up, out hit, (myBox.size.y / 2) * transform.localScale.magnitude))
        {
            onFloor = true;
        }
        else
        {
            onFloor = false;
        }
    }

    void Acceleration()
    {


        // Check for player depth input, the car is movement, and the car is on the ground.
        if (Input.GetAxis("Vertical") != 0 && onFloor)
        {
            // Add force to car
            if (Input.GetAxis("Vertical") > 0 && rb.velocity.magnitude < topSpeed / 2)
            {
                rb.AddForce(transform.forward * (accel), ForceMode.Acceleration);
            }
            else if (Input.GetAxis("Vertical") < 0 && rb.velocity.magnitude > -topSpeed / 2)
            {
                rb.AddForce(-transform.forward * (accel / 2), ForceMode.Acceleration);
            }
        }
    }

    void Steering()
    {
        if (rb.velocity.magnitude != 0 && onFloor)
        {
            if (Input.GetButtonDown("Drift"))
            {
                topSpeed *= 0.8f;
            }
            else if (Input.GetButtonUp("Drift"))
            {
                topSpeed *= 1.25f;
            }

            if (Input.GetAxis("Horizontal") != 0)
            {
                actualTurnSpeed = (turnSpeed * 1f);

                actualTurnSpeed = (turnSpeed);


                if (Input.GetButton("Drift"))
                {
                    isDrifting = true;
                    actualTurnSpeed = (turnSpeed * 1.5f);

                    // Play particle system
                    if (sparks[0].isPlaying == false)
                    {
                        sparks[0].Play();
                        sparks[1].Play();
                    }

                    // Rotate mesh by the horizontal input
                    carRotation = Input.GetAxis("Horizontal") * 1;

                    carMesh.transform.localRotation = Quaternion.Euler(new Vector3(0, carRotation, 0));
                }
                else if (!Input.GetButton("Drift"))     //Make correction when player lets go of the drifting key
                {
                    isDrifting = false;
                    actualTurnSpeed = turnSpeed;

                    // Stop partical system
                    if (sparks[0].isPlaying)
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
            transform.Rotate(transform.up, Input.GetAxis("Horizontal") * (actualTurnSpeed - tempfloat));
        }
    }

    void MidAirControl()
    {
        if (!onFloor)
        {
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                rb.AddTorque(transform.up * Input.GetAxis("Horizontal") * midAirYaw);
            //    rb.AddTorque(transform.right * Input.GetAxis("Vertical") * midAirPitch);
            }
        }


    }

    void CameraMethod()
    {
        myCam.fieldOfView = ((rb.velocity.magnitude / 2)) + 70;

        if (myCam.fieldOfView >= 105)
        {
            myCam.fieldOfView = 105;
        }
        else if (myCam.fieldOfView < 70)
        {
            myCam.fieldOfView = 70;
        }
        camchange.Lerping(rb.velocity.magnitude/(topSpeed/2));
    }
}