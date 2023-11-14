using UnityEngine;

public class CoinCollection : Collectible
{
    [SerializeField] private CoinConfig _coinConfig;

    public override void Collect()
    {
        if (enabled) {
            enabled = false;
            GetComponent<Animator>().SetBool("Collected", true);
            CoinManager.instance.AddCoinValue(_coinConfig.coinValue);
        }
        
    }
}
