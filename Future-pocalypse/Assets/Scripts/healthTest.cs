using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class healthTest : MonoBehaviour {

    public Text healthText;
    private int playerHealth;
    // Use this for initialization

    void Start()
    {
        playerHealth = 100;
        SetHealthText();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("smallDamage"))
        {
            playerHealth = playerHealth - 1;
            SetHealthText();
        }

        if (other.gameObject.CompareTag("mediumDamage"))
        {
            playerHealth = playerHealth - 25;
            SetHealthText();
        }

        if (other.gameObject.CompareTag("largeDamage"))
        {
            playerHealth = playerHealth - 50;
            SetHealthText();
        }
    }

    void SetHealthText()
    {
        healthText.text = "Health: " + playerHealth.ToString();
        if (playerHealth <= 0)
        {
            healthText.text = "You're dead boi";
        }
    }
}
