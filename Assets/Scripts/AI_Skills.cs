using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using weapons;
public class AI_Skills : MonoBehaviour
{

    [SerializeField]
    private int chargespeed;
    [SerializeField]
    private AI myai;
    [SerializeField]
    private RaceSystem rs;
    [SerializeField]
    private GameObject warningcube;
    private Lapcheckpoint lcp;

    [SerializeField]
    private ParticleSystem[] ps;
    [SerializeField]
    private PlayerWeapons.Weapontypes myweapon;

	// Use this for initialization
	void Start ()
    {
        lcp = GetComponent<Lapcheckpoint>();
        chargespeed = (myai.MyDifficulty/10) + 1;
        StartCoroutine(Myupdate());
        StartCoroutine(CheckhowbehindIam());
    }

    private IEnumerator CheckhowbehindIam()
    {
        yield return new WaitUntil(() => myai.canmove == true);
        Transform temp = rs.Returnplayerahead(lcp);
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (rs.Returnplayerahead(lcp) != null)
            {
                temp = rs.Returnplayerahead(lcp);
               

                if (Vector3.Distance(transform.position, temp.position) > 500)
                {
                    myweapon = PlayerWeapons.Weapontypes.TimeSlow;
                }
                else if (Vector3.Distance(transform.position, temp.position) < 500 && Vector3.Distance(transform.position, temp.position) > 250)
                {
                    myweapon = PlayerWeapons.Weapontypes.Machinegunfire;
                }
                else
                {
                    myweapon = PlayerWeapons.Weapontypes.Missiledash;
                }
            }
            else
            {
               
            }
        }
    }

    private IEnumerator Myupdate()
    {
        int maxtotal = ((int)myweapon * 25) + 30;
        int charge = 0;
        yield return new WaitUntil(() => myai.canmove == true);

        while (true)
        {
           
            charge += chargespeed;
            yield return new WaitForSeconds(2);

            if (charge >= maxtotal)
            {
                GameObject warning = Instantiate(warningcube, transform.position, transform.rotation);

                for (int i = 0; i < 100; i++)
                {
                    switch (myweapon)
                    {
                        case (PlayerWeapons.Weapontypes.Missiledash):
                            {
                                warning.transform.localScale = new Vector3(0.75f, 1, 10);
                                warning.transform.position = transform.position + (transform.forward*warning.transform.localScale.z/2);
                                warning.transform.parent = this.transform;

                                break;
                            }
                        case (PlayerWeapons.Weapontypes.TimeSlow):
                            {
                                Transform nextthing = rs.Returnplayerahead(lcp);


                                if (nextthing != null)
                                {
                                    warning.transform.localScale = new Vector3(1, 1, 5);
                                    warning.transform.rotation = nextthing.transform.rotation;
                                    if (Physics.Raycast(nextthing.transform.position + (nextthing.transform.right *3), nextthing.transform.right,10) == false)
                                    {
                                        Debug.DrawRay(nextthing.transform.position, nextthing.transform.right*3);
                                        warning.transform.position = nextthing.transform.position + (nextthing.transform.right*3);
                                    }
                                    else if (Physics.Raycast(nextthing.transform.position - (nextthing.transform.right * 3), -nextthing.transform.right,10) == false)
                                    {
                                        Debug.DrawRay(nextthing.transform.position, -nextthing.transform.right*10);
                                        warning.transform.position = nextthing.transform.position - (nextthing.transform.right *3);
                                    }
                                    else
                                    {
                                        warning.transform.position = nextthing.transform.position + (nextthing.transform.forward * 3);
                                        Debug.DrawRay(nextthing.transform.position, nextthing.transform.right * 3);
                                        Debug.DrawRay(nextthing.transform.position, -nextthing.transform.right * 3);
                                    }
                                    warning.transform.parent = nextthing;
                                    lcp.Setmyplace(nextthing.GetComponent<Lapcheckpoint>().ReturnmyPlace());
                                    
                                }

                                break;
                            }
                        case (PlayerWeapons.Weapontypes.Machinegunfire):
                            {
                                Transform nextthing = rs.Returnplayerahead(lcp);
                                if (nextthing != null)
                                {
                                    warning.transform.position = nextthing.position;
                                    warning.transform.localScale = new Vector3(0.5f, 0.3f, 0.5f);
                                }
                                
                                break;
                            }
                        default:
                            {
                                myweapon = PlayerWeapons.Weapontypes.Missiledash;
                                break;
                            }

                    }
                    
                    yield return new WaitForSeconds(0.1f);
                }
                //spawn something.
                Destroy(warning);

                switch (myweapon)
                {
                    case (PlayerWeapons.Weapontypes.Missiledash):
                        {
                            foreach(ParticleSystem myps in ps)
                            {
                                myps.Play();
                            }
                            print("Boost! " + name);
                            int oldspeed = myai.vehicleSpeed;
                            transform.localEulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
                            GetComponent<Rigidbody>().AddForce(transform.forward * 5, ForceMode.VelocityChange);
                            for (int i = 0; i < 2; i++)
                            {

                                myai.vehicleSpeed +=15;
                                yield return new WaitForSeconds(1);
                            }
                            foreach (ParticleSystem myps in ps)
                            {
                                myps.Stop();
                            }
                            myai.vehicleSpeed = oldspeed;
                            break;
                        }
                    case (PlayerWeapons.Weapontypes.TimeSlow):
                        {
                            Transform nextthing = rs.Returnplayerahead(lcp);
                            if (nextthing != null)
                            {
                                if (Physics.Raycast(nextthing.transform.position + (nextthing.transform.right * 3), nextthing.transform.right,10) == false)
                                {
                                    Debug.DrawRay(nextthing.transform.position, nextthing.transform.right * 3);
                                    transform.position = nextthing.transform.position + (nextthing.transform.right*3);
                                }
                                else if (Physics.Raycast(nextthing.transform.position - (nextthing.transform.right * 3), -nextthing.transform.right,10) == false)
                                {
                                    Debug.DrawRay(nextthing.transform.position, -nextthing.transform.right * 3);
                                    transform.position = nextthing.transform.position - (nextthing.transform.right*3);
                                }
                                else
                                {
                                    transform.position = nextthing.transform.position + (nextthing.transform.forward*3);
                                }
                            }
                            transform.rotation = nextthing.rotation;
                            GetComponent<Rigidbody>().velocity = nextthing.GetComponent<Rigidbody>().velocity;
                            GetComponent<Rigidbody>().AddForce(transform.forward * 10, ForceMode.VelocityChange);

                            break;
                        }
                    case (PlayerWeapons.Weapontypes.Machinegunfire):
                        {
                            Transform nextthing = rs.Returnplayerahead(lcp);
                            if (nextthing != null)
                            {
                                healthTest ht = nextthing.gameObject.GetComponent<healthTest>();
                                for (int i = 0; i < 5; i++)
                                {
                                    if (Vector3.Distance(transform.position, nextthing.position) < 475)
                                    {
                                        ht.takedamage(12);
                                        yield return new WaitForSeconds(0.5f);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                    default:
                        {

                            break;
                        }

                }

                charge = 0;
            }
        }
    }
}
