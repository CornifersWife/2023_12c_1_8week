using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeybindsManager : MonoBehaviour
{
    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
    public TextMeshProUGUI left, right, jump, attack, pause;
    private GameObject currentKey;

    private Color32 normal = new Color32(255, 255, 255, 255);
    private Color32 selected = new Color32(200, 200, 200, 255);
    
    void Start()
    {
        keys.Add("Left", (KeyCode)System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Left", "A")));
        keys.Add("Right", (KeyCode)System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Right", "D")));
        keys.Add("Jump", (KeyCode)System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Jump", "Space")));
        keys.Add("Attack", (KeyCode)System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Attack", "Mouse0")));
        keys.Add("Pause", (KeyCode)System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Pause", "Escape")));
        
        left.text = keys["Left"].ToString();
        right.text = keys["Right"].ToString();
        jump.text = keys["Jump"].ToString();
        attack.text = keys["Attack"].ToString();
        pause.text = keys["Pause"].ToString();
    }

    void OnGUI()
    {
        if (currentKey != null)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                keys[currentKey.name] = e.keyCode;
                currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();
                currentKey.GetComponent<Image>().color = normal;
                currentKey = null;
            }
        }
    }

    public void ChangeKey(GameObject clicked)
    {
        if (currentKey != null)
        {
            currentKey.GetComponent<Image>().color = normal;
        }
        currentKey = clicked;
        currentKey.GetComponent<Image>().color = selected;
    }

    public void SaveKeys()
    {
        //TODO czemu sie nie zapisuje?
        foreach (var key in keys)
        {
            PlayerPrefs.SetString(key.Key, key.Value.ToString());
        }
        PlayerPrefs.Save();
    }
}
