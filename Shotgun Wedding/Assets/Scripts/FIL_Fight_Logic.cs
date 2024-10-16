using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FIL_Fight_Logic : MonoBehaviour
{
    private Rigidbody2D FILBody;
    private Animator FILAnim;
    private Animator fianceAnim;

    private float xVelocity = 0;

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
        float fianceDistance = Vector2.Distance(transform.position, Fiance_Hitbox.position);
        
        // IMPORTANT THIS IS ONLY THE ANIMATION
        // The actual damage is dealt by the {punch}CheckDamage function
        // which is called by the animator on the impact frame
        if (fianceDistance <= 3)
        {
            Block();
        }
        else if (fianceDistance <= 5)
        {
            Cross();
        }
        else if (fianceDistance <= 7)
        {
            Jab();
        }
        else if (fianceDistance > 7)
        {
            moveForward(5);
        }
        else
        {
            Idle();
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

    private void Cross()
    {
        FILAnim.SetBool("FIL_throwingCross", true);
        FILAnim.SetBool("FIL_throwingJab", false);
        FILAnim.SetBool("FIL_blocking", false);
    }

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

    private void moveForward(float distance)
    {
        // Stop all actions
        Idle();

        // Get current location
        Vector2 startPosition = transform.position;
        Vector2 currentPosition = transform.position;

        FILAnim.SetBool("FIL_moveForward", true);
        while (Vector2.Distance(startPosition, currentPosition) <= distance)
        {
            FILBody.velocity = new Vector2(FILMoveSpeed, FILBody.velocity.y);
        }
    }
}
