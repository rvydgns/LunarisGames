using UnityEngine;

namespace ClearSky
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class PlayerRespawn : MonoBehaviour
    {
        [Header("Spawn Points")]
        public Transform playerStart;         // Sahne başlangıcı
        public Transform currentCheckpoint;   // Son checkpoint

        [Header("Fall Death")]
        public bool enableFallDeath = true;
        public float deathY = -20f;

        [Header("Respawn Settings")]
        public bool restoreFullHealthOnRespawn = true;

        private Rigidbody2D rb;
        private Animator anim;
        private PlayerMovement movement;
        private PlayerHealth health;

        private bool isRespawning = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            movement = GetComponent<PlayerMovement>();
            health = GetComponent<PlayerHealth>();
        }

        private void Start()
        {
            // 1️⃣ PlayerStart atanmamışsa SpawnPoint bul
            if (playerStart == null)
            {
                GameObject spawn = GameObject.Find("SpawnPoint");
                if (spawn != null)
                    playerStart = spawn.transform;
            }

            // 2️⃣ Checkpoint yoksa başlangıcı kullan
            if (currentCheckpoint == null)
                currentCheckpoint = playerStart;

            // 3️⃣ Oyuncuyu başlangıç noktasına koy
            if (currentCheckpoint != null)
                transform.position = currentCheckpoint.position;
        }

        private void Update()
        {
            if (!enableFallDeath || isRespawning) return;

            // Aşağı düşerse respawn (can işini PlayerHealth halleder)
            if (transform.position.y < deathY)
            {
                Respawn();
            }
        }

        // ⭐ Checkpoint trigger çağırır
        public void SetCheckpoint(Transform checkpoint)
        {
            currentCheckpoint = checkpoint;
            Debug.Log("Checkpoint set: " + checkpoint.name);
        }

        // ⭐ PlayerHealth / DeathZone çağırır
        public void Respawn()
        {
            if (isRespawning) return;

            Transform spawnPoint = currentCheckpoint != null
                ? currentCheckpoint
                : playerStart;

            if (spawnPoint == null)
            {
                Debug.LogWarning("Respawn failed: No spawn point.");
                return;
            }

            isRespawning = true;

            // Movement kapat
            if (movement != null)
                movement.enabled = false;

            // Fizik sıfırla
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.rotation = 0f;

            // Konum taşı
            transform.position = spawnPoint.position;

            // Can yenile
            if (restoreFullHealthOnRespawn && health != null)
                health.RestoreFullHealth();

            // Animasyon reset
            anim.Rebind();
            anim.Update(0f);
            anim.Play("Idle");

            // Movement aç
            if (movement != null)
                movement.enabled = true;

            isRespawning = false;
        }
    }
}
