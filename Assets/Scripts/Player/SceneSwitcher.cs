

using UnityEditor;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] private SceneAsset scene;

    private void Awake()
    {
        EventSystem.SaveEventSystem.OnSaveGame += SaveGame;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null && collision.name == "Player")
        {
            SaveSystem.SimpleSaveSystem.SaveBinary();
            SceneManager.LoadScene(scene.name);
        }
    }

    private void SaveGame(SaveData data) 
    { 
        data.LevelName = scene.name;
    }
}
