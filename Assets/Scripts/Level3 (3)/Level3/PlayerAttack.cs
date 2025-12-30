using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRadius = 0.5f;
    public int attackDamage = 10;
    public LayerMask enemyLayer;
    [SerializeField] private AudioClip attackSFX;
    private AudioSource audioSource;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }
    }

    void Attack()
{
    anim.SetTrigger("attack");

    if (audioSource != null && attackSFX != null)
        audioSource.PlayOneShot(attackSFX);

    Collider2D[] hits = Physics2D.OverlapCircleAll(
        attackPoint.position,
        attackRadius,
        enemyLayer
    );

    Debug.Log("HIT COUNT: " + hits.Length);

    foreach (Collider2D hit in hits)
    {
        EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(attackDamage);
        }
    }
}



    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
