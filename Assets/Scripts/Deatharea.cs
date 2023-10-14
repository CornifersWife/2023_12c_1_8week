using UnityEngine;

public class Deatharea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.position = PlayerHealth.lastCheckpointPos;
        }
    }
}
