using UnityEngine;

namespace ClearSky
{
    public class PlayerRespawn : MonoBehaviour
    {
        public Transform currentCheckpoint;
        private PlayerHealth health;
        private Animator anim;
        private SimplePlayerController controller;
        private Rigidbody2D rb;

        private void Awake()
        {
            health = GetComponent<PlayerHealth>();
            anim = GetComponent<Animator>();
            controller = GetComponent<SimplePlayerController>();
            rb = GetComponent<Rigidbody2D>();
        }

        public void SetCheckpoint(Transform checkpoint)
        {
            currentCheckpoint = checkpoint;
        }

        public void RespawnPlayer()
        {
            if (currentCheckpoint == null)
            {
                Debug.LogWarning("No checkpoint assigned!");
                return;
            }

            // 1. Karakteri kontrol edilmeyen ölüm pozisyonundan kurtar
            rb.velocity = Vector2.zero;

            // 2. Karakteri checkpoint pozisyonuna taşı
            transform.position = currentCheckpoint.position;

            // 3. Sağlığı full yap
            health.RestoreFullHealth();

            // 4. Karakter davranışlarını aç
            controller.enabled = true;

            // 5. Ölüm animasyonunu kapat
            anim.ResetTrigger("die");
            anim.Play("Idle"); // Idle animasyonuna dön

            Debug.Log("Player Respawned!");
        }
    }
}
