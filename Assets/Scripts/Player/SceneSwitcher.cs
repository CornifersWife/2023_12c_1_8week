#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.Build.Content;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public bool isOpen = false;
    [SerializeField] private string SceneName;

    private void Awake()
    {
        EventSystem.SaveEventSystem.OnSaveGame += SaveGame;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOpen)
        {
            if (collision != null && collision.name == "Player")
            {
              SaveSystem.SimpleSaveSystem.SaveBinary();
              SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
            }
        }
    }

    private void SaveGame(SaveData data) 
    { 
        data.LevelName = SceneName;    
    }
}
