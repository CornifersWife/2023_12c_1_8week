using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float jumpHeight = 7f;
    [SerializeField] private float moveSpeed = 7f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float axisR;

    private bool isGrounded;
    private bool hasDoubleJumped;
    private bool hasJumped;

    private enum MovementState { idle, running, jumping, falling, doubleJump};
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isGrounded = false;
        hasDoubleJumped = true;
        hasJumped = false;
    }

    
    private void Update()
    {
        axisR = Input.GetAxisRaw("Horizontal");
       
        if (Input.GetButtonDown("Jump") && !hasDoubleJumped)
        {
            hasJumped = true;
            if(!isGrounded) hasDoubleJumped = true;
        }

        updateAnimation();
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(axisR * moveSpeed, rb.velocity.y);
        if (hasJumped)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            hasJumped = false;
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
        hasDoubleJumped = false;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
    private void updateAnimation()
    {
        MovementState state = MovementState.idle;
        
        if (axisR < 0f) 
        {
            state = MovementState.running;
            spriteRenderer.flipX = true;
        } 
        else if (axisR > 0f) 
        {
            state = MovementState.running;
            spriteRenderer.flipX = false;
        }

        if (rb.velocity.y > 0.1f) 
        {
            state = hasDoubleJumped? MovementState.doubleJump: MovementState.jumping;
        } 
        else if (rb.velocity.y < -0.1f) 
        {
            state = MovementState.falling;
            
        }

        if(animator.GetInteger("State") != (int)state)
            animator.SetInteger("State", (int)state);
    }

}
