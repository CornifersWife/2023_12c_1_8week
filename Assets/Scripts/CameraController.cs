using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    // Update is called once per frame
    void Update()
    {
        if(player != null)
            transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }
}
