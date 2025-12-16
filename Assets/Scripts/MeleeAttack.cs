using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    // empty transforms in front of player
    public Transform attackPoint;
    public Animator animator;

    [Header("Attack Settings")]
    // size of hit area
    public float attackRadius = 1.2f;
    // how far in front hit center is
    public float attackRangeForward = 1.2f;
    public int damage = 25;
    public float attackCooldown = 0.4f;

    public LayerMask enemyLayers;

    private bool canAttack = true;

    // prevent double hits in one swing if calling DoDamage multiple times
    private readonly HashSet<Collider> hitThisSwing = new HashSet<Collider>();

    void Reset()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // Left Mouse input
        if (canAttack && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        canAttack = false;
        hitThisSwing.Clear();

        // play animation
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        yield return new WaitForSeconds(0.15f);
        DoDamage();
        canAttack = true;
    }

    // call from animation event at impact frame
    public void DoDamage()
    {
        if (attackPoint == null) return;

        // center of hit area in front of player
        Vector3 center = attackPoint.position + attackPoint.forward * attackRangeForward;

        Collider[] hits = Physics.OverlapSphere(center, attackRadius, enemyLayers, QueryTriggerInteraction.Ignore);

        foreach (Collider c in hits)
        {
            if (hitThisSwing.Contains(c)) continue;
            hitThisSwing.Add(c);

            // generic damage interface first
            IDamageable dmg = c.GetComponentInParent<IDamageable>();
            if (dmg != null)
            {
                dmg.TakeDamage(damage);
                continue;
            }

            // or fall back to simple EnemyHealth component:
            /*
            EnemyHealth hp = c.GetComponentInParent<EnemyHealth>();
            if (hp != null)
            {
                hp.TakeDamage(damage);
            }*/
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Vector3 center = attackPoint.position + attackPoint.forward * attackRangeForward;
        Gizmos.DrawWireSphere(center, attackRadius);
    }
}

// simple interface
public interface IDamageable
{
    void TakeDamage(int amount);
}
