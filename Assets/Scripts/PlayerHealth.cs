using UnityEngine;

namespace ClearSky
{
    public class PlayerHealth : MonoBehaviour
    {
        public int maxHealth = 100;
        public int currentHealth;

        private Animator anim;
        private Rigidbody2D rb;
        private PlayerMovement movement;
        private PlayerRespawn respawnSystem;
        private UIManager ui;

        private bool isDead = false;

        private void Awake()
        {
            currentHealth = maxHealth;
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            movement = GetComponent<PlayerMovement>();
            respawnSystem = GetComponent<PlayerRespawn>();
            ui = FindObjectOfType<UIManager>();
        }

        private void Update()
        {
            HandleLioraHealPower();
        }

        // ============================
        // LIORA HEAL POWER (+20 HP)
        // ============================
        private void HandleLioraHealPower()
        {
            if (GameManager.instance == null) return;
            if (!GameManager.instance.isBossFightActive) return;
            if (!GameManager.instance.hasLioraHeal) return;
            if (isDead) return;

            if (Input.GetKeyDown(KeyCode.H))
            {
                Heal(20);

                GameManager.instance.hasLioraHeal = false;

                if (ui != null)
                    ui.HideLioraHealPowerUI();

                Debug.Log("Liora Heal Power kullanıldı (+20 HP)");
            }
        }

        // ============================
        // DAMAGE & DEATH
        // ============================
        public void TakeDamage(int amount, int attackerDirection)
        {
            if (isDead) return;

            currentHealth -= amount;

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            isDead = true;

            anim.SetTrigger("die");
            rb.velocity = Vector2.zero;
            rb.simulated = false;
            movement.enabled = false;

            Invoke(nameof(Respawn), 0.8f);
        }

        private void Respawn()
        {
            rb.simulated = true;
            movement.enabled = true;
            respawnSystem.RespawnPlayer();
        }

        // ============================
        // HEAL FUNCTIONS
        // ============================
        public void RestoreFullHealth()
        {
            currentHealth = maxHealth;
            isDead = false;
        }

        public void Heal(int amount)
        {
            if (isDead) return;

            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);

            Debug.Log("Heal uygulandı. Güncel HP: " + currentHealth);

            if (ui != null)
            {
                // ui.UpdateHealth(currentHealth); // UI hazır olunca açılır
            }
        }
    }
}
