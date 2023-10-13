using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private Transform[] patrolPoints;

    [Header("Attack")]
    [SerializeField] private AIPlayerDetector playerDetectorLongRange;
    [SerializeField] private AIPlayerDetector playerDetectorShortRange;
    [SerializeField] private float attackPreparationTime = 2.0f;
    [SerializeField] private float attackAnimationTime = 1.0f;
    [SerializeField] private int attackDamage = 2;
    [SerializeField] private PlayerHealth playerHealth;

    private enum MovementState { idle, running, jumping, falling, anticipating, attack};
    
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private int patrolDestination;
    
    private float timeLeftToAttack;
    private float timeLeftToFinishAttack;

    private bool isPreparingToAttack = false;
    private bool hasAttacked = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = rb.GetComponent<SpriteRenderer>();
        animator = rb.GetComponent<Animator>();
        timeLeftToAttack = attackPreparationTime;
        timeLeftToFinishAttack = attackAnimationTime;
    }
    private void Update()
    {
        if (hasAttacked)
        {
            timeLeftToFinishAttack -= Time.deltaTime;
            if (timeLeftToFinishAttack > 0)
                return;

            hasAttacked = false;
            timeLeftToFinishAttack = attackAnimationTime;
        }
        else if (isPreparingToAttack)
        {
            timeLeftToAttack -= Time.deltaTime;
            if (timeLeftToAttack > 0)
                return;

            if (playerDetectorShortRange != null && playerDetectorShortRange.isPlayerDetected)
                playerHealth.takeDamage(attackDamage);

            timeLeftToAttack = attackPreparationTime;
            hasAttacked = true;
        }

        if (playerDetectorLongRange != null)
        {
            if (playerDetectorLongRange.isPlayerDetected)
            {
                if (!isPreparingToAttack)
                {
                    timeLeftToAttack = attackPreparationTime;
                    isPreparingToAttack = true;
                }
            }
            else
                isPreparingToAttack = false;
        }
                
        updateAnimation();
    }
    void FixedUpdate()
    {
        if (isPreparingToAttack || hasAttacked)
            return;

        rb.velocity = new Vector2((rb.position.x - patrolPoints[patrolDestination].position.x) < 0 ? moveSpeed : -moveSpeed, rb.velocity.y);
        if (Mathf.Abs(rb.position.x - patrolPoints[patrolDestination].position.x) < .2f)
        {
            patrolDestination++;
            if (patrolDestination >= patrolPoints.Length)
                patrolDestination = 0;
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

        if (hasAttacked)
        {
            state = MovementState.attack;
            return (int)state;
        }

        if (isPreparingToAttack)
        {
            state = MovementState.anticipating;
            return (int)state;
        }

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

        return (int)state;
    }
}
