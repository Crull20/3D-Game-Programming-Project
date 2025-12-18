using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public class EnemyMeleeAttack : MonoBehaviour
{
    [Header("Damage")]
    public int damage = 10;

    [Header("Attack")]
    public float attackRadius = 1.1f;
    public float attackCooldown = 1.0f;
    public LayerMask playerLayer;

    private EnemyAI ai;
    private float nextAttackTime;

    private void Awake()
    {
        ai = GetComponent<EnemyAI>();
        nextAttackTime = 0f;
    }

    private void Update()
    {
        if (ai == null || ai.target == null) return;

        // flat distance so height doesnt prevent attack
        Vector3 a = transform.position; a.y = 0f;
        Vector3 b = ai.target.position; b.y = 0f;
        float dist = Vector3.Distance(a, b);

        if (dist > attackRadius) return;
        if (Time.time < nextAttackTime) return;

        TryHitPlayer();
    }

    void TryHitPlayer()
    {
        // center overlap at target
        Vector3 center = ai.target.position;

        // hurtbox is trigger so use collide
        Collider[] hits = Physics.OverlapSphere(center, attackRadius, playerLayer, QueryTriggerInteraction.Collide);

        foreach (var c in hits)
        {
            IDamageable dmg = c.GetComponentInParent<IDamageable>();
            if (dmg == null) continue;

            var playerHealth = c.GetComponentInParent<PlayerHealth>();
            if (playerHealth != null)
            {
                Debug.Log($"[EnemyAttack] Hit for {damage}. Player HP before: {playerHealth.currentHealth}/{playerHealth.maxHealth}");
            }

            dmg.TakeDamage(damage);
            nextAttackTime = Time.time + Mathf.Max(attackCooldown, 0.05f);
            return;
        }

        // nothing valid hit don't start cooldown
    }

    private void OnDrawGizmosSelected()
    {
        if (ai != null && ai.target != null)
            Gizmos.DrawWireSphere(ai.target.position, attackRadius);
    }
}
