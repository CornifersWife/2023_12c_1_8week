using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatteredBox : MonoBehaviour
{
    [SerializeField] private float flickerFrequency;
    [SerializeField] private int flickerCount;

    private Renderer[] renderers;

    private void Awake()
    {
        renderers = gameObject.GetComponentsInChildren<Renderer>();
    }
    
    private void Start()
    {
        StartCoroutine(Flicker());
        StartCoroutine(Disappear());
    }

    private IEnumerator Disappear()
    {
        yield return new WaitForSeconds(flickerFrequency * flickerCount * 2);
        Destroy(gameObject);
    }

    private IEnumerator Flicker()
    {
        for (int i = 0; i < flickerCount; i++)
        {
            foreach(Renderer rend in renderers){
                rend.enabled = false;
            }
            
            yield return new WaitForSeconds(flickerFrequency);
            
            foreach(Renderer rend in renderers){
                rend.enabled = true;
            }
            
            yield return new WaitForSeconds(flickerFrequency);
        }
    }
}
