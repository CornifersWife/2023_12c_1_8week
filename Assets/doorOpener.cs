#if UNITY_EDITOR
using UnityEditor.Animations;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorOpener : MonoBehaviour
{
    SceneSwitcher sceneSwitcher;
    Animator anim;
    void Start()
    {
        anim = GetComponentInParent<Animator>();
        sceneSwitcher = GetComponentInParent<SceneSwitcher>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (anim != null)
        {
            if (collision.CompareTag("Player"))
            {
                anim.SetBool("isOpen", true);
                sceneSwitcher.isOpen = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (anim != null)
        {
            if (collision.CompareTag("Player"))
            {
                anim.SetBool("isOpen", false);
                sceneSwitcher.isOpen = false;
            }
        }
    }

}
