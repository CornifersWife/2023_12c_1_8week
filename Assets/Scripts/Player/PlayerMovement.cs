using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public bool canDoubleJump;
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

    // odpowaiada za zmian� aktualnej animacji
    private enum MovementState { idle, running, jumping, falling, doubleJump, swordAttack_1, swordAttack_2, swordAttack_3, swordAirAttack_1, swordAirAttack_2, swordThrow};

    
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isGrounded = false;
        hasDoubleJumped = true;
        hasJumped = false;
        canAttack = true;
        canDoubleJump = false;
    }



    private void Update()
    {
        
        axisR = Input.GetAxisRaw("Horizontal");
        //sprawddza czy stopy dotykaj� ziemi (potrzebne do double jump)
        checkGrounded();
        //je�eli cooldown z ataku mieczem nie min�� to zmniejsza ile go zosta�o
        if(swordAttackCooldownLeft > 0) swordAttackCooldownLeft-= Time.deltaTime;
        //je�eli miecz ma nadal zadawa� dmg to zmniejsza ile zosta�o czasu w kt�rym nadal b�dzie zadawa�
        if(attackDamageTimeLeft > 0) attackDamageTimeLeft -= Time.deltaTime;
        
        updateAnimation();
    }
    private void FixedUpdate()
    {
        //porusza gracza lewo prawo
        rb.velocity = new Vector2(axisR * moveSpeed, rb.velocity.y);
        if (hasJumped)
        {
            //dodaje jumpHeight jako si�e skoku do gracza
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            hasJumped = false;
        }

    }

    //wywo�ywane przez inputManager gdy player ma skoczy�
    public void jump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (hasJumped) return;
        if (isGrounded)
        {
            hasJumped = true;
            return;
        }
        if (!hasDoubleJumped && canDoubleJump)
        {
            hasDoubleJumped = true;
            hasJumped = true;
        }
    }
    public void checkGrounded()
    {
        //robi k�ko dooko�a st�p gracza i szuka czy jest tam co� z zmiennej LayerMask )
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
    //wywo�ywane przez inputManager gdy player zmienia bro�
    public void changeWeapon(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        //je�eli player ma domy�ln� wersje bez broni
        if (animator.runtimeAnimatorController == animatorBasic)
        {
            //zmie� na wersj� z broni�
            animator.runtimeAnimatorController = animatorWithSword;
            canAttack = true; // jak ma bro� to mo�e atakowa�
        }
        else
        {
            //i na odwr�t
            animator.runtimeAnimatorController = animatorBasic;
            canAttack = false; 
        }
    }



    public void attack(InputAction.CallbackContext context)
    {
        if (!canAttack) return;
        if (!context.performed) return;
        //je�eli cooldown min�� z poprzedniego ataku
        if (swordAttackCooldownLeft <= 0)
        {
            //je�eli na ziemi to atak w stron� gdzie si� patrzy a jak nie to pod nim
            //Coroutine jest jak Thread w javie
            if (isGrounded)
                StartCoroutine(damageWhileSlahsing(attackGroundTransform, attackGroundRange));
            else
                StartCoroutine(damageWhileSlahsing(attackAirTransform, attackAirRange));
        }
    }
    //odpowiada za zadawanie obra�e� przez jaki� czas w trakcie ataku 
    public IEnumerator damageWhileSlahsing(Transform transform, float range)
    {
        hasAttacked = true;
        //ustawia cooldowns
        swordAttackCooldownLeft = swordAttackCooldown;
        attackDamageTimeLeft = attackDamageTime;
        //lista ude�onych obiekt�w by nic 2 razy nie ude�y�
        List<HealthSystem> damaged = new List<HealthSystem>();
        while (attackDamageTimeLeft > 0)
        {
            //pobiera wszystkich attackable z okr�gu dooko�a miecza
            hits = Physics2D.CircleCastAll(transform.position, range, transform.right, 0f, attackableLayer);
            for (int i = 0; i < hits.Length; i++)
            {
                HealthSystem healthSystem = hits[i].collider.GetComponent<HealthSystem>();
                //zadaje im dmg
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

    //rysuje k�eczka dooko�a miecza by mo�na by�o to edytowa�
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackGroundTransform.position, attackGroundRange);
        Gizmos.DrawWireSphere(attackAirTransform.position, attackAirRange);
    }
    //zmienia animacje na podstawie state i  boolean�w
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
