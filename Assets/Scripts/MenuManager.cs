#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainView;
    [SerializeField] private GameObject _creditsView;
    [SerializeField] private GameObject _continueView;
    private void Awake()
    {
        _mainView.SetActive(true);
        _creditsView.SetActive(false);
        
        EventSystem.SaveEventSystem.OnLoadGame += LoadGame;

        if (File.Exists(Application.dataPath + "/saves/save.suffering"))
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
        SceneManager.LoadScene(SceneName);
    }

    

    public void OptionsClicked() 
    {
        //TODO: Menu opcji
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
        SaveSystem.SimpleSaveSystem.LoadBinary();
    }

    private void LoadGame(SaveData data) { SceneManager.LoadScene(data.LevelName); }

    public void RemoveClicked() 
    {
        File.Delete(Application.dataPath + "/saves/save.suffering");
        _continueView.SetActive(false);
    }
    #endregion

    #region Credits View
    public void BackClicked() 
    {
        _creditsView.SetActive(false);
        _mainView.SetActive(true);
    }
    #endregion
}
