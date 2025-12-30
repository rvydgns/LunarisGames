using UnityEngine;
using ClearSky;

public class DeathZone : MonoBehaviour
{
    public int damage = 999;

    private void OnTriggerEnter2D(Collider2D other)
    {
       
        PlayerHealth health = other.GetComponent<PlayerHealth>();
        PlayerRespawn respawn = other.GetComponent<PlayerRespawn>();

        if (health != null)
        {
            health.TakeDamage(damage);
        }

        
        if (respawn != null)
        {
            respawn.Respawn();
        }
    }
}
