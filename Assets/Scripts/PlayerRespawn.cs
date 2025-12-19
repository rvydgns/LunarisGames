using UnityEngine;

namespace ClearSky
{
    public class PlayerRespawn : MonoBehaviour
    {
        [Header("Spawn Points")]
        public Transform playerStart;         // Sahnenin başlangıcı
        public Transform currentCheckpoint;   // Son checkpoint

        private PlayerHealth health;
        private Animator anim;
        private Rigidbody2D rb;

        private void Awake()
        {
            health = GetComponent<PlayerHealth>();
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            // Sahne başında checkpoint yoksa PlayerStart'ı kullan
            if (currentCheckpoint == null && playerStart != null)
                currentCheckpoint = playerStart;
        }

        public void SetCheckpoint(Transform checkpoint)
        {
            currentCheckpoint = checkpoint;
            Debug.Log("Checkpoint set: " + checkpoint.name);
        }

        public void RespawnPlayer()
        {
            // 1) Spawn noktası seç
            Transform spawnPoint = currentCheckpoint != null ? currentCheckpoint : playerStart;

            if (spawnPoint == null)
            {
                Debug.LogWarning("No spawn point! Assign PlayerStart in Inspector.");
                return;
            }

            // 2) Fizik sıfırla (sonsuz düşme / yan yatma buglarının ilacı)
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.rotation = 0f;

            // 3) Taşı
            transform.position = spawnPoint.position;

            // 4) Can yenile
            health.RestoreFullHealth();

            // 5) Animasyonu toparla
            anim.Rebind();
            anim.Update(0f);
            anim.Play("Idle");

            Debug.Log("Respawned to: " + spawnPoint.name);
        }
    }
}
