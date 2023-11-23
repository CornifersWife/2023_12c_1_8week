#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseView;
    [SerializeField] private GameObject _exitView;
    [SerializeField] private GameObject _bg;
    [SerializeField] private GameObject _optionsView;
    [SerializeField] private GameObject _keybindsView;
    [SerializeField] private PlayerInput _playerController;


    private void Awake()
    {
        _bg.SetActive(false);
        _pauseView.SetActive(false);
        _exitView.SetActive(false);
    }

    public void PauseGame(InputAction.CallbackContext context)
    {
        //Wywoływane przez input managera, jeśli menu aktywne to je wyłączy, jeśli nie to włączy

        if (_bg.activeInHierarchy)
        {
            ContinueClicked();        
        }
        else
        {
            _playerController.SwitchCurrentActionMap("Menu");
            _bg.SetActive(true);
            _pauseView.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void OnLevelWasLoaded(int level)
    {
        if (File.Exists(Application.dataPath + "/saves/save.suffering"))
        {
            SaveSystem.SimpleSaveSystem.LoadBinary();
        }     
    }

    #region PauseMenu

    public void ContinueClicked()
    {
        _playerController.SwitchCurrentActionMap("Player");
        _bg.SetActive(false);
        _pauseView.SetActive(false);
        _exitView.SetActive(false);
        _optionsView.SetActive(false);
        _keybindsView.SetActive(false);
        Time.timeScale = 1.0f;
    }
    public void RestartClicked() 
    { 
        //TODO: Zrestartowanie gracza do poprzedniego checkpointu

        ContinueClicked();
    }

    public void OptionsClicked() 
    {
        _pauseView.SetActive(false);
        _optionsView.SetActive(true);
    }

    public void ExitClicked() 
    {
        _pauseView.SetActive(false);
        _exitView.SetActive(true);
    }
    #endregion

    #region ExitView
    public void TitleClicked(string SceneName) 
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneName);
    }

    public void DesktopClicked()
    {
        #if UNITY_EDITOR
                EditorApplication.isPlaying = false;
        #else
                    Application.Quit();
        #endif
    }

    #endregion
    #region Options View
    public void KBClicked()
    {
        _optionsView.SetActive(false);
        _keybindsView.SetActive(true);
    }
    #endregion
    #region Universal
    public void BackClicked()
    {
        _pauseView.SetActive(true);
        _exitView.SetActive(false);
        _optionsView.SetActive(false);

    }
    public void KBBackClicked()
    {
        _optionsView.SetActive(true);
        _keybindsView.SetActive(false);

    }
    #endregion
}
