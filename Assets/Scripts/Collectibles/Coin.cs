using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollection : Collectible
{
    [SerializeField] private CoinConfig _coinConfig;

    public override void Collect()
    {
        Destroy(GetComponent<Collider2D>());
        GetComponent<Animator>().SetBool("Collected", true);
        CoinManager.instance.AddCoinValue(_coinConfig.coinValue);
    }
}
