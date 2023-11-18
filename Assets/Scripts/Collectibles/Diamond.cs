using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : Collectible
{
    [SerializeField] private DiamondConfig _diamondConfig;

    public override void Collect()
    {
        if (enabled) 
        {
            enabled = false;
            GetComponent<Animator>().SetBool("Collected", true);
            CollectibleManager.instance.AddDiamondValue(_diamondConfig.diamondValue);
        }
        
    }
}
