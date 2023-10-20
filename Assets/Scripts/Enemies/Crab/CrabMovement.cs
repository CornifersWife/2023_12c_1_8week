using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private Transform[] patrolPoints;

    [Header("Attack")]
    [SerializeField] private AIPlayerDetector playerDetectorShortRange;
    [SerializeField] private float attackPreparationTime = 0f;
    [SerializeField] private float attackAnimationTime = 1.0f;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private PlayerHealth playerHealth;


    private enum MovementState { idle, running, jumping, falling, anticipating, attack};
    
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private int patrolDestination;
    private HealthSystem healthSystem;

    private float timeLeftToAttack;
    private float timeLeftToFinishAttack;

    private bool isPreparingToAttack = false;
    private bool hasAttacked = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = rb.GetComponent<SpriteRenderer>();
        animator = rb.GetComponent<Animator>();
        healthSystem = rb.GetComponent<HealthSystem>();
        timeLeftToAttack = attackPreparationTime;
        timeLeftToFinishAttack = attackAnimationTime;
    }
    private void Update()
    {
        if (healthSystem != null && healthSystem.isDead) return;

        updateAttack();
        
        
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

  
    private void updateAttack()
    {
        if (hasAttacked)
        {
            timeLeftToFinishAttack -= Time.deltaTime;
            if (timeLeftToFinishAttack > 0)
                return;

            hasAttacked = false;
            timeLeftToFinishAttack = attackAnimationTime;
        }

        if (timeLeftToAttack > 0) timeLeftToAttack -= Time.deltaTime;
        isPreparingToAttack = playerDetectorShortRange.isPlayerDetected;
        if (isPreparingToAttack)
        {

            if (timeLeftToAttack > 0)
                return;

            if (playerDetectorShortRange != null && playerDetectorShortRange.isPlayerDetected)
                playerHealth.takeDamage(attackDamage);

            timeLeftToAttack = attackPreparationTime;
            hasAttacked = true;
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
