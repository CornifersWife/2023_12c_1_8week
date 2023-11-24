using UnityEngine;

public class CoinCollection : Collectible
{
    [SerializeField] private CoinConfig _coinConfig;

    public override void Collect()
    {
        if (!enabled) return;
        enabled = false;
        GetComponent<Animator>().SetBool("Collected", true);
        CollectibleManager.instance.AddCoinValue(_coinConfig.coinValue);
    }
}
