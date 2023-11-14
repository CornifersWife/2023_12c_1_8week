using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance { get; private set; }

    private int coins = 0;
    private int diamonds = 0;

    int Coins
    {

        get { return coins; }
        set { coins = value; UpdateHUD(); }
    }
    int Diamonds
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
