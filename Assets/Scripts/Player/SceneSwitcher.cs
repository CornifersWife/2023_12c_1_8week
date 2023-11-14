

using UnityEditor;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] private SceneAsset scene;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null && collision.name == "Player")
        {
            SceneManager.LoadScene(scene.name);
        }
    }
}
