using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    //[SerializeField] private bool shoot = true;
    [SerializeField] private GameObject cannonballPrefab;
    [SerializeField] private GameObject fireEffectPrefab;
    [SerializeField] private float fireDelay = 1f;
    [SerializeField] private float fireAnimationTime = 1f;
    [SerializeField] Transform firePoint;
    [SerializeField] Transform effectPoint;
    private float fireCooldownLeft;
    private float animationTimeLeft;

    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        fireCooldownLeft = fireDelay; 
    }

    void Update()
    {
        fireCooldownLeft -= Time.deltaTime; 

        if (fireCooldownLeft <= 0)
        {
            animator.SetBool("Shooting", true);
            fireCooldownLeft = fireDelay + fireAnimationTime;
            StartCoroutine(ShootWithDelay());
        }
        else
        {
            animator.SetBool("Shooting", false);
        }
    }

    IEnumerator ShootWithDelay()
    {
        yield return new WaitForSeconds(fireAnimationTime);

        if (cannonballPrefab != null && firePoint != null)
        {
            Instantiate(fireEffectPrefab, effectPoint.position, effectPoint.rotation);
            Instantiate(cannonballPrefab, firePoint.position, firePoint.rotation);
        }
    }

}
