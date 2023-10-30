
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptable Objects/Coin Config", fileName = "Coin config")]
public class CoinConfig : ScriptableObject
{
    [SerializeField] public int coinValue = 1;
}
