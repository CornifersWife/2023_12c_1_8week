using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    PlayerManager playerManager;
    private int coins;
    [SerializeField] Text coinText;

    public int Coins 
    { 
        get { return coins; }
        set { coins = value; UpdateHUD(); }
    }
    public void Awake()
    {
        if (playerManager == null) playerManager = PlayerManager.getInstance();
        instance = this;
    }

    public void Start()
    {
        coins = 0;
        if (playerManager.values.ContainsKey("coins"))
        {
            coins = (int)playerManager.values["coins"];
        }
        UpdateHUD();
    }
    private void OnDestroy()
    {
        playerManager.values["coins"] = coins;
    }

    private void UpdateHUD() 
    {
        coinText.text = "Coins: " + coins;
    }

    public void AddValue(int value) 
    {
        coins += value;  
        UpdateHUD();
    }
}
