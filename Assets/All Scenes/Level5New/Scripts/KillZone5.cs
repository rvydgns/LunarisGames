using UnityEngine;

public class KillZone5 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerRespawn5 respawn = other.GetComponent<PlayerRespawn5>();
        if (respawn != null)
        {
            respawn.Respawn();
        }
    }
}
