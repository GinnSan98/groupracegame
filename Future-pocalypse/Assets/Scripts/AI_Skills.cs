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
    private PlayerWeapons.Weapontypes myweapon;

	// Use this for initialization
	void Start ()
    {
        lcp = GetComponent<Lapcheckpoint>();
        chargespeed = (myai.MyDifficulty/10) + 1;
        StartCoroutine(Myupdate());
	}
	

    private IEnumerator Myupdate()
    {
        int maxtotal = ((int)myweapon * 25) + 70;
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
                                warning.transform.localScale = new Vector3(3, 2, 60);
                                warning.transform.position = transform.position + (transform.forward*warning.transform.localScale.z/2);
                                warning.transform.parent = this.transform;

                                break;
                            }
                        case (PlayerWeapons.Weapontypes.TimeSlow):
                            {
                                Transform nextthing = rs.Returnplayerahead(lcp);


                                if (nextthing != null)
                                {
                                    warning.transform.localScale = new Vector3(5, 5, 15);
                                    warning.transform.rotation = nextthing.transform.rotation;
                                    if (Physics.Raycast(nextthing.transform.position + (nextthing.transform.right * 10), nextthing.transform.right,10) == false)
                                    {
                                        Debug.DrawRay(nextthing.transform.position, nextthing.transform.right*10);
                                        warning.transform.position = nextthing.transform.position + (nextthing.transform.right*10);
                                    }
                                    else if (Physics.Raycast(nextthing.transform.position - (nextthing.transform.right * 10), -nextthing.transform.right,10) == false)
                                    {
                                        Debug.DrawRay(nextthing.transform.position, -nextthing.transform.right*10);
                                        warning.transform.position = nextthing.transform.position - (nextthing.transform.right * 10);
                                    }
                                    else
                                    {
                                        warning.transform.position = nextthing.transform.position + (nextthing.transform.forward * 10);
                                        Debug.DrawRay(nextthing.transform.position, nextthing.transform.right * 10);
                                        Debug.DrawRay(nextthing.transform.position, -nextthing.transform.right * 10);
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
                                    warning.transform.localScale = new Vector3(5, 5, 5);
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
                            int oldspeed = myai.vehicleSpeed;
                            for (int i = 0; i < 10; i++)
                            {
                                myai.vehicleSpeed +=20;
                                yield return new WaitForSeconds(4);
                            }

                            myai.vehicleSpeed = oldspeed;
                            break;
                        }
                    case (PlayerWeapons.Weapontypes.TimeSlow):
                        {
                            Transform nextthing = rs.Returnplayerahead(lcp);
                            if (nextthing != null)
                            {
                                if (Physics.Raycast(nextthing.transform.position + (nextthing.transform.right * 10), nextthing.transform.right,10) == false)
                                {
                                    Debug.DrawRay(nextthing.transform.position, nextthing.transform.right * 10);
                                    transform.position = nextthing.transform.position + (nextthing.transform.right*10);
                                }
                                else if (Physics.Raycast(nextthing.transform.position - (nextthing.transform.right * 10), -nextthing.transform.right,10) == false)
                                {
                                    Debug.DrawRay(nextthing.transform.position, -nextthing.transform.right * 10);
                                    transform.position = nextthing.transform.position - (nextthing.transform.right*10);
                                }
                                else
                                {
                                    transform.position = nextthing.transform.position - (nextthing.transform.forward*10);
                                }
                            }
                            transform.rotation = nextthing.rotation;
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
                                    if (Vector3.Distance(transform.position, nextthing.position) < 75)
                                    {
                                        ht.takedamage(5);
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
