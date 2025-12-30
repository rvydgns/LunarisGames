using UnityEngine;

public class GemBreaker5 : MonoBehaviour
{
    public KeyCode breakKey = KeyCode.J;
    public float breakRange = 1.5f;
    public LayerMask gemLayer;

    [Header("Attack Cooldown")]
    public float attackCooldown = 0.5f; // saniye
    private float nextAttackTime = 0f;

    void Update()
    {
        if (Input.GetKeyDown(breakKey) && Time.time >= nextAttackTime)
        {
            TryBreakGem();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void TryBreakGem()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, breakRange, gemLayer);
        if (hit == null) return;

        BreakableGem5 gem = hit.GetComponent<BreakableGem5>();
        if (gem != null)
            gem.Break();
    }
}
