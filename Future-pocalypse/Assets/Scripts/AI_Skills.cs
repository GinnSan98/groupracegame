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
        chargespeed = myai.MyDifficulty + 1;
        StartCoroutine(Myupdate());
	}
	

    private IEnumerator Myupdate()
    {
        int maxtotal = ((int)myweapon * 5) + 20;
        int charge = 0;
        yield return new WaitUntil(() => myai.canmove == true);

        while (true)
        {
            charge += chargespeed;
            print(charge + " / " +  maxtotal);
            yield return new WaitForSeconds(1);

            if (charge >= maxtotal)
            {
                GameObject warning = Instantiate(warningcube, transform.position, transform.rotation);

                for (int i = 0; i < 10; i++)
                {
                    switch (myweapon)
                    {
                        case (PlayerWeapons.Weapontypes.Missiledash):
                            {
                                warning.transform.localScale = new Vector3(3, 2, 80);
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
                                    if (Physics.Raycast(nextthing.transform.position, nextthing.transform.right) == false)
                                    {
                                        warning.transform.position = nextthing.transform.position + (nextthing.transform.right * 15);
                                    }
                                    else if (Physics.Raycast(nextthing.transform.position, -nextthing.transform.right) == false)
                                    {
                                        warning.transform.position = nextthing.transform.position - (nextthing.transform.right * 15);
                                    }
                                    else
                                    {
                                        warning.transform.position = nextthing.transform.position - (nextthing.transform.forward * 15);
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
                    
                    yield return new WaitForSeconds(1);
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
                                if (Physics.Raycast(nextthing.transform.position, nextthing.transform.right) == false)
                                {
                                    transform.position = nextthing.transform.position + (nextthing.transform.right*15);
                                }
                                else if (Physics.Raycast(nextthing.transform.position, -nextthing.transform.right) == false)
                                {
                                    transform.position = nextthing.transform.position - (nextthing.transform.right*15);
                                }
                                else
                                {
                                    transform.position = nextthing.transform.position - (nextthing.transform.forward*15);
                                }
                            }

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
