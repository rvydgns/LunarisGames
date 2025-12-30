using UnityEngine;

namespace ClearSky
{
    public class CheckpointTrigger : MonoBehaviour
    {
        [SerializeField] private Transform checkpointPoint;

        private void Reset()
        {
            checkpointPoint = transform; // otomatik kendisini referans alsın
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            PlayerRespawn player = collision.GetComponent<PlayerRespawn>();
            if (player == null) return;

            // checkpointPoint boşsa kendini kullan
            Transform cp = checkpointPoint != null ? checkpointPoint : transform;

            player.SetCheckpoint(cp);
        }
    }
}
