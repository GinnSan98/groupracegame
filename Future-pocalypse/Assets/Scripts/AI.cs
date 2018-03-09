using System.Collections;
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
    private RaceSystem rs;

    [SerializeField]
    private Lapcheckpoint lcp;

    private int difficulty;

    private int position;

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




            rayLength = rb.velocity.magnitude * judgementCoeffient;

            if (onfloor == true)
            {
                if (rb.velocity.magnitude < maxspeed)
                {

                    //Right wall, left turn;
                    if (Physics.Raycast(transform.position, (transform.right + transform.forward * 2), out hit, rayLength / 20) && hit.transform.tag != "Checkpoint" && hit.transform.tag != "smallDamage" && hit.transform.tag != "mediumDamage")
                    {
                        if (hit.transform.tag == "Enemy")
                        {

                            dodgeShit(-1f / ((hit.distance) + 1f));
                        }
                        if (hit.transform.tag == "Environment")
                        {

                            dodgeShit(-6f / ((hit.distance) + 1f));
                        }
                        else if (hit.transform.tag == "Checkpoint")
                        {

                            dodgeShit(3f / ((hit.distance / 2) + 1f));
                        }
                        else if (hit.transform.tag == "Player")
                        {

                            dodgeShit(-1f / ((hit.distance / 4) + 1f));
                        }
                   

                    }

                    //Right wall forward twice.
                    if (Physics.Raycast(transform.position, (transform.right + (transform.forward * 8)), out hit, rayLength / 10) && hit.transform.tag != "Checkpoint" && hit.transform.tag != "smallDamage" && hit.transform.tag != "mediumDamage")
                    {
                        if (hit.transform.tag == "Enemy")
                        {

                            dodgeShit(-1f / ((hit.distance) + 1f));
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

                            dodgeShit(-16f / ((hit.distance) + 1f));
                        }

                    }

                    //Left wall right turn
                    if (Physics.Raycast(transform.position, (-transform.right + transform.forward * 2), out hit, rayLength / 20) && hit.transform.tag != "Checkpoint" && hit.transform.tag != "smallDamage" && hit.transform.tag != "mediumDamage")
                    {
                        if (hit.transform.tag == "Enemy")
                        {

                            dodgeShit(1f / ((hit.distance) + 1f));
                        }
                        if (hit.transform.tag == "Environment")
                        {

                            dodgeShit(6f / ((hit.distance) + 1f));
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

                            dodgeShit(16f / ((hit.distance) + 1f));
                        }

                    }

                    if (Physics.Raycast(transform.position, (-transform.right + (transform.forward * 8)), out hit, rayLength / 10) && hit.transform.tag != "Checkpoint" && hit.transform.tag != "smallDamage" && hit.transform.tag != "mediumDamage")
                    {
                        if (hit.transform.tag == "Enemy")
                        {

                            dodgeShit(1f / ((hit.distance) + 1f));
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

                            dodgeShit(16f / ((hit.distance) + 1f));
                        }

                    }


                    //rb.MovePosition(transform.position + transform.forward * vehicleSpeed * Time.deltaTime);
                    if (onfloor == true)
                    {
                       // position = 1;
                        position = rs.returnposition(lcp);
                        rb.AddForce(transform.forward * (vehicleSpeed + (position * 5 )), ForceMode.Acceleration);
                        rb.velocity = Vector3.ClampMagnitude(rb.velocity, (maxspeed/2) + (difficulty + (position * 20)));
                        
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
