using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

public class KeybindsManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset _playerActions;


    private GameObject currentButton;

    private Color32 normal = new Color32(255, 255, 255, 255);
    private Color32 selected = new Color32(200, 200, 200, 255);
    private void Awake()
    {
        LoadKeys();
    }

    private RebindingOperation rebindingOperation;

    public void ChangeKey(RebindButtonData data)
    {
        //Zmiana koloru przycisku
        data.itself.GetComponent<Image>().color = selected;

        //Operacja zmiany przypisania klawisza. Nie pozwala przypisać przycisków myszki.
        rebindingOperation = data.actionReference.action
            .PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => ChangeKeyComplete(data))
            .Start();

        
    }

    private void ChangeKeyComplete(RebindButtonData data) 
    {
        data.itself.GetComponent<Image>().color = normal;

        UpdateText(data);

        rebindingOperation.Dispose();
    }

    public void UpdateText(RebindButtonData data) 
    {
        //Tajna magia zmieniająca tekst na taki jaki ma typ na klawiaturze. A przynajmniej powinno.
        int bindingIndex = data.actionReference.action.GetBindingIndexForControl(data.actionReference.action.controls[0]);

        data.textField.text = InputControlPath.ToHumanReadableString(
            data.actionReference.action.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    public void SaveKeys()
    {
        //Zapisanie bindów do PlayerPrefs.
        string bindings = _playerActions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("bindings", bindings);
    }

    public void LoadKeys() 
    {
        //Wczytanie bindów z PlayerPrefs.
        string bindings = PlayerPrefs.GetString("bindings", string.Empty);

        if (string.IsNullOrEmpty(bindings)) { return; }

        _playerActions.LoadBindingOverridesFromJson(bindings);
    }
}
