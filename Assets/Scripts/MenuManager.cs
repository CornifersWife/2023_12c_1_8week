#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainView;
    [SerializeField] private GameObject _creditsView;
    [SerializeField] private GameObject _optionsView;
    [SerializeField] private GameObject _keybindsView;
    [SerializeField] private GameObject _continueView;
    private void Awake()
    {
        _mainView.SetActive(true);
        _creditsView.SetActive(false);
        _optionsView.SetActive(false);
        _keybindsView.SetActive(false);

        EventSystem.SaveEventSystem.OnLoadGame += LoadGame;

        //Jeśli jest save to pozwalamy wczytać, inaczej ni
        if (File.Exists(SaveSystem.SimpleSaveSystem.GetSaveLocation()))
        {
            _continueView.SetActive(true);
        }
        else 
        {
            _continueView.SetActive(false);
        }


    }
    private void OnDestroy()
    {
        EventSystem.SaveEventSystem.OnLoadGame -= LoadGame;
    }

    #region Main view
    public void StartClicked(string SceneName) 
    {
        SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
    }

    public void OptionsClicked() 
    {
        _mainView.SetActive(false);
        _continueView.SetActive(false);
        _optionsView.SetActive(true);
    }
    public void CreditsClicked() 
    { 
        _mainView.SetActive(false);
        _creditsView.SetActive(true);
    }
    public void ExitClicked() 
    {
        #if UNITY_EDITOR
                EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    #endregion

    #region Continue View
    public void ContinueClicked()
    {
        //Wywołanie wczytania gry
        SaveSystem.SimpleSaveSystem.LoadBinary();
    }

    private void LoadGame(SaveData data) 
    {
        SceneManager.LoadScene(data.LevelName, LoadSceneMode.Single);
    }

    public void RemoveClicked() 
    {
        File.Delete(SaveSystem.SimpleSaveSystem.GetSaveLocation());
        _continueView.SetActive(false);
    }
    #endregion

    #region Universal
    public void BackClicked() 
    {
        _creditsView.SetActive(false);
        _optionsView.SetActive(false);
        _mainView.SetActive(true);
    }

    public void KBBackClicked()
    {
        _optionsView.SetActive(true);
        _keybindsView.SetActive(false);

    }
    #endregion

    #region Options View
    public void KBClicked() 
    {
        _optionsView.SetActive(false);
        _keybindsView.SetActive(true);
    }
    #endregion
}
