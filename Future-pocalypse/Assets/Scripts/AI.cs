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

    private IEnumerator Recorrect()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            GameObject nextcp = GameObject.Find("Checkpoint " + lcp.Checkpoint);

            if (Vector3.Dot(transform.position,nextcp.transform.position) < 0)
            {
                yield return new WaitForSeconds(0.1f);
                transform.LookAt(nextcp.transform);
            }
        }
    }
    
    void Start()
    {
        difficulty = Random.Range(20, 60);
        vehicleWidth = bc.size.x;

        rb.centerOfMass = -transform.up;
        Vector3 left = transform.TransformPoint(0, 0, offset);
        Vector3 right = transform.TransformPoint(0, 0, -offset);
        Vector3 avoid = Vector3.zero;

        StartCoroutine(Recorrect());
        
    }
    private void DodgeShit(float  direction)
    {
        rb.MoveRotation(Quaternion.Euler(0, Mathf.Clamp(direction,-10 - judgementCoeffient,10 + judgementCoeffient) * (vehicleWidth), 0) * transform.rotation);
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




            rayLength = (rb.velocity.magnitude/5) * judgementCoeffient;

            if (onfloor == true)
            {
                if (rb.velocity.magnitude < maxspeed)
                {

                    //Right wall, left turn;
                    if (Physics.Raycast(transform.position, (transform.right + transform.forward * 2), out hit, rayLength / 10) && hit.transform.tag != "Checkpoint" && hit.transform.tag != "smallDamage" && hit.transform.tag != "mediumDamage")
                    {
                        if (hit.transform.tag == "Environment")
                        {

                            DodgeShit((-35f / (hit.distance)) + 1f);
                        }
                        else if (hit.transform.tag == "Checkpoint")
                        {

                           // dodgeShit(3f / ((hit.distance * 2) + 1f));
                        }
                        else if (hit.transform.tag == "Player")
                        {

                            DodgeShit(-3f / ((hit.distance * 8) + 1f));
                        }
                   

                    }

                    //Right wall forward twice.
                    if (Physics.Raycast(transform.position, (transform.right + (transform.forward * 4)), out hit, rayLength / 5) && hit.transform.tag != "Checkpoint" && hit.transform.tag != "smallDamage" && hit.transform.tag != "mediumDamage")
                    {
                        if (hit.transform.tag == "Enemy")
                        {

                           DodgeShit(5f / (hit.distance));
                        }
                        else if (hit.transform.tag == "Checkpoint")
                        {

                           // dodgeShit(3f / ((hit.distance * 2) + 1f));
                        }
                        else if (hit.transform.tag == "Player")
                        {

                            DodgeShit(-10f / ((hit.distance * 8) + 1f));
                        }
                        //else
                        {

                            DodgeShit(-32f / ((hit.distance/2) + 1f));
                        }

                    }

                    //Left wall right turn
                    if (Physics.Raycast(transform.position, (-transform.right + transform.forward * 2), out hit, rayLength / 5) && hit.transform.tag != "Checkpoint" && hit.transform.tag != "smallDamage" && hit.transform.tag != "mediumDamage")
                    {
                        if (hit.transform.tag == "Environment")
                        {

                            DodgeShit((35f / (hit.distance)) + 1f);
                        }
                        else if (hit.transform.tag == "Checkpoint")
                        {

                         //   dodgeShit(-3f / ((hit.distance * 2) + 1f));
                        }
                        else if (hit.transform.tag == "Player")
                        {

                            DodgeShit(3f / ((hit.distance * 8) + 1f));
                        }

                    }

                    if (Physics.Raycast(transform.position, (-transform.right + (transform.forward * 4)), out hit, rayLength / 10) && hit.transform.tag != "Checkpoint" && hit.transform.tag != "smallDamage" && hit.transform.tag != "mediumDamage")
                    {
                        if (hit.transform.tag == "Enemy")
                        {
                            DodgeShit(-5f / (hit.distance));
                        }
                        else if (hit.transform.tag == "Checkpoint")
                        {

                           // dodgeShit(-3f / ((hit.distance * 2) + 1f));
                        }
                        else if (hit.transform.tag == "Player")
                        {

                            DodgeShit(3f / ((hit.distance * 8) + 1f));
                        }
                     //   else
                        {

                            DodgeShit(32f / ((hit.distance/2) + 1f));
                        }

                    }


                    //rb.MovePosition(transform.position + transform.forward * vehicleSpeed * Time.deltaTime);
                    if (onfloor == true)
                    {
                        // position = 1;
                        position = rs.BetweenPlayers(lcp,GameObject.FindGameObjectWithTag("Player").GetComponent<Lapcheckpoint>());
                        rb.AddForce(transform.forward * (vehicleSpeed + (position)), ForceMode.Acceleration);
                        rb.velocity = Vector3.ClampMagnitude(rb.velocity, (maxspeed/2) + (difficulty + (position))/4);
                        
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
