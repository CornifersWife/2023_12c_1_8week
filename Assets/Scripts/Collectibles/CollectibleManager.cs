using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager instance { get; private set; }

    private int coins = 0;
    private int diamonds = 0;
    int Coins
    {

        get { return coins; }
        set { coins = value; UpdateCoins(); }
    }
    int Diamonds
    {
        get { return diamonds; }
        set { diamonds = value; UpdateDiamonds(); }
    }

    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private GameObject _uncollectedDiamond;
    [SerializeField] private GameObject _collectedDiamond;
    [SerializeField] private GameObject _diamondBox;
    private List<Transform> _diamondPositions;
    private List<GameObject> _diamondsActive;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else 
        {

            // Zapisujemy instancję dla singletonu
            instance = this;

            _diamondPositions = new List<Transform>();
            _diamondsActive = new List<GameObject>();

            //Pobieramy listę pozycji gdzie mają się znajdować diamenciki
            Transform[] allChildren = _diamondBox.GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren) 
            { 
                _diamondPositions.Add(child);
            }
            UpdateCoins();
            UpdateDiamonds();

            EventSystem.SaveEventSystem.OnSaveGame += SaveGame;
            EventSystem.SaveEventSystem.OnLoadGame += LoadGame;

        }
    }

    private void OnDestroy()
    {
        EventSystem.SaveEventSystem.OnSaveGame -= SaveGame;
        EventSystem.SaveEventSystem.OnLoadGame -= LoadGame;
    }

    private void SaveGame(SaveData data) 
    {
        data.CoinCount = Coins;
        data.DiamondCount = Diamonds;
    }

    private void LoadGame(SaveData data)
    {
        Coins = data.CoinCount;
        Diamonds = data.DiamondCount;
    }

    public void Start()
    {
        
    }

    private void UpdateCoins() { coinText.text = coins.ToString(); }

    private void UpdateDiamonds() 
    {
        //Wywalamy wszystkie istniejące diamenty i tworzymy na nowo, tyle ile jest aktywnych a reszta niekatywna
        foreach(GameObject o in _diamondsActive){ Destroy(o); }
        GameObject inst = null;
        for (int i = 1; i < _diamondPositions.Count; i++) 
        {
            if (i <= diamonds)
            {
                inst = Instantiate(_collectedDiamond, _diamondPositions[i].position, _diamondPositions[i].rotation, _diamondPositions[0]);
            }
            else 
            {
                inst = Instantiate(_uncollectedDiamond, _diamondPositions[i].position, _diamondPositions[i].rotation, _diamondPositions[0]);
            }
            inst.transform.localScale = _diamondPositions[i].localScale;
            _diamondsActive.Add(inst);
        }       
    }

    public void AddCoinValue(int value) 
    {
        coins += value;  
        UpdateCoins();
    }

    public void AddDiamondValue(int value) 
    { 
        diamonds += value;
        UpdateDiamonds();
    }
}
