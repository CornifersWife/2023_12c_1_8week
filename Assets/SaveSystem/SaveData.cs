
using System;

[Serializable]
public class SaveData
{
    public string LevelName { get; set; }
    public int CoinCount { get; set; }
    public int DiamondCount { get; set; }
    public bool DoubleJump { get; set; }
    public int MaxHealth { get; set; }
    public int Health { get; set; }
    public bool Weapon { get; set; }

}
