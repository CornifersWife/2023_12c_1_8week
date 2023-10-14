using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float jumpHeight = 7f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private Transform feet;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float swordAttackCooldown = 5;
    [SerializeField] private float attackDamageTime = .5f;


    [SerializeField] private RuntimeAnimatorController animatorBasic;
    [SerializeField] private RuntimeAnimatorController animatorWithSword;
    [SerializeField] private Transform attackGroundTransform;
    [SerializeField] private Transform attackAirTransform;
    [SerializeField] private float attackGroundRange = 1;
    [SerializeField] private float attackAirRange = 1;
    [SerializeField] private int attackDamage = 2;
    [SerializeField] private LayerMask attackableLayer;

    private RaycastHit2D[] hits;

    private Rigidbody2D rb;
    private Animator animator;
    private float axisR;


    private bool isGrounded;
    private bool hasDoubleJumped;
    private bool hasJumped;
    private bool hasAttacked;
    private bool canAttack;

    private int swordGroundAnimationType = 0;
    private int sworAirAnimationType = 0;
    private float swordAttackCooldownLeft = 0;
    private float attackDamageTimeLeft = 0;

    private enum MovementState { idle, running, jumping, falling, doubleJump, swordAttack_1, swordAttack_2, swordAttack_3, swordAirAttack_1, swordAirAttack_2, swordThrow};

    
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isGrounded = false;
        hasDoubleJumped = true;
        hasJumped = false;
        canAttack = true;
    }



    private void Update()
    {
        axisR = Input.GetAxisRaw("Horizontal");
        checkGrounded();

        if(swordAttackCooldownLeft > 0) swordAttackCooldownLeft-= Time.deltaTime;
        if(attackDamageTimeLeft > 0) attackDamageTimeLeft -= Time.deltaTime;
        
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

    public void jump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!hasDoubleJumped)
        {
            hasJumped = true;
            if (!isGrounded) hasDoubleJumped = true;
        }
    }
    
    public void changeWeapon(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (animator.runtimeAnimatorController == animatorBasic)
        {
            animator.runtimeAnimatorController = animatorWithSword;
            canAttack = true;
        }
        else
        {
            animator.runtimeAnimatorController = animatorBasic;
            canAttack = false; 
        }
    }



    public void checkGrounded()
    {
        if (Physics2D.OverlapCircle(feet.position, 0.3f, groundLayer))
        {
            isGrounded = true;
            hasDoubleJumped = false;
        }
        else
        {
            isGrounded = false;
        }
    }
    public void attack(InputAction.CallbackContext context)
    {
        if (!canAttack) return;
        if (!context.performed) return;
        if (swordAttackCooldownLeft <= 0)
        {
            if (isGrounded)
                StartCoroutine(damageWhileSlahsing(attackGroundTransform, attackGroundRange));
            else
                StartCoroutine(damageWhileSlahsing(attackAirTransform, attackAirRange));
        }
    }
    public IEnumerator damageWhileSlahsing(Transform transform, float range)
    {
        hasAttacked = true;
        swordAttackCooldownLeft = swordAttackCooldown;
        attackDamageTimeLeft = attackDamageTime;

        List<HealthSystem> damaged = new List<HealthSystem>();
        while (attackDamageTimeLeft > 0)
        {
            hits = Physics2D.CircleCastAll(transform.position, range, transform.right, 0f, attackableLayer);
            for (int i = 0; i < hits.Length; i++)
            {
                HealthSystem healthSystem = hits[i].collider.GetComponent<HealthSystem>();
                if (healthSystem != null && !healthSystem.isDamaged)
                {
                    damaged.Add(healthSystem);
                    healthSystem.takeDamage(attackDamage);
                }
            }
            yield return null;
        }
        foreach(HealthSystem hs in damaged)
        {
            hs.isDamaged = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackGroundTransform.position, attackGroundRange);
        Gizmos.DrawWireSphere(attackAirTransform.position, attackAirRange);
    }
    private void updateAnimation()
    {
        MovementState state = MovementState.idle;

        if(hasAttacked)
        {
            hasAttacked = false;
            if (isGrounded)
            {
                animator.SetInteger("State", (int)MovementState.swordAttack_1 + swordGroundAnimationType++);
                if(swordGroundAnimationType >= 3) swordGroundAnimationType = 0;
            }
            else
            {
                animator.SetInteger("State", (int)MovementState.swordAirAttack_1 + sworAirAnimationType++);
                if (sworAirAnimationType >= 2) sworAirAnimationType = 0;
            }
            return;
        }
        
        if (axisR < 0f) 
        {
            state = MovementState.running;
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            
        } 
        else if (axisR > 0f) 
        {
            state = MovementState.running;
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
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
