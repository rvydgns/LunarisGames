using UnityEngine;

namespace ClearSky
{
    public class PlayerHealth : MonoBehaviour
    {
        [Header("Health Settings")]
        public int maxHealth = 100;
        public int currentHealth;

        [Header("Knockback Settings")]
        public float knockbackForceX = 5f;
        public float knockbackForceY = 2f;

        private Animator anim;
        private Rigidbody2D rb;
        private SimplePlayerController controller;
        private PlayerRespawn respawnSystem;
        private GameUI ui;

        private bool isDead = false;

        private void Awake()
        {
            currentHealth = maxHealth;

            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            controller = GetComponent<SimplePlayerController>();
            respawnSystem = GetComponent<PlayerRespawn>();

            ui = FindObjectOfType<GameUI>();

            if (ui != null)
                ui.UpdateHealth(currentHealth, maxHealth);
        }

        public void TakeDamage(int amount, int attackerDirection)
        {
            if (isDead) return;

            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            if (ui != null)
                ui.UpdateHealth(currentHealth, maxHealth);

            anim.SetTrigger("hurt");
            ApplyKnockback(attackerDirection);

            if (currentHealth <= 0)
                Die();
        }

        private void ApplyKnockback(int attackerDirection)
        {
            rb.velocity = Vector2.zero;

            float direction = attackerDirection == 0 ? -1 : attackerDirection;
            rb.AddForce(new Vector2(direction * knockbackForceX, knockbackForceY),
                        ForceMode2D.Impulse);
        }

        private void Die()
        {
            isDead = true;

            anim.SetTrigger("die");
            controller.enabled = false;
            rb.velocity = Vector2.zero;

            Invoke(nameof(OpenDeathUI), 1.2f);
        }

        private void OpenDeathUI()
        {
            if (ui != null)
                ui.ShowDeathScreen();
        }

        // ⭐ DeathZone burayı çağırır
        public void TakeDeathZoneDamage(int amount)
        {
            if (isDead) return;

            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            if (ui != null)
                ui.UpdateHealth(currentHealth, maxHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                respawnSystem.Respawn();
            }
        }

        public void RestoreFullHealth()
        {
            currentHealth = maxHealth;
            isDead = false;

            if (ui != null)
                ui.UpdateHealth(currentHealth, maxHealth);
        }
    }
}
