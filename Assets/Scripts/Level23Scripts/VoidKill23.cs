using UnityEngine;
using ClearSky;

public class VoidKill : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
{
    Debug.Log("Void triggered by: " + collision.name);

    if (collision.CompareTag("Player"))
    {
        PlayerHealth health = collision.GetComponent<PlayerHealth>();

        if (health != null)
        {
            health.TakeDamage(999, 0);
        }
    }
}

}