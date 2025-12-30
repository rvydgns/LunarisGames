using System.Collections;
using UnityEngine;

public class BossMalvaren5 : MonoBehaviour
{
    public static BossMalvaren5 Instance;

    [Header("Health")]
    public int maxHealth = 5;
    private int currentHealth;

    [Header("Death VFX")]
    public GameObject deathVfx;
    public float vfxDuration = 2.5f;
    public float vfxScale = 10f;
    public float vfxHeightOffset = 1.5f;

    [Header("Boss Disappear")]
    public float bossDisappearDelay = 0.2f;

    [Header("Death SFX")]
    public AudioClip deathSfx;
    [Range(0f, 1f)] public float deathSfxVolume = 0.9f;

    private bool isDead;
    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        currentHealth = maxHealth;

        // AudioSource garanti olsun
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f; // 2D
    }

    public void TakeGemDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (currentHealth <= 0)
            StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        isDead = true;

        // Boss'u dondur
        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = Vector2.zero;
            rb.simulated = false;
        }

        // Diğer scriptleri kapat
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            if (script != this)
                script.enabled = false;
        }

        // 🔊 Ölüm sesi (GARANTİ)
        if (deathSfx != null)
        {
            audioSource.PlayOneShot(deathSfx, deathSfxVolume);
        }

        // 💥 Ölüm VFX
        if (deathVfx != null)
        {
            Vector3 spawnPos = transform.position + Vector3.up * vfxHeightOffset;

            GameObject fx = Instantiate(deathVfx, spawnPos, Quaternion.identity);
            fx.transform.localScale = Vector3.one * vfxScale;

            Destroy(fx, vfxDuration);
        }

        // Biraz bekle, sonra boss gitsin
        yield return new WaitForSeconds(bossDisappearDelay);
        Destroy(gameObject);
    }
}
