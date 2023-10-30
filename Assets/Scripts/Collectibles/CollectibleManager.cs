using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance { get; private set; }

    private int coins
    {
        get { return coins; }
        set { coins = value; UpdateHUD(); }
    }
    private int diamonds
    {
        get { return diamonds; }
        set { diamonds = value; UpdateHUD(); }
    }

    [SerializeField] private Text coinText;
    [SerializeField] private Text diamondText;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public void Start()
    {
        coins = 0;
        diamonds = 0;
        UpdateHUD();
    }

    private void UpdateHUD() 
    {
        coinText.text = "Coins: " + coins;
        diamondText.text = "Diamonds: " + diamonds;
    }

    public void AddCoinValue(int value) 
    {
        coins += value;  
        UpdateHUD();
    }

    public void AddDiamondValue(int value) 
    { 
        diamonds += value;
        UpdateHUD();
    }
}
