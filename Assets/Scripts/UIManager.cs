#if UNITY_EDITOR
    using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseView;
    [SerializeField] private GameObject _exitView;
    [SerializeField] private GameObject _bg;

    private void Awake()
    {
        _bg.SetActive(false);
        _pauseView.SetActive(false);
        _exitView.SetActive(false);
    }

    public void PauseGame(InputAction.CallbackContext context)
    {
        if (_pauseView.activeInHierarchy || _exitView.activeInHierarchy)
        {
            ContinueClicked();
        }
        else
        {
            _bg.SetActive(true);
            _pauseView.SetActive(true);
            Time.timeScale = 0;
        }
    }

    #region PauseMenu

    public void ContinueClicked()
    {
        _bg.SetActive(false);
        _pauseView.SetActive(false);
        _exitView.SetActive(false);
        Time.timeScale = 1.0f;
    }
    public void RestartClicked() 
    { 
        //TODO: Zrestartowanie gracza do poprzedniego checkpointu

        ContinueClicked();
    }

    public void OptionsClicked() 
    { 
        //TODO: Uruchomienie opcji
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

    public void BackClicked() 
    {
        _pauseView.SetActive(true);
        _exitView.SetActive(false);
    }
    #endregion
}
