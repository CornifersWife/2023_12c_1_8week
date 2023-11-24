using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private Transform[] patrolPoints;

    private enum MovementState { idle, running, jumping, falling, anticipating, attack };

    private Rigidbody2D rb;
    private Animator animator;
    private int patrolDestination;

    bool canMove;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = rb.GetComponent<Animator>();
        canMove = true;
    }
    private void Update()
    {
        updateAnimation();
    }

    void FixedUpdate()
    {
        if (!canMove)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }
        rb.velocity = new Vector2((rb.position.x - patrolPoints[patrolDestination].position.x) < 0 ? moveSpeed : -moveSpeed, rb.velocity.y);
        if (Mathf.Abs(rb.position.x - patrolPoints[patrolDestination].position.x) < .2f)
        {
            patrolDestination++;
            if (patrolDestination >= patrolPoints.Length)
                patrolDestination = 0;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canMove = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            canMove = true;
        }
    }

    private void updateAnimation()
    {
        int state = updateMovementState();
        if (animator.GetInteger("State") != (int)state)
            animator.SetInteger("State", (int)state);
    }

    private int updateMovementState()
    {
        MovementState state = MovementState.idle;

        if (rb.velocity.x < 0f)
        {
            state = MovementState.running;
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        else if (rb.velocity.x > 0f)
        {
            state = MovementState.running;
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }

        if (rb.velocity.y > 0.1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -0.1f)
        {
            state = MovementState.falling;
        }

        return (int)state;
    }
}
