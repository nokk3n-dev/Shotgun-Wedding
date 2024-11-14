using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D fianceBody;
    private Animator fianceAnim;
    private Animator FILAnim;
    private SpriteRenderer fianceSprite;

    private float xVelocity = 0;

    /************************************************************
     * Fiance Fight Statistics
     ************************************************************
     * fianceMoveSpeed - This controls how fast the fiance moves
     * left and right
     ************************************************************
     * fianceReach - This controls how close the fiance has to 
     * be in order for the punch to hit
     ************************************************************
     * fianceJabDamage - This controls how much damage the jab
     * will do
     ************************************************************
     * fianceBlockDamage - This controls how much damage the
     * cross will do if the FIL is blocking
     ************************************************************
     * fianceCrossDamage - This controls how much damage the 
     * cross will do
     ************************************************************
     * rumPowerUpMultiplier - This controls the damage multiplier
     * for the rum power up
     ************************************************************
     * damageMultiplier - This should always be set to 1, unless
     * the fiance picked up a powerup like the 151 rum which will
     * cause him to do more damage
     ************************************************************/
    private float fianceMoveSpeed = 4f;
    private float fianceReach = 3.7f;
    private float fianceJabDamage = 5.0f;
    private float fianceBlockDamage = 5.0f;
    private float fianceCrossDamage = 10.0f;
    private float rumPowerUpMultiplier = 1.5f;
    private float damageMultiplier = 1.0f;

    // Reference to the HealthManager
    private HealthManager healthManager;

    // Reference to the FIL
    private Transform FIL_Hitbox;

    // Start is called before the first frame update
    void Start()
    {
        fianceBody = GetComponent<Rigidbody2D>();
        fianceAnim = GetComponent<Animator>();
        fianceSprite = GetComponent<SpriteRenderer>();

        // Find the HealthManager
        healthManager = GameObject.FindObjectOfType<HealthManager>();

        // Set up the FIL
        FILAnim = GameObject.FindGameObjectWithTag("FIL").GetComponent<Animator>();
        FIL_Hitbox = GameObject.FindGameObjectWithTag("FIL").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.GameIsPaused)
        {
            return;
        }

        xVelocity = Input.GetAxisRaw("Horizontal");
        fianceBody.velocity = new Vector2(xVelocity * fianceMoveSpeed, fianceBody.velocity.y);

        // Handling Attacks
        if (Input.GetButtonDown("Jab") && xVelocity == 0f)
        {
            fianceAnim.SetBool("throwingJab", true);
            fianceAnim.SetBool("throwingCross", false);
            fianceAnim.SetBool("blocking", false);

            // This will be called during a specific frame in the animation
            //JabFIL();
        } 
        else if (Input.GetButtonDown("Cross") && xVelocity == 0f)
        {
            fianceAnim.SetBool("throwingCross", true);
            fianceAnim.SetBool("throwingJab", false);
            fianceAnim.SetBool("blocking", false);

            // This will be called during a specific frame in the animation
            //CrossFIL();
        }
        else if (Input.GetButton("Block") && xVelocity == 0f)
        {
            fianceAnim.SetBool("blocking", true);
            fianceAnim.SetBool("throwingJab", false);
            fianceAnim.SetBool("throwingCross", false);
        }
        else
        {
            fianceAnim.SetBool("blocking", false);
        }

        // Handling Forward/Backward animations
        if (xVelocity > 0f)
        {
            fianceAnim.SetBool("blocking", false);
            fianceAnim.SetBool("throwingJab", false);
            fianceAnim.SetBool("throwingCross", false);
            fianceAnim.SetBool("moveForward", true);
            fianceAnim.SetBool("moveBack", false);
        }
        else if (xVelocity < 0f)
        {
            fianceAnim.SetBool("blocking", false);
            fianceAnim.SetBool("throwingJab", false);
            fianceAnim.SetBool("throwingCross", false);
            fianceAnim.SetBool("moveBack", true);
            fianceAnim.SetBool("moveForward", false);
        }
        else
        {
            fianceAnim.SetBool("moveForward", false);
            fianceAnim.SetBool("moveBack", false);
        }   // End Forward/Backward Animations
    }   // End Update

    private void StopPunch()
    {
        fianceAnim.SetBool("throwingJab", false);
        fianceAnim.SetBool("throwingCross", false);
    }   // End Stop Punch

    private void JabFIL()
    {
        float distanceToFIL = Vector2.Distance(transform.position, FIL_Hitbox.position);
        
        if (distanceToFIL <= fianceReach && !FILAnim.GetBool("FIL_blocking"))
        {
            Debug.Log("Fiance Deals " + fianceJabDamage*damageMultiplier + " damage");
            healthManager.FILTakeDamage(fianceJabDamage * damageMultiplier);
        }
    }

    private void CrossFIL()
    {
        float distanceToFIL = Vector2.Distance(transform.position, FIL_Hitbox.position);
        
        if (distanceToFIL <= fianceReach)
        {
            if (FILAnim.GetBool("FIL_blocking"))
            {
                Debug.Log("Fiance Deals " + fianceBlockDamage*damageMultiplier + " damage");
                healthManager.FILTakeDamage(fianceBlockDamage * damageMultiplier);
            }
            else 
            {
                Debug.Log("Fiance Deals " + fianceCrossDamage*damageMultiplier + " damage");
                healthManager.FILTakeDamage(fianceCrossDamage * damageMultiplier);
            }
        }
    }

    /************************************************************
     * ActivateRumPowerUp
     ************************************************************
     * Description: This function will return change the Fiance's
     * damage multiplier to the rumPowerUpMultiplier. Which is
     * around (depending on if I changed it recently)
     ************************************************************
     * Parameters: None
     ************************************************************
     * Returns: None
     ************************************************************/
    public void ActivateRumPowerUp()
    {
        damageMultiplier = rumPowerUpMultiplier;
        Debug.Log("Rum Power Up Activated! Damage Multiplier = " + damageMultiplier);
    }

    /************************************************************
     * ResetDamageMultiplier
     ************************************************************
     * Description: This function will return the Fiance's
     * damage multiplier back to normal
     ************************************************************
     * Parameters: None
     ************************************************************
     * Returns: None
     ************************************************************/
    public void ResetDamageMultiplier()
    {
        damageMultiplier = 1.0f;
    }

}   // End class
