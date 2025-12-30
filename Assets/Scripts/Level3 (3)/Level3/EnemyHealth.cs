using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 30;
    private int currentHealth;

    private Animator anim;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip deathSFX;


    void Awake()
    {
        currentHealth = maxHealth;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
      
            if (audioSource != null && deathSFX != null)
                audioSource.PlayOneShot(deathSFX);

            Die();
        }
    }


    void Die()
    {
        GetComponent<Collider2D>().enabled = false;

      
        Destroy(gameObject, 0.5f);
    }

}
