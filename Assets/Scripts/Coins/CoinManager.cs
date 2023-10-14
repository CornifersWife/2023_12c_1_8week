using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    private int coins;
    [SerializeField] Text coinText;

    public int Coins 
    { 
        get { return coins; }
        set { coins = value; }
    }
    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        coins = 0;
        UpdateHUD();
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
