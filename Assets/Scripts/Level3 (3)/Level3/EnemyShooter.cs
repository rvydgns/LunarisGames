using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("Refs")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public Transform target;

    [Header("Shoot")]
    public float shootInterval = 0.9f;
    public float shootRange = 13f;

    private float timer;
    private Animator anim;
    private SpriteRenderer sr;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>(); 
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        timer = shootInterval * 0.5f;

        if (target == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) target = p.transform;
        }
    }

    private void Update()
    {
        if (target == null || bulletPrefab == null || firePoint == null) return;

        float distX = Mathf.Abs(target.position.x - transform.position.x);
        if (distX > shootRange) return;


        FaceTarget();

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Shoot();
            timer = shootInterval;
        }
    }

    void FaceTarget()
    {
        if (!sr) return;
        sr.flipX = target.position.x < transform.position.x;
    }

    void Shoot()
    {
        if (anim != null) anim.SetTrigger("shoot");

        GameObject b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Vector2 dir = (target.position.x >= transform.position.x) ? Vector2.right : Vector2.left;

        var proj = b.GetComponent<EnemyProjectile>();
        if (proj != null) proj.SetDirection(dir);
    }
}
