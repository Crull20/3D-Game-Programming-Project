using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public class EnemyMeleeAttack : MonoBehaviour
{
    public int damage = 10;
    public float attackRadius = 1.1f;
    public float attackCooldown = 1.0f;

    public LayerMask playerLayer;

    EnemyAI ai;
    float nextAttackTime;

    private void Awake()
    {
        ai = GetComponent<EnemyAI>();
    }

    private void Update()
    {
        if (ai == null || ai.target == null) return;
        if (Time.time < nextAttackTime) return;

        // attack when close enough to target
        float dist = Vector3.Distance(transform.position, ai.target.position);
        if (dist > ai.stopDistance + 0.2f) return;

        TryHitPlayer();
    }

    void TryHitPlayer()
    {
        // center hit forward from enemy
        Vector3 center = transform.position + transform.forward * 0.6f;

        Collider[] hits = Physics.OverlapSphere(center, attackRadius, playerLayer, QueryTriggerInteraction.Ignore);
        foreach (var c in hits)
        {
            IDamageable dmg = c.GetComponentInParent<IDamageable>();
            if (dmg != null)
            {
                dmg.TakeDamage(damage);
                nextAttackTime = Time.time + attackCooldown;
                break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + transform.forward * 0.6f, attackRadius);
    }
}
