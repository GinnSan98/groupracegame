using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class healthTest : MonoBehaviour
{ 
    [SerializeField]
    private int playerHealth;
    [SerializeField]
    private int playermaxhealth;
    // Use this for initialization



    public float Returnhealth()
    {
        return playerHealth;
    }

    public float ReturnMaxhealth()
    {
        return playermaxhealth;
    }
    void Start()
    {
        playermaxhealth = 100;
        playerHealth = 100;
      
    }

    // Update is called once per frame

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("smallDamage"))
        {
            playerHealth -= 1;

        }

        if (other.gameObject.CompareTag("mediumDamage"))
        {
            playerHealth -=25;

        }

        if (other.gameObject.CompareTag("largeDamage"))
        {
            playerHealth -= 50;

        }
    }
}
