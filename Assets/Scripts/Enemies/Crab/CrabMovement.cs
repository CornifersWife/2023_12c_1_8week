using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabMovement : MonoBehaviour
{
    [SerializeField] private int moveSpeed = 3;

    private enum MovementState { idle, running, jumping, falling };
    private MovementState state;
    
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private int dir;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = rb.GetComponent<SpriteRenderer>();
        animator = rb.GetComponent<Animator>();

        dir = (int)(Random.value*3)-1;
    }

    // Update is called once per frame
    void Update()
    {
        if(dir > 0)
            spriteRenderer.flipX = true;
        else if(dir < 0)
            spriteRenderer.flipX = false;
        updateAnimation();

    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dir * moveSpeed, rb.velocity.y);
    }


    private void updateAnimation()
    {
        MovementState state = MovementState.idle;
        
        if (rb.velocity.x < 0f)
        {
            state = MovementState.running;
            spriteRenderer.flipX = false;
        }
        else if (rb.velocity.x > 0f)
        {
            state = MovementState.running;
            spriteRenderer.flipX = true;
        }

        if (rb.velocity.y > 0.1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -0.1f)
        {
            state = MovementState.falling;

        }

        if (animator.GetInteger("State") != (int)state)
            animator.SetInteger("State", (int)state);
    }
}
