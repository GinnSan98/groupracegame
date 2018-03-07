﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    public bool canmove;

    public bool isdead;
    private bool onfloor;
    public int vehicleSpeed;
    public float vehicleWidth;
    public float judgementCoeffient;
    float offset = 2f;
    Vector3 left;
    Vector3 right;
    Vector3 avoid;
    float rayLength;
    RaycastHit hit;
    [SerializeField]
    private BoxCollider bc;
    [SerializeField]
    private int maxspeed;
    [SerializeField]
    private healthTest ht;

    [SerializeField]
    private Lapcheckpoint lcp;

    private int difficulty;

    //For Controlling Sound
    [SerializeField]
    private AudioSource audioSource;




    void Start()
    {

        
        difficulty = Random.Range(10, 60);
        vehicleWidth = bc.size.x;

        rb.centerOfMass = -transform.up;
        Vector3 left = transform.TransformPoint(0, 0, offset);
        Vector3 right = transform.TransformPoint(0, 0, -offset);
        Vector3 avoid = Vector3.zero;
        
    }
    private void dodgeShit(float direction)
    {
        
        rb.MoveRotation(Quaternion.Euler(0, direction * vehicleWidth, 0) * transform.rotation);
        avoid = hit.normal * vehicleWidth;


    }

    void OnTriggerEnter(Collider other)
    {
        GameObject nextcp = GameObject.Find("Checkpoint " + lcp.Checkpoint);
        if (other.tag == "Checkpoint")
        {
            
            transform.LookAt(nextcp.transform, Vector3.up);
        }
        else if (other.tag == "Correction")
        {
            transform.LookAt(GameObject.Find("Checkpoint " + lcp.Checkpoint).transform);
        }
    }

    void Update()
    {
        if (canmove == true)
        {
            //Sound of the engine
            EngineSound();
            if (Physics.Raycast(transform.position, -transform.up, out hit, bc.size.y / 2))
            {
                onfloor = true;
            }
            else
            {
                onfloor = false;
            }




            rayLength = vehicleSpeed * judgementCoeffient;

            if (onfloor == true)
            {
                if (rb.velocity.magnitude < maxspeed)
                {

                    //Right wall, left turn;
                    if (Physics.Raycast(transform.position, (transform.right + transform.forward), out hit, rayLength / 10) && hit.transform.tag != "Checkpoint" && hit.transform.tag != "smallDamage" && hit.transform.tag != "mediumDamage")
                    {
                        if (hit.transform.tag == "Enemy")
                        {

                            dodgeShit(-2f / ((hit.distance / 2) + 1f));
                        }
                        else if (hit.transform.tag == "Checkpoint")
                        {

                            dodgeShit(3f / ((hit.distance / 2) + 1f));
                        }
                        else if (hit.transform.tag == "Player")
                        {

                            dodgeShit(1f / ((hit.distance / 4) + 1f));
                        }
                        else
                        {

                            dodgeShit(-50f / ((hit.distance / 2) + 1f));
                        }

                    }

                    //Right wall forward twice.
                    if (Physics.Raycast(transform.position, (transform.right + (transform.forward * 2)), out hit, rayLength / 5) && hit.transform.tag != "Checkpoint" && hit.transform.tag != "smallDamage" && hit.transform.tag != "mediumDamage")
                    {
                        if (hit.transform.tag == "Enemy")
                        {

                            dodgeShit(-1f / ((hit.distance / 2) + 1f));
                        }
                        else if (hit.transform.tag == "Checkpoint")
                        {

                            dodgeShit(3f / ((hit.distance / 2) + 1f));
                        }
                        else if (hit.transform.tag == "Player")
                        {

                            dodgeShit(-1f / ((hit.distance / 4) + 1f));
                        }
                        else
                        {

                            dodgeShit(-25f / ((hit.distance / 2) + 1f));
                        }

                    }

                    //Left wall right turn
                    if (Physics.Raycast(transform.position, (-transform.right + transform.forward), out hit, rayLength / 10) && hit.transform.tag != "Checkpoint" && hit.transform.tag != "smallDamage" && hit.transform.tag != "mediumDamage")
                    {
                        if (hit.transform.tag == "Enemy")
                        {

                            dodgeShit(2f / ((hit.distance / 2) + 1f));
                        }
                        else if (hit.transform.tag == "Checkpoint")
                        {

                            dodgeShit(-3f / ((hit.distance / 2) + 1f));
                        }
                        else if (hit.transform.tag == "Player")
                        {

                            dodgeShit(-1f / ((hit.distance / 4) + 1f));
                        }
                        else
                        {

                            dodgeShit(38f / ((hit.distance / 2) + 1f));
                        }

                    }

                    if (Physics.Raycast(transform.position, (-transform.right + (transform.forward * 2)), out hit, rayLength / 5) && hit.transform.tag != "Checkpoint" && hit.transform.tag != "smallDamage" && hit.transform.tag != "mediumDamage")
                    {
                        if (hit.transform.tag == "Enemy")
                        {

                            dodgeShit(1f / ((hit.distance / 2) + 1f));
                        }
                        else if (hit.transform.tag == "Checkpoint")
                        {

                            dodgeShit(-3f / ((hit.distance / 2) + 1f));
                        }
                        else if (hit.transform.tag == "Player")
                        {

                            dodgeShit(1f / ((hit.distance / 4) + 1f));
                        }
                        else
                        {

                            dodgeShit(25f / ((hit.distance / 2) + 1f));
                        }

                    }


                    //rb.MovePosition(transform.position + transform.forward * vehicleSpeed * Time.deltaTime);
                    if (onfloor == true)
                    {
                        rb.AddForce(transform.forward * (vehicleSpeed), ForceMode.Acceleration);
                        rb.velocity = Vector3.ClampMagnitude(rb.velocity, (maxspeed/2) + difficulty);
                        
                    }
                }
            }
        }
        else if (canmove == false && isdead == true)
        {
            if (ht.addhealth() == true)
            {
                isdead = false;
                canmove = true;
            }
        }
    }
    //The faster you move, the higher the pitch
    void EngineSound()
    {
        audioSource.pitch = rb.velocity.magnitude / maxspeed + 1;
    }
}
