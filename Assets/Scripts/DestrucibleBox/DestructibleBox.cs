using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBox : MonoBehaviour
{
    [SerializeField] private GameObject shatteredVersion;
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
    public void GetHit()
    {
        animator.SetTrigger("IsHit");
        animator.SetInteger("HitCount", animator.GetInteger("HitCount")+1);
        if (animator.GetInteger("HitCount") == 2)
        {
            SwitchToShattered();   
        }
    }

    private void SwitchToShattered()
    {
        Instantiate(shatteredVersion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    
}
