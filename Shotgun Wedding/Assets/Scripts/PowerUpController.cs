using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    // This will allow me to trigger the power up by calling a function in the PlayerMovement script
    private PlayerMovement playerMovement;
    
    // This will allow me to trigger the damage reduction by reducing the FIL's attack
    private FIL_Fight_Logic FILController;

    private void Start()
    {
        playerMovement = GameObject.FindWithTag("Fiance").GetComponent<PlayerMovement>();
        FILController = GameObject.FindWithTag("FIL").GetComponent<FIL_Fight_Logic>();
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Rum"))
        {
            // Make sure it got initialized properly and then call the function
            playerMovement?.ActivateRumPowerUp();
            FILController?.ActivateRumDamageReduction();
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Coconut"))
        {
            // Make sure it got initialized properly and then call the function
            FILController?.ActivateCoconutArmor();
            Destroy(collision.gameObject);
        }
    }
}
