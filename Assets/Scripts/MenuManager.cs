#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainView;
    [SerializeField] private GameObject _creditsView;
    [SerializeField] private GameObject _optionsView;

    private void Awake()
    {
        _mainView.SetActive(true);
        _creditsView.SetActive(false);
        _optionsView.SetActive(false);
    }

    #region Main view
    public void StartClicked(string SceneName) 
    {
        SceneManager.LoadScene(SceneName);
    }

    public void ContinueClicked() 
    { 
        //TODO
    }

    public void OptionsClicked() 
    {
        _mainView.SetActive(false);
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

    #region Credits View
    public void BackClicked() 
    {
        _creditsView.SetActive(false);
        _optionsView.SetActive(false);
        _mainView.SetActive(true);
    }
    #endregion
}
