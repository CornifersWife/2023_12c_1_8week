using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    private PlayerHealth playerHealth;
    private Vector3 lastCameraPos;

    void Awake()
    {
        playerHealth = player.GetComponent<PlayerHealth>();
    }
    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            if (playerHealth.health > 0)
            {
                transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
                lastCameraPos = transform.position;
            }

            else
            {
                transform.position = lastCameraPos;
            }
        }
    }
}
