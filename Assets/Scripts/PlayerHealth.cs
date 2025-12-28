using UnityEngine;
using UnityEngine.SceneManagement;

namespace ClearSky
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class PlayerHealth : MonoBehaviour
    {
        [Header("Health")]
        public int maxHealth = 100;
        public int currentHealth;

        [Header("Death Behaviour")]
        public bool reloadLevelOnDeath = false;
        public bool useRespawnSystem = true;

        [Header("Knockback")]
        public bool useKnockback = true;
        public float knockbackForceX = 5f;
        public float knockbackForceY = 2f;

        [Header("Heal Power")]
        public bool allowHealPower = false;
        public int healPowerAmount = 20;
        public KeyCode healKey = KeyCode.H;

        [Header("Audio")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip hurtSFX;

        private Animator anim;
        private Rigidbody2D rb;
        private PlayerMovement movement;
        private PlayerRespawn respawnSystem;
        private GameUI gameUI;
        private UIManager uiManager;

        private bool isDead = false;

        private void Awake()
        {
            currentHealth = maxHealth;

            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            movement = GetComponent<PlayerMovement>();
            respawnSystem = GetComponent<PlayerRespawn>();

            gameUI = FindObjectOfType<GameUI>();
            uiManager = FindObjectOfType<UIManager>();

            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();

            UpdateUI();
        }

        private void Update()
        {
            HandleHealPower();
        }

        // =========================
        // DAMAGE
        // =========================
        public void TakeDamage(int amount, int attackerDirection = 0)
        {
            if (isDead) return;

            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            anim.SetTrigger("hurt");

            if (audioSource && hurtSFX)
                audioSource.PlayOneShot(hurtSFX);

            if (useKnockback)
                ApplyKnockback(attackerDirection);

            UpdateUI();

            if (currentHealth <= 0)
                Die();
        }

        private void ApplyKnockback(int attackerDirection)
        {
            rb.velocity = Vector2.zero;
            float dir = attackerDirection == 0 ? -1 : attackerDirection;

            rb.AddForce(
                new Vector2(dir * knockbackForceX, knockbackForceY),
                ForceMode2D.Impulse
            );
        }

        // =========================
        // DEATH
        // =========================
        private void Die()
        {
            if (isDead) return;
            isDead = true;

            anim.SetTrigger("die");
            rb.velocity = Vector2.zero;

            if (movement != null)
                movement.enabled = false;

            if (reloadLevelOnDeath)
                Invoke(nameof(ReloadLevel), 1f);
            else if (useRespawnSystem && respawnSystem != null)
                Invoke(nameof(Respawn), 1f);
            else
                Invoke(nameof(OpenDeathUI), 1f);
        }

        private void ReloadLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void Respawn()
        {
            isDead = false;
            currentHealth = maxHealth;

            if (movement != null)
                movement.enabled = true;

            respawnSystem.Respawn();
            UpdateUI();
        }

        private void OpenDeathUI()
        {
            if (gameUI != null)
                gameUI.ShowDeathScreen();
        }

        // =========================
        // HEAL
        // =========================
        public void Heal(int amount)
        {
            if (isDead) return;

            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
            UpdateUI();
        }

        private void HandleHealPower()
        {
            if (!allowHealPower || isDead) return;
            if (GameManager.instance == null) return;
            if (!GameManager.instance.hasLioraHeal) return;

            if (Input.GetKeyDown(healKey))
            {
                Heal(healPowerAmount);
                GameManager.instance.hasLioraHeal = false;

                if (uiManager != null)
                    uiManager.HideLioraHealPowerUI();
            }
        }

        // =========================
        // UTILS
        // =========================
        public void RestoreFullHealth()
        {
            currentHealth = maxHealth;
            isDead = false;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (gameUI != null)
                gameUI.UpdateHealth(currentHealth, maxHealth);
        }
    }
}
