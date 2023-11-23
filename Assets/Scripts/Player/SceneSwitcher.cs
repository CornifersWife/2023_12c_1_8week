using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] private string SceneName;

    private void Awake()
    {
        EventSystem.SaveEventSystem.OnSaveGame += SaveGame;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null && collision.tag == "Player")
        {
            SaveSystem.SimpleSaveSystem.SaveBinary();
            SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
        }
    }

    private void SaveGame(SaveData data) 
    { 
        data.LevelName = SceneName;    
    }
}
