using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public bool canDoubleJump;
    [SerializeField] public float jumpHeight = 7f;
    [SerializeField] private float jumpDuration = 0.5f;
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

    private float jumpTime = 0f;
    private bool isJumpingHigher = false;

    private RaycastHit2D[] hits;

    public Rigidbody2D rb;
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
    private bool isJumping;

    // odpowaiada za zmianê aktualnej animacji
    private enum MovementState { idle, running, jumping, falling, doubleJump, swordAttack_1, swordAttack_2, swordAttack_3, swordAirAttack_1, swordAirAttack_2, swordThrow};

    
    PlayerManager playerManager;
    
    void Awake()
    {
        if (playerManager == null) playerManager = PlayerManager.getInstance();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isGrounded = false;
        hasDoubleJumped = true;
        hasJumped = false;

        canAttack = true;
        canDoubleJump = false;
        Debug.Log(playerManager == null);
        if (playerManager.values.ContainsKey("canAttack"))
        {
            canAttack = (bool)playerManager.values["canAttack"];
        }
        if (playerManager.values.ContainsKey("canDoubleJump"))
        {
            Debug.Log("Loaded jump");
            canDoubleJump = (bool)playerManager.values["canDoubleJump"];
        }
        if (playerManager.values.ContainsKey("animator"))
        {
            animator.runtimeAnimatorController = (RuntimeAnimatorController)playerManager.values["animator"];
        }
        isJumping = false;
    }
 


    private void OnDestroy()
    {
        Debug.Log("saving");
        playerManager.values["canAttack"] = canAttack;
        playerManager.values["canDoubleJump"] = canDoubleJump;
        playerManager.values["animator"] = animator.runtimeAnimatorController;

    }
    private void Update()
    { 
        axisR = Input.GetAxisRaw("Horizontal");
        //sprawddza czy stopy dotykaj¹ ziemi (potrzebne do double jump)
        checkGrounded();
        //je¿eli cooldown z ataku mieczem nie min¹³ to zmniejsza ile go zosta³o
        if(swordAttackCooldownLeft > 0) swordAttackCooldownLeft-= Time.deltaTime;
        //je¿eli miecz ma nadal zadawaæ dmg to zmniejsza ile zosta³o czasu w którym nadal bêdzie zadawaæ
        if(attackDamageTimeLeft > 0) attackDamageTimeLeft -= Time.deltaTime;
        
        updateAnimation();
    }
    private void FixedUpdate()
    {
        //porusza gracza lewo prawo
        rb.velocity = new Vector2(axisR * moveSpeed, rb.velocity.y);
        if (hasJumped)
        {
            //dodaje jumpHeight jako si³e skoku do gracza
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            hasJumped = false;
        }

        if (isJumping)
        {
            jumpTime += Time.fixedDeltaTime;

            if (jumpTime < jumpDuration && isJumpingHigher)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            }
            else
            {
                jumpTime = 0;
                isJumping = false;
            }
        }

    }

    //wywo³ywane przez inputManager gdy player ma skoczyæ
    public void jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isJumpingHigher = true;
            return;
        }
        if (context.canceled)
        {
            isJumpingHigher = false;
            return;
        }

        if (hasJumped) return;
        if (isGrounded)
        {
            isJumping = true;
            hasJumped = true;
            isJumpingHigher = true;
            return;
        }
        if (!hasDoubleJumped && canDoubleJump)
        {
            isJumping = true;
            isJumpingHigher = true;
            hasDoubleJumped = true;
            hasJumped = true;
        }
    }
    public void checkGrounded()
    {
        //robi kó³ko dooko³a stóp gracza i szuka czy jest tam coœ z zmiennej LayerMask )
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
    //wywo³ywane przez inputManager gdy player zmienia broñ
    public void changeWeapon(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        //je¿eli player ma domyœln¹ wersje bez broni
        if (animator.runtimeAnimatorController == animatorBasic)
        {
            //zmieñ na wersjê z broni¹
            animator.runtimeAnimatorController = animatorWithSword;
            canAttack = true; // jak ma broñ to mo¿e atakowaæ
        }
        else
        {
            //i na odwrót
            animator.runtimeAnimatorController = animatorBasic;
            canAttack = false;
        }
    }

    public void attack(InputAction.CallbackContext context)
    {
        if (!canAttack) return;
        if (!context.performed) return;
        //je¿eli cooldown min¹³ z poprzedniego ataku
        if (swordAttackCooldownLeft <= 0)
        {
            //je¿eli na ziemi to atak w stronê gdzie siê patrzy a jak nie to pod nim
            //Coroutine jest jak Thread w javie
            if (isGrounded)
                StartCoroutine(damageWhileSlahsing(attackGroundTransform, attackGroundRange));
            else
                StartCoroutine(damageWhileSlahsing(attackAirTransform, attackAirRange));
        }
    }
    //odpowiada za zadawanie obra¿eñ przez jakiœ czas w trakcie ataku 
    public IEnumerator damageWhileSlahsing(Transform transform, float range)
    {
        hasAttacked = true;
        //ustawia cooldowns
        swordAttackCooldownLeft = swordAttackCooldown;
        attackDamageTimeLeft = attackDamageTime;
        //lista ude¿onych obiektów by nic 2 razy nie ude¿yæ
        List<HealthSystem> damaged = new List<HealthSystem>();
        Dictionary<int, DestructibleBox> destructibles = new Dictionary<int, DestructibleBox>();
        while (attackDamageTimeLeft > 0)
        {
            //pobiera wszystkich attackable z okrêgu dooko³a miecza
            hits = Physics2D.CircleCastAll(transform.position, range, transform.right, 0f, attackableLayer);
            for (int i = 0; i < hits.Length; i++)
            {
                // sprawdza czy uderzyliśmy jakiś destructible i dodaje do słownika jeżeli jeszcze go tam nie ma
                if (hits[i].collider.gameObject.CompareTag("Destructible") && !destructibles.ContainsKey(hits[i].collider.gameObject.GetHashCode()))
                {
                    destructibles.Add(hits[i].collider.gameObject.GetHashCode(), hits[i].collider.GetComponent<DestructibleBox>());
                }
                else
                {
                    if (hits[i].collider.name == "Chain")
                    {
                        Destroy(hits[i].collider.gameObject);
                    }
                    else
                    {
                        HealthSystem healthSystem = hits[i].collider.GetComponent<HealthSystem>();
                        //zadaje im dmg
                        if (healthSystem != null && !healthSystem.isDamaged)
                        {
                            damaged.Add(healthSystem);
                            healthSystem.takeDamage(attackDamage);
                        }
                    }
                }
            }
            yield return null;
        }
        foreach(HealthSystem hs in damaged)
        {
            hs.isDamaged = false;
        }
        
        // jeżeli uderzyliśmy jakieś przedmioty, które się rozpadają
        if (destructibles.Count > 0)
        {
            foreach (KeyValuePair<int, DestructibleBox> destr in destructibles)
            {
                destr.Value.GetHit();
            }
        }
    }

    //rysuje kó³eczka dooko³a miecza by mo¿na by³o to edytowaæ
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackGroundTransform.position, attackGroundRange);
        Gizmos.DrawWireSphere(attackAirTransform.position, attackAirRange);
    }
    //zmienia animacje na podstawie state i  booleanów
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
