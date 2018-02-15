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



    public float Returnhealth()
    {
        return health;
    }

    public float ReturnMaxhealth()
    {
        return maxHealth;
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
        if (other.gameObject.CompareTag("smallDamage"))
        {
            health -= 1;

        }

        if (other.gameObject.CompareTag("mediumDamage"))
        {
            health -=25;

        }

        if (other.gameObject.CompareTag("largeDamage"))
        {
            health -= 50;

        }
    }
}
