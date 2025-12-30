using UnityEngine;

public class Level5LunaAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius = 0.4f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Cooldown")]
    [SerializeField] private float attackCooldown = 0.4f;
    private float cooldownTimer;

    private bool canDealDamage;

    private void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

 
        if (Input.GetKeyDown(KeyCode.J) && cooldownTimer <= 0f)
        {
            cooldownTimer = attackCooldown;
         
        }
    }

  

    
    public void EnableAttackHitbox()
    {
        canDealDamage = true;
        DoDamage();
    }

   
    public void DisableAttackHitbox()
    {
        canDealDamage = false;
    }

    private void DoDamage()
    {
        if (!canDealDamage) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRadius,
            enemyLayer
        );
    }


    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}