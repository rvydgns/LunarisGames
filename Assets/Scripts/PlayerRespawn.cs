using UnityEngine;

namespace ClearSky
{
    public class PlayerRespawn : MonoBehaviour
    {
        [Header("Checkpoint")]
        public Transform currentCheckpoint;

        [Header("Fall Death Settings")]
        public float deathY = -20f;

        private Animator anim;
        private SimplePlayerController controller;
        private Rigidbody2D rb;

        private bool isRespawning = false;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            controller = GetComponent<SimplePlayerController>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            // Level başında checkpoint yoksa SpawnPoint al
            if (currentCheckpoint == null)
            {
                GameObject spawnPoint = GameObject.Find("SpawnPoint");
                if (spawnPoint != null)
                {
                    currentCheckpoint = spawnPoint.transform;
                }
            }

            if (currentCheckpoint != null)
            {
                transform.position = currentCheckpoint.position;
            }
        }

        private void Update()
        {
            // Aşağı düşerse SADECE respawn (can işini PlayerHealth çözer)
            if (!isRespawning && transform.position.y < deathY)
            {
                Respawn();
            }
        }

        // Checkpoint trigger çağırır
        public void SetCheckpoint(Transform checkpoint)
        {
            currentCheckpoint = checkpoint;
            Debug.Log("Checkpoint set: " + checkpoint.name);
        }

        // ⭐ PlayerHealth ve DeathZone burayı çağırır
        public void Respawn()
        {
            if (currentCheckpoint == null || isRespawning)
                return;

            isRespawning = true;

            // Kontrolleri kapat
            if (controller != null)
                controller.enabled = false;

            // Fizik temizle
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            // Checkpoint'e taşı
            transform.position = currentCheckpoint.position;

            // Animasyon reset
            if (anim != null)
            {
                anim.ResetTrigger("die");
                anim.Play("Idle");
            }

            // Kontrolleri aç
            if (controller != null)
                controller.enabled = true;

            isRespawning = false;
        }
    }
}
