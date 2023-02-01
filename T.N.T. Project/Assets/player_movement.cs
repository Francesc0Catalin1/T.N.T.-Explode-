using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    public Rigidbody2D rb;

    public float MaxrunSpeed = 100f;
    public float climbSpeed = 30f;
    public float WalkSpeed = 40f;
    public float Acceleration = 3f;
    public float Deceleration = -3f;

    float horizontalMove = 0f;
    float verticalMove = 0f;
    float defaultGravity = 0f;

    bool jump = false;
    bool isLadder = false;
    bool isClimbing = false;
    bool directionRight = true;

    private void Start()
    {
        defaultGravity = rb.gravityScale;
        
    }
    // Update is called once per frame
    void Update()
    {
        
        horizontalMove = Input.GetAxisRaw("Horizontal") * WalkSpeed;
        verticalMove = Input.GetAxisRaw("Vertical") * climbSpeed * 10;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        


        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJump", true);
        }

        if(isLadder && Mathf.Abs(verticalMove) > 0f)
        {
            isClimbing = true;
        }
        else
        {
            rb.gravityScale = defaultGravity;
        }

        CalculateSpeed();
    }

    public void OnLanding ()
    {
        animator.SetBool("IsJump", false);
    }
    private void CalculateSpeed()
    {
        

        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            //WalkSpeed += Acceleration * Time.deltaTime;
            if (directionRight == true)
            {
                WalkSpeed += Acceleration * Time.deltaTime;
                directionRight = true;
            }
            if (directionRight == false)
            {
                //Fa cose diverse se cambi direzione
                WalkSpeed = 40;
                WalkSpeed += (Deceleration*2) * Time.deltaTime;
                directionRight = true;
            }
        }

        if (Input.GetAxisRaw("Horizontal") == -1)
        {
            if (directionRight == false) { 
                WalkSpeed += Acceleration * Time.deltaTime;
                directionRight = false; 
            }
            if (directionRight == true)
            {
                //Fa cose diverse se cambi direzione
                
                WalkSpeed = 40;
                WalkSpeed += (Deceleration * 2) * Time.deltaTime;
                directionRight = false;
            }
        }
        WalkSpeed += Deceleration * Time.deltaTime;
        
        if (Input.GetButtonUp("Horizontal") == true)
        { WalkSpeed += Deceleration; }
        
        WalkSpeed = Mathf.Clamp(WalkSpeed, 0, MaxrunSpeed);
    }
    void FixedUpdate()
    {
        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, verticalMove * Time.fixedDeltaTime);
        }

        

        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        { isLadder = true; }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        { 
            isLadder = false;
            isClimbing = false;
        }
    }
}
