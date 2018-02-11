using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    private bool onfloor;
    public int vehicleSpeed;
    public float vehicleWidth;
    public float judgementCoeffient;
    float offset = 2f;
    Vector3 left;
    Vector3 right;
    Vector3 avoid;
    float rayLength;
    Ray rightRay;
    Ray leftRay;
    Ray centreRay;
    RaycastHit hit;
    [SerializeField]
    private BoxCollider bc;

    private int maxspeed = 1100;

    [SerializeField]
    private Lapcheckpoint lcp;

    private int difficulty;

    void Start()
    {
        difficulty = Random.Range(10, 30);
        vehicleWidth = bc.size.x;
        judgementCoeffient = 18;
      //  vehicleSpeed = 80 + difficulty;
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

        if (Physics.Raycast(transform.position, -transform.up, out hit, bc.size.y))
        {
            onfloor = true;
        }
        else
        {
            onfloor = false;
        }


        
       
        rayLength = vehicleSpeed * judgementCoeffient ;

        if (onfloor == true)
        {
            if (rb.velocity.magnitude < vehicleSpeed)
            {

                if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength / 4))
                {
                    if (hit.transform.tag == "Checkpoint")
                    {
                        //transform.LookAt(hit.transform);
                        Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.green);

                    }

                    else
                    {
                        vehicleSpeed = 60 + difficulty; 
                        Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);
                        dodgeShit(-25f / hit.distance);
                    }


                }
                else
                {

                    vehicleSpeed = 100 + difficulty; 
                }
                if (Physics.Raycast(transform.position, (transform.right + transform.forward), out hit, rayLength / 20) && hit.transform.tag != "Checkpoint")
                {
                    if (hit.transform.tag == "Enemy")
                    {
                        Debug.DrawRay(transform.position, (transform.right + transform.forward) * hit.distance, Color.blue);
                        dodgeShit(-2f / (hit.distance + 1f));
                    }
                    else
                    {
                        Debug.DrawRay(transform.position, (transform.right + transform.forward) * hit.distance, Color.blue);
                        dodgeShit(-50f / (hit.distance + 1f));
                    }

                }
                if (Physics.Raycast(transform.position, (-transform.right + transform.forward), out hit, rayLength / 20) && hit.transform.tag != "Checkpoint")
                {
                    if (hit.transform.tag == "Enemy")
                    {
                        Debug.DrawRay(transform.position, (-transform.right + transform.forward) * hit.distance, Color.red);
                        dodgeShit(2f / (hit.distance + 1f));
                    }
                    else
                    {
                        Debug.DrawRay(transform.position, (-transform.right + transform.forward) * hit.distance, Color.red);
                        dodgeShit(38f / (hit.distance + 1f));
                    }

                }
                //rb.MovePosition(transform.position + transform.forward * vehicleSpeed * Time.deltaTime);
                if (rb.velocity.magnitude <  maxspeed)
                {
                    rb.AddForce(transform.forward * (vehicleSpeed), ForceMode.Acceleration);
                }
            }
        }
    }
}
