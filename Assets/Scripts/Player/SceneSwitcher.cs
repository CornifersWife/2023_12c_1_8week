

using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public bool isOpen = false;
    [SerializeField] private SceneAsset scene;

    private void Awake()
    {
        isOpen = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOpen)
        {
            if (collision != null && collision.name == "Player")
            {
                SceneManager.LoadScene(scene.name);
            }
        }
    }
}
