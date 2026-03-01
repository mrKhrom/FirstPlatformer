using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;   

public class MeleeEnemy : MonoBehaviour
{
    [Header("Attack parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    
    [Header("Collider parameters")]
    [SerializeField] private float coliedrDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    // References
    private Animator anim;
    private Health playerHealth;
    private EnemyPatrol enemyPatrol;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        //Attack only when player is in range and cooldown is over
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("meleeAttack");
            }
        }
        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight(); 
    }

    private bool PlayerInSight()
    {
        Vector2 origin = boxCollider.bounds.center
                        + transform.right * range * transform.localScale.x * coliedrDistance;

        Vector2 size   = new Vector2(boxCollider.bounds.size.x * range,
                                    boxCollider.bounds.size.y);

        float angle    = 0f;
        Vector2 dir    = transform.right;            // or Vector2.left, depending on orientation
        float dist     = range * coliedrDistance;   // whatever makes sense

        RaycastHit2D hit = Physics2D.BoxCast(origin, size, angle, dir, dist, playerLayer);
        
        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * coliedrDistance, 
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
    private void DamagePlayer()
    {
        //If player is still in range, damage him
        if (PlayerInSight())
            playerHealth.TakeDamage(damage);
  
    }
        
}