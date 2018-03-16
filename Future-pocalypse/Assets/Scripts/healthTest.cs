using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class healthTest : MonoBehaviour
{ 
    [SerializeField]
    private int health;
    [SerializeField]
    private int maxHealth;
    // Use this for initialization

    public void takedamage(int damage)
    {
        health -= damage;
        DisableVehicle();
    }

    public float Returnhealth()
    {
        return health;
    }

    public float ReturnMaxhealth()
    {
        return maxHealth;
    }

    public bool addhealth()
    {
        health++;

        if (health < maxHealth)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void Start()
    {
        maxHealth = 100;
        health = maxHealth;
      
    }

     void Update()
    {
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
    }

    // Update is called once per frame

    void OnTriggerEnter(Collider other)
    {
        if (transform.tag == "Player")
        {
            if (GetComponent<CarDriving>().isDead == false)
            {
                if (other.gameObject.CompareTag("smallDamage"))
                {
                    health -= 5;

                }

                if (other.gameObject.CompareTag("mediumDamage"))
                {
                    health -= 15;

                }

                if (other.gameObject.CompareTag("largeDamage"))
                {
                    health -= 30;

                }
            }

        }

        if (transform.tag == "Enemy")
        {
            if (GetComponent<AI>().isdead == false)
            {
                if (other.gameObject.CompareTag("smallDamage"))
                {
                    health -= 5;

                }

                if (other.gameObject.CompareTag("mediumDamage"))
                {
                    health -= 15;

                }

                if (other.gameObject.CompareTag("largeDamage"))
                {
                    health -= 30;

                }
            }
        }

        DisableVehicle();
    }

    private void DisableVehicle()
    {
        if (health <= 0 && transform.tag == "Player")
        {
            GetComponent<CarDriving>().canDrive = false;
            GetComponent<CarDriving>().isDead = true;
        }
        else if (health <= 0 && transform.tag == "Enemy")
        {
            GetComponent<AI>().canmove = false;
            GetComponent<AI>().isdead = true;
        }
    }
}
