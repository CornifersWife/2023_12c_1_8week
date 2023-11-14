using System.Collections.Generic;

public class KeyManager
{
    static private KeyManager instance;
    private Dictionary<string, int> keys;
    
    public KeyManager() { 
        keys = new Dictionary<string, int>();
    }

    public void addKey(string key)
    {
        if (keys.ContainsKey(key))
        {
            keys[key]++;
        }
        else
        {
            keys[key] = 1;
        }
    }
    public bool tryUseKey(string key)
    {
        if (!keys.ContainsKey(key)) return false;
        if (keys[key] <= 0) return false;
        keys[key]--;
        return true;  
    }

    public static KeyManager GetKeyManager()
    {
        if (instance == null)
        {
            instance = new KeyManager();
        }
        return instance;
    }
}
