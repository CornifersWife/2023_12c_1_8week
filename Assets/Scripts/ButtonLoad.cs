using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLoad : MonoBehaviour
{
    [SerializeField] private KeybindsManager keybindsManager;
    void Start()
    {
        keybindsManager.UpdateText(GetComponent<RebindButtonData>());
    }

}
