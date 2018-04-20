using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> trapSpawns;
    public int checkpointnum;
    public GameObject[] trapsToSpawn;
    public float cooldownTime;
    public bool cooldown = false;


    private void Update()
    {
        if (cooldown == true)
        {
            if(cooldownTime > 0)
            {
                cooldownTime -= Time.deltaTime;
            }

            else
            {
                cooldown = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if(cooldown == false)
        {
            if (other.gameObject.transform.tag == "Player" || other.gameObject.transform.tag == "Enemy")
            {
                if(other.GetComponent<Lapcheckpoint>().CheckLapProgress() == true)
                {
                    if (trapSpawns.Capacity > 0)
                    {

                        int randomTrap = Random.Range(0, 3);
                        int randomLocation = Random.Range(0, trapSpawns.Capacity - 1);
                        GameObject newTrap = Instantiate(trapsToSpawn[randomTrap], trapSpawns[randomLocation].transform.position, trapSpawns[randomLocation].transform.localRotation);
                        trapSpawns.Remove(trapSpawns[randomLocation]);
                        cooldown = true;
                        cooldownTime = 5;
                    }
                }            

            }

        }

    }
}
