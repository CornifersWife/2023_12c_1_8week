using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public abstract class Collectible : MonoBehaviour
{
    public abstract void Collect();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collect();
    }

}
