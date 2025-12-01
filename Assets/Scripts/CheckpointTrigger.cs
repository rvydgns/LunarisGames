using UnityEngine;
using ClearSky;

public class CheckpointTrigger : MonoBehaviour
{
    public Transform checkpoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerRespawn player = collision.GetComponent<PlayerRespawn>();

        if (player != null)
        {
            player.SetCheckpoint(checkpoint);
            Debug.Log("Checkpoint Updated!");
        }
    }
}
