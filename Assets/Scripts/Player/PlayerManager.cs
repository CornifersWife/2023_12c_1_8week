using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    public Dictionary<string, object> values;
    public static PlayerManager instance;


    public PlayerManager()
    {
        if (instance != null) return;
        values = new Dictionary<string, object>();
        instance = this;
    }
    public static PlayerManager getInstance()
    {
        if(instance == null)
            instance = new PlayerManager();
        return instance;
    }
}
