using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FIL_Fight_Logic : MonoBehaviour
{
    private Rigidbody2D FILBody;
    private Animator FILAnim;
    private Animator fianceAnim;

    // This will help me stop the FIL from jabbing non-stop
    private bool canJab = true;
    private bool canCross = true;
    private float jabCooldown = 0.5f;
    private float crossCooldown = 1f;

    [SerializeField] float FILMoveSpeed = 3f;
    [SerializeField] float FILReach = 4f;

    // Reference to the HealthManager
    private HealthManager healthManager;

    // Reference to the FIL
    private Transform Fiance_Hitbox;

    // Start is called before the first frame update
    void Start()
    {
        // Set up FIL
        FILBody = GetComponent<Rigidbody2D>();
        FILAnim = GetComponent<Animator>();

        // Find the HealthManager
        healthManager = GameObject.FindObjectOfType<HealthManager>();

        // Set up Fiance
        fianceAnim = GameObject.FindGameObjectWithTag("Fiance").GetComponent<Animator>();
        Fiance_Hitbox = GameObject.FindGameObjectWithTag("Fiance").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.GameIsPaused)
        {
            return;
        }

        float fianceDistance = Vector2.Distance(transform.position, Fiance_Hitbox.position);
        
        if (fianceDistance > 10)
        {
            moveForward();
        }

        // Stopping the movement
        if (fianceDistance <= 10)
        {
            FILBody.velocity = Vector2.zero;
            FILAnim.SetBool("FIL_moveForward", false);
        }

        // IMPORTANT THIS IS ONLY THE ANIMATION
        // The actual damage is dealt by the {punch}CheckDamage function
        // which is called by the animator on the impact frame
        // Only throw punches or idle if not moving
        if (!fianceAnim.GetBool("FIL_moveForward") && !fianceAnim.GetBool("FIL_moveBack"))
        {
            if (fianceDistance < 3.5)
            {
                Cross();
            }
            else if (fianceDistance <= 5)
            {
                Block();
            } 
            else if (fianceDistance <= 6)
            {
                Jab();
            }
            else
            {
                Idle();
            }
        }
    }

    private void JabDamageCheck()
    {
        float distanceToFiance = Vector2.Distance(transform.position, Fiance_Hitbox.position);

        if (distanceToFiance <= FILReach && !fianceAnim.GetBool("blocking"))
        {
            healthManager.FianceTakeDamage(15);
        }
    }   // End JabDamageCheck

    private void CrossDamageCheck()
    {
        float distanceToFiance = Vector2.Distance(transform.position, Fiance_Hitbox.position);

        if (distanceToFiance <= FILReach)
        {
            if (fianceAnim.GetBool("blocking"))
            {
                healthManager.FianceTakeDamage(5);
            }
            else 
            {
                healthManager.FianceTakeDamage(25);
            }
        }
    }

    private void Jab()
    {
        FILAnim.SetBool("FIL_throwingJab", true);
        FILAnim.SetBool("FIL_throwingCross", false);
        FILAnim.SetBool("FIL_blocking", false);
    }

    // Fix Later
    // public void StartJabCooldown()
    // {
    //     // Set the cooldown timer of the jab
    //     StartCoroutine(JabTimer());
    // }

    // // Jab Cooldown
    // IEnumerator JabTimer()
    // {
    //     canJab = false;
    //     yield return new WaitForSeconds(jabCooldown);
    //     canJab = true;
    // }

    private void Cross()
    {
        FILAnim.SetBool("FIL_throwingCross", true);
        FILAnim.SetBool("FIL_throwingJab", false);
        FILAnim.SetBool("FIL_blocking", false);
    }

    // Fix later
    // public void StartCrossCooldown()
    // {
    //     // Set the cooldown timer of the jab
    //     StartCoroutine(CrossTimer());
    // }

    // // Jab Cooldown
    // IEnumerator CrossTimer()
    // {
    //     canCross = false;
    //     yield return new WaitForSeconds(crossCooldown);
    //     canCross = true;
    // }

    private void Block()
    {
        FILAnim.SetBool("FIL_blocking", true);
        FILAnim.SetBool("FIL_throwingCross", false);
        FILAnim.SetBool("FIL_throwingJab", false);
    }

    private void Idle()
    {
        FILAnim.SetBool("FIL_throwingJab", false);
        FILAnim.SetBool("FIL_throwingCross", false);
        FILAnim.SetBool("FIL_blocking", false);
        FILAnim.SetBool("FIL_moveForward", false);
        FILAnim.SetBool("FIL_moveBack", false);
    }

    private void moveForward()
    {
        FILAnim.SetBool("FIL_moveForward", true);
        FILBody.velocity = new Vector2(-FILMoveSpeed, FILBody.velocity.y);
    }

    private void moveBackward()
    {
        FILAnim.SetBool("FIL_moveBack", true);
        FILBody.velocity = new Vector2(FILMoveSpeed, FILBody.velocity.y);
    }
}
