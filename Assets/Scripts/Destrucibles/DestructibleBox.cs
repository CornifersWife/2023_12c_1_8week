using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class DestructibleBox : Destructible
{
    [SerializeField] public GameObject shatteredVersion;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
    public override void GetHit()
    {
        animator.SetTrigger("IsHit");
        animator.SetInteger("HitCount", animator.GetInteger("HitCount")+1);
        if (animator.GetInteger("HitCount") == 2)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            SwitchToShattered();   
        }
    }

    public override void SwitchToShattered()
    {
        Instantiate(shatteredVersion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    
}
