using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBox : MonoBehaviour
{
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
    public IEnumerator GetHit()
    {
        animator.SetTrigger("IsHit");
        animator.SetInteger("HitCount", animator.GetInteger("HitCount")+1);
        if (animator.GetInteger("HitCount") >= 2)
        {
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            Destroy(gameObject);   
        }
        //TODO: Poprawić ostatni etap animacji
    }
}
