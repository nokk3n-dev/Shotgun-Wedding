using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    // This will allow me to trigger the power up by calling a function in the PlayerMovement script
    private PlayerMovement playerMovement;
    
    // This will allow me to trigger the damage reduction by reducing the FIL's attack
    private FIL_Fight_Logic FILController;

    // This will allow me to call the heal function from the HealthManager
    private HealthManager healthManager;

    private void Start()
    {
        playerMovement = GameObject.FindWithTag("Fiance").GetComponent<PlayerMovement>();
        FILController = GameObject.FindWithTag("FIL").GetComponent<FIL_Fight_Logic>();
        healthManager = GameObject.FindObjectOfType<HealthManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Rum"))
        {
            // Make sure it got initialized properly and then call the function
            playerMovement?.ActivateRumPowerUp();
            FILController?.ActivateRumDamageReduction();
            healthManager.FianceHeal(10f);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Coconut"))
        {
            // Make sure it got initialized properly and then call the function
            FILController?.ActivateCoconutArmor();
            healthManager.FianceHeal(25f);
            Destroy(collision.gameObject);
        }
    }
}
