using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyProjectile : MonoBehaviour
{
    public float speed = 4f;
    public int damage = 10;
    public float lifeTime = 2.5f;

  
    public float maxYDifference = 0.6f;

    private Vector2 dir;
    private Rigidbody2D rb;
    private Collider2D col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

    
        rb.gravityScale = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.freezeRotation = true;

     
        col.enabled = false;
    }

    private void Start()
    {
        
        Invoke(nameof(EnableCollider), 0.1f);

        Destroy(gameObject, lifeTime);
    }

    void EnableCollider()
    {
        col.enabled = true;
    }

    public void SetDirection(Vector2 direction)
    {
        dir = direction.normalized;
        rb.velocity = dir * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Collider2D playerCol = other.GetComponent<Collider2D>();
        if (playerCol == null) return;

        float bulletY = transform.position.y;

       
        if (bulletY < playerCol.bounds.min.y || bulletY > playerCol.bounds.max.y)
            return;

        var ph = other.GetComponent<ClearSky.PlayerHealth>();
        if (ph != null)
            ph.TakeDamage(damage);

        Destroy(gameObject);
    }

}
