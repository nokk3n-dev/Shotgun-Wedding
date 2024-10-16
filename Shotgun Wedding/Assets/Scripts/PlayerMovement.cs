using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D fianceBody;
    private Animator fianceAnim;
    private SpriteRenderer fianceSprite;

    private float xVelocity = 0;

    [SerializeField] float fianceMoveSpeed = 3.7f;
    [SerializeField] float fianceReach = 5f;

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

        // Find the Father in Law's hitbox
        FIL_Hitbox = GameObject.FindGameObjectWithTag("FIL").transform;
    }

    // Update is called once per frame
    void Update()
    {
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
        
        if (distanceToFIL <= fianceReach)
        {
            healthManager.FILTakeDamage(5);
        }
    }

    private void CrossFIL()
    {
        float distanceToFIL = Vector2.Distance(transform.position, FIL_Hitbox.position);
        
        if (distanceToFIL <= fianceReach)
        {
            healthManager.FILTakeDamage(10);
        }
    }

}   // End class
