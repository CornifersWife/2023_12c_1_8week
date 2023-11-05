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
        set { coins = value; UpdateHUD(); }
    }
    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        coins = 0;
        if (PlayerPrefs.HasKey("coins"))
        {
            coins = PlayerPrefs.GetInt("coins");
        }
        UpdateHUD();
    }
    private void OnDestroy()
    {
        PlayerPrefs.SetInt("coins", coins);
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
