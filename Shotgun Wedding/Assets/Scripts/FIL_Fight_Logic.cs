using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FIL_Fight_Logic : MonoBehaviour
{
    // Define state machine for FIL's behavior
    private enum FILState { Idle, Approaching, Attacking, Retreating };
    private FILState currentState;
    private float stateStartTime;

    private Rigidbody2D FILBody;
    private Animator FILAnim;
    private Animator fianceAnim;

    // This will help me stop the FIL from jabbing non-stop
    private bool canJab = true;
    private bool canCross = true;
    private float jabCooldown = 0.5f;
    private float crossCooldown = 1f;

    /************************************************************
     * Father in Law (FIL) Fight Statistics
     ************************************************************
     * FILMoveSpeed - This controls how fast the FIL moves 
     * left and right
     ************************************************************
     * FILReach - This controls how close the FIL has to be for 
     * his punches to connect
     ************************************************************
     * FILJabDamage - This controls how much damage the FIL's 
     * jab does
     ************************************************************
     * FILBlockDamage - This controls how much damage the FIL's 
     * cross does if the Fiance is blocking
     ************************************************************
     * FILCrossDamage - This controls how much damage the FIL's 
     * cross does
     ************************************************************
     * currentDamageMultiplier - This should always be 1 unless
     * the fiance has picked up a power up. It will determine
     * how much more or less damage the FIL will do.
     ************************************************************/
    private float FILMoveSpeed = 3f;
    private float FILReach = 4f;
    private float FILJabDamage = 5f;
    private float FILBlockDamage = 5f;
    private float FILCrossDamage = 15f;
    private float currentDamageMultiplier = 1f;

    /************************************************************
     * Fiance Damage Reduction Stats
     ************************************************************
     * immortalFiance - This will multiply the damage done to the
     * fiance by 0, which means he takes no damage, because he
     * is immortal. I am not sure if I will ever use this, but 
     * it is here if necessary.
     ************************************************************
     * coconutArmor - This will reduce the damage that the fiance
     * takes by 50%. This happens if the fiance gets the coconut
     * powerup. When I say it reduces the damage taken, I really
     * mean that it just makes the FIL do less damage.
     ************************************************************
     * tutorialArmor - This will reduce the damage that the
     * fiance takes by 75%. It is meant to make the tutorial
     * hard to lose at. Same with the coconut armor, this works 
     * by making the FIL do only 25% of his normal damage.
     ************************************************************
     * rumDamageReduction - This will reduce the damage that the
     * fiance takes by 25% due to the Rum Power Up. Same with 
     * the coconut armor, this works by making the FIL do only 
     * 75% of his normal damage.
     ************************************************************/
    private float immortalFiance = 0f;
    private float coconutArmor = 0.5f;
    private float tutorialArmor = 0.25f;
    private float rumDamageReduction = 0.75f;

    // Reference to the HealthManager
    private HealthManager healthManager;

    // Reference to the FIL
    private Transform Fiance_Hitbox;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            ActivateTutorialArmour();
        } else {
            ResetDamageMultiplier();
        }
        // Set up FIL
        FILBody = GetComponent<Rigidbody2D>();
        FILAnim = GetComponent<Animator>();

        // Find the HealthManager
        healthManager = GameObject.FindObjectOfType<HealthManager>();

        // Set up Fiance
        fianceAnim = GameObject.FindGameObjectWithTag("Fiance").GetComponent<Animator>();
        Fiance_Hitbox = GameObject.FindGameObjectWithTag("Fiance").transform;

        currentState = FILState.Attacking;
        stateStartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.GameIsPaused)
        {
            return;
        }

        float fianceDistance = Vector2.Distance(transform.position, Fiance_Hitbox.position);
        
        if (healthManager.GetFILHealth() <= 25)
        {
            FinalStand(fianceDistance);
        }
        else if (healthManager.GetFILHealth() <= 50)
        {
            AdvancedFighting(fianceDistance);
        } 
        else 
        {
            BasicFighting(fianceDistance);
        }
    }

    private void JabDamageCheck()
    {
        float distanceToFiance = Vector2.Distance(transform.position, Fiance_Hitbox.position);

        if (distanceToFiance <= FILReach && !fianceAnim.GetBool("blocking"))
        {
            if (!fianceAnim.GetBool("blocking"))
            {
                fianceAnim.SetTrigger("impact");
            }
            //Debug.Log("FIL Deals " + FILJabDamage * currentDamageMultiplier + " damage");
            healthManager.FianceTakeDamage(FILJabDamage * currentDamageMultiplier);
        }
    }   // End JabDamageCheck

    private void CrossDamageCheck()
    {
        float distanceToFiance = Vector2.Distance(transform.position, Fiance_Hitbox.position);

        if (distanceToFiance <= FILReach)
        {
            if (fianceAnim.GetBool("blocking"))
            {
                //Debug.Log("FIL Deals " + FILBlockDamage * currentDamageMultiplier + " damage");
                fianceAnim.SetTrigger("blockImpact");
                healthManager.FianceTakeDamage(FILBlockDamage * currentDamageMultiplier);
            }
            else 
            {
                //Debug.Log("FIL Deals " + FILCrossDamage * currentDamageMultiplier + " damage");
                fianceAnim.SetTrigger("impact");
                healthManager.FianceTakeDamage(FILCrossDamage * currentDamageMultiplier);
            }
        }
    }

    private void Jab()
    {
        Idle();
        FILAnim.SetBool("FIL_throwingJab", true);
    }

    private void Cross()
    {
        Idle();
        FILAnim.SetBool("FIL_throwingCross", true);
    }

    private void JabCross()
    {
        Idle();
        FILAnim.SetBool("FIL_throwingJabCross", true);
    }

    private void CrossJab()
    {
        Idle();
        FILAnim.SetBool("FIL_throwingCrossJab", true);
    }

    private void Block()
    {
        Idle();
        FILAnim.SetBool("FIL_blocking", true);
    }

    private void Idle()
    {
        FILAnim.SetBool("FIL_throwingJab", false);
        FILAnim.SetBool("FIL_throwingCross", false);
        FILAnim.SetBool("FIL_throwingCrossJab", false);
        FILAnim.SetBool("FIL_throwingJabCross", false);
        FILAnim.SetBool("FIL_blocking", false);
        FILAnim.SetBool("FIL_moveForward", false);
        FILAnim.SetBool("FIL_moveBack", false);
    }

    private void moveForward()
    {
        Idle();
        FILAnim.SetBool("FIL_moveForward", true);
        FILBody.velocity = new Vector2(-FILMoveSpeed, FILBody.velocity.y);
    }

    private void moveBackward()
    {
        Idle();
        FILAnim.SetBool("FIL_moveBack", true);
        FILBody.velocity = new Vector2(FILMoveSpeed, FILBody.velocity.y);
    }

    /************************************************************
     * ActivateTutorialArmour
     ************************************************************
     * Description: This function will change the current damage
     * multiplier variable to the tutorialArmor. This will mean
     * that the FIL is doing 25% of his normal damage. You can
     * also think of it as the Fiance has 75% damage reduction.
     ************************************************************
     * Parameters: None
     ************************************************************
     * Returns: None
     ************************************************************/
    public void ActivateTutorialArmour()
    {
        currentDamageMultiplier = tutorialArmor;
    }

    /************************************************************
     * ActivateCoconutArmor
     ************************************************************
     * Description: This function will be called when the fiance
     * picks up the coconut power up. It will make the FIL do
     * 50% of his normal damage, which means the Fiance has a
     * 50% damage reduction.
     ************************************************************
     * Parameters: None
     ************************************************************
     * Returns: None
     ************************************************************/
    public void ActivateCoconutArmor()
    {
        currentDamageMultiplier = coconutArmor;
    }

    /************************************************************
     * ActivateImmortalFiance
     ************************************************************
     * Description: This function will multiply the FIL's damage
     * output by 0 which means he will do no damage.
     ************************************************************
     * Parameters: None
     ************************************************************
     * Returns: None
     ************************************************************/
    public void ActivateImmortalFiance()
    {
        currentDamageMultiplier = immortalFiance;
    }

    /************************************************************
     * ActivateRumDamageReduction
     ************************************************************
     * Description: This function will set the current damage
     * multiplier to the rumDamageReduction
     ************************************************************
     * Parameters: None
     ************************************************************
     * Returns: None
     ************************************************************/
    public void ActivateRumDamageReduction()
    {
        currentDamageMultiplier = rumDamageReduction;
    }

    /************************************************************
     * ResetDamageMultiplier
     ************************************************************
     * Description: This function will return the FIL's damage
     * multiplier back to 1, so he just does normal damage.
     ************************************************************
     * Parameters: None
     ************************************************************
     * Returns: None
     ************************************************************/
    public void ResetDamageMultiplier()
    {
        currentDamageMultiplier = 1f;
    }

    /************************************************************
     * BasicFighting
     ************************************************************
     * Description: This function will control the movement and
     * the punches that the FIL throw. 
     ************************************************************
     * Parameters: 
     * fianceDistance - This function controls the FIL's Basic 
    * fighting logic. The FIL will move toward the target, throw 
    * a punch, and retreat. The behavior cycles through states 
    * for a dynamic and strategic engagement.
     ************************************************************
     * Returns: None
     ************************************************************/
    public void BasicFighting(float fianceDistance)
    {
        // Transition logic
        switch (currentState)
        {
            case FILState.Idle:
                Idle();

                //Debug.Log("FIL is Idle");

                if (fianceDistance > FILReach)
                {
                    currentState = FILState.Approaching;
                    stateStartTime = Time.time;
                }
                else
                {
                    currentState = FILState.Attacking;
                    stateStartTime = Time.time;
                }
                break;

            case FILState.Approaching:
                moveForward();

                //Debug.Log("FIL is Approaching");

                if (fianceDistance <= FILReach)
                {
                    currentState = FILState.Attacking;
                    stateStartTime = Time.time;
                }
                break;

            case FILState.Attacking:
                
                //Debug.Log("FIL is Attacking");

                // Throw a combo based on proximity
                if (fianceDistance <= 2)
                {
                    currentState = FILState.Retreating;
                }
                else if (fianceDistance <= 2.5)
                {
                    Cross();
                }
                else if (fianceDistance <= 3.8)
                {
                    Jab();
                }
                else if (fianceDistance <= 4.5)
                {
                    JabCross();
                } 
                else if (fianceDistance >= 5)
                {
                    currentState = FILState.Approaching;
                }

                // Allow attacks for a short duration, then retreat
                if (Time.time - stateStartTime > 1f) // 1 second of attacking
                {
                    currentState = FILState.Retreating;
                    stateStartTime = Time.time;
                }
                break;

            case FILState.Retreating:
                moveBackward();
                //Debug.Log("FIL is Retreating for " + (Time.time) + "-" + stateStartTime + " seconds");

                if (Time.time - stateStartTime > 2f) // Retreat for 2 seconds, then reengage
                {
                    currentState = FILState.Idle;
                    stateStartTime = Time.time;
                }
                break;
        }

    }

    /************************************************************
    * AdvancedFighting
    ************************************************************
    * Description: This function controls the FIL's advanced 
    * fighting logic. The FIL will move toward the target, throw 
    * a combo, and retreat. The behavior cycles through states 
    * for a dynamic and strategic engagement.
    ************************************************************
    * Parameters:
    * fianceDistance - The distance from the FIL to the fiance. 
    * Determines whether to approach, attack, or retreat.
    ************************************************************
    * Returns: None
    ************************************************************/
    public void AdvancedFighting(float fianceDistance)
    {

        // Transition logic
        switch (currentState)
        {
            case FILState.Idle:
                Idle();

                //Debug.Log("FIL is Idle");

                if (fianceDistance > FILReach)
                {
                    currentState = FILState.Approaching;
                    stateStartTime = Time.time;
                }
                else
                {
                    currentState = FILState.Attacking;
                    stateStartTime = Time.time;
                }
                break;

            case FILState.Approaching:
                moveForward();

                //Debug.Log("FIL is Approaching");

                if (fianceDistance <= FILReach)
                {
                    currentState = FILState.Attacking;
                    stateStartTime = Time.time;
                }
                break;

            case FILState.Attacking:
                
                //Debug.Log("FIL is Attacking");

                // Throw a combo based on proximity
                if (fianceDistance <= 2)
                {
                    Block();
                }
                else if (fianceDistance <= 2.5)
                {
                    CrossJab();
                }
                else if (fianceDistance <= 3.8)
                {
                    JabCross();
                }
                else if (fianceDistance <= 4.5)
                {
                    Block();
                } 
                else if (fianceDistance >= 5)
                {
                    currentState = FILState.Approaching;
                }

                // Allow attacks for a short duration, then retreat
                if (Time.time - stateStartTime > 1f) // 1 second of attacking
                {
                    currentState = FILState.Retreating;
                    stateStartTime = Time.time;
                }
                break;

            case FILState.Retreating:
                moveBackward();
                //Debug.Log("FIL is Retreating for " + (Time.time) + "-" + stateStartTime + " seconds");

                if (Time.time - stateStartTime > 1f) // Retreat for 2 seconds, then reengage
                {
                    currentState = FILState.Idle;
                    stateStartTime = Time.time;
                }
                break;
        }
    }

    /************************************************************
    * Final Stand
    ************************************************************
    * Description: This function controls the FIL's fighting
    * logic for when he is very low health. Basically he will
    * no longer retreat and will make a final stand.
    ************************************************************
    * Parameters:
    * fianceDistance - The distance from the FIL to the fiance. 
    * Determines whether to approach, attack, or retreat.
    ************************************************************
    * Returns: None
    ************************************************************/
    public void FinalStand(float fianceDistance)
    {

        // Transition logic
        switch (currentState)
        {
            case FILState.Idle:
                Idle();

                //Debug.Log("FIL is Idle");

                if (fianceDistance > FILReach)
                {
                    currentState = FILState.Approaching;
                    stateStartTime = Time.time;
                }
                else
                {
                    currentState = FILState.Attacking;
                    stateStartTime = Time.time;
                }
                break;

            case FILState.Approaching:
                moveForward();

                //Debug.Log("FIL is Approaching");

                if (fianceDistance <= FILReach)
                {
                    currentState = FILState.Attacking;
                    stateStartTime = Time.time;
                }
                break;

            case FILState.Attacking:
                
                //Debug.Log("FIL is Attacking");

                // Throw a combo based on proximity
                if (fianceDistance <= 2.5)
                {
                    JabCross();
                }
                else if (fianceDistance <= 3.8)
                {
                    CrossJab();
                }
                else if (fianceDistance <= 4.5)
                {
                    Block();
                } 
                else if (fianceDistance >= 5)
                {
                    currentState = FILState.Approaching;
                }
                break;

            // We should never reach this state during the final stand
            case FILState.Retreating:
                currentState = FILState.Attacking;
                break;
        }
    }
}
