using UnityEngine;
using ClearSky;

public class DeathZone : MonoBehaviour
{
    public int damage = 999;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Health al
        PlayerHealth health = other.GetComponent<PlayerHealth>();
        PlayerRespawn respawn = other.GetComponent<PlayerRespawn>();

        if (health != null)
        {
            health.TakeDamage(damage);
        }

        // ⭐ Ölüm sonrası respawn
        if (respawn != null)
        {
            respawn.Respawn();
        }
    }
}
