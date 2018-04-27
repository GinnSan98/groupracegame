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
    private int truetopspeed;
    [SerializeField]
    private healthTest ht;

    [SerializeField]
    private RaceSystem rs;

    [SerializeField]
    private Lapcheckpoint lcp;

    private int difficulty;
    private int position;


    public int MyDifficulty
    {
        get { return difficulty; }
       
    }

    //For Controlling Sound
    [SerializeField]
    private AudioSource audioSource;

    private IEnumerator Recorrect()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            GameObject nextcp = GameObject.Find("Checkpoint " + lcp.Checkpoint);

            if (Vector3.Dot(transform.position,nextcp.transform.position) < 0.5f)
            {
                yield return new WaitForSeconds(0.1f);
                transform.LookAt(nextcp.transform);
            }
        }
    }
    
    void Start()
    {
        difficulty = Random.Range(20, 30);
        vehicleWidth = bc.size.x;
        truetopspeed = maxspeed;
        rb.centerOfMass = -transform.up;
        Vector3 left = transform.TransformPoint(0, 0, offset);
        Vector3 right = transform.TransformPoint(0, 0, -offset);
        Vector3 avoid = Vector3.zero;

        StartCoroutine(Recorrect());
        
    }
    private void DodgeShit(float  direction)
    {
        rb.MoveRotation(Quaternion.Euler(0, Mathf.Clamp(direction,-2 - (judgementCoeffient/10),2 + (judgementCoeffient/10)) * (vehicleWidth), 0) * transform.rotation);
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
          //  EngineSound();
            if (Physics.Raycast(transform.position, -transform.up, out hit, (bc.size.y) * transform.localScale.magnitude) || Physics.Raycast(transform.position - transform.forward, -transform.up, out hit, (bc.size.y) * transform.localScale.magnitude))
            {
                onfloor = true;
            }
            else if(Physics.Raycast(transform.position, -transform.up, out hit, (bc.size.y) * transform.localScale.magnitude) == false && onfloor == true|| Physics.Raycast(transform.position - transform.forward, -transform.up, out hit, (bc.size.y) * transform.localScale.magnitude) == false && onfloor == true)
            {
                rb.AddForce(transform.forward * 5);
                onfloor = false;
            }
         


            bool turning = false;

            rayLength = (rb.velocity.magnitude / 50) * judgementCoeffient;

            if (onfloor == true)
            {
                if (rb.velocity.magnitude < maxspeed)
                {

                    //Right wall, left turn;
                    if (Physics.Raycast(transform.position, (transform.right + transform.forward * 4), out hit, rayLength) && hit.transform.tag != "Checkpoint" && hit.transform.tag != "smallDamage" && hit.transform.tag != "mediumDamage")
                    {
                        if (hit.transform.tag == "Environment")
                        {
                            turning = true;
                            DodgeShit((-15f / (hit.distance)) - 1f);
                        }
                       
                        else if (hit.transform.tag == "Player")
                        {

                            DodgeShit(3f / ((hit.distance * 8) - 1f));
                        }
                   

                    }

                    //Right wall forward twice.
                    /*if (Physics.Raycast(transform.position, (transform.right + (transform.forward * 4)), out hit, rayLength / 60) && hit.transform.tag != "Checkpoint" && hit.transform.tag != "smallDamage" && hit.transform.tag != "mediumDamage")
                    {
                        if (hit.transform.tag == "Enemy")
                        {

                           DodgeShit(-1f / (hit.distance));
                        }
                        else if (hit.transform.tag == "Checkpoint")
                        {

                           DodgeShit(3f / ((hit.distance * 2) + 1f));
                        }
                        else if (hit.transform.tag == "Player")
                        {

                            DodgeShit(-10f / ((hit.distance * 8) + 1f));
                        }
                        else
                        {
                            turning = true;
                            DodgeShit(-22f / ((hit.distance/2) + 1f));
                        }

                    }*/

                    //Left wall right turn
                    if (Physics.Raycast(transform.position, (-transform.right + transform.forward * 4), out hit, rayLength) && hit.transform.tag != "Checkpoint" && hit.transform.tag != "smallDamage" && hit.transform.tag != "mediumDamage")
                    {
                        if (hit.transform.tag == "Environment")
                        {
                           turning = true;
                           DodgeShit((15f / (hit.distance)) - 1f);
                        }
                
                        else if (hit.transform.tag == "Player")
                        {

                            DodgeShit(-3f / ((hit.distance * 8) + 1f));
                        }

                    }

                    /*if (Physics.Raycast(transform.position, (-transform.right + (transform.forward * 1.5f)), out hit, rayLength / 75) && hit.transform.tag != "Checkpoint" && hit.transform.tag != "smallDamage" && hit.transform.tag != "mediumDamage")
                    {
                        if (hit.transform.tag == "Enemy")
                        {
                            DodgeShit(1f / (hit.distance));
                        }
                        else if (hit.transform.tag == "Checkpoint")
                        {

                           DodgeShit(-3f / ((hit.distance * 2) + 1f));
                        }
                        else if (hit.transform.tag == "Player")
                        {

                            DodgeShit(3f / ((hit.distance * 8) + 1f));
                        }
                        else
                        {
                            turning = true;
                            DodgeShit(22f / ((hit.distance/2) + 1f));
                        }

                    }*/


                    rb.MovePosition(transform.position + transform.forward * vehicleSpeed * Time.deltaTime);
                    if (onfloor == true)
                    {
                        // position = 1;
                        if (rb.velocity.magnitude < maxspeed)
                        {
                            rb.AddForce((transform.forward * vehicleSpeed), ForceMode.Acceleration);
                        }
                        position = rs.BetweenPlayers(lcp,GameObject.FindGameObjectWithTag("Player").GetComponent<Lapcheckpoint>());

                        if (turning == false)
                        {
                            maxspeed = truetopspeed;
                        }
                        else
                        {
                            maxspeed = truetopspeed / 2;
                            
                        }

                       
                        
                    }
                    else
                    {
                        float newLocalAnglesx;
                        newLocalAnglesx = Mathf.Clamp(transform.localEulerAngles.x, -10, 10);
                        transform.localEulerAngles = new Vector3(newLocalAnglesx, transform.eulerAngles.y, transform.eulerAngles.z);
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
