using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private Transform[] patrolPoints;

    private enum MovementState { idle, running, jumping, falling};
    
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private int patrolDestination;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = rb.GetComponent<SpriteRenderer>();
        animator = rb.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        
        rb.velocity = new Vector2((rb.position.x - patrolPoints[patrolDestination].position.x)<0?moveSpeed:-moveSpeed, rb.velocity.y);
        if (Mathf.Abs(rb.position.x - patrolPoints[patrolDestination].position.x) < .2f)
        {
            patrolDestination++;
            if (patrolDestination >= patrolPoints.Length) patrolDestination = 0;
        }
    
        updateAnimation();
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
