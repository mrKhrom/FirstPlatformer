using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;   

public class MeleeEnemy : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private float coliedrDistance;
    [SerializeField] private int damage;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
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
        return hit.collider != null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * coliedrDistance, 
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}